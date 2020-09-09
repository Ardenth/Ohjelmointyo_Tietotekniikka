using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler//, IPointerExitHandler
{

    public TabGroup tabGroup;
    public Image tabView;

    // Start is called before the first frame update
    void Start()
    {
        tabView = GetComponent<Image>();
        tabGroup.Allocate(this);
    }

    //public void OnPointerExit(PointerEventData eventData)
   // {
    //    tabGroup.TabExit(this);
    //}

    /// <summary>
    /// Function for Tab's functionality on click
    /// </summary>
    /// <param name="eventData">Mouse click event data</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.TabSelected(this);
    }

}
