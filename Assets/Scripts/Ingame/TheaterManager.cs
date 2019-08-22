using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class TheaterManager : MonoBehaviour
    {
        public GameObject Window, AlertPanel;
        public RectTransform ObjMasterParent;
        public TheaterUnit[] Units;
        public GameObject[] AddonSlotPurchased;

        private void Start()
        {
            for(int i = 0; i < 5; i++)
            {
                if(Units[i] != null)
                {
                    Units[i].Data = IngameManager.Instance.Data.Theater.Units[i];
                    for(int j = 0; j < 4; j++)
                    {
                        if(Units[i].Slots[j] != null)
                            Units[i].Slots[j].Data = IngameManager.Instance.Data.Theater.Units[i].Slots[j];
                    }
                    if (Units[i].Data.Unlocked)
                        Units[i].Activate();
                }
            }

            if (Units[1].Data.Unlocked)
                AddonSlotPurchased[0].SetActive(true);
            if (Units[2].Data.Unlocked)
                AddonSlotPurchased[1].SetActive(true);
            if (Units[4].Data.Unlocked)
                AddonSlotPurchased[2].SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowTheaterPanel()
        {
            Window.SetActive(true);
        }

        public void HideTheaterPanel()
        {
            Window.SetActive(false);
        }

        public void UnlockSlot(int index)
        {
            if(index == 0)
            {
                if (IngameManager.Instance.Data.Money < 100)
                    AlertPanel.SetActive(true);
                else
                {
                    IngameManager.Instance.Data.Money -= 100;
                    Units[1].Data.Unlocked = true;
                    Units[1].Activate();
                    AddonSlotPurchased[index].SetActive(true);
                }
            }
            else if(index == 1)
            {
                if (IngameManager.Instance.Data.Money < 100)
                    AlertPanel.SetActive(true);
                else
                {
                    IngameManager.Instance.Data.Money -= 100;
                    Units[2].Data.Unlocked = true;
                    Units[2].Activate();
                    AddonSlotPurchased[index].SetActive(true);
                }
            }
            else if(index == 2)
            {
                if (IngameManager.Instance.Data.Money < 100)
                    AlertPanel.SetActive(true);
                else
                {
                    IngameManager.Instance.Data.Money -= 100;
                    Units[4].Data.Unlocked = true;
                    Units[4].Activate();
                    AddonSlotPurchased[index].SetActive(true);
                }
            }
        }
        
        public (float, int, int) ApplySendResultData()
        {
            float totalAppeal = 0;
            int totalFan = 0;
            foreach(var unit in Units)
            {
                foreach(var slot in unit.Slots)
                {
                    if(slot.Data.Idols != null)
                    {
                        if (slot.Data.Idols.Count > 0)
                        {
                            var appeal = slot.Data.Idols.CalculateAppeal(IngameManager.Instance.Data.Songs[slot.Data.SongIndex]);
                            totalAppeal += appeal.Item1;
                            for (int i = 0; i < appeal.Item2.Length; i++)
                            {
                                if (slot.Data.Idols.IdolIndices[i] != -1)
                                {
                                    IngameManager.Instance.Data.Idols[slot.Data.Idols.IdolIndices[i]].Fan += CalculateFan(appeal.Item2[i]);
                                    totalFan += CalculateFan(appeal.Item2[i]);
                                }
                            }
                        }
                    }
                }
            }
            IngameManager.Instance.Data.Money += CalculateMoney(totalAppeal);
            return (totalAppeal, CalculateMoney(totalAppeal), totalFan);
        }

        public int CalculateMoney(float appeal)
        {
            return Mathf.RoundToInt(appeal * 0.02f);
        }

        public int CalculateFan(float appeal)
        {
            return Mathf.RoundToInt(appeal * 0.15f);
        }

        public void SaveTheater()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Units[i] != null)
                {
                    IngameManager.Instance.Data.Theater.Units[i] = Units[i].Data;
                    for (int j = 0; j < 4; j++)
                    {
                        if (Units[i].Slots[j] != null)
                            IngameManager.Instance.Data.Theater.Units[i].Slots[j] = Units[i].Slots[j].Data;
                    }
                }
            }
        }
    }
}