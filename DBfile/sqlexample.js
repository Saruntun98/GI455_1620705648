const sqlite3 = require('sqlite3').verbose();

let db = new sqlite3.Database('./db/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>{
    if(err) throw err;

    console.log('Connected to database.');

    var dataFromClient = {
        eventName: "Login",
        data:"test001#1236#AA"
    }
    
    var splitStr = dataFromClient.data.split('#');
    var userID = splitStr[0];
    var password = splitStr[1];
    var name = splitStr[2];
    

    var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+userID+"' AND Password='"+password+"'";//login
    var sqlInsert = "INSERT INTO UserData (UserID, Password, Name) VALUES ('"+userID+"', '"+password+"','"+name+"')"; //register
    
    db.all(sqlInsert, (err, rows)=>{
        if (err)
        {
            
            var callbackMsg = {
                eventName: "Register",
                data:"fail"
                
            }

            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("[0]" +toJsonStr);
        }
        else{

            var callbackMsg = {
                eventName:"Register",
                data:"Success"
            }

            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("[1]" + toJsonStr);
        }
    });
    
    /*db.all('SELECT * FROM UserData', (state,err,row)=>{
        if(err){
            console.log(err);
        }
        console.log(state);
        console.log(rows);
    });*/
});
