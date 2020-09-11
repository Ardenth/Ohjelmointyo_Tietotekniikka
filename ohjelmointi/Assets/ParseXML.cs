using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ParseXML : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, string>> allTextDic = ParseFile();
        Dictionary<string, string> dic = allTextDic[0];
        Debug.Log("hello");
        Debug.Log(dic["name"]);
        Debug.Log(dic["level"]);
        Debug.Log(dic["prerequisite"]);
        Debug.Log(dic["description"]);
    }

    public List<Dictionary<string, string>> ParseFile()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("feats_general");
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("Feats").Elements("feat");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        foreach (var oneDict in allDict)
        {

            string gfName = oneDict.Elements("featname").ToString().Replace("<string>", "").Replace("</string>", "");
            string gfLevel = oneDict.Elements("level").ToString().Replace("<string>", "").Replace("</string>", "");
            string gfPrerequisite = oneDict.Elements("prerequisite").ToString().Replace("<string>", "").Replace("</string>", "");
            string gfDescription = oneDict.Elements("description").ToString().Replace("<string>", "").Replace("</string>", "");

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
