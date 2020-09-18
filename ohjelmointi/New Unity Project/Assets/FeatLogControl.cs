using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeatLogControl : MonoBehaviour
{
    [SerializeField]
    private GameObject featTemplate;

    private List<GameObject> featItems;

    private void Start()
    {
        featItems = new List<GameObject>();
    }

    public void LogText(List<string> newFeatString)
    {
        if (featItems.Count == 10)
        {
            GameObject tempFeat = featItems[0];
            Destroy(tempFeat.gameObject);
            featItems.Remove(tempFeat);
        }
        featTemplate = Resources.Load("Feat") as GameObject;
        GameObject newFeat = Instantiate(featTemplate) as GameObject;
        newFeat.SetActive(true);
        int i = 0;
        foreach (Transform child in newFeat.transform)
        {
            if (newFeatString[i].Contains("("))
            {
                string[] formatString = newFeatString[i].Split('(');
                child.GetComponent<TextMeshProUGUI>().text = formatString[0] + "\n(" + formatString[1];
            }
            else
            {
                child.GetComponent<TextMeshProUGUI>().text = newFeatString[i];
            }
            i++;
        }
        newFeat.transform.SetParent(GameObject.Find("FeatContent").transform);
        newFeat.gameObject.transform.localScale = new Vector3(1, 1, 1);

        featItems.Add(newFeat.gameObject);
    }
}
