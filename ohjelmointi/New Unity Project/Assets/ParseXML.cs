using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ParseXML : MonoBehaviour
{
    public static List<Dictionary<string, string>>[] classProgDicArray = new List<Dictionary<string, string>>[12];
    public static string[] playableClasses = { "alchemist", "barbarian", "bard", "champion", "cleric", "druid", "fighter", "monk", "ranger", "rogue", "sorcerer", "wizard" };
    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, string>> generalFeatDic = ParseGeneralFeat();
        List<Dictionary<string, string>> skillFeatDic = ParseSkillFeat();
        List<Dictionary<string, string>> classFeatDic = ParseClassFeat();
        List<Dictionary<string, string>> ancestryFeatDic = ParseAncestryFeat();
        List<Dictionary<string, string>> ancestryDic = ParseAncestry();
        List<Dictionary<string, string>> backgroundDic = ParseBackground();
        List<Dictionary<string, string>> proficiencyDic = ParseProficiency();
        List<Dictionary<string, string>> classAdvDic = ParseAdvancement();
        List<Dictionary<string, string>> alchemistProgDic = ParseClassProgression("alchemist");


        //add other dictionaries

        //loop class dictionaries works
        for (int i = 0; i < classProgDicArray.Length; i++)
        {
            classProgDicArray[i] = ParseClassProgression(playableClasses[i]);
        }
        /*
        Dictionary<string, string> generalFeats = generalFeatDic[1];
        Dictionary<string, string> alchemistProg = alchemistProgDic[1];
        Dictionary<string,string> alchemistProg1 = classProgDicArray[0][1];
        Debug.Log(generalFeats["featname"]);
        Debug.Log(generalFeats["level"]); 
        Debug.Log(generalFeats["prerequisite"]);
        Debug.Log(generalFeats["description"]);
        Debug.Log(alchemistProg["advancement1"]);
        Debug.Log(alchemistProg1["advancement1"]);
        */
        //creating dictionaries with a loop? would help with classes...


    }

    /// <summary>
    ///  
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseGeneralFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_general");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("Feats").Elements("feat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "featname", "level", "prerequisite", "description"};
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseSkillFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_skill");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("Feats").Elements("feat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "featname", "level", "prerequisite", "description"};
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseClassFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_class");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("ClassFeats").Elements("classFeat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "classFeatName", "class", "level", "prerequisite", "description"};
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseAncestryFeat()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_ancestry");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("AncestryFeats").Elements("ancestryFeat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        { "ancFeatName", "ancestry", "level", "prerequisite", "description"};
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseAncestry()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("ancestry");
        var doc = XDocument.Parse(txtXmlAsset.text);
        var allDict = doc.Element("Ancestry").Elements("race");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        {"ancName", "hp", "size", "speed", "boost", "flaw", "language", "trait", "special" };
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
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseAdvancement()
{
    TextAsset txtXmlAsset = Resources.Load<TextAsset>("class_advancement");
    var doc = XDocument.Parse(txtXmlAsset.text);

    var allDict = doc.Element("Advancements").Elements("advancement");
    List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
    string profInfo;
    XElement element;
    string[] elementNames = new string[]
    {"advName", "class", "effect", "description" };
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseProficiency()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("proficiencies");
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("Proficiencies").Elements("proficiency");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        string profInfo;
        XElement element;
        string[] elementNames = new string[]
        {"class", "initialBoost", "hp", "fortitudeSave", "reflexSave", "willSave",
                "skill1", "skill2", "skill3", "languages",
                "attack1", "attack2", "attack3", "attack4", "attackSpecial",
                "defense1", "defense2", "defense3", "defense4", "classDC", "spellDC"};
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

    //class progressions (a ton how?) (multiple cases based on string parameter that specifies the class?)
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
