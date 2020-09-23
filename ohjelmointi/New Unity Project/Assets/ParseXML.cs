using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ParseXML : MonoBehaviour
{
    public static List<Dictionary<string, string>>[] classProgDicArray = new List<Dictionary<string, string>>[12];
    public static string[] playableClasses = { "alchemist", "barbarian", "bard", "champion", "cleric", "druid", "fighter", "monk", "ranger", "rogue", "sorcerer", "wizard" };
    public static List<Dictionary<string, string>> generalFeatDic;
    public static List<Dictionary<string, string>> skillFeatDic;
    public static List<Dictionary<string, string>> classFeatDic;
    public static List<Dictionary<string, string>> ancestryFeatDic;
    public static List<Dictionary<string, string>> ancestryDic;
    public static List<Dictionary<string, string>> backgroundDic;
    public static List<Dictionary<string, string>> proficiencyDic;
    public static List<Dictionary<string, string>> classAdvDic;

    // Prepare dictionaries for character's
    void Start()
    {
        generalFeatDic = ParseGeneralFeat();
        skillFeatDic = ParseSkillFeat();
        classFeatDic = ParseClassFeat();
        ancestryFeatDic = ParseAncestryFeat();
        ancestryDic = ParseAncestry();
        backgroundDic = ParseBackground();
        proficiencyDic = ParseProficiency();
        classAdvDic = ParseAdvancement();

        //loop for class dictionaries
        for (int i = 0; i < classProgDicArray.Length; i++)
        {
            classProgDicArray[i] = ParseClassProgression(playableClasses[i]);
        }
    }

    /// <summary>
    ///  Parses general feat XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseGeneralFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_general");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("Feats").Elements("feat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "name", "level", "prerequisite", "description"};
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }


        return allTextDic;
    }

    /// <summary>
    ///  Parses skill feat XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseSkillFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_skill");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("Feats").Elements("feat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "name", "level", "prerequisite", "description"};
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }


        return allTextDic;
    }

    /// <summary>
    ///  Parses class feat XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseClassFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_class");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("ClassFeats").Elements("classFeat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "name", "class", "level", "prerequisite", "description"};
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }


        return allTextDic;
    }

    /// <summary>
    ///  Parses ancestry feat XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseAncestryFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_ancestry");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("AncestryFeats").Elements("ancestryFeat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "name", "ancestry", "level", "prerequisite", "description"};
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);
        }


        return allTextDic;
    }

    /// <summary>
    ///  Parses ancestries XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseAncestry()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("ancestry");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("Ancestry").Elements("race");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        {"ancName", "hp", "size", "speed", "boost", "flaw", "languages", "trait", "special" };
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }


        return allTextDic;
    }

    /// <summary>
    ///  Parses backgrounds XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseBackground()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("backgrounds");
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("Backgrounds").Elements("background");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        {"bckgrName", "boost", "skill", "feat", "description" };
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }


        return allTextDic;
    }

    /// <summary>
    ///  Parses advancements XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseAdvancement()
{
    TextAsset txtXmlAsset = Resources.Load<TextAsset>("class_advancement");
    var doc = XDocument.Parse(txtXmlAsset.text);

    var allDict = doc.Element("Advancements").Elements("advancement");
    List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
    string profInfo;
    XElement element;
    string[] elementNames = new string[]
    {"name", "class", "effect", "description" };
    foreach (var oneDict in allDict)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        for (int i = 0; i < elementNames.Length; i++)
        {
            element = oneDict.Element(elementNames[i]);
            profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
            dic.Add(elementNames[i], profInfo);
        }
        allTextDic.Add(dic);

    }


    return allTextDic;
}

    /// <summary>
    ///  Parses proficiencies XML -file to dictionary
    /// </summary>
    /// <returns>Parsed dictionary</returns>
    public List<Dictionary<string, string>> ParseProficiency()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("proficiencies");
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("Proficiencies").Elements("proficiency");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        {"class", "primaryStat", "hp", "fortitudeSave", "reflexSave", "willSave",
                "skill1", "skill2", "skill3", "skills", "languages",
                "unarmed", "simple", "martial", "advanced", "attackSpecial",
                "unarmored", "light", "medium", "heavy", "classDC", "spellDC"};
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }

        return allTextDic;
    }

    /// <summary>
    /// Parses class progression XML -file to dictionary
    /// </summary>
    /// <param name="className">class for XML -file specification</param>
    /// <returns>parsed class dictionary based on the parameter</returns>
    public List<Dictionary<string, string>> ParseClassProgression(string className)
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("class_progression_" + className);
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("Progressions").Elements("progression");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        {"level", "advancement1","advancement2","advancement3","advancement4","advancement5","advancement6","advancement7","advancement8","advancement9","advancement10"};
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < elementNames.Length; i++)
            {
                element = oneDict.Element(elementNames[i]);
                profInfo = element.ToString().Replace("<" + elementNames[i] + ">", "").Replace("</" + elementNames[i] + ">", "");
                dic.Add(elementNames[i], profInfo);
            }
            allTextDic.Add(dic);

        }

        return allTextDic;
    }

}
