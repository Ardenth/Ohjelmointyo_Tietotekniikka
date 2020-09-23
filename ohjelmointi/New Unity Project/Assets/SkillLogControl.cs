using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillLogControl : MonoBehaviour
{
    [SerializeField]
    private GameObject skillTemplate;

    private List<GameObject> skillItems;

    /// <summary>
    /// Logs skill into parent object
    /// </summary>
    /// <param name="newSkillPair">Information for skills</param>
    /// <param name="parentName">parent object's name</param>
    public void LogSkillText(KeyValuePair<string, string> newSkillPair, string parentName)
    {
        skillItems = new List<GameObject>();
        //use Skill prefab
        skillTemplate = Resources.Load("Skill") as GameObject;
        GameObject newSkill = Instantiate(skillTemplate) as GameObject;
        newSkill.SetActive(true);
        int i = 0;
        //control key and value pair for information application
        foreach (Transform child in newSkill.transform)
        {
            if (i == 0)
            {
                child.GetComponent<TextMeshProUGUI>().text = newSkillPair.Key;
            }
            else
            {
                child.GetComponent<TextMeshProUGUI>().text = newSkillPair.Value;
            }
            i++;
        }
        newSkill.transform.SetParent(GameObject.Find(parentName).transform);
        //control scale to normal
        newSkill.gameObject.transform.localScale = new Vector3(1, 1, 1);

        skillItems.Add(newSkill.gameObject);
    }
}
