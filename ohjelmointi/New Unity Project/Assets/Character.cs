using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{


    public static int[] stats = new int[] { 10, 10, 10, 10, 10, 10 };
    public static int[] statsMod = UpdateMods(stats);
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

    //determine how correct stat gets increased
    public static void UpdateCharStat(string stat)
    {
        string[] statSplit = SkillParse(stat);
        if (statSplit.Length < 2)
        {
            switch (statSplit[0])
            {
                case "Strength":
                    stats[0] = stats[0] + 2;
                    break;
                case "Dexterity":
                    stats[1] = stats[1] + 2;
                    break;
                case "Constitution":
                    stats[2] = stats[2] + 2;
                    break;
                case "Intelligence":
                    stats[3] = stats[3] + 2;
                    break;
                case "Wisdom":
                    stats[4] = stats[4] + 2;
                    break;
                case "Charisma":
                    stats[5] = stats[5] + 2;
                    break;
                default:
                    Debug.Log("no stat");
                    break;

            }
        }
        else
        {
            switch (statSplit[0])
            {
                case "Strength":
                    stats[0] = stats[0] + int.Parse(statSplit[1]);
                    break;
                case "Dexterity":
                    stats[1] = stats[1] + int.Parse(statSplit[1]);
                    break;
                case "Constitution":
                    stats[2] = stats[2] + int.Parse(statSplit[1]);
                    break;
                case "Intelligence":
                    stats[3] = stats[3] + int.Parse(statSplit[1]);
                    break;
                case "Wisdom":
                    stats[4] = stats[4] + int.Parse(statSplit[1]);
                    break;
                case "Charisma":
                    stats[5] = stats[5] + int.Parse(statSplit[1]);
                    break;
                default:
                    Debug.Log("no stat");
                    break;

            }
        }
    }

    //creates skills into understandable skillName|trainingLevel
    public static string[] SkillParse(string parse)
    {
        string[] newString = parse.Split('|');
        return newString;
    }

    public static string[] RequirementParse(string parse)
    {
        string[] newString = parse.Split(new[] { "||" }, StringSplitOptions.None);
        Debug.Log(newString[1]); //testing
        return newString;
    }

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
