using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrontLogControl : MonoBehaviour
{
    /// <summary>
    /// Logs frontpage information
    /// </summary>
    /// <param name="newFrontStrings">Information for frontpage</param>
    /// <param name="parentName">parent object for frontpage</param>
    public void LogFrontText(List<string> newFrontStrings, string parentName)
    {
        //set parentname as a GameObject
        GameObject currentParent = GameObject.Find(parentName);

        int i = 0;
        //set Information into frontpage
        foreach (Transform child in currentParent.transform)
        {
            if (child.CompareTag("PlayerInput"))
            {
                continue;
            }
            else
            {
                child.GetComponent<TextMeshProUGUI>().text = newFrontStrings[i];
                i++;
            }
        }
    }
}
