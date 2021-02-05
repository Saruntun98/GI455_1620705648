using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHideShow : MonoBehaviour
{

    public GameObject objectNode;
    
    public void Hide()
    {

        if (objectNode != null)
        {
            objectNode.SetActive(false);
        }
    }

    public void Show()
    {
        if (objectNode != null)
        {
            objectNode.SetActive(true);
        }
    }
}

