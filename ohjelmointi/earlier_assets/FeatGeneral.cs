using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class FeatGeneral : MonoBehaviour
{
    [XmlElement("FeatName")]
    public string featname;

    [XmlElement("Level")]
    public float level;

    [XmlElement("Prerequisite")]
    public string prerequisite;

    [XmlElement("Description")]
    public string description;

}
