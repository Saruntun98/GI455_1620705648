using System;
//using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Messenger
{
    public struct SocketEvent
    {
        public string eventName;
        public string data;

        public SocketEvent(string eventName, string data)
        {
            this.eventName = eventName;
            this.data = data;
        }
    }
    public class Chat : MonoBehaviour
    {
        [SerializeField]
        List<Message> messageList = new List<Message>();
        [SerializeField]
        private InputField textChat;
        private string callbackData;
        private string tempMessageString;
        public int maxMessage = 25;
        public GameObject chatPanel, textObject;
        public GameObject showUsername;
        public Button sendButton;
        public Text textSead;
        public Text textRead;
        private List<string> messageQueue = new List<string>();

        public string usernameText;
        
        
        private WebSocket websocket;
        
        public delegate void DelegateHandle(SocketEvent result);

        public event DelegateHandle OnConnectionSuccess;
        public event DelegateHandle OnConnectionFail;
        public event DelegateHandle OnReceiveMessage;
        public event DelegateHandle OnCreateRoom;
        public event DelegateHandle OnJoinRoom;
        public event DelegateHandle OnLeaveRoom;
        
        [SerializeField]
        private InputField  inputUser;
        public InputField  inputIp;
        public InputField  inputPort;
        
        
        void Start()
        {
            websocket = new WebSocket($"ws://127.0.0.1:5500/");
            //websocket = new WebSocket($"ws://127.0.0.1:5500/");
            websocket.Connect();
            websocket.OnMessage += OnMessage;
            Button btn = sendButton.GetComponent<Button>();
            btn.onClick.AddListener(SearchButton);
            
            
        }
        
     
        void Update()
        {
            
            if (tempMessageString != "")
            {
                textChat.text += tempMessageString + "\n";
                tempMessageString = "";
            }
            
            UpdateNotifyMessage();
            /*if(messageQueue.Count > 0)
            {
                SeadMessageToChat(messageQueue[0],callbackData);
                messageQueue.RemoveAt(0);
            }
            
            showUsername.GetComponent<Text>().text = $"[<color=green>{inputUser.text}</color>]";
            if (textChat.text != "")
                
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SearchButton();
                }
            }
            else
            {
                if (!textChat.isFocused && Input.GetKeyDown(KeyCode.Return))
                {
                    textChat.ActivateInputField();
                }
            }
            
            if (!textChat.isFocused)
            {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            SeadMessageToChat("<color=red>Please input letters!!</color>",callbackData);
                            Debug.Log("Space");
                        }
            }*/
        }
        
        
        public void SearchButton()
        {
            //SeadMessageToChat($"<color=#C1F55B>{inputUser.text}</color> : {textChat.text}");
            
            
            /*if (websocket.ReadyState == WebSocketState.Open)
            {
                websocket.Send($"{usernameText} : {textChat.text}");
            }*/
            //websocket.Send($"{inputUser.text} > {textChat.text}");
            //textChat.text = $"";
            
            if(!string.IsNullOrEmpty(inputUser.text))
            {
                websocket.Send(textChat.text);
            }

            textChat.text = "";
            //Debug.Log("ok");
        }
        
        public void SeadMessageToChat(string text, string callbackData)
        {
            
            if (messageList.Count >= maxMessage)
            {
                Destroy(messageList[0].textObject.gameObject);
                messageList.Remove(messageList[0]);
            }
            Message newMessage = new Message();
            newMessage.text = text;
            GameObject newText = Instantiate(textObject, chatPanel.transform);
            newMessage.textObject = newText.GetComponent<Text>();
            newMessage.textObject.text = newMessage.text;
            //newMessage.textObject.alignment = TextAnchor.MiddleRight;
            //textObject.a = TextAnchor.MiddleRight;
            messageList.Add(newMessage);
            Debug.Log("OnMessage : " + callbackData);
            
            
            if (newMessage.text == "" || websocket.ReadyState != WebSocketState.Open)
            {
                return;
            }

            MessageData newMessageData = new MessageData();
            newMessageData.username = usernameText;
            newMessageData.message = newMessage.text;

            string toJsonStr = JsonUtility.ToJson(newMessageData);
            websocket.Send(toJsonStr);
            newMessage.text = "";

            /*EventServer recieveEvent = JsonUtility.FromJson<EventServer>(callbackData);

            switch (recieveEvent.eventName)
            {
                case "CreateRoom":
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(recieveEvent.data);
                    break;
                }
                case "JoinRoom":
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(recieveEvent.data);
                    break;
                }
                case "LeaveRoom":
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(recieveEvent.data);
                    break;
                }
                case "Message":
                {
                    Debug.Log("message : "+ recieveEvent.data);
                    if (OnReceiveMessage != null)
                        OnReceiveMessage(recieveEvent.data);
                    break;
                }
                case "RequestToken":
                {
                    Debug.Log("message : " + recieveEvent.data);
                    break;
                }
            }*/
        }

        private void OnDestroy()
        {
            Disconnect();
        }
        
        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);
                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }
        }
        
        /*public void OnMessage(object semder, MessageEventArgs messageEventArgs)
        {
            CreateRoom("TESTRoom1");
            Debug.Log("Receive msg : " + messageQueue);
            messageQueue.Add(messageEventArgs.Data);
            SeadMessageToChat($"<color=#C1F55B>{messageQueue}</color>",callbackData);
            
            //SeadMessageToChat(messageEventArgs.Data);
        }/*
        
        /*public void CreateRoom(string nameRoom)
        {
            if (websocket.ReadyState == WebSocketState.Open)
            {
                SocketEvent socketEvent = new SocketEvent("Create Room",nameRoom);
                string jsonStr = JsonUtility.ToJson(socketEvent);
                websocket.Send(jsonStr);
            }
        }*/
        
        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            CreateRoom("TESTRoom1");
            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;
        }
        
        public void CreateRoom(string roomName)
        {
            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            websocket.Send(toJsonStr);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            websocket.Send(toJsonStr);
        }

        public void Disconnect()
        {
            if (websocket != null)
                websocket.Close();
        }
        
        public class MessageData
        {
            public string username;
            public string message;
        }

    }
    
    [Serializable]
    public class Message
    {
        public string text;
        public Text textObject;
    }
    
    [System.Serializable]
    public class WSEvent
    {
        public string eventName;
    }

    [System.Serializable]
    public class EventServer : WSEvent
    {
        public string data;
    }
}
