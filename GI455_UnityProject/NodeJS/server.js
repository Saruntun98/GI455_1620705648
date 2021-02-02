var WebSocket = require("ws")

var callbackInitServer = ()=>{
    console.log("Server is running.");
} 

var wss = new WebSocket.Server ({port:15565}, callbackInitServer);