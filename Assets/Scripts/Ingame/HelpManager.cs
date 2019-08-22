using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class HelpManager : MonoBehaviour
    {
        public GameObject MasterPanel;
        public GameObject[] HelpPanels;

        public void ShowHelp(int index)
        {
            MasterPanel.SetActive(true);
            for (int i = 0; i < HelpPanels.Length; i++)
            {
                if (i == index)
                    HelpPanels[i].SetActive(true);
                else
                    HelpPanels[i].SetActive(false);
            }
        }

        public void Close()
        {
            MasterPanel.SetActive(false);
            foreach (var panel in HelpPanels)
                panel.SetActive(false);
        }
    }
}