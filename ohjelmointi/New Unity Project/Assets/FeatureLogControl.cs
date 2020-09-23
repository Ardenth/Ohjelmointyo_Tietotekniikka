using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeatureLogControl : MonoBehaviour
{
    [SerializeField]
    private GameObject featureTemplate;

    private List<GameObject> featureItems;

    /// <summary>
    /// Logs the Key-Value -pairs with given values
    /// </summary>
    /// <param name="newFeaturePair">Key pairs for features. Key|Value</param>
    /// <param name="parentName">parent object's name</param>
    public void LogFeatureText(KeyValuePair<string, string> newFeaturePair, string parentName)
    {
        featureItems = new List<GameObject>();
        //Skill template (name + value)
        featureTemplate = Resources.Load("Skill") as GameObject;
        //create a newFeature GameObject into game
        GameObject newFeature = Instantiate(featureTemplate) as GameObject;
        newFeature.SetActive(true);
        int i = 0;
        //set values for the newFeature GameObject
        foreach (Transform child in newFeature.transform)
        {
            if (i == 0)
            {
                child.GetComponent<TextMeshProUGUI>().text = newFeaturePair.Key;
            }
            else
            {
                //do not allow empty fields, give a value
                if (newFeaturePair.Value == "")
                {
                    child.GetComponent<TextMeshProUGUI>().text = "none";
                }
                else
                {
                    child.GetComponent<TextMeshProUGUI>().text = newFeaturePair.Value;
                }
            }
            i++;
        }
        //Assign parent for UI format
        newFeature.transform.SetParent(GameObject.Find(parentName).transform);
        //Fix GameObject scaling which happens after setting parent
        newFeature.gameObject.transform.localScale = new Vector3(1, 1, 1);
        //Add to current list of Objects
        featureItems.Add(newFeature.gameObject);
    }
}