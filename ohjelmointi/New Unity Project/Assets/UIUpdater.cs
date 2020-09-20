using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.Serialization;

public class UIUpdater : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI strengthchar;
    private TextMeshProUGUI constitutionchar;
    private TextMeshProUGUI dexteritychar;
    private TextMeshProUGUI intelligencechar;
    private TextMeshProUGUI wisdomchar;
    private TextMeshProUGUI charismachar;
    [SerializeField]
    private FeatLogControl featControl;
    Character baseline = new Character();


    // Start is called before the first frame update
    void Start()
    {
        baseline.UpdateMods();
        baseline.LevelUp(20);
        UIFeatUpdate("FeatContent");

        //testing requirementparse
    }

    /// <summary>
    /// Update character sheet's statistics and modifiers based on the character's information
    /// </summary>
    void UIModUpdate()
    {
        strengthchar = GameObject.Find("StrengthChar").GetComponent<TextMeshProUGUI>();
        dexteritychar = GameObject.Find("DexterityChar").GetComponent<TextMeshProUGUI>();
        constitutionchar = GameObject.Find("ConstitutionChar").GetComponent<TextMeshProUGUI>();
        intelligencechar = GameObject.Find("IntelligenceChar").GetComponent<TextMeshProUGUI>();
        wisdomchar = GameObject.Find("WisdomChar").GetComponent<TextMeshProUGUI>();
        charismachar = GameObject.Find("CharismaChar").GetComponent<TextMeshProUGUI>();

        strengthchar.text = "Strength: " + baseline.GetStat("Strength").ToString() + "\n Mod: +" + baseline.GetMod("Strength").ToString();
        dexteritychar.text = "Dexterity: " + baseline.GetStat("Dexterity").ToString() + "\n Mod: +" + baseline.GetMod("Dexterity").ToString();
        constitutionchar.text = "Constitution: " + baseline.GetStat("Constitution").ToString() + "\n Mod: +" + baseline.GetMod("Constitution").ToString();
        intelligencechar.text = "Intelligence: " + baseline.GetStat("Intelligence").ToString() + "\n Mod: +" + baseline.GetMod("Intelligence").ToString();
        wisdomchar.text = "Wisdom: " + baseline.GetStat("Wisdom").ToString() + "\n Mod: +" + baseline.GetMod("Wisdom").ToString();
        charismachar.text = "Charisma: " + baseline.GetStat("Charisma").ToString() + "\n Mod: +" + baseline.GetMod("Charisma").ToString();

    }


    /// <summary>
    /// Updates Feats on FeatContent of first page
    /// </summary>
    void UIFeatUpdate(string objectName)
    {
        featControl = GameObject.Find(objectName).GetComponent<FeatLogControl>();
        List<Dictionary<string,string>> featsDictionary = baseline.GetFeats();
        List<string> featInfo = new List<string>();
        foreach (var feat in featsDictionary)
        {
            featInfo.Clear();
            featInfo.Add(feat["name"]);
            Debug.Log(feat["name"]);
            featInfo.Add(feat["description"]);
            Debug.Log(feat["description"]);
            featControl.LogText(featInfo);
        }
    }



    // Update is called once per frame
    void Update()
    {
        UIModUpdate(); //change it so only if something happens
    }
}
