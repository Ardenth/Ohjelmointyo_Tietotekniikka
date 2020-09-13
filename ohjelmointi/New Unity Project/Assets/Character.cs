using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.CrashReporting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Analytics;

class Character
{
    //TODO:
    //XML parset
    //Character information => character UI
    //Character Advancement and changes to the character details
    //Generate character
    //Manipulation to automated character generation (randomizer for start)

    //ONLY Advancements work, no feats?


    /*LEVELUP:
      advancement lapikaynti:
          katsotaan mita tapahtuu levelupissa
          kun tulee vastaan esimerkiksi Feat(General) => luodaan lista mahdollisista General Feateista
          jokaista General Feattia kohden tehdaan tarvittavat tarkistukset requirements elementin kautta
          taman tarkistuksen aikana kaytetaan RequirementParsea apuna, jonka kautta mennaan RequirementTranslate
          RequirementTranslate maarittaa mita tulkkia taytyy kayttaa, esimerkiksi StatCheck, ModCheck, FeatCheck tai SkillCheck
       */

    internal int characterLevel = 0;
    internal string characterClass;
    internal string characterAncestry;

    //consider creating stats and statsMod into dictionaries for easier management through Keys?
    // internal Dictionary<string, int> stats = new Dictionary<string, stat> { {"Strength", 10}, {"Dexterity", 10}, {"Constitution", 10}, {"Intelligence", 10}, {"Wisdom", 10}, {"Charisma", 10}};
    internal Dictionary<string, int> statsDic = new Dictionary<string, int> { { "Strength", 10 }, { "Dexterity", 10 }, { "Constitution", 10 }, { "Intelligence", 10 }, { "Wisdom", 10 }, { "Charisma", 10 } };
    internal Dictionary<string, int> statsModsDic = new Dictionary<string, int> { { "Strength", 10 }, { "Dexterity", 10 }, { "Constitution", 10 }, { "Intelligence", 10 }, { "Wisdom", 10 }, { "Charisma", 10 } };
    internal List<Dictionary<string, string>> classProgDic = new List<Dictionary<string, string>>();
    //TODO: lore maaritys
    //Skill luonti
    internal Dictionary<string, string> skills = new Dictionary<string, string>
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
        {"Thievery","Untrained" },

