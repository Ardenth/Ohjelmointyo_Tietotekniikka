using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;

 [XmlRoot("FeatGeneralCollection")]
public class FeatGeneralContainer 
{

    [XmlArray("FeatsGeneral")]
    [XmlArrayFeat("FeatGeneral")]
    public List<FeatGeneral> feats = new List<feat>();

    public static FeatGeneralContainer load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(FeatGeneralContainer));

        StringReader reader = new StringeReader(_xml.text);

        FeatGeneralContainer feats = serializer.Deserialize(reader) as FeatContainer;

        reader.Close;

        return feats;
    }
}
