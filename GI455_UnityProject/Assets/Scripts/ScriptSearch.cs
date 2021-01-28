using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextSearch
{
    
    public class ScriptSearch : MonoBehaviour
    {
        [SerializeField] string textInput;
        [SerializeField] Text textShow;
        public List<string> textList;
        public GameObject inputField;
        public GameObject showSearch;
    
        void Start()
        {
            foreach (var textShowList in textList)
            {
                textShow.text += textShowList + "\n";;
            }
        }
    
        public void SearchButton()
        {
            if (textList.Contains(textInput = inputField.GetComponent<Text>().text))
            {
                showSearch.GetComponent<Text>().text = $"[<color=green>{textInput}</color>] is found in data.";
            }
            else
            {
                showSearch.GetComponent<Text>().text = $"[<color=red>{textInput}</color>] is not found in data.";
            }
        }
    }
}