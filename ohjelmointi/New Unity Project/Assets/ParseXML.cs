using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ParseXML : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, string>> generalFeatDic = ParseGeneralFeat();
        List<Dictionary<string, string>> ancestryFeatDic = ParseAncestryFeat();
        List<Dictionary<string, string>> skillFeatDic = ParseSkillFeat();
        Dictionary<string, string> generalFeats = generalFeatDic[1];
        Debug.Log(generalFeats["name"]);
        Debug.Log(generalFeats["level"]); 
        Debug.Log(generalFeats["prerequisite"]);
        Debug.Log(generalFeats["description"]);
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
        foreach (var oneDict in allDict)
        {


            XElement first = oneDict.Element("featname");
            XElement second = oneDict.Element("level");
            XElement third = oneDict.Element("prerequisite");
            XElement fourth = oneDict.Element("description");
            string gfName = first.ToString().Replace("<featname>", "").Replace("</featname>", "");
            string gfLevel = second.ToString().Replace("<level>", "").Replace("</level>", "");
            string gfPrerequisite = third.ToString().Replace("<prerequisite>", "").Replace("</prerequisite>", "");
            string gfDescription = fourth.ToString().Replace("<description>", "").Replace("</description>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", gfName);
            dic.Add("level", gfLevel);
            dic.Add("prerequisite", gfPrerequisite);
            dic.Add("description", gfDescription);

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
        foreach (var oneDict in allDict)
        {
            XElement first = oneDict.Element("ancFeatName");
            XElement second = oneDict.Element("ancestry");
            XElement third = oneDict.Element("level");
            XElement fourth = oneDict.Element("prerequisite");
            XElement fifth = oneDict.Element("description");
            string afName = first.ToString().Replace("<ancFeatName>", "").Replace("<ancFeatName>", "");
            string afAncestry = second.ToString().Replace("<ancestry>", "").Replace("</ancestry>", "");
            string afLevel = third.ToString().Replace("<level>", "").Replace("</level>", "");
            string afPrerequisite = fourth.ToString().Replace("<prerequisite>", "").Replace("</prerequisite>", "");
            string afDescription = fifth.ToString().Replace("<description>", "").Replace("</description>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", afName);
            dic.Add("ancestry", afAncestry);
            dic.Add("level", afLevel);
            dic.Add("prerequisite", afPrerequisite);
            dic.Add("description", afDescription);

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
        foreach (var oneDict in allDict)
        {


            XElement first = oneDict.Element("featname");
            XElement second = oneDict.Element("level");
            XElement third = oneDict.Element("prerequisite");
            XElement fourth = oneDict.Element("description");
            string sfName = first.ToString().Replace("<featname>", "").Replace("</featname>", "");
            string sfLevel = second.ToString().Replace("<level>", "").Replace("</level>", "");
            string sfPrerequisite = third.ToString().Replace("<prerequisite>", "").Replace("</prerequisite>", "");
            string sfDescription = fourth.ToString().Replace("<description>", "").Replace("</description>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", sfName);
            dic.Add("level", sfLevel);
            dic.Add("prerequisite", sfPrerequisite);
            dic.Add("description", sfDescription);

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

        var allDict = doc.Element("AncestryFeats").Elements("ancestryFeat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        foreach (var oneDict in allDict)
        {
            XElement first = oneDict.Element("classFeatName");
            XElement second = oneDict.Element("class");
            XElement third = oneDict.Element("level");
            XElement fourth = oneDict.Element("prerequisite");
            XElement fifth = oneDict.Element("description");
            string cName = first.ToString().Replace("<classFeatName>", "").Replace("<classFeatName>", "");
            string cClass = second.ToString().Replace("<class>", "").Replace("</class>", "");
            string cLevel = third.ToString().Replace("<level>", "").Replace("</level>", "");
            string cPrerequisite = fourth.ToString().Replace("<prerequisite>", "").Replace("</prerequisite>", "");
            string cDescription = fifth.ToString().Replace("<description>", "").Replace("</description>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", cName);
            dic.Add("class", cClass);
            dic.Add("level", cLevel);
            dic.Add("prerequisite", cPrerequisite);
            dic.Add("description", cDescription);

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
        foreach (var oneDict in allDict)
        {


            XElement first = oneDict.Element("ancName");
            XElement second = oneDict.Element("hp");
            XElement third = oneDict.Element("size");
            XElement fourth = oneDict.Element("speed");
            XElement fifth = oneDict.Element("boost");
            XElement sixth = oneDict.Element("flaw");
            XElement seventh = oneDict.Element("language");
            XElement eight = oneDict.Element("trait");
            XElement ninth = oneDict.Element("special");
            string aName = first.ToString().Replace("<ancName>", "").Replace("</ancName>", "");
            string aHP = second.ToString().Replace("<hp>", "").Replace("</hp>", "");
            string aSize = third.ToString().Replace("<size>", "").Replace("</size>", "");
            string aSpeed = fourth.ToString().Replace("<speed>", "").Replace("</speed>", "");
            string aBoost = fifth.ToString().Replace("<boost>", "").Replace("</boost>", "");
            string aFlaw = sixth.ToString().Replace("<flaw>", "").Replace("</flaw>", "");
            string aLanguage = seventh.ToString().Replace("<language>", "").Replace("</language>", "");
            string aTrait = eight.ToString().Replace("<trait>", "").Replace("</trait>", "");
            string aSpecial = ninth.ToString().Replace("<special>", "").Replace("</special>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", aName);
            dic.Add("hp", aHP);
            dic.Add("size", aSize);
            dic.Add("speed", aSpeed);
            dic.Add("boost", aBoost);
            dic.Add("flaw", aFlaw);
            dic.Add("language", aLanguage);
            dic.Add("trait", aTrait);
            dic.Add("special", aSpecial);
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
        foreach (var oneDict in allDict)
        {


            XElement first = oneDict.Element("bckgrName");
            XElement second = oneDict.Element("boost");
            XElement third = oneDict.Element("skill");
            XElement fourth = oneDict.Element("feat");
            XElement fifth = oneDict.Element("description");
            string bckName = first.ToString().Replace("<bckgrName>", "").Replace("</bckgrName>", "");
            string bckBoost = second.ToString().Replace("<boost>", "").Replace("</boost>", "");
            string bckSkill = third.ToString().Replace("<skill>", "").Replace("</skill>", "");
            string bckFeat = fourth.ToString().Replace("<feat>", "").Replace("</feat>", "");
            string bckDescription = fifth.ToString().Replace("<description>", "").Replace("</description>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", bckName);
            dic.Add("boost", bckBoost);
            dic.Add("skill", bckSkill);
            dic.Add("feat", bckFeat);
            dic.Add("description", bckDescription);

            allTextDic.Add(dic);

        }


        return allTextDic;
    }


    //class progressions (a ton how?)


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Dictionary<string, string>> ParseAdvancement()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("Class_advancement");
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("Advancements").Elements("advancement");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        foreach (var oneDict in allDict)
        {
            XElement first = oneDict.Element("advName");
            XElement second = oneDict.Element("class");
            XElement third = oneDict.Element("effect");
            XElement fourth = oneDict.Element("description");
            string advName = first.ToString().Replace("<advName>", "").Replace("</advName>", "");
            string advClass = second.ToString().Replace("<class>", "").Replace("</class>", "");
            string advEffect = third.ToString().Replace("<effect>", "").Replace("</effect>", "");
            string advDescription = fourth.ToString().Replace("<description>", "").Replace("</description>", "");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", advName);
            dic.Add("class", advClass);
            dic.Add("effect", advEffect);
            dic.Add("description", advDescription);

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
        foreach (var oneDict in allDict)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] profInfo = new string[21];
            XElement[] elements = new XElement[21];
            string[] elementNames = new string[]
            {"class", "initialBoost", "hp", "fortitudeSave", "reflexSave", "willSave",
                "skill1", "skill2", "skill3", "languages",
                "attack1", "attack2", "attack3", "attack4", "attackSpecial",
                "defense1", "defense2", "defense3", "defense4", "classDC", "spellDC"};

            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = oneDict.Element(elementNames[i]);
                profInfo[i] = elements[0].ToString().Replace("<" + elements[0] + ">", "").Replace("</" + elements[0] + ">", "");
                dic.Add(elementNames[i], profInfo[i]);
            }
            allTextDic.Add(dic);

        }


        return allTextDic;
    }



}
