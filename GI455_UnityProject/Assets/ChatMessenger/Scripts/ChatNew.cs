using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ChatNew : MonoBehaviour
{
    public GameObject rootConnection;
    public GameObject rootMessenger;

    public InputField inputText;
    public Text sendText;
    public Text receiveText;

    private WebSocket ws;

    private string tempMessengerString;
    
    // Start is called before the first frame update
    public void Start()
    {
        rootConnection.SetActive(true);
        rootMessenger.SetActive(false);
    }

    public void Connect()
    {
        string url = $"ws://127.0.0.1:5500/";
        
        ws = new WebSocket(url);
        //ws.OnMessage += OnMessage;
        ws.Connect();
        
        rootConnection.SetActive(false);
        rootMessenger.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
