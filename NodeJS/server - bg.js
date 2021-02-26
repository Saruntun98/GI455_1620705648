var websocket = require("ws");

var callbackInitServer = ()=>{
    console.log("Server Saruntun is running.");
} 

var wss = new websocket.Server ({port:5500}, callbackInitServer);

var wsList = [];
var roomList = [];

wss.on("connection", (ws)=>{

    {
        //LobbyZone
        ws.on("message", (data)=>{

            console.log(data);
            var toJson = JSON.parse(data);
            //console.log(toJson["eventName"]);
            //console.log(toJson.eventName);


            if (toJson.eventName== "Create Room")//CreateRoom
             {
                console.log("Client request Create Room ["+toJson.data+"]")
             }
            else if (toJson.eventName == "Join Room")//JoinRoom
              {
                 console.log("Client request Join Room")
              }
              
            else if(toJson.eventName = "LeaveRoom")
            {
                var isFound = false;
                for(var i = 0; i < roomList.length[i].wsList.length; j++)
                {
                    
                }
            }
              //-console.log("send from client : "+ data);
              //-Broadcast(data);
              
        });
    }

    console.log("Client Connected.");
    wsList.push(ws);



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

