mergeInto(LibraryManager.library, {

  BindOn: function (eventName) {
    var name = Pointer_stringify(eventName);
    if (!window.socketConn) {
        window.socketConn = window.io.connect();
    }
    window.socketConn.on(name, (function(data) {
        SendMessage("WebApi", "OnEvent", name + "\u2014" + JSON.stringify(data));
    }));
  },
  
  Emit: function(eventName, data) {
    if (!window.socketConn) window.socketConn = window.io.connect();
    window.socketConn.emit(Pointer_stringify(eventName), JSON.parse(Pointer_stringify(data)));
  }
});