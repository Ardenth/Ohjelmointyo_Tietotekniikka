using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

 [XmlRoot("FeatGeneralCollection")]
public class FeatGeneralContainer 
{

    [XmlArray("FeatsGeneral")]
    [XmlArrayItem("FeatGeneral")]
    public List<FeatGeneral> feats = new List<FeatGeneral>();

    public static FeatGeneralContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(FeatGeneralContainer));

        StringReader reader = new StringReader(_xml.text);

        FeatGeneralContainer feats = serializer.Deserialize(reader) as FeatGeneralContainer;


        reader.Close();

        return feats;
    }
}
