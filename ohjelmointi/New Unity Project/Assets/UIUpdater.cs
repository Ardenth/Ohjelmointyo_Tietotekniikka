using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.Serialization;
using System.Linq;
using System;

public class UIUpdater : MonoBehaviour
{
    private GameObject statsTab;
    private GameObject frontPage;
    private GameObject[] tabObjects;
    [SerializeField]
    private FeatLogControl featControl;
    [SerializeField]
    private SkillLogControl skillControl;
    [SerializeField]
    private FrontLogControl infoControl;
    [SerializeField]
    private SpellLogControl spellControl;
    [SerializeField]
    private FeatureLogControl featureControl;
    Character baseline = new Character();


    // Start is called before the first frame update
    void Start()
    {
        //collect all tabs for use
        tabObjects = GameObject.FindGameObjectsWithTag("Tab");
        //force basic view
        ControlTabActivity(false);
    }


    /// <summary>
    /// Create a new random character
    /// </summary>
    public void GenerateNewCharacter()
    {
        //activate all tabs for editing
        ControlTabActivity(true);
        //find desired character level
        string levelInfo = GameObject.FindGameObjectWithTag("GenerateLevel").GetComponent<TextMeshProUGUI>().text;
        int levels;
        //deal with null case -- special because of how TextMeshProUGUI works
        if (levelInfo != "\u200B")
        {
            //remove the hidden character used in string
            string levelInfoClean = levelInfo.Replace("\u200B", "");
            levels = int.Parse(levelInfoClean);
        }
        else
        {
            levels = 1;
        }
        //reset character
        baseline = new Character();
        baseline.UpdateMods();
        //level up the character
        baseline.LevelUp(levels);
        //handle UI changes for the character
        UIFeatUpdate("FeatContent2");
        UISkillUpdate("SkillContent2");
        UIFeatureUpdate("FeatureContent2");
        UISpellUpdate("SpellContent2");
        UIModUpdate();
        UIFrontPageUpdate("CharacterInfo");
        //return to basic view
        ControlTabActivity(false);
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

    /// <summary>
    /// Controls tab's state to allow editing
    /// </summary>
    /// <param name="activityState">desired tab state</param>
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
        //activate all elements for editing
        ControlTabActivity(true);
        featControl = GameObject.Find(objectName).GetComponent<FeatLogControl>();
        GameObject featView = GameObject.Find(objectName);
        //get all character's feats
        List<Dictionary<string,string>> featsDictionary = baseline.GetFeats();
        List<string> featInfo = new List<string>();
        //get featLog parent for old popup's deletion
        Transform featLog = featView.transform.parent.transform.parent;

        //destroy all popups
        for (int i = 3; i < featLog.childCount; i++)
        {
            Transform currentChild = featLog.GetChild(i);
            GameObject.Destroy(currentChild.gameObject);
        }

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
            featInfo.Add(feat["currentLevel"]);
            featInfo.Add(feat["type"]);
            featInfo.Add(feat["description"]);
            featControl.LogFeatText(featInfo, objectName);
        }
        //return to basic state
        ControlTabActivity(false);
    }


    /// <summary>
    /// Update UI spell section -- Another controller for future modification. Currently could use FeatLogControl, but created paths for UI for future modification when XML gets implemented for spells
    /// </summary>
    /// <param name="objectName">Parent object to hold all feat's information</param>
    void UISpellUpdate(string objectName)
    {
        //activate all elements for editing
        ControlTabActivity(true);
        spellControl = GameObject.Find(objectName).GetComponent<SpellLogControl>();
        GameObject SpellControl = GameObject.Find(objectName);
        //get all character's feats
        List<Dictionary<string, string>> featsDictionaryFilter = baseline.GetFeats();
        List<Dictionary<string, string>> featsDictionary = new List<Dictionary<string, string>>();
        //control the dictionary to only include spellcasting specific information
        foreach (var feat in featsDictionaryFilter)
        {
            if (feat["name"].Contains("Spell"))
            {
                featsDictionary.Add(feat);
            }
        }
        List<string> featInfo = new List<string>();
        //Removes all current children from the object
        foreach (Transform child in SpellControl.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //Adds all character's feats for the object
        foreach (var feat in featsDictionary)
        {
            //if the feat dictiary is empty (if the character is not a caster)
            if (featsDictionary.Count == 0)
            {
                break;
            }
            //clear previous information for use before creating a list for the Log to use
            featInfo.Clear();
            featInfo.Add(feat["name"]);
            featInfo.Add(feat["currentLevel"]);
            featInfo.Add(feat["type"]);
            featInfo.Add(feat["description"]);
            spellControl.LogSpellText(featInfo, objectName);
        }
        //return to basic view
        ControlTabActivity(false);
    }

    /// <summary>
    /// Update UI skill section
    /// </summary>
    /// <param name="objectName">Parent object to hold all skill's information</param>
    void UISkillUpdate(string objectName)
    {
        //activate all tabs for editing
        ControlTabActivity(true);
        skillControl = GameObject.Find(objectName).GetComponent<SkillLogControl>();
        GameObject SkillContent = GameObject.Find(objectName);

        //Removes all current children from the object
        foreach (Transform child in SkillContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Dictionary<string, string> skillsDictionary = baseline.GetSkills();
        //Adds all skills as children for the object
        foreach (KeyValuePair<string, string> item in skillsDictionary)
        {
            skillControl.LogSkillText(item, objectName);
        }
        //return to basic state
        ControlTabActivity(false);
    }

    /// <summary>
    /// Update UI character information section
    /// </summary>
    /// <param name="objectName">Parent object for the character info</param>
    void UIFrontPageUpdate(string objectName)
    {
        //activate all tabs for editing
        ControlTabActivity(true);
        infoControl = GameObject.Find(objectName).GetComponent<FrontLogControl>();
        //add character info
        List<string> characterInfo = new List<string>();
        characterInfo.Add(baseline.characterLevel.ToString());
        characterInfo.Add(baseline.characterAncestry);
        //if the background's name has a number, remove it before adding
        if (baseline.characterBackground["bckgrName"].Any(char.IsDigit))
        {
            string characterBackground = baseline.characterBackground["bckgrName"];
            characterInfo.Add(characterBackground.Remove(characterBackground.Length - 1, 1));
        }
        else
        {
            characterInfo.Add(baseline.characterBackground["bckgrName"]);
        }
        characterInfo.Add(baseline.CapitalizeFirstChar(baseline.characterClass));
        characterInfo.Add(baseline.characterFeatures["primaryStat"]);
        characterInfo.Add(baseline.classInitialFeat);
        //input information to UI
        infoControl.LogFrontText(characterInfo, objectName);
        //return to basic state
        ControlTabActivity(false);
    }


    /// <summary>
    /// Update UI Feature information section
    /// </summary>
    /// <param name="objectName">Parent object to hold all skill's information</param>
    void UIFeatureUpdate(string objectName)
    {
        //activate all tabs for editing
        ControlTabActivity(true);
        featureControl = GameObject.Find(objectName).GetComponent<FeatureLogControl>();
        GameObject FeatureContent = GameObject.Find(objectName);

        //Removes all current children from the object
        foreach (Transform child in FeatureContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Dictionary<string, string> featuresDictionary = baseline.GetFeatures();
        //Adds all skills as children for the object
        foreach (KeyValuePair<string, string> item in featuresDictionary)
        {
            featureControl.LogFeatureText(item, objectName);
        }
        //return to basic state
        ControlTabActivity(false);
    }


}
