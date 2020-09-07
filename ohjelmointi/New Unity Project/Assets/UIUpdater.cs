using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;

public class UIUpdater : MonoBehaviour
{
    private TextMeshProUGUI strengthchar;
    private TextMeshProUGUI constitutionchar;
    public static int[] mods = new int[6];
    public static int[] hello = new int[] { 12, 14, 16, 18, 19, 12 };

    // Start is called before the first frame update
    void Start()
    {
        Character baselineChar = new Character();
        mods = Character.UpdateMods(hello);
        //textMesH = GetComponent<TextMeshProUGUI>();
        constitutionchar = GameObject.Find("ConstitutionChar").GetComponent<TextMeshProUGUI>();
        strengthchar = GameObject.Find("StrengthChar").GetComponent<TextMeshProUGUI>();

    }



    //find how to edit specific TMPro - found
    // Update is called once per frame
    void Update()
    {
        strengthchar.text = "Strength: " + hello[0].ToString() + "\n Mod: +" + mods[0].ToString();
        constitutionchar.text = "Constitution: " + hello[4].ToString() + "\n Mod: +" + mods[4].ToString();
        //
        //if (gameObject.name == "StrengthChar (1)")
        //{
        //    textMesH.text = "Strength: " + Character.stats[0].ToString() + "\n Mod: +" + (Character.statsMod[0]+2).ToString();
        //}
    }
}
