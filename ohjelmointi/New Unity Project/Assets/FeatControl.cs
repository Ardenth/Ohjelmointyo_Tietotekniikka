using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatControl : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Descriptions;

    private void Start()
    {
        Descriptions = new List<Transform>();
        Transform parent = this.transform.parent;
        Debug.Log("PARENTS NAME IN START: "+parent.name);
        foreach (Transform child in parent)
        {
            Debug.Log(child.name);
            if (child.name == "FeatPopUp")
            {
                Debug.Log("FeatsPopUp FOUND");
                Descriptions.Add(child);
                //determine the parent outside the mask area
                Transform thisParent = parent.parent.transform.parent.transform.parent;
                //move the feat description out of the mask area
                child.SetParent(thisParent);
                child.position = new Vector3(320, -200, 0);
                child.gameObject.SetActive(false);

            }
        }

    }

    public void DescriptionButtonPress()
    {

        Debug.Log("Button was pressed!!");
        Debug.Log(Descriptions.Count);
        foreach (var child in Descriptions)
        {
            if (child.name == "FeatPopUp")
            {
                Debug.Log(child.GetSiblingIndex());
                Debug.Log("found description image");
                if (child.gameObject.activeSelf != true)
                {
                    Debug.Log("trying to set true");
                    child.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("trying to set false");
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
