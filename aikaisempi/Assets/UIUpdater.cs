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
    public static int[] mods = new int[6];
    public static int[] hello = new int[] { 12, 14, 16, 18, 19, 12 };
    Character baseline = new Character();


    // Start is called before the first frame update
    void Start()
    {
        //Character baselineChar = new Character();
        baseline.SetCharStat(hello);
        baseline.UpdateMods();
        baseline.LevelUp(4);
        //testing requirementparse
        //string[] testsplit = Character.RequirementParse(testing);

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

        strengthchar.text = "Strength: " + baseline.GetStat("Str").ToString() + "\n Mod: +" + baseline.GetMod("Str").ToString();
        dexteritychar.text = "Dexterity: " + baseline.GetStat("Dex").ToString() + "\n Mod: +" + baseline.GetMod("Dex").ToString();
        constitutionchar.text = "Constitution: " + baseline.GetStat("Con").ToString() + "\n Mod: +" + baseline.GetMod("Con").ToString();
        intelligencechar.text = "Intelligence: " + baseline.GetStat("Int").ToString() + "\n Mod: +" + baseline.GetMod("Int").ToString();
        wisdomchar.text = "Wisdom: " + baseline.GetStat("Wis").ToString() + "\n Mod: +" + baseline.GetMod("Wis").ToString();
        charismachar.text = "Charisma: " + baseline.GetStat("Cha").ToString() + "\n Mod: +" + baseline.GetMod("Cha").ToString();

    }

    // Update is called once per frame
    void Update()
    {
        UIModUpdate(); //change it so only if something happens
    }
}
