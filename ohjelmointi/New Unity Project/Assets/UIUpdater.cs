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
    public string testing = "hellooo||hellomydude";


    // Start is called before the first frame update
    void Start()
    {
        //Character baselineChar = new Character();
        mods = Character.UpdateMods(hello);
        //textMesH = GetComponent<TextMeshProUGUI>();
        strengthchar = GameObject.Find("StrengthChar").GetComponent<TextMeshProUGUI>();
        dexteritychar = GameObject.Find("DexterityChar").GetComponent<TextMeshProUGUI>();
        constitutionchar = GameObject.Find("ConstitutionChar").GetComponent<TextMeshProUGUI>();
        intelligencechar = GameObject.Find("IntelligenceChar").GetComponent<TextMeshProUGUI>();
        wisdomchar = GameObject.Find("WisdomChar").GetComponent<TextMeshProUGUI>();
        charismachar = GameObject.Find("CharismaChar").GetComponent<TextMeshProUGUI>();
        //testing requirementparse
        //string[] testsplit = Character.RequirementParse(testing);

    }

    //Update character sheet's statistics and modifiers based on the pre-determined character's information
    void UIModUpdate()
    {
        strengthchar.text = "Strength: " + hello[0].ToString() + "\n Mod: +" + mods[0].ToString();
        dexteritychar.text = "Dexterity: " + hello[1].ToString() + "\n Mod: +" + mods[1].ToString();
        constitutionchar.text = "Constitution: " + hello[2].ToString() + "\n Mod: +" + mods[2].ToString();
        intelligencechar.text = "Intelligence: " + hello[3].ToString() + "\n Mod: +" + mods[3].ToString();
        wisdomchar.text = "Wisdom: " + hello[4].ToString() + "\n Mod: +" + mods[4].ToString();
        charismachar.text = "Charisma: " + hello[5].ToString() + "\n Mod: +" + mods[5].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UIModUpdate();
        //
        //if (gameObject.name == "StrengthChar (1)")
        //{
        //    textMesH.text = "Strength: " + Character.stats[0].ToString() + "\n Mod: +" + (Character.statsMod[0]+2).ToString();
        //}
    }
}
