using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.Serialization;

public class UIUpdater : MonoBehaviour
{
    private TextMeshProUGUI strengthchar;
    private TextMeshProUGUI constitutionchar;
    private TextMeshProUGUI dexteritychar;
    private TextMeshProUGUI intelligencechar;
    private TextMeshProUGUI wisdomchar;
    private TextMeshProUGUI charismachar;
    private TextMeshProUGUI advancementName;
    private TextMeshProUGUI advancementDescr;
    Character baseline = new Character();


    // Start is called before the first frame update
    void Start()
    {
        baseline.UpdateMods();
        baseline.LevelUp(1);

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
    /// 
    /// </summary>
    void UIFeatUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UIModUpdate(); //change it so only if something happens
    }
}
