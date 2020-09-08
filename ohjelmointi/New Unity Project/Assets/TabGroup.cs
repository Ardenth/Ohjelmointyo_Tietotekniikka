using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class TabGroup   : MonoBehaviour
{
    //character tab active on start for highlight?


    public List<TabButton> tabButtons;
    public Color32 idleColor = new Color32(255,255,255,90); //change colors
    public Color32 activeColor = new Color32(255, 255, 255, 50); //change colors
    //public TabButton selectedTab;
    public List<GameObject> viewSwap;



    //allocates the tab for later use
    public void Allocate(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    //public void TabExit(TabButton button)
    //{
        //ResetTabs();
    //}

    public void TabSelected(TabButton button)
    {
        ResetTabs();
        button.tabView.color = activeColor;
        int tabIndex = button.transform.GetSiblingIndex();
        for (int i = 0; i < viewSwap.Count; i++)
        {
            if (i == tabIndex)
            {
                viewSwap[i].SetActive(true);
            }
            else
            {
                viewSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {

        foreach(TabButton button in tabButtons)
        {
            button.tabView.color = idleColor;
        }
    }
}
