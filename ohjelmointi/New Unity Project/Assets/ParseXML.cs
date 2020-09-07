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
        Dictionary<string, string> generalFeats = generalFeatDic[1];
        Debug.Log(generalFeats["name"]);
        Debug.Log(generalFeats["level"]); 
        Debug.Log(generalFeats["prerequisite"]);
        Debug.Log(generalFeats["description"]);
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
