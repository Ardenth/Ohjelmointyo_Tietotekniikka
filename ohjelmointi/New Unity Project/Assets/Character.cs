using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.CrashReporting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

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
    internal Dictionary<string, string> characterBackground;

    //consider creating stats and statsMod into dictionaries for easier management through Keys?
    // internal Dictionary<string, int> stats = new Dictionary<string, stat> { {"Strength", 10}, {"Dexterity", 10}, {"Constitution", 10}, {"Intelligence", 10}, {"Wisdom", 10}, {"Charisma", 10}};
    internal Dictionary<string, int> statsDic = new Dictionary<string, int> { { "Strength", 10 }, { "Dexterity", 10 }, { "Constitution", 10 }, { "Intelligence", 10 }, { "Wisdom", 10 }, { "Charisma", 10 } };
    internal Dictionary<string, int> statsModsDic = new Dictionary<string, int> { { "Strength", 10 }, { "Dexterity", 10 }, { "Constitution", 10 }, { "Intelligence", 10 }, { "Wisdom", 10 }, { "Charisma", 10 } };
    internal List<Dictionary<string, string>> classProgDic = new List<Dictionary<string, string>>();
    internal int levelUpHP;
    
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
        {"Lore", "Untrained" },
        {"Intimidation","Untrained" },
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

    //character's features: hp, size, speed, etc.
    internal Dictionary<string, string> characterFeatures = new Dictionary<string, string>()
    {
        {"hp", "0" },{"size", "" },{"speed", "" },
        {"fortitudeSave", "" },{"reflexSave", "" },{"willSave", "" },
        {"unarmed", "" },{"simple", "" },{"martial", "" },{"advanced", "" },{"attackSpecial", "" },                 //attackSpecial used as a definition for others, such as bombs or initial weapons
        {"unarmored", "" },{"light", "" },{"medium", "" },{"heavy", "" },
        {"classDC", "" },{"spellDC", "" },
        {"trait", "" }, {"primaryStat", "" },{"deity","" }, {"languages", ""}
    };

    //has to be outside of methods or will cause duplicate choices
    static readonly System.Random rand = new System.Random();


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
        Debug.Log("stat increased: " +stat);
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
    /// Find the given general feat from the dictionary list
    /// </summary>
    /// <param name="featName">Feat to search for</param>
    /// <returns>Found feat's information in dictionary format</returns>
    internal Dictionary<string, string> FindGeneralFeat(string featName)
    {
        Dictionary<string, string> foundFeat = new Dictionary<string, string>();
        foreach (var dic in ParseXML.generalFeatDic)
        {
            if (dic["name"] == featName)
            {
                foundFeat = dic;
            }
        }
        return foundFeat;
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
            if (skillName.Contains("Lore"))
            {
                string[] skillNameArr = skillName.Split('|');
                this.skills.Add(skillNameArr[0], skillNameArr[1]);
                Debug.Log(skillName + " was added");
            }
            else
            {
                string[] skillNameArr = skillName.Split('|');
                this.skills[skillNameArr[0]] = skillNameArr[1];
                //special case for initial feat - Medium armor proficiency increases at the same pace as Light armor proficiency
                if (skillNameArr[0] == "Light" && (this.CheckFeats("Warpriest") || this.CheckFeats("Ruffian")))
                {
                    this.characterFeatures["Medium"] = skillNameArr[1];
                }
            }
        }
        //if the string for the skill does not contain the value it will be raised one tier higher
        else if (skillsKeys.Contains(skillName))
        {
            foreach (var skill in skillsKeys)
            {

                //find the correct skill
                if (skill == skillName)
                {
                    for (int i = 0; i < skillTraining.Length; i++)
                    {
                        //find out the skill's tier
                        if (this.skills[skill] == skillTraining[i])
                        {
                            this.skills[skill] = skillTraining[i + 1];
                            break;
                        }
                    }
                }
            }
        }

        //keeping the Lore skill at the highest value of any specific lore skill for the purposes of feats (happens when any of the Lore skills is being changed or increased)
        if (skillName.Contains("Lore") && skillsKeys.Contains(skillName))
        {
            Debug.Log(skillName + "---- LORE WAS INCREASED IN THE LIST");
            List<int> loreValues = new List<int>();
            //go through all Lore -skills
            foreach (string key in skillsKeys)
            {
                if (key.Contains("Lore"))
                {
                    loreValues.Add(Array.IndexOf(skillTraining, this.skills[key]));
                }
            }
            this.skills["Lore"] = skillTraining[loreValues.Max()];
        }

        //for general/skill feat skill training (to increase a random untrained skill to trained)
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
            this.characterLevel++;
            this.RandomClass();
            this.FindProgression();
        }
        else
        {
            this.characterLevel++;
            this.FindProgression();
            int addedHP = levelUpHP + this.GetMod("Constitution") + int.Parse(this.characterFeatures["hp"]);
            this.characterFeatures["hp"] = addedHP.ToString();
            //increase hp per con??
        }



        /*Debug.Log("Owned feats at level: " + this.characterLevel);
        for (int i = 0; i < featsDic.Count; i++)
        {
            //Debug.Log(featsDic[i]["name"]);
        }
        */
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
        var classes = new List<string> { "alchemist", "barbarian", "bard", "champion", "cleric", "druid", "fighter", "monk", "ranger", "rogue", "sorcerer", "wizard" };
        int index = rand.Next(classes.Count);
        this.characterClass = classes[index];
        Debug.Log("character class: "+this.characterClass);

        //applies the progression dic that the character has for the character sheet
        FindProgressionDic();

        //apply initial feat for your class
        List<Dictionary<string, string>> InitialFeatDic = new List<Dictionary<string, string>>();
        Debug.Log("level when the progression dic is linked: "+classProgDic[0]["level"]);
        foreach (var item in ParseXML.classFeatDic)
        {
            //if the initial Feat for your class exists
            if (item["level"].ToLower() == "initial" && item["class"].Contains(CapitalizeFirstChar(this.characterClass)))
            {
                InitialFeatDic.Add(item);
            }

        }
        //Generate random Initial Feat
        index = rand.Next(InitialFeatDic.Count);
        if (InitialFeatDic.Count() > 0)
        {
            this.featsDic.Add(InitialFeatDic[index]);
            Debug.Log("INITIAL FEAT FOUND: " + InitialFeatDic[index]["name"]);
        }
        else
        {
            Debug.Log(this.characterClass + " does not have an initial feat");
        }


        //apply class proficiencies
        ApplyProficienciesEffects();
    }

    /// <summary>
    /// Applies the effects and bonuses you gain from your class proficiencies
    /// </summary>
    internal void ApplyProficienciesEffects()
    {
        List<Dictionary<string, string>> classProfs = FilterDictionary(ParseXML.proficiencyDic, "Proficiencies");
        Dictionary<string, string> profs = classProfs[0];
        List<string> keyList = new List<string>(profs.Keys);
        //all proficiencies except the filtering proficiency used to filter list variables based on class
        foreach (string proficiency in keyList.Skip(1))
        {
            if (profs[proficiency].Contains("none"))
            {
                continue;
            }
            else if (proficiency.Contains("hp"))
            {
                RandomAncestry();
                RandomBackground();
                string proficiencyConString = profs[proficiency].Replace("+Con","");
                this.levelUpHP = int.Parse(proficiencyConString);
                int proficiencyCon = levelUpHP + this.GetMod("Constitution");
                this.characterFeatures[proficiency] = proficiencyCon.ToString();
            }
            else if (proficiency.Contains("skill"))
            {
                this.IncreaseSkill(profs[proficiency]);
            }
            //handle adding languages and process the earlier added languages in ancestries to a more readable format
            else if (proficiency == "languages")
            {
                string[] langCountArr = profs[proficiency].Split('|');
                string langCount = langCountArr[0].Replace("+Int", "");
                int langsInt = int.Parse(langCount);


                //language list of the game to randomize values from
                List<string> languages = new List<string>{
                            "Draconic", "Dwarven", "Elven", "Gnomish", "Goblin", "Halfling", "Jotun", "Orcish", "Sylvan", "Undercommon",
                            "Abyssal", "Aklo", "Aquan", "Auran", "Celestial", "Gnoll", "Ignan", "Infernal", "Necril", "Shadowtongue", "Terran"};
                List<string> langListAdd = new List<string>();

                //randomize languages up to the intmodifier
                for (int i = 0; i < this.GetMod("Intelligence") + langsInt; i++)
                {
                    int index = rand.Next(languages.Count);
                    string languageToAdd = languages[index];
                    //check if the random language is already known and randomize new if it is
                    if (this.characterFeatures[proficiency].Contains(languageToAdd) || langListAdd.Contains(languageToAdd))
                    {
                        while (this.characterFeatures[proficiency].Contains(languageToAdd) || langListAdd.Contains(languageToAdd))
                        {
                            index = rand.Next(languages.Count);
                            languageToAdd = languages[index];      //randomize a new language
                        }
                    }
                    Debug.Log(languageToAdd);
                    langListAdd.Add(languageToAdd);
                }

                //create a single string from the randomized languages list
                string replacementString = string.Join(", ", langListAdd);

                //reformat ancestry's languages into a more readable form for the user
                string langsProcessed = this.characterFeatures[proficiency];
                langsProcessed = langsProcessed.Replace("/", ", ");
                langsProcessed = langsProcessed.Replace("intmod", replacementString);
                //reformat last ',' out if there is nothing to add

                //add the ancestry languages to the character's sheet
                this.characterFeatures[proficiency] = langsProcessed;
                Debug.Log(this.characterFeatures[proficiency]);

            }
            else
            {
                if (proficiency == "primaryStat")
                {
                    if (profs[proficiency].Contains('-') || this.characterClass == "rogue")
                    {
                        List<string> primaryStatList = profs[proficiency].Split('-').ToList();
                        //special cases for Rogue initial feats
                        if (this.CheckFeats("Scoundrel"))
                        {
                            primaryStatList.Add("Charisma");
                        }
                        else if (this.CheckFeats("Ruffian"))
                        {
                            primaryStatList.Add("Strength");
                            this.skills["Intimidation"] = "Trained";
                        }

                        int index = rand.Next(primaryStatList.Count);
                        UpdateCharStat(primaryStatList[index]);
                        this.characterFeatures[proficiency] = primaryStatList[index];
                        foreach (var item in primaryStatList)
                        {
                            Debug.Log("Primary stat choices: "+item);
                        }
                    }
                    else
                    {
                        UpdateCharStat(profs[proficiency]);
                        this.characterFeatures[proficiency] = profs[proficiency];
                    }
                }
            }

        }

        //apply 4 last free boosts
        List<string> keyStatList = new List<string>(statsDic.Keys);
        string boostSuggestion;
        int addCount = 0;
        while (addCount<4)
        {
            int index = rand.Next(keyStatList.Count);
            boostSuggestion = keyStatList[index];
            //if the primaryStat is not used yet, suggest it
            if (keyStatList.Contains(this.characterFeatures["primaryStat"]))
            {
                boostSuggestion = this.characterFeatures["primaryStat"];
            }
            //if the stat is an allowed choice by the game rules, use it
            if (statsDic[boostSuggestion] < 18)
            {
                UpdateCharStat(boostSuggestion);
                addCount++;
            }
            //remove suggested stat from list. If it was used, it can't be taken again. If it was not chosen, it is not an option.
            keyStatList.Remove(boostSuggestion);
        }
    }


    /// <summary>
    /// Defines the character's ancestry randomly                                                               ---- Lacks application of ancestry modifiers
    /// </summary>
    internal void RandomAncestry()
    {
        int index = rand.Next(ParseXML.ancestryDic.Count);
        Dictionary<string, string> ancestry = ParseXML.ancestryDic[index];

        this.characterAncestry = ancestry["ancName"];
        Debug.Log(this.characterAncestry);

        //apply ancestry effects
        ApplyAncestryEffects(ParseXML.ancestryDic[index]);
    }

    /// <summary>
    /// Applies effects and bonuses you gain from your ancestry
    /// </summary>
    /// <param name="ancestryInfo"></param>
    internal void ApplyAncestryEffects(Dictionary<string, string> ancestryInfo)
    {
        List<string> keyList = new List<string>(ancestryInfo.Keys);
        foreach (var key in keyList)
        {
            if (ancestryInfo[key] != "none")
            {
                Debug.Log("Ancestry effect found: " + key);
                switch (key)
                {
                    //incrcease character HP by the ancestry's given amount
                    case ("hp"):
                        
                        int newhp = (int.Parse(characterFeatures[key]) + int.Parse(ancestryInfo[key]));
                        characterFeatures[key] = newhp.ToString();
                        break;

                    //set the character size
                    case ("size"):
                        characterFeatures[key] = ancestryInfo[key];
                        break;

                    //set the character speed
                    case ("speed"):
                        characterFeatures[key] = ancestryInfo[key];
                        break;

                    //applies boosts to the character's sheet
                    case ("boost"):
                        List<string> boostList = ancestryInfo[key].Split('/').ToList();
                        List<string> addedFree = new List<string>();
                        foreach (string boost in boostList)
                        {
                            //free boost
                            if (boost == "Free")
                            {
                                string randomKey = "Free";
                                while (boostList.Contains(randomKey) || randomKey == "Free" || addedFree.Contains(randomKey))
                                {
                                    if (rand.Next(0, 2) == 0)
                                    {
                                        randomKey = this.characterFeatures["primaryStat"];
                                    }
                                    else
                                    {
                                        List<string> statList = new List<string>(statsDic.Keys);
                                        randomKey = statList[rand.Next(statList.Count)];
                                    }
                                    Debug.Log("attempted random key -------- "+randomKey);
                                }
                                UpdateCharStat(randomKey);
                                addedFree.Add(randomKey);
                            }
                            //otherwise deal with the stat increase
                            else
                            {
                                UpdateCharStat(boost);
                            }
                        }
                        break;

                    //applies stat flaw to the character's sheet
                    case ("flaw"):
                        this.statsDic[ancestryInfo[key]] -= 2;
                        UpdateMods();
                        break;

                    //applies languages from the ancestry to the character
                    case ("languages"):
                     
                        //add the ancestry languages to the character's sheet
                        this.characterFeatures[key] = ancestryInfo[key];
                        break;

                    //sets the ancestries traits to the character
                    case ("trait"):
                        this.characterFeatures[key] = ancestryInfo[key].Replace("/", ", ");
                        break;

                    //adds the character feature the character has to the feat list. These are requirements for some feats but are only available through ancestries
                    case ("special"):
                        if (ancestryInfo[key] == "none")
                        {
                            break;
                        }
                        Dictionary<string, string> featToAdd = new Dictionary<string, string>();
                        switch (ancestryInfo[key])
                        {
                            case ("Darkvision"):
                                featToAdd.Add("name", ancestryInfo[key]);
                                featToAdd.Add("description", "You can see in darkness and dim light just as well as you can see in bright light, though your vision in darkness is in black and white.");
                                break;
                            case ("LowLight Vision"):
                                featToAdd.Add("name", ancestryInfo[key]);
                                featToAdd.Add("description", "You can see in dim light as though it were bright light, so you ignore the concealed condition due to dim light.");
                                break;
                            case ("Keen Eyes"):
                                featToAdd.Add("name", ancestryInfo[key]);
                                featToAdd.Add("description", 
                                    "You gain a +2 circumstance bonus when using the Seek action to find hidden or undetected creatures within 30 feet of you." +
                                    " Reduce the DC of the flat check to 3 for a concealed target or 9 for a hidden one.");
                                break;
                            default:
                                break;
                        }
                        this.featsDic.Add(featToAdd);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Debug.Log(ancestryInfo["ancName"] + " was empty for the key: " + key);
            }
        }
    }


    /// <summary>
    /// Defines the character's background                                                                      ---- UNTESTED
    /// </summary>
    internal void RandomBackground()
    {
        int index = rand.Next(ParseXML.backgroundDic.Count);
        Dictionary<string, string> background = ParseXML.backgroundDic[index];

        this.characterBackground = background;

        //apply background effects
        this.ApplyBackgroundEffects(background);
    }

    /// <summary>
    /// Applies effects and bonuses you gain from the background in the parameter to the character sheet
    /// </summary>
    /// <param name="backgroundInfo">Background which benefits will be applied</param>
    internal void ApplyBackgroundEffects(Dictionary<string, string> backgroundInfo)
    {
        List<string> keyList = new List<string>(backgroundInfo.Keys);
        foreach (var key in keyList)
        {
            if (backgroundInfo[key] != "none" && backgroundInfo[key] != "placeholder")
            {
                Debug.Log("Background effect found: " + key + "--- it was: "+backgroundInfo[key]);
                switch (key)
                {
                    //applies boosts to the character information
                    case ("boost"):
                        List<string> boostList = backgroundInfo[key].Split('/').ToList();
                        foreach (string boost in boostList)
                        {
                            //free boost
                            if (boost == "Free")
                            {
                                List<string> statsList = new List<string>(statsDic.Keys);
                                string randomKey;
                                if (rand.Next(0, 2) == 0)
                                {
                                    randomKey = this.characterFeatures["primaryStat"];
                                }
                                else
                                {
                                    randomKey = statsList[rand.Next(statsList.Count)];
                                }
                                UpdateCharStat(randomKey);
                            }
                            //otherwise deal with the OR statement
                            else
                            {
                                List<string> boostOrList = boost.Split('-').ToList();
                                int index = rand.Next(boostOrList.Count);
                                UpdateCharStat(boostOrList[index]);
                            }
                        }
                        break;
                    //applies stat flaw to the character information
                    case ("flaw"):
                        this.statsDic[backgroundInfo[key]] -= 2;
                        UpdateMods();
                        break;
                    //increases the specified skill value to the given value. Adds the new lore to the dictionary
                    case ("skill"):
                        List<string> skillList = backgroundInfo[key].Split('/').ToList();
                        foreach (string adv in skillList)
                        {
                            //if there is an OR choice in the advancement
                            if (adv.Contains('-'))
                            {
                                string[] advOrArr = adv.Split('-');
                                int index = rand.Next(0, advOrArr.Length);
                                this.IncreaseSkill(advOrArr[index]);

                            }
                            else
                            {
                                this.IncreaseSkill(adv);
                            }
                        }
                        break;
                    //adds the free random feat the character gets
                    case ("feat"):
                        string[] featSplit = backgroundInfo[key].Split('(');
                        Dictionary<string, string> featToAdd = FindGeneralFeat(featSplit[0]);
                        /*
                         *  if featToAdd["name"] == Assurance
                         *  => Metodi Assurance lisäämiselle? (TrainedSkill) sattumanvaraisesti? - ei kuulu tässä vaiheessa
                         */
                        featToAdd["name"] = backgroundInfo[key];
                        featsDic.Add(featToAdd);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log(backgroundInfo["bckgrName"]+" was empty for the key: " + key);
            }
        }
    }


    /// <summary>
    /// Applies the effect of the advancement to this character                                                 (else statement? needed?)
    /// </summary>
    /// <param name="advancement">dictionary which contains the information of the advancement</param>
    internal void ApplyAdvancementEffect(Dictionary<string,string> advancement)
    {
        string effect = advancement["effect"];
        if (effect == "placeholder" || effect == "none")
        {
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
                //increase skill
                if (skills.ContainsKey(effectArr[0]))
                {
                    this.IncreaseSkill(effect);
                }
                else if (characterFeatures.ContainsKey(effectArr[0].ToLower()))
                {
                    //increase speed
                    if (effectArr[0] == "speed")
                    {
                        int nextSpeed;
                        nextSpeed = int.Parse(characterFeatures[effectArr[0]]) + int.Parse(effectArr[1]);
                        characterFeatures[effectArr[0]] = nextSpeed.ToString();
                    }
                    if (effectArr[0] == "deity")
                    {
                        string[] deityArr = {"Abadar", "Asmodeus", "Calistria", "Cayden Cailean", "Desna", 
                                             "Erastil", "Gorum", "Gozreh", "Iomedae", "Irori", "Lamashtu", 
                                             "Nethys", "Norgorber", "Pharasma", "Rovagug", "Sarenrae", "Shelyn", 
                                             "Torag", "Urgathoa", "Zon Kuthon"};
                        int deityIndex = rand.Next(0, deityArr.Length);
                        characterFeatures[effectArr[0]] = deityArr[deityIndex];
                    }
                    else
                    {
                        //change character feature
                        characterFeatures[LowerFirstChar(effectArr[0])] = effectArr[1];
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
    /// Find the correct dictionary for class progression based on character's class
    /// </summary>
    internal void FindProgressionDic()
    {
        string charClass = this.characterClass;
        for (int i = 0; i < ParseXML.playableClasses.Length; i++)
        {
            if (charClass == ParseXML.playableClasses[i])
            {
                this.classProgDic = ParseXML.classProgDicArray[i];
                Debug.Log("dictionary for progressions found"); //remove later
            }
        }
    }


    /// <summary>
    /// Adds the specified advancement to the character
    /// consider adding to the dictionary current level and feat type, which would mean to define variable for the dic and then add current level and case type into it as last 2 Key variables (UI updating)
    /// </summary>
    /// <param name="advancementName">requested advancement</param>
    internal void AddAdvancement(string advancementName)
    {
        int index;
        List<Dictionary<string, string>> advancementFilteredDics = new List<Dictionary<string, string>>();
        // connecting feat with xml file and avoiding Initial Feat problems
        if (advancementName.Contains("Feat") && advancementName != "Feat(Initial)" && advancementName != "Initial Feat")
        {
            //can be made to have less repetition, not done to test specifically skill feat's functionality
            if (advancementName == "Feat(General)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.generalFeatDic, advancementName);
            }
            else if (advancementName == "Feat(Skill)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.skillFeatDic, advancementName);
            }
            else if (advancementName == "Feat(Ancestry)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.ancestryFeatDic, advancementName);
            }
            else if (advancementName == "Feat(Class)")
            {
                advancementFilteredDics = this.FilterDictionary(ParseXML.classFeatDic, advancementName);
                //Debug.Log("available class feats: " + advancementFilteredDics.Count);
            }

            Dictionary<string, string> featToAdd = WeightedFeatChoice(advancementFilteredDics);
            //currently character skills aren't being increased through the feat effects or advancement effects, thus the character does not ALWAYS have feat choices to choose from. Will be fixed as more gets implemented
            //Skill Training special case
            if (featToAdd["name"] == "Skill Training")
            {
                this.IncreaseSkill(featToAdd["name"]);
            }
            featToAdd.Add("currentLevel",this.characterLevel.ToString());
            featToAdd.Add("type", advancementName);
            this.featsDic.Add(featToAdd);

        }
        else   // else case for other type of advancements, such as boost, skill or overall class specific
        {
            if (advancementName == "Ability Boost")      //applies boost to a stat
            {
                List<string> keyList = new List<string>(statsDic.Keys);
                string randomKey;
                if (rand.Next(0, 2) == 0)
                {
                    randomKey = this.characterFeatures["primaryStat"];
                }
                else
                {
                    randomKey = keyList[rand.Next(keyList.Count)];
                }
                UpdateCharStat(randomKey);
            }
            //increases the tier of a skill by one
            else if (advancementName == "Skill Increase")
            {
                List<string> skillsKeysUnfiltered = new List<string>(skills.Keys);
                List<string> skillsKeys = new List<string>();

                //filter skills by the level requirement for skill increases
                foreach (string key in skillsKeysUnfiltered)
                {
                    if (this.characterLevel < 7)
                    {
                        if (skills[key] == "Untrained" || skills[key] == "Trained")
                        {
                            skillsKeys.Add(key);
                        }
                    }
                    else if (this.characterLevel < 15)
                    {
                        if (skills[key] == "Untrained" || skills[key] == "Trained" || skills[key] == "Expert")
                        {
                            skillsKeys.Add(key);
                        }
                    }
                    else
                    {
                        skillsKeys.Add(key);
                    }
                }
                index = rand.Next(skillsKeys.Count);
                //random skill
                string skillToAdd = skillsKeys[index];



                //only Lore(X) are allowed to be increased, Lore|X is kept as an tracking tool for feats
                if (skillToAdd == "Lore" || this.skills[skillToAdd] == "Legendary")
                {
                    while (skillToAdd == "Lore" || this.skills[skillToAdd] == "Legendary")
                    {
                        index = rand.Next(skillsKeys.Count);
                        //random skill
                        skillToAdd = skillsKeys[index];
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


    /// <summary>
    /// Controls the weighting of the feats to choose from -- Rough version (currently forces 50/50 -choice between feats through progression and all available feats)
    /// </summary>
    /// <param name="dicToFilter">List of feats available to choose from</param>
    /// <returns>Feat that was chosen through means of weighting</returns>
    internal Dictionary<string,string> WeightedFeatChoice(List<Dictionary<string,string>> dicToFilter)
    {
        List<Dictionary<string, string>> weightedFilteredDics = new List<Dictionary<string, string>>();
        Dictionary<string, string> featToAdd;
        int index;

        //create a second list of feats with prerequisite (skill being a certain tier or own a certain feat)
        foreach (var feat in dicToFilter)
        {
            if (feat["prerequisite"] != "none")
            {
                weightedFilteredDics.Add(feat);
            }
        }
        // 50/50 randomizer to determine which list to choose from; targeted prerequisite or all available feats
        if(rand.Next(0,2) == 0 && weightedFilteredDics.Count != 0)
        {
            index = rand.Next(weightedFilteredDics.Count);
            Debug.Log("available number of feats in the weighted dic: "+weightedFilteredDics.Count);
            Debug.Log("weighted system index number: "+index);
            featToAdd = weightedFilteredDics[index];
            Debug.Log("FEAT CHOSEN THROUGH WEIGHTED SYSTEM");
        }
        else
        {
            index = rand.Next(dicToFilter.Count);
            featToAdd = dicToFilter[index];
            Debug.Log("FEAT CHOSEN THROUGH NORMAL SYSTEM");
        }
        return featToAdd;
    }

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
                        //if level, prerequisites, class and no duplicates is fine
                        if (this.characterLevel >= featLevelPreq && this.ParseRequirement(item["prerequisite"]) && item["class"].Contains(CapitalizeFirstChar(this.characterClass)) && !this.CheckFeats(item["name"]))
                        {
                            filteredDic.Add(item);
                        }
                    }

                }
                break;
            case ("Proficiencies"):
                foreach (var item in dicToFilter)
                {
                    if (item["class"] == CapitalizeFirstChar(this.characterClass))
                    {
                        filteredDic.Add(item);
                    }
                }
                break;
            default:
                break;
        }

        return filteredDic;
    }
}
