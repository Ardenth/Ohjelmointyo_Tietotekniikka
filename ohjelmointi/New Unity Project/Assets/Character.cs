﻿using System;
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
    //character's skills
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
        {"Performance", "Untrained" },
        {"Perception", "Untrained" }
    };

    //character's known feats
    internal List<Dictionary<string, string>> featsDic = new List<Dictionary<string, string>>();

    //character's known languages
    internal Dictionary<string, string> languages = new Dictionary<string, string>();
    // example: LangName, trained, etc.

    //character's features: hp, size, speed, etc.
    internal Dictionary<string, string> characterFeatures = new Dictionary<string, string>()
    {
        {"hp", "" },{"size", "" },{"speed", "" },
        {"fortitudeSave", "" },{"reflexSave", "" },{"willSave", "" },
        {"unarmed", "" },{"simple", "" },{"martial", "" },{"advanced", "" },{"attackSpecial", "" },                 //attackSpecial used as a definition for others, such as bombs or initial weapons
        {"unarmored", "" },{"light", "" },{"medium", "" },{"heavy", "" },
        {"classDC", "" },{"spellDC", "" },
        {"trait", "" },{"special", "" }, {"focusPool", "" }    //special as feat (darkvision, lowlight vision, keen eyes), requirement for advances (characters can have both lowlight vision and darkvision)(XML?)
    };


    /// <summary>
    /// Updates character's statistics from a string, either by an increment of 2 or to a specific value
    /// </summary>
    /// <param name="stat"></param>
    internal void UpdateCharStat(string stat)
    {
        string[] statSplit = stat.Split('|');
        //for dictionary
        if (statSplit.Length < 2)                                   //parameter only has key for statistic, thus increase it by increment of 2
        {
            this.statsDic[stat] += 2;
        }
        else
        {
            this.statsDic[statSplit[0]] = int.Parse(statSplit[1]);  //parameter holds information to what value the statistic will be increased, thus increase statistic to given value
        }

        UpdateMods();
    }


    /// <summary>
    /// Sets the character's statistics with the parameter
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


    /// <summary>
    /// Capitalize first character for a string
    /// </summary>
    /// <param name="str">String to capitalize first character for</param>
    /// <returns>Capitalized parameter string</returns>
    internal string CapitalizeFirstChar(string str)
    {
        return str?.First().ToString().ToUpper() + str?.Substring(1).ToLower();
    }

    /// <summary>
    /// Lower first character for a string
    /// </summary>
    /// <param name="str">String to lower first character for</param>
    /// <returns>Lowered parameter string</returns>
    internal string LowerFirstChar(string str)
    {
        return str?.First().ToString().ToLower() + str?.Substring(1);
    }


    /// <summary>
    /// Parses bool value for the given string parameter for an advancement
    /// </summary>
    /// <param name="requirement">string for the advancement requirement</param>
    /// <returns>bool value of the string parameter</returns>
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


    /// <summary>
    /// Parses the value for the OR statement for the given string parameter
    /// </summary>
    /// <param name="requirement">advancements's string requirement which will be parsed for bool information</param>
    /// <returns>bool value of the string parameter</returns>
    internal bool ParseRequirementOr(string requirement)
    {
        List<string> requirementList = requirement.Split('-').ToList();
        List<bool> truthList = new List<bool>();
        foreach (string req in requirementList)
        {
            truthList.Add(CheckRequirement(req));
        }
        if (truthList.Contains(true))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Checks through parameter string if the requirement holds true for the character
    /// </summary>
    /// <param name="requirement">string requirement for the check, either feat name or skill|tier</param>
    /// <returns>bool value for character's qualifications</returns>
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


    /// <summary>
    /// Updates character's stat modifiers to match the statistics
    /// </summary>
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
        return this.statsDic[statName];
    }


    /// <summary>
    /// Used to get a specific statistic's modifier of the character
    /// </summary>
    /// <param name="statName">Which statistic's modifier is being requested</param>
    /// <returns>Returns the statistics's modifier specified in the given parameter</returns>
    internal int GetMod(string statName)
    {
        return this.statsModsDic[statName];
    }


    /// <summary>
    /// Used to get get character's feats as a list dictionary
    /// </summary>
    /// <returns>character's feats</returns>
    internal List<Dictionary<string, string>> GetFeats()
    {
        return this.featsDic;
    }


    /// <summary>
    /// Increases skill to the given value in the string parameter OR increases the skill by one tier
    /// </summary>
    /// <param name="skillName">Information in a string for which skill to increase and possibly to what tier</param>
    internal void IncreaseSkill(string skillName)
    {
        List<string> skillsKeys = new List<string>(this.skills.Keys);
        string[] skillTraining = { "Untrained", "Trained", "Expert", "Master", "Legendary" };
        //if the string for the skill contains the value it will be raised to
        if (skillName.Contains("|"))
        {
            string[] skillNameArr = skillName.Split('|');
            this.skills[skillNameArr[0]] = skillNameArr[1];
        }
        //if the string for the skill does not contain the value it will be raised one tier higher
        else if (skillsKeys.Contains(skillName))
        {
            foreach (var skill in skillsKeys)
            {
                if (skill == skillName)     //find the correct skill
                {
                    for (int i = 0; i < skillTraining.Length; i++)
                    {
                        if (this.skills[skill] == skillTraining[i])     //find out the skill's tier
                        {
                            this.skills[skill] = skillTraining[i + 1];
                            break;
                        }
                    }
                }
            }
        } 
        //for general/skill feat skill training
        else if (skillName == "Skill Training")
        {
            List<string> skillsKeysUntrained = new List<string>();
            //create list of untrained skills
            foreach (string key in skillsKeys)
            {
                //add key if untrained
                if (this.skills[key] == "Untrained")
                {
                    skillsKeysUntrained.Add(key);
                }
            }
            var random = new System.Random();
            int index = random.Next(skillsKeysUntrained.Count);
            string skillToAdd = skillsKeysUntrained[index];
            this.IncreaseSkill(skillToAdd);
        }
    }


    /// <summary>
    /// Levels character up
    /// </summary>
    /// <param name="levels">Desired character level</param>
    internal void LevelUp(int levels)
    {
        if (this.characterLevel == 0)
        {
            this.RandomClass();
            this.RandomAncestry();
        }
        this.characterLevel++;
        this.FindProgression();

        // add levelup hp increase from class


        //Debug.Log("Owned feats at level: " + this.characterLevel);
        for (int i = 0; i < featsDic.Count; i++)
        {
            //Debug.Log(featsDic[i]["name"]);
        }
        //recursive
        levels--;
        if (levels >= 1)
        {
            this.LevelUp(levels);
        }
    }


    /// <summary>
    /// Defines the class for the character randomly and assigns Initial Feat                                   ---- Lacks application of class modifiers
    /// </summary>
    internal void RandomClass()
    {
        var random = new System.Random();
        var classes = new List<string> { "alchemist", "barbarian", "bard", "champion", "cleric", "druid", "fighter", "monk", "ranger", "rogue", "sorcerer", "wizard" };
        int index = random.Next(classes.Count);
        this.characterClass = classes[index];
        Debug.Log("character class: "+this.characterClass);

        //apply initial feat for your class
        List<Dictionary<string, string>> InitialFeatDic = new List<Dictionary<string, string>>();
        foreach (var item in ParseXML.classFeatDic)
        {
            //if the initial Feat for your class exists
            if (item["level"].ToLower() == "initial" && item["class"].Contains(CapitalizeFirstChar(this.characterClass)))
            {
                InitialFeatDic.Add(item);
            }

        }
        index = random.Next(InitialFeatDic.Count);
        if (index > 0)
        {
            this.featsDic.Add(InitialFeatDic[index]);
            Debug.Log("INITIAL FEAT FOUND: " + InitialFeatDic[index]["name"]);
        }
        else
        {
            Debug.Log(this.characterClass + " does not have an initial feat");
        }

        //apply class proficiencies
        // METHOD
    }


    /// <summary>
    /// Defines the character's ancestry randomly                                                               ---- Lacks application of ancestry modifiers
    /// </summary>
    internal void RandomAncestry()
    {
        var random = new System.Random();
        int index = random.Next(ParseXML.ancestryDic.Count);
        Dictionary<string, string> ancestry = ParseXML.ancestryDic[index];

        this.characterAncestry = ancestry["ancName"];

        //apply ancestry effects
        // METHOD
    }


    /// <summary>
    /// Defines the character's background                                                                      ---- Lacks implementation
    /// </summary>
    internal void RandomBackground()
    {
        //apply background effects
        // METHOD
    }


    /// <summary>
    /// Applies the effect of the advancement to this character                                                 ---- UNTESTED ----                      ---------- UNFINISHED -----
    /// </summary>
    /// <param name="advancement">dictionary which contains the information of the advancement</param>
    internal void ApplyAdvancementEffect(Dictionary<string,string> advancement)
    {
        string effect = advancement["effect"];
        if (effect == "placeholder" || effect == "none")
        {
            Debug.Log("no effect for the feat");
            return;
        }


        List<string> effectList = effect.Split('/').ToList();
        // going through the effects while taking into account the possibility of AND statements
        foreach (string eff in effectList)
        {
            //applies effect for each | effect
            if (eff.Contains("|"))
            {
                string[] effectArr = eff.Split('|');
                if (skills.ContainsKey(effectArr[0]))
                {
                    this.IncreaseSkill(effect);
                    Debug.Log("skill: " + effectArr[0] + " is now: " + effectArr[1]);
                }
                else if (characterFeatures.ContainsKey(effectArr[0].ToLower()))
                {
                    if (effectArr[0] == "speed")
                    {
                        int nextSpeed;
                        nextSpeed = int.Parse(characterFeatures[effectArr[0]]) + int.Parse(effectArr[1]);
                        characterFeatures[effectArr[0]] = nextSpeed.ToString();
                        Debug.Log("increase speed");
                    }
                    else
                    {
                        characterFeatures[LowerFirstChar(effectArr[0])] = effectArr[1];
                        Debug.Log("character feature: " + effectArr[0]+" is now: "+ effectArr[1]);
                    }
                }
            }
            else
            {
                //if the effect is not holding | and isn't placeholder/none
            }
        }
    }


    /// <summary>
    /// Finds the progression table used for the character
    /// </summary>
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


    /// <summary>
    /// Adds the specified advancement to the character
    /// consider adding to the dictionary current level and feat type, which would mean to define variable for the dic and then add current level and case type into it as last 2 Key variables (UI updating)
    /// </summary>
    /// <param name="advancementName">requested advancement</param>
    internal void AddAdvancement(string advancementName)
    {
        var random = new System.Random();
        int index;
        // connecting feat with xml file and avoiding Initial Feat problems
        if (advancementName.Contains("Feat") && advancementName != "Feat(Initial)")
        {
            //can be made to have less repetition, not done to test specifically skill feat's functionality
            if (advancementName == "Feat(General)")
            {
                List<Dictionary<string, string>>  advancementFilteredDics = this.FilterDictionary(ParseXML.generalFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                //Skill Training special case
                if (advancementFilteredDics[index]["name"] == "Skill Training")
                {
                    this.IncreaseSkill(advancementFilteredDics[index]["name"]);          //is it a problem if not this.?
                }
                this.featsDic.Add(advancementFilteredDics[index]);
            }
            else if (advancementName == "Feat(Skill)")
            {
                List<Dictionary<string, string>>  advancementFilteredDics = this.FilterDictionary(ParseXML.skillFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);

                //currently character skills aren't being increased through the feat effects or advancement effects, thus the character does not ALWAYS have feat choices to choose from. Will be fixed as more gets implemented
                if (index > 0)
                {
                    //Skill Training special case
                    if (advancementFilteredDics[index]["name"] == "Skill Training")
                    {
                        this.IncreaseSkill(advancementFilteredDics[index]["name"]);          //is it a problem if not this.?
                    }
                    this.featsDic.Add(advancementFilteredDics[index]);
                }
            }
            else if (advancementName == "Feat(Ancestry)")
            {
                List<Dictionary<string, string>>  advancementFilteredDics = this.FilterDictionary(ParseXML.ancestryFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                this.featsDic.Add(advancementFilteredDics[index]);
            }
            else if (advancementName == "Feat(Class)")
            {
                List<Dictionary<string, string>>  advancementFilteredDics = this.FilterDictionary(ParseXML.classFeatDic, advancementName);
                index = random.Next(advancementFilteredDics.Count);
                this.featsDic.Add(advancementFilteredDics[index]);
            }
        }
        else   // else case for other type of advancements, such as boost, skill or overall class specific
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
                if (this.skills[skillToAdd] == "Legendary")  //check if the skill is at max tier
                {
                    while (this.skills[skillToAdd] == "Legendary")
                    {
                        index = random.Next(skillsKeys.Count); //not truly random (System.Random) is based on a time, thus will make loop go through couple of times
                        skillToAdd = skillsKeys[index];      //randomize a new skill if already at full tier
                    }
                }
                this.IncreaseSkill(skillToAdd);
            }
            //specified class advancement in the levelup gets added
            else
            {
                for (int i = 0; i < ParseXML.classAdvDic.Count; i++)
                {
                    if (ParseXML.classAdvDic[i]["name"] == advancementName)
                    {
                        this.featsDic.Add(ParseXML.classAdvDic[i]);
                        this.ApplyAdvancementEffect(ParseXML.classAdvDic[i]);
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

    /// <summary>
    /// Filters a new dictionary for use, based on the character information and the dictionary type (the dic type format allows to know what to filter)
    ///             ------------------- does not currently allow duplicates of any kind (even though some feats allow for multiple selection)
    /// </summary>
    /// <param name="dicToFilter">Dictionary to filter through</param>
    /// <param name="dicType">The type of dictionary the filterable dictionary is</param>
    /// <returns>Filtered dictionary with character's qualifications</returns>
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
