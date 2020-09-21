using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.Serialization;
using System.Linq;

public class UIUpdater : MonoBehaviour
{
    private GameObject statsTab;
    private GameObject frontPage;
    private GameObject[] tabObjects;
    [SerializeField]
    private FeatLogControl featControl;
    [SerializeField]
    private SkillLogControl skillControl;
    Character baseline = new Character();


    // Start is called before the first frame update
    void Start()
    {
        tabObjects = GameObject.FindGameObjectsWithTag("Tab");
        baseline.UpdateMods();
        baseline.LevelUp(1);
        ControlTabActivity(false);
        UIFeatUpdate("FeatContent");
        UIFeatUpdate("FeatContent2");
        UISkillUpdate("SkillContent");
        UISkillUpdate("SkillContent2");
        UIModUpdate();

        //testing requirementparse
    }

    /// <summary>
    /// Update character sheet's statistics and modifiers based on the character's information
    /// </summary>
    void UIModUpdate()
    {
        statsTab = GameObject.Find("Stats_Tab");
        foreach  (Transform child in statsTab.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                string statName = child2.name.Remove(child2.name.Length - 4, 4);
                if (baseline.GetMod(statName)>=0)
                {
                    child2.GetComponent<TextMeshProUGUI>().text = statName + ": " + baseline.GetStat(statName).ToString() + "\n Mod: +" + baseline.GetMod(statName).ToString();
                }
                else
                {
                    child2.GetComponent<TextMeshProUGUI>().text = statName + ": " + baseline.GetStat(statName).ToString() + "\n Mod: " + baseline.GetMod(statName).ToString();
                }
            }
        }
    }

    void ControlTabActivity(bool activityState)
    {
        if (frontPage == null)
        {
            frontPage = GameObject.Find("Tab_View_Character");
        }
        foreach (GameObject go in tabObjects)
        {
            go.SetActive(activityState);
        }
        frontPage.SetActive(true);
    }

    /// <summary>
    /// Update UI feat section
    /// </summary>
    /// <param name="objectName">Parent object to hold all feat's information</param>
    void UIFeatUpdate(string objectName)
    {
        ControlTabActivity(true);
        featControl = GameObject.Find(objectName).GetComponent<FeatLogControl>();
        GameObject featView = GameObject.Find(objectName);
        List<Dictionary<string,string>> featsDictionary = baseline.GetFeats();
        List<string> featInfo = new List<string>();
        //Removes all current children from the object
        foreach (Transform child in featView.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //Adds all character's feats for the object
        foreach (var feat in featsDictionary)
        {
            featInfo.Clear();
            featInfo.Add(feat["name"]);
            featInfo.Add(feat["description"]);
            featControl.LogFeatText(featInfo, objectName);
        }
        //leaves only starting tab active
        if (objectName.Any(char.IsDigit))
        {
            GameObject changedObject = GameObject.Find(objectName);
            GameObject tabView = changedObject.transform.parent.parent.parent.gameObject;

            tabView.SetActive(false);
        }
        ControlTabActivity(false);
    }


    /// <summary>
    /// Update UI skill section
    /// </summary>
    /// <param name="objectName">Parent object to hold all skill's information</param>
    void UISkillUpdate(string objectName)
    {
        ControlTabActivity(true);
        skillControl = GameObject.Find(objectName).GetComponent<SkillLogControl>();
        GameObject skillView = GameObject.Find(objectName);

        //Removes all current children from the object
        foreach (Transform child in skillView.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Dictionary<string, string> skillsDictionary = baseline.GetSkills();
        //Adds all skills as children for the object
        foreach (KeyValuePair<string, string> item in skillsDictionary)
        {
            skillControl.LogSkillText(item, objectName);
        }
        //leaves only starting tab active
        if (objectName.Any(char.IsDigit))
        {
            GameObject changedObject = GameObject.Find(objectName);
            GameObject tabView = changedObject.transform.parent.parent.parent.gameObject;

            tabView.SetActive(false);
        }
        ControlTabActivity(false);
    }



    // Update is called once per frame
    void Update()
    {
    }
}
