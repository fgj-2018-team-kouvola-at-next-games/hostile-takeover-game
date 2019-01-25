var app = require("express")();
var http = require("http").Server(app);
var io = require("socket.io")(http);
const uuid = require("uuid/v4");

const data = [];

for (let i = 0; i < 20; i++) {
  data.push({
    id: uuid(),
    x: Math.floor(Math.random() * 100),
    y: Math.floor(Math.random() * 100),
    r: 0.5,
    g: 0.5,
    b: 0.5,
    type: "block"
  });
}

app.get("/", function(req, res) {
  res.sendFile(__dirname + "/index.html");
});

io.on("connection", function(socket) {
  currentUser = {
    id: uuid(),
    x: Math.floor(Math.random() * 100),
    y: Math.floor(Math.random() * 100),
    r: Math.random(),
    g: Math.random(),
    b: Math.random(),
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
    currentUser.x += x;
    currentUser.y += y;

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

    currentUser.carries = blockId;
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

    currentUser.carries = undefined;
    block.x = currentUser.x + 1;
    block.y = currentUser.y;

    setTimeout(() => io.emit("update", block), 0);
    setTimeout(() => io.emit("update", currentUser), 0);
  });

  socket.on("disconnect", function() {
    console.log("user disconnected");
  });
});

http.listen(3000, function() {
  console.log("listening on *:3000");
});
