using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Idol;

namespace Ingame
{
    public class LessonSlot : MonoBehaviour
    {
        public int AbilityIndex;
        public IdolPickVisualizer Visualizer;
        public Text SpendMoney;
        [HideInInspector]
        public IdolPickGroup Idols;

        private int totalSpendMoney = 0;

        private void Awake()
        {
            ResetSlot();
        }

        // Update is called once per frame
        void Update()
        {
            Visualizer.Visualize(Idols);
            SpendMoney.text = totalSpendMoney.ToString();
        }

        public void ResetSlot()
        {
            Idols = new IdolPickGroup(4);
            totalSpendMoney = 0;
        }

        public async void SetIdol()
        {
            Idols = await IdolPicker.Instance.Show(Idols, true);

            int spendMoney = 0;
            for(int i = 0; i < Idols.Capacity; i++)
            {
                if(Idols.IdolIndices[i] != -1)
                {
                    var idol = IngameManager.Instance.Data.Idols[Idols.IdolIndices[i]];
                    switch(AbilityIndex)
                    {
                        case 0:
                            spendMoney += LessonManager.SpendMoneyTable[idol.Vocal];
                            break;
                        case 1:
                            spendMoney += LessonManager.SpendMoneyTable[idol.Dance];
                            break;
                        case 2:
                            spendMoney += LessonManager.SpendMoneyTable[idol.Visual];
                            break;
                        case 3:
                            spendMoney += LessonManager.SpendMoneyTable[idol.Variety];
                            break;
                        default:
                            break;
                    }
                }
            }
            totalSpendMoney = spendMoney;
        }

        public void ApplyLesson()
        {
            for(int i = 0; i < Idols.Capacity; i++)
            {
                if(Idols.IdolIndices[i] != -1)
                {
                    var idol = IngameManager.Instance.Data.Idols[Idols.IdolIndices[i]];
                    switch (AbilityIndex)
                    {
                        case 0:
                            idol.Vocal += 1;
                            break;
                        case 1:
                            idol.Dance += 1;
                            break;
                        case 2:
                            idol.Visual += 1;
                            break;
                        case 3:
                            idol.Variety += 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            IngameManager.Instance.Data.Money -= totalSpendMoney;
        }
    }
}