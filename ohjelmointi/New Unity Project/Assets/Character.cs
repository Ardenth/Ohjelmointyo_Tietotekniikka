using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{


    public static int[] stats = new int[] { 10, 10, 10, 10, 10, 10 };
    public static int[] statsMod = new int[]
    { ((stats[0] - 10)/ 2),
      ((stats[1] - 10)/ 2),
      ((stats[2] - 10)/ 2),
      ((stats[3] - 10)/ 2),
      ((stats[4] - 10)/ 2),
      ((stats[5] - 10)/ 2) 
    };
    //TODO: lore maaritys
    //Skill luonti
    public static Dictionary<string,string> skills = new Dictionary<string, string>
    {
        {"Acrobatics","Untrained" },
        {"Arcana","Untrained" },
        {"Athletics","Untrained" },
        {"Crafting","Untrained" },
        {"Deception","Untrained" },
        {"Diplomacy","Untrained" },
        {"Intimidation","Untrained" },
        {"Lore","Untrained" },
        {"Medicine","Untrained" },
        {"Nature","Untrained" },
        {"Occultism","Untrained" },
        {"Religion","Untrained" },
        {"Society","Untrained" },
        {"Stealth","Untrained" },
        {"Survival","Untrained" },
        {"Thievery","Untrained" }
    };

    static Dictionary<string, string> feats = new Dictionary<string, string>
    {
        {"placeholder feat", "ancestry" }
    };

    public static int[] UpdateMods(int[] statArray)
    {
        int[] statsMod = new int[6];
        for (int i = 0; i < statArray.Length; i++)
        {
            statsMod[i] = ((statArray[i]-10)/2);
        }
        return statsMod;
    }
}
