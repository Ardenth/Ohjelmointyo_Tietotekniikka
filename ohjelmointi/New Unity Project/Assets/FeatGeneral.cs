using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class FeatGeneral : MonoBehaviour
{
    [XmlAttribute("Name")]
    public string name;

    [XmlAttribute("Level")]
    public float level;

    [XmlAttribute("Prerequisites")]
    public string prerequisites;

    [XmlAttribute("Description")]
    public string description;
}
