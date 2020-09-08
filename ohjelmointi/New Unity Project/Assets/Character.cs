using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    //TODO:
    //XML parset
    //Character information => character UI
    //Character Advancement and changes to the character details
    //Generate character
    //Manipulation to automated character generation (randomizer for start)


  /*LEVELUP:
    advancement lapikaynti:
        katsotaan mita tapahtuu levelupissa
        kun tulee vastaan esimerkiksi Feat(General) => luodaan lista mahdollisista General Feateista
        jokaista General Feattia kohden tehdaan tarvittavat tarkistukset requirements elementin kautta
        taman tarkistuksen aikana kaytetaan RequirementParsea apuna, jonka kautta mennaan RequirementTranslate
        RequirementTranslate maarittaa mita tulkkia taytyy kayttaa, esimerkiksi StatCheck, ModCheck, FeatCheck tai SkillCheck
     */


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

    public static int GetStat(string statName)
    {
        int characterStat = 0;
        switch (statName)
        {
            case ("Strength"):
            case ("Str"):
                characterStat = stats[0];
            break;
            case ("Dexterity"):
            case ("Dex"):
                characterStat = stats[1];
                break;
            case ("Constitution"):
            case ("Con"):
                characterStat = stats[2];
                break;
            case ("Intelligence"):
            case ("Int"):
                characterStat = stats[3];
                break;
            case ("Wisdom"):
            case ("Wis"):
                characterStat = stats[4];
                break;
            case ("Charisma"):
            case ("Cha"):
                characterStat = stats[5];
                break;
            default:
                break;
        }
        return characterStat;
    }

    public static int GetMod(string statName)
    {
        int statMod = 0;
        switch (statName)
        {
            case ("Strength"):
            case ("Str"):
                statMod = stats[0];
                break;
            case ("Dexterity"):
            case ("Dex"):
                statMod = stats[1];
                break;
            case ("Constitution"):
            case ("Con"):
                statMod = stats[2];
                break;
            case ("Intelligence"):
            case ("Int"):
                statMod = stats[3];
                break;
            case ("Wisdom"):
            case ("Wis"):
                statMod = stats[4];
                break;
            case ("Charisma"):
            case ("Cha"):
                statMod = stats[5];
                break;
            default:
                break;
        }
        return statMod;
    }

    //tarkistetaan onko hahmolla tarvittava Feat (boolean return, string parametri?)
    //string parametri on Featin Nimi
    public static void CheckFeats()
    {
        //
    }

    //tarkistetaan onko hahmon skilli tarvittavalla tasolla (boolean return, string parametri?)
    //string parametri olisi skill||taso => joka menee parsen kautta takaisin aliohjelmaan
    //jossa se tarkastetaan tietokannan kautta
    public static void CheckSkills()
    {
        //
    }

    //muuta haluttua skillia korkeammaksi
    //parametrina kyseisen stringin nimi
    //untrained => trained => expert => master => legendary
    //saatetaan muuttaa ylikirjoittamiseksi yhden tason nousun sijaan, jolloin tarvitaan parametriksi myos mika on haluttu taso
    //dictionary parametrilla muutetaan taso
    public static void IncreaseSkill(string skillName)
    {
        //
    }

}
