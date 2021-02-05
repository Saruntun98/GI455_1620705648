using System;
//using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Messenger
{
    public class Chat : MonoBehaviour
    {
        [SerializeField]
        List<Message> messageList = new List<Message>();
        [SerializeField]
        private InputField textChat;
        
        public int maxMessage = 25;
        public GameObject chatPanel, textObject;
        public GameObject showUsername;
        public Button sendButton;
        public Text textSead;
        public Text textRead;
        public string textMessage;
        
        public string usernameText;
        
        
        private WebSocket websocket;
        
        
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
                            SeadMessageToChat("<color=red>Please input letters!!</color>");
                            Debug.Log("Space");
                        }
            }
            if (textMessage != null)
            {
                updatMessage();
            }
        }
        
        
        public void SearchButton()
        {
            //SeadMessageToChat($"<color=#C1F55B>{inputUser.text}</color> : {textChat.text}");
            
            
            /*if (websocket.ReadyState == WebSocketState.Open)
            {
                websocket.Send($"{usernameText} : {textChat.text}");
            }*/
            websocket.Send($"{inputUser.text} : {textChat.text}");
            textChat.text = $"";
            //Debug.Log("ok");
        }
        
        public void SeadMessageToChat(string text)
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
            
        }
        
        void updatMessage()
        {
            string[] temp = textMessage.Split(':');
            
            if (temp[0] == usernameText)
            {
                foreach (var i in textMessage)
                {
                    textSead.text += i ;
                }
            }
            else
            {
                foreach (var i in textMessage)
                {
                    textRead.text += i ;
                }
            }
            textMessage = null;
        }
        
        private void OnDestroy()
        {
            if (websocket != null)
            {
                websocket.Close();
            }
        }
        public void OnMessage(object semder, MessageEventArgs messageEventArgs)
        {
            Debug.Log("Receive msg : " + textMessage);
            textMessage = messageEventArgs.Data;
            SeadMessageToChat($"<color=#C1F55B>{textMessage}</color>");
            
            //SeadMessageToChat(messageEventArgs.Data);
        }
    }
    
    [Serializable]
    public class Message
    {
        public string text;
        public Text textObject;
    }
}