        {"Recall Knowledge", "Untrained" },
        {"Skill", "Untrained" },
        {"Skill Training", "Untrained" },
        {"Performance", "UnTrained" },
        {"Perception", "untrained" }
    };

    internal List<Dictionary<string, string>> featsDic = new List<Dictionary<string, string>>();

    internal List<Dictionary<string, string>> languageDic = new List<Dictionary<string, string>>();




    //determine how correct stat gets increased
    internal void UpdateCharStat(string stat)
    {
        string[] statSplit = stat.Split('|');
        //for dictionary
        if (statSplit.Length < 2)
        {
            this.statsDic[stat] += 2;
        }
        else
        {
            this.statsDic[statSplit[0]] = int.Parse(statSplit[1]);
        }

        UpdateMods();
    }


    /// <summary>
    /// Sets the statistics of the character to match the array
    /// </summary>
    /// <param name="statsWanted">Wanted character stats</param>
    internal void SetCharStat(int[] statsWanted)
    {
        int index = 0;
        List<string> keys = new List<string>(statsDic.Keys);
        foreach (string key in keys)
        {
            statsDic[key] = statsWanted[index];
            index++;
        }

        UpdateMods();
    }


    internal string CapitalizeFirstChar(string str)
    {
        return str?.First().ToString().ToUpper() + str?.Substring(1).ToLower();
    }


    internal bool ParseRequirement(string requirement)
    {
        List<string> requirementList = requirement.Split('/').ToList();
        List<bool> truthList = new List<bool>();
        foreach (string req in requirementList)
        {
            if (req.Contains('-'))
            {
                truthList.Add(ParseRequirementOr(req));
            }
            else
            {
                truthList.Add(CheckRequirement(req));
            }
        }
        if (!truthList.Contains(false))
        {
            return true;
        }

        return false;
    }


    internal bool ParseRequirementOr(string requirement)
    {
        List<string> requirementList = requirement.Split('-').ToList();
        List<bool> truthList = new List<bool>();
        foreach (string req in requirementList)
        {
            truthList.Add(CheckRequirement(req));
        }
        if (!truthList.Contains(false))
        {
            return true;
        }
        return false;
    }


    internal bool CheckRequirement(string requirement)
    {
        //requirement contains information in two pieces
        if (requirement.Contains('|'))
        {
            string[] requirementArr = requirement.Split('|');
            //requirement type: statistic
            if (this.statsDic.ContainsKey(requirementArr[0]))
            {
                if (this.statsDic[requirementArr[0]] >= int.Parse(requirementArr[1]))
                {
                    return true;
                }
            }
            //requirement type: skill
            else if (this.skills.ContainsKey(requirementArr[0]))
            {
                if (this.skills[requirementArr[0]] == requirementArr[1])
                {
                    return true;
                }
            }
        }
        else
        {
            if (requirement == "none")
            {
                return true;
            }
            else
            {
                //requirement type: feat
                return CheckFeats(requirement);
            }
        }

        return false;
    }


    /// <summary>
    /// Checks if the feat is found in the character's feat dictionary
    /// </summary>
    /// <param name="searchedFeat">the name of the Feat which is being searched</param>
    /// <returns>boolean value of whether the feat was found or not</returns>
    internal bool CheckFeats(string searchedFeat)
    {
        for (int i = 0; i < this.featsDic.Count; i++)
        {
            Dictionary<string, string> feat = this.featsDic[i];
            if (feat["name"] == searchedFeat)
            {
                return true;
            }
        }
        return false;
    }

    internal void UpdateMods()
    {
        int index = 0;
        List<string> keys = new List<string>(this.statsDic.Keys);
        foreach (string key in keys)
        {
            this.statsModsDic[key] = ((this.statsDic[key] - 10)/2);
            index++;
        }
    }


    /// <summary>
    /// Used to get a specific statistic of the character
    /// </summary>
    /// <param name="statName">Which statistic is being requested</param>
    /// <returns>Returns the statistic specified in the given parameter</returns>
    internal int GetStat(string statName)
    {
        return statsDic[statName];
    }


    /// <summary>
    /// Used to get a specific statistic's modifier of the character
    /// </summary>
    /// <param name="statName">Which statistic's modifier is being requested</param>
    /// <returns>Returns the statistics's modifier specified in the given parameter</returns>
    internal int GetMod(string statName)
    {
        return statsModsDic[statName];
    }


    internal List<Dictionary<string, string>> GetFeats()
    {
        return featsDic;
    }

    //TODO: CheckSkills vs Check Requirement
    //check skills tarkistaa omasta, check requirement paattaa meneeko check feat vai check skill => KORJAA REQUIREMENTS TARKISTUKSEEN


    //tarkistetaan onko hahmon skilli tarvittavalla tasolla (boolean return, string parametri?)
    //string parametri olisi skill||taso => joka menee parsen kautta takaisin aliohjelmaan
    //jossa se tarkastetaan tietokannan kautta


    /// <summary>
    /// Increases skill to the given value in the string parameter OR increases the skill by one tier
    /// </summary>
    /// <param name="skillName">Information in a string for which skill to increase and possibly to what tier</param>
    internal void IncreaseSkill(string skillName)
    {
        List<string> skillsKeys = new List<string>(skills.Keys);
        string[] skillTraining = { "Untrained", "Trained", "Expert", "Master", "Legendary" };
        if (skillName.Contains("|"))    //if the string for the skill contains the value it will be raised to
        {
            string[] skillNameArr = skillName.Split('|');
            this.skills[skillNameArr[0]] = skillNameArr[1];
        }
        else                            //if the string for the skill does not contain the value it will be raised one tier higher
        {
            for (int i = 0; i < skillsKeys.Count; i++)                          //find correct skill =>
            {
                if (skillsKeys[i] == skillName)                                 //if correct skill is found =>
                {
                    for (int j = 0; j < skillTraining.Length; j++)              //find correct skill's tier =>
                    {
                        if (this.skills[skillName] == skillTraining[j])         //if correct skill is found => increase it by one
                        {
                            this.skills[skillName] = skillTraining[j+1];
                            break;
                        }
                    }
                }
            }
        }
    }


    internal void LevelUp(int levels)
    {
        if (this.characterLevel == 0)
        {
            this.RandomClass();
            this.RandomAncestry();
        }
        this.characterLevel++;
        this.FindProgression();
        Debug.Log("Owned feats at level: "+ this.characterLevel);
        for (int i = 0; i < featsDic.Count; i++)
        {
            Debug.Log(featsDic[i]["name"]);
        }

        
            
            //recursive
        levels--;
        if (levels >= 1)
        {
            this.LevelUp(levels);
        }
    }


    internal void RandomClass()
    {
        var random = new System.Random();
        var classes = new List<string> { "alchemist", "barbarian", "bard", "champion", "cleric", "druid", "fighter", "monk", "ranger", "rogue", "sorcerer", "wizard" };
        int index = random.Next(classes.Count);
        this.characterClass = classes[index];
        Debug.Log("character class: "+this.characterClass);
    }


    internal void RandomAncestry()
    {
        var random = new System.Random();
        int index = random.Next(ParseXML.ancestryDic.Count);
        Dictionary<string, string> ancestry = ParseXML.ancestryDic[index];

        this.characterAncestry = ancestry["ancName"];
    }


    //saatetaan liittaa LevelUp (miksi etsia progression jos niita ei lisata?)
    internal void FindProgression()
    {
        int charLevel = this.characterLevel;
        string charClass = this.characterClass;
        if (this.classProgDic.Count == 0)
        {
            for (int i = 0; i < ParseXML.playableClasses.Length; i++)
            {
                if (charClass == ParseXML.playableClasses[i])
                {
                    this.classProgDic = ParseXML.classProgDicArray[i];
                    Debug.Log("dictionary for progressions found"); //remove later
                }
            }
        }

        Dictionary<string, string> characterProg = this.classProgDic[charLevel-1];
        Dictionary<string, string>.ValueCollection characterProgValues = characterProg.Values;
        foreach (string item in characterProgValues)
        {
            string advancement = item;
            if (advancement == "none")
            {
                break;
            }
            this.AddAdvancement(advancement);



        }
    }






    //UNDER HEAVY WORK







    /// <summary>
    /// Adds the specified advancement to the character
    /// consider adding to the dictionary current level and feat type, which would mean to define variable for the dic and then add current level and case type into it as last 2 Key variables
    /// </summary>
    /// <param name="advancementName">requested advancement</param>
    internal void AddAdvancement(string advancementName)
    {
        List<Dictionary<string, string>> advancementFilteredDics = new List<Dictionary<string, string>>();
        var random = new System.Random();
        int index;
        if (advancementName.Contains("Feat"))                               // if case for Feats, so effects can be applied specifically depending on the XML table
        {
            if (advancementName == "Feat(General)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.generalFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                this.featsDic.Add(advancementFilteredDics[index]);
            }
            else if (advancementName == "Feat(Skill)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.skillFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                this.featsDic.Add(advancementFilteredDics[index]);
            }
            else if (advancementName == "Feat(Ancestry)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.ancestryFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                this.featsDic.Add(advancementFilteredDics[index]);
            }
            else if (advancementName == "Feat(Class)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.classFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                this.featsDic.Add(advancementFilteredDics[index]);
            }
        }
        else                                                              // else case for other type of advancements, such as boost, skill or overall class specific
        {
            if (advancementName == "Ability Boost")      //applies boost to a stat
            {
                List<string> keyList = new List<string>(statsDic.Keys);
                System.Random rand = new System.Random();
                string randomKey = keyList[rand.Next(keyList.Count)];
                UpdateCharStat(randomKey);
            }
            else if (advancementName == "Skill Increase") //increases the tier of a skill by one
            {
                List<string> skillsKeys = new List<string>(skills.Keys);
                index = random.Next(skillsKeys.Count);
                string skillToAdd = skillsKeys[index];
                this.IncreaseSkill(skillToAdd);
                
            }
            else                                        //specified class advancement in the levelup gets added
            {
                for (int i = 0; i < ParseXML.classAdvDic.Count; i++)
                {
                    if (ParseXML.classAdvDic[i]["name"] == advancementName)
                    {
                        this.featsDic.Add(ParseXML.classAdvDic[i]);                 //feats or adv dic? -------- should be fine if only name and description wanted
                    }
                }
            }
        }
    }



    /*  things to remember:
     *  levels
     *  types: class/ancestry/skill(general)
     *  prerequisites: skills/feats
     *  no duplicate entries to take
     *  special case for initial feat, how to define?
     *  only feats!!!
     */
    /// <summary>                                                                                                                                                       ------------UNTESTED----------
    /// 
    /// Filters dictionaries based on the character information and the dictionary type (the dic type format allows to know what to filter) and returns them for further use
    /// 
    ///             ------------------- create adding initial feat (check if feats contain initial for ancestry or class) 
    ///             ------------------- does not currently allow duplicates of any kind (even though some feats allow for added benefits)
    /// </summary>
    /// <param name="dicToFilter"></param>
    /// <param name="dicType"></param>
    /// <returns></returns>
    internal List<Dictionary<string, string>> FilterDictionary(List<Dictionary<string,string>> dicToFilter, string dicType)
    {
        List<Dictionary<string, string>> filteredDic = new List<Dictionary<string, string>>();
        int featLevelPreq;
        //different cases for if you are adding in class/ancestry initial feat? Such as initialClass or initialAncestry?
        switch (dicType)
        {
            case ("Feat(Skill)"):
            case ("Feat(General)"):
                foreach (var item in dicToFilter)
                { 
                    //if prerequisite and no duplicates is fine //I spent hour trying to do something that I had made into method already...
                    if (this.ParseRequirement(item["prerequisite"]) && !this.CheckFeats(item["name"]))            
                    {
                        filteredDic.Add(item);
                    }
                }
                break;
            case ("Feat(Ancestry)"):
                foreach (var item in dicToFilter)
                {
                    if (item["level"].ToLower() != "initial")                     // parse level into int form while avoiding "initial" -string error
                    {
                        featLevelPreq = int.Parse(item["level"]);
                        //if level, prerequisites, ancestry and no duplicates is fine //I spent hour trying to do something that I had made into method already...
                        if (this.characterLevel >= featLevelPreq && this.ParseRequirement(item["prerequisite"]) && item["ancestry"].Contains(this.characterAncestry) && !this.CheckFeats(item["name"]))            
                        {
                            filteredDic.Add(item);
                        }
                    }
                }
                break;
            case ("Feat(Class)"):
                foreach (var item in dicToFilter)
                {
                    if (item["level"].ToLower() != "initial")                     // parse level into int form while avoiding "initial" -string error
                    {
                        featLevelPreq = int.Parse(item["level"]);
                        //if level, prerequisites, class and no duplicates is fine //I spent hour trying to do something that I had made into method already...
                        if (this.characterLevel >= featLevelPreq && this.ParseRequirement(item["prerequisite"]) && item["class"].Contains(CapitalizeFirstChar(this.characterClass)) && !this.CheckFeats(item["name"]))
                        {
                            filteredDic.Add(item);
                        }
                    }

                }
                break;
            default:
                break;
        }

        return filteredDic;
    }
}
