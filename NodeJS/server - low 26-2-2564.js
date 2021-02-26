var websocket = require("ws");

var callbackInitServer = ()=>{
    console.log("Server Saruntun is running.");
}

var wss = new websocket.Server ({port:25570}, callbackInitServer);

var wsList = [];

wss.on("connection", (ws)=>{

    console.log("Client Connected.");
    wsList.push(ws);

    ws.on("message", (data)=>{
        console.log("send from client : "+ data);
        Broadcast(data);
    });

    ws.on("close", ()=>{
        wsList = ArrayRemove(wsList, ws);
        console.log("client Disconnected.");
    });
});

var ArrayRemove = (arr, value)=>{
    return arr.filter((element)=>{
        return element != value;
    });
}

var Broadcast = (data)=>{
    data
    for(var i = 0; i < wsList.length; i++){
        wsList[i].send(data);
    };
}

