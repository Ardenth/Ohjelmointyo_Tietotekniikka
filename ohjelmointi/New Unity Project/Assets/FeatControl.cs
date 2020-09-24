using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatControl : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Descriptions;

    /// <summary>
    /// Controls Feats' description to become a popup
    /// </summary>
    private void Start()
    {
        Descriptions = new List<Transform>();
        Transform parent = this.transform.parent;
        foreach (Transform child in parent)
        {
            if (child.name == "FeatPopUp")
            {
                Descriptions.Add(child);
                //determine the parent outside the mask area
                Transform thisParent = parent.parent.transform.parent.transform.parent;
                //move the feat description out of the mask area
                child.SetParent(thisParent);
                child.gameObject.SetActive(false);

            }
            else if (child.name == "Button")
            {
                Descriptions.Add(child);
            }
        }

    }

    /// <summary>
    /// Button for description popups
    /// </summary>
    public void DescriptionButtonPress()
    {
        
        foreach (var child in Descriptions)
        {

            if (child.name == "FeatPopUp")
            {
                //set true if not active
                if (child.gameObject.activeSelf != true)
                {
                    //isn't properly set, force a local position in view
                    child.localPosition = new Vector3(-40, -40, 0);
                    child.gameObject.SetActive(true);
                    Descriptions[0].transform.GetComponent<Image>().color = new Color32(250, 14, 14, 255);
                }
                //otherwise set inactive
                else
                {
                    child.gameObject.SetActive(false);
                    Descriptions[0].transform.GetComponent<Image>().color = new Color32(171, 14, 14, 255);
                }
            }
        }
    }
}
