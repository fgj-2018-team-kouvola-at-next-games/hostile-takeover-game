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

    setTimeout(() => io.emit("updateUser", currentUser), 0);
  });
  socket.on("disconnect", function() {
    console.log("user disconnected");
  });
});

function updateClients() {
  io.emit("update", "boop");
}

http.listen(3000, function() {
  console.log("listening on *:3000");
});
