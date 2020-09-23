using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeatLogControl : MonoBehaviour
{
    [SerializeField]
    private GameObject featTemplate;

    private List<GameObject> featItems;

    /// <summary>
    /// Logs new feat into parent object
    /// </summary>
    /// <param name="newFeatString">List of string values for feat</param>
    /// <param name="parentName">parent object's name</param>
    public void LogFeatText(List<string> newFeatString, string parentName)
    {
        featItems = new List<GameObject>();
        //use feat prefab
        featTemplate = Resources.Load("Feat") as GameObject;
        GameObject newFeat = Instantiate(featTemplate) as GameObject;
        newFeat.SetActive(true);
        int i = 0;
        foreach (Transform child in newFeat.transform)
        {
            //button does not have anything added
            if (child.name == "Button")
            {
                continue;
            }
            //description is further down in the child -tree
            else if (child.name == "FeatPopUp")
            {
                //assign description
                Transform childDescription = child.GetChild(0).GetChild(0);
                childDescription.GetComponent<TextMeshProUGUI>().text = newFeatString[i];
                //assign popup title
                Transform childIndicator = child.GetChild(0).GetChild(1);
                childIndicator.GetComponent<TextMeshProUGUI>().text = newFeatString[0];
            }
            else
            {
                child.GetComponent<TextMeshProUGUI>().text = newFeatString[i];
            }
            i++;
        }
        newFeat.transform.SetParent(GameObject.Find(parentName).transform);
        //force normal scale
        newFeat.gameObject.transform.localScale = new Vector3(1, 1, 1);

        featItems.Add(newFeat.gameObject);
    }
}
