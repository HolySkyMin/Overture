using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class LessonManager : MonoBehaviour
    {
        public static int[] SpendMoneyTable { get { return new[] { 5, 10, 15, 20, 30, 40, 60, 80, 120, 200 }; } }

        public GameObject Window;
        public LessonSlot[] Lessons;

        public void ShowLessonPanel()
        {
            Window.SetActive(true);
        }

        public void HideLessonPanel()
        {
            Window.SetActive(false);
        }

        public (int[], int) ApplySendResultData()
        {
            int totalMoney = 0;
            for(int i = 0; i < Lessons[0].Idols.Capacity; i++)
            {
                if(Lessons[0].Idols.IdolIndices[i] != -1)
                {
                    IngameManager.Instance.Data.Money -= SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[0].Idols.IdolIndices[i]].Vocal];
                    IngameManager.Instance.Data.Idols[Lessons[0].Idols.IdolIndices[i]].Vocal++;
                    totalMoney += SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[0].Idols.IdolIndices[i]].Vocal];
                }
            }
            for (int i = 0; i < Lessons[1].Idols.Capacity; i++)
            {
                if(Lessons[1].Idols.IdolIndices[i] != -1)
                {
                    IngameManager.Instance.Data.Money -= SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[1].Idols.IdolIndices[i]].Dance];
                    IngameManager.Instance.Data.Idols[Lessons[1].Idols.IdolIndices[i]].Dance++;
                    totalMoney += SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[1].Idols.IdolIndices[i]].Dance];
                }
            }
            for (int i = 0; i < Lessons[2].Idols.Capacity; i++)
            {
                if(Lessons[2].Idols.IdolIndices[i] != -1)
                {
                    IngameManager.Instance.Data.Money -= SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[2].Idols.IdolIndices[i]].Visual];
                    IngameManager.Instance.Data.Idols[Lessons[2].Idols.IdolIndices[i]].Visual++;
                    totalMoney += SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[2].Idols.IdolIndices[i]].Visual];
                }
            }
            for (int i = 0; i < Lessons[3].Idols.Capacity; i++)
            {
                if(Lessons[3].Idols.IdolIndices[i] != -1)
                {
                    IngameManager.Instance.Data.Money -= SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[3].Idols.IdolIndices[i]].Variety];
                    IngameManager.Instance.Data.Idols[Lessons[3].Idols.IdolIndices[i]].Variety++;
                    totalMoney += SpendMoneyTable[IngameManager.Instance.Data.Idols[Lessons[3].Idols.IdolIndices[i]].Variety];
                }
            }
            var idolCounts = new List<int>();
            for (int i = 0; i < Lessons.Length; i++)
                idolCounts.Add(Lessons[i].Idols.Count);
            return (idolCounts.ToArray(), totalMoney);
        }

        public void LoadLessonData()
        {
            for(int i = 0; i < Lessons.Length; i++)
                Lessons[i].Idols = IngameManager.Instance.Data.LessonIdols[i];
        }

        public void SaveLessonData()
        {
            for (int i = 0; i < Lessons.Length; i++)
                IngameManager.Instance.Data.LessonIdols[i] = Lessons[i].Idols;
        }
    }
}