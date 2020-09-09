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



    /// <summary>
    /// allocating buttons for use
    /// </summary>
    /// <param name="button">button which will be allocated for use for the TabGroup</param>
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




    /// <summary>
    /// functionality for currently selected tab
    /// </summary>
    /// <param name="button">which selected tab's state is being altered</param>
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

    /// <summary>
    /// reset tabs to basic state
    /// </summary>
    public void ResetTabs()
    {

        foreach(TabButton button in tabButtons)
        {
            button.tabView.color = idleColor;
        }
    }
}
