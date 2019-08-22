using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class WorkManager : MonoBehaviour
    {
        public GameObject Window;
        public RectTransform SlotParent;
        public GameObject SlotTemplate;

        private List<GameObject> slotObj = new List<GameObject>();

        public void ShowWorkPanel()
        {
            Window.SetActive(true);
        }

        public void HideWorkPanel()
        {
            Window.SetActive(false);
        }

        public int CalculateMoney(float appeal, int coeff)
        {
            return Mathf.RoundToInt(appeal * 0.15f * coeff);
        }

        public int CalculateHonor(float appeal, int coeff)
        {
            return Mathf.RoundToInt(appeal * 0.05f * coeff);
        }

        public int CalculateFan(float appeal, int coeff)
        {
            return Mathf.RoundToInt(appeal * 0.3f * coeff);
        }

        public (int, int, int) ApplySendResultData()
        {
            int totalMoney = 0, totalHonor = 0, totalFan = 0;

            foreach(var work in IngameManager.Instance.Data.Work)
            {
                if(work.IdolSlot.Count > 0)
                {
                    var appeal = work.IdolSlot.CalculateAppeal(work);
                    totalMoney += CalculateMoney(appeal.Item1, work.RewardCoeff[0]);
                    for(int i = 0; i < appeal.Item2.Length; i++)
                    {
                        if(work.IdolSlot.IdolIndices[i] != -1)
                        {
                            IngameManager.Instance.Data.Idols[work.IdolSlot.IdolIndices[i]].Honor += CalculateHonor(appeal.Item2[i], work.RewardCoeff[1]);
                            totalHonor += CalculateHonor(appeal.Item2[i], work.RewardCoeff[1]);
                            IngameManager.Instance.Data.Idols[work.IdolSlot.IdolIndices[i]].Fan += CalculateFan(appeal.Item2[i], work.RewardCoeff[2]);
                            totalFan += CalculateFan(appeal.Item2[i], work.RewardCoeff[2]);
                        }
                    }
                }
            }
            return (totalMoney, totalHonor, totalFan);
        }

        public void CreateWorkDatas()
        {
            IngameManager.Instance.Data.Work = WorkData.Generate(3).ToArray();
        }

        public void SaveWorkDatas()
        {
            for(int i = 0; i < slotObj.Count; i++)
                IngameManager.Instance.Data.Work[i] = slotObj[i].GetComponent<WorkSlot>().Data;
        }

        public void LoadWorkSlots()
        {
            while(slotObj.Count > 0)
            {
                Destroy(slotObj[0]);
                slotObj.RemoveAt(0);
            }

            for(int i = 0; i < IngameManager.Instance.Data.Work.Length; i++)
            {
                var newObj = Instantiate(SlotTemplate);
                var newSlot = newObj.GetComponent<WorkSlot>();
                newSlot.SetWork(IngameManager.Instance.Data.Work[i]);
                newObj.transform.SetParent(SlotParent);
                newObj.transform.localScale = Vector3.one;
                newObj.SetActive(true);
                slotObj.Add(newObj);
            }
        }
    }
}