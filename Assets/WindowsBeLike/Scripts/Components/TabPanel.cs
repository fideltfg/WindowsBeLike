/// <summary>
/// Handles the functionality of a tab panel, allowing switching between tabs and displaying corresponding content.
/// </summary>
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsBeLike
{
    public class TabPanel : MonoBehaviour
    {
        public bool useMask = true;
        public List<Button> Tabs = new List<Button>();
        public List<Transform> Pages = new List<Transform>();
        private int defaultTab = 0;
        private bool built = false;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (built == false)
            {
                CloseAllTabs();
                OpenTab(defaultTab);
                built = true;
            }
        }

        /// <summary>
        /// Closes all tabs, deactivates their corresponding pages, and hides masks if applicable.
        /// </summary>
        private void CloseAllTabs()
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                Pages[i].gameObject.SetActive(false);

                if (useMask)
                {
                    Transform mask = Tabs[i].transform.Find("Mask");
                    if (mask != null)
                    {
                        mask.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Opens the specified tab, deactivates other tabs, activates the corresponding page, and shows masks if applicable.
        /// </summary>
        /// <param name="index">The index of the tab to open.</param>
        public void OpenTab(int index)
        {
            CloseAllTabs();
            Pages[index].gameObject.SetActive(true);

            if (useMask)
            {
                Transform mask = Tabs[index].transform.Find("Mask");
                if (mask != null)
                {
                    mask.gameObject.SetActive(true);
                }
            }
        }
    }
}
