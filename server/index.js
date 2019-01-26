const express = require("express");
var app = express();
var http = require("http").Server(app);
var io = require("socket.io")(http);
const uuid = require("uuid/v4");

const data = [];

for (let i = 0; i < 100; i++) {
  data.push({
    id: uuid(),
    x: Math.floor(Math.random() * 50),
    y: Math.floor(Math.random() * 50),
    r: 0.5,
    g: 0.5,
    b: 0.5,
    type: "block"
  });
}

app.use(express.static("build"));

io.on("connection", function(socket) {
  const currentUser = {
    id: uuid(),
    x: Math.floor(Math.random() * 50),
    y: Math.floor(Math.random() * 50),
    r: Math.random(),
    g: Math.random(),
    b: Math.random(),
    directionX: 0,
    directionY: 1,
    type: "user"
  };

  console.log(`a user connected with id ${currentUser.id}`);

  // Send all data
  data.forEach(item => socket.emit("initItem", item));

  // Set current user
  data.push(currentUser);

  // Send current user
  socket.emit("setCurrentUser", currentUser);
  io.emit("initItem", currentUser);

  socket.on("move", function({ x, y }) {
    // hit test
    const newItem = {
      ...currentUser,
      x: currentUser.x + x,
      y: currentUser.y + y
    };
    if (hitTestAll(newItem)) {
      return;
    }

    currentUser.x += x;
    currentUser.y += y;
    currentUser.directionX = x;
    currentUser.directionY = y;

    setTimeout(() => io.emit("update", currentUser), 0);
  });

  socket.on("pick", function({ blockId }) {
    const block = data.find(
      item => item.type === "block" && item.id === blockId
    );
    if (!block) {
      console.error(`Block with id ${blockId} not found`);
      return;
    }

    if (!isTouching(currentUser, block)) {
      return;
    }

    currentUser.carries = blockId;
    block.owner = currentUser.id;
    block.isCarried = true;
    block.r = currentUser.r;
    block.g = currentUser.g;
    block.b = currentUser.b;

    setTimeout(() => io.emit("update", block), 0);
    setTimeout(() => io.emit("update", currentUser), 0);
  });

  socket.on("place", function() {
    if (!currentUser.carries) return;

    const block = data.find(
      item => item.type === "block" && item.id === currentUser.carries
    );
    if (!block) {
      console.error(`Block with id ${blockId} not found`);
      return;
    }

    if (hitTestAll({ ...block, y: currentUser.y + currentUser.directionY, x: currentUser.x + currentUser.directionX })) {
      return;
    }

    currentUser.carries = undefined;
    block.isCarried = false;
    block.x = currentUser.x + currentUser.directionX;
    block.y = currentUser.y + currentUser.directionY;

    // Check which blocks are touching the current block
    const touching = findTouching(block);
    const newOwnerId = findOwnerWithMostBlocks(touching);
    const newOwner = data.find(i => i.id === newOwnerId);

    touching.forEach(touchingBlock => {
      touchingBlock.owner = newOwnerId;
      touchingBlock.r = newOwner.r;
      touchingBlock.g = newOwner.g;
      touchingBlock.b = newOwner.b;
      setTimeout(() => io.emit("update", touchingBlock), 0);
    });

    setTimeout(() => io.emit("update", currentUser), 0);

    updateLeaderboard()
  });

  socket.on("disconnect", function() {
    console.log("user disconnected");
  });
});

const PORT = process.env.PORT || 3000;
http.listen(PORT, function() {
  console.log("listening on *:" + PORT);
});

function findTouching(item) {
  let touchingItems = [item];
  let lastTouchingItems;
  do {
    lastTouchingItems = touchingItems;
    touchingItems = data.filter(
      i => i.type === "block" && lastTouchingItems.some(i2 => isTouching(i, i2))
    );
  } while (lastTouchingItems.length !== touchingItems.length);

  return touchingItems;
}

function isTouching(item1, item2) {
  const xDiff = item1.x - item2.x;
  const yDiff = item1.y - item2.y;

  return xDiff ** 2 + yDiff ** 2 <= 1;
}

function findOwnerWithMostBlocks(blocks) {
  const owners = [...new Set(blocks.map(i => i.owner).filter(Boolean))];
  if (owners.length === 1) return owners[0];

  const withWeights = owners.map(o => ({
    owner: o,
    num: blocks.filter(block => block.owner === o).length
  }));

  return withWeights.sort((a, b) => b.num - a.num)[0].owner;
}

function hitTestAll(target) {
  return data.some(
    item =>
      item.id !== target.id &&
      !item.isCarried &&
      target.x === item.x &&
      target.y === item.y
  );
}

function updateLeaderboard() {
  const leaderboard = data.reduce((lb, item) => {
    if (item.type !== "block") return lb
    if (!item.owner) return lb

    const numBlocks = lb[item.owner] || 0
    return {
      ...lb,
      [item.owner]: numBlocks + 1
    }
  }, {})

  const asArray = Object.entries(leaderboard).map(([id, numBlocks]) => ({ id, numBlocks })).sort((a,b) => b.numBlocks - a.numBlocks)

  const top10 = asArray.slice(0, 5)
  const withUserData = top10.map((u, position) => Object.assign(u, { position }, data.find(b => b.id === u.id)))

  withUserData.forEach(boardItem => io.emit("leaderboard", boardItem));
}