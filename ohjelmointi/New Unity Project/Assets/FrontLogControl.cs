using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrontLogControl : MonoBehaviour
{
    public void LogFrontText(List<string> newFrontStrings, string parentName)
    {
        //parentName == characterInfo -- currently
        GameObject currentParent = GameObject.Find(parentName);

        int i = 0;
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
