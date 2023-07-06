using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabPanel : MonoBehaviour
{
    public bool useMask = true;
    public List<Button> Tabs = new List<Button>();
    public List<Transform> Pages = new List<Transform>();
    int defaultTab = 0;
    bool built = false;
    

    private void Awake()
    {
        if (built == false)
        {
            CloseAllTabs();
            // set the default tab
            OpenTab(defaultTab);

            built = true;
        }
    }

    private void CloseAllTabs()
    {
        for (int i = 0; i < Tabs.Count; i++)
        {
            Pages[i].gameObject.SetActive(false);
            if (useMask)
            {
                Tabs[i].transform.Find("Mask").gameObject.SetActive(false);
            }
        }

    }

    public void OpenTab(int i)
    {
        CloseAllTabs();
        Pages[i].gameObject.SetActive(true);
        if (useMask)
        {
            Tabs[i].transform.Find("Mask").gameObject.SetActive(true);
        }
  
    }

}
