using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class TEST : MonoBehaviour
{
    private WebSocket websocket;
    [SerializeField] private string textInput;
    [SerializeField] private Text textShow;
    public List<string> textList;
    public GameObject inputField;
    public GameObject showSearch;
       
     // Start is called before the first frame update
     void Start()
     {
         websocket = new WebSocket("ws://127.0.0.1:5500/");
         websocket.Connect();

         websocket.OnMessage += OnMessage;
            
         websocket.Send("I'm join.");
         
         foreach (var textShowList in textList)
         {
             textShow.text += textShowList + "\n";;
         }
     }
   
     // Update is called once per frame
     void Update()
     {
         if (Input.GetKeyDown(KeyCode.Return))
         {

                 showSearch.GetComponent<Text>().text = $"s";
         }
     }

     private void OnDestroy()
     {
         if (websocket != null)
         {
             websocket.Close();
         }
     }

     public void SearchButton()
     {
         if (textList.Contains(textInput = inputField.GetComponent<Text>().text))
         {
             showSearch.GetComponent<Text>().text = $"[<color=green>{textInput}</color>] HI";
         }
         else
         {
             showSearch.GetComponent<Text>().text = $"[<color=red>{textInput}</color>] is not found in data.";
         }
     }
     public void OnMessage(object semder, MessageEventArgs messageEventArgs)
     {
         Debug.Log("Receive msg : " + messageEventArgs.Data);
     }
}

