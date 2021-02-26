using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace ChatMessenger
{
    public class WebSocketConnection : MonoBehaviour
    {
        //-------------------GameObject----------------
        public GameObject rootConnection;
        public GameObject rootLogin;
        public GameObject rootLogout;
        public GameObject rootRegister;
        public GameObject rootShowUser;
        public GameObject rootLobby;
        public GameObject rootCreateRoom;
        public GameObject rootJoinRoom;
        public GameObject rootRoom;
        public GameObject rootPopUp;
        //---------------------------------------------
        
        public InputField inputUserID;
        public InputField inputUsername;
        public InputField inputMessage;
        
        public Text textRoom;
        public Text PopUptext;
        public Text sendText;
        public Text receiveText;
        
        private WebSocket ws;

        private string tempMessageString;

        public class MessageData
        {
            public string username;
            public string message;
        }
        
        public void Start()
        {
            //Menu Button
                rootConnection.SetActive(true);
                rootLogin.SetActive(false);
                rootRegister.SetActive(false);
                rootShowUser.SetActive(false);
                rootLobby.SetActive(false);
                rootCreateRoom.SetActive(false);
                rootJoinRoom.SetActive(false);
                rootRoom.SetActive(false);
            //------------
            
        }

        public void Connect()
        {
            string url = $"ws://127.0.0.1:25570/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            rootConnection.SetActive(false);
            rootLogin.SetActive(true);
        }

        //-------------Button
        public void Login()
        {
            rootLogin.SetActive(false);
            rootShowUser.SetActive(true);
            rootLobby.SetActive(true);
            rootCreateRoom.SetActive(false);
            rootJoinRoom.SetActive(false);
            rootRoom.SetActive(false);
        }
        
        public void Logout()
        {
            rootLogin.SetActive(true);
            rootShowUser.SetActive(false);
        }
        
        public void GetRegister()
        {
            rootLogin.SetActive(false);
            rootRegister.SetActive(true);
        }
        
        public void Register()
        {
            rootRegister.SetActive(false);
            rootLogin.SetActive(true);
        }
        
        /*public void ShowUser()
        {
            rootLogin.SetActive(true);
            rootLogout.SetActive(false);
            rootLobby.SetActive(false);
        }*/
        public void OneGoBack()
        {
            rootCreateRoom.SetActive(false);
            rootJoinRoom.SetActive(false);
            rootLobby.SetActive(true);
        }
        
        public void LeaveRoom()
        {
            rootLobby.SetActive(true);
            rootRoom.SetActive(false);
            rootCreateRoom.SetActive(false);
            rootJoinRoom.SetActive(false);
        }
        
        public void GetCreateRoom()
        {
            rootCreateRoom.SetActive(true);
            rootShowUser.SetActive(true);
            rootLobby.SetActive(false);
        }
        
        public void CreateRoom()
        {
            rootCreateRoom.SetActive(false);
            rootRoom.SetActive(true);
        }
        
        public void GetJoinRoom()
        {
            rootJoinRoom.SetActive(true);
            rootShowUser.SetActive(true);
            rootLobby.SetActive(false);
        }
        
        public void JoinRoom()
        {
            rootJoinRoom.SetActive(false);
            rootLobby.SetActive(false);
            rootRoom.SetActive(true);
        }

        public void RoomChat()
        {
            rootRoom.SetActive(false);
            rootLobby.SetActive(true);
        }

        public void PopupOK()
        {
            rootPopUp.SetActive(false);
        }
        
        public void Disconnect()
        {
            if (ws != null)
            {
                ws.Close();
            }
        }
        //############################
        
        private void OnCreateRoom(string msg)
        {
            if(msg == "fail")
            {
                Debug.Log("Create room fail.");
                ShowPopup(msg);
            }
            else
            {
                Debug.Log("Create room success.");
                textRoom.text = "Room : [" + msg + "]";
                //OpenUIRoot(UIRootType.Chat);
            }
        }
        
        public void ShowPopup(string msg)
        {
            Debug.Log("Show popup");
            rootPopUp.SetActive(true);
            PopUptext.text = msg;
        }
        
        public void SendMessage()
        {
            if (inputMessage.text == "" || ws.ReadyState != WebSocketState.Open)
            {
                return;
            }

            MessageData newMessageData = new MessageData();
            newMessageData.username = inputUsername.text;
            newMessageData.message = inputMessage.text;

            string toJsonStr = JsonUtility.ToJson(newMessageData);
            ws.Send(toJsonStr);
            inputMessage.text = "";
        }

        private void OnDestroy()
        {
            if (ws != null)
                ws.Close();
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);
                if (receiveMessageData.username == inputUsername.text)
                {
                    sendText.text += "<color=#FFA533>" + receiveMessageData.username + "</color> : " + receiveMessageData.message + "\n";
                    receiveText.text += "\n";
                }
                else
                {
                    receiveText.text += "<color=#33B2FF>" + receiveMessageData.username + "</color> : " + receiveMessageData.message + "\n";
                    sendText.text += "\n";
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            tempMessageString = messageEventArgs.Data;
            
        }
    }
}