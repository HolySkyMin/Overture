using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Ingame
{
    public class AuditionManager : MonoBehaviour
    {
        public bool AlreadyHeld { get; set; }

        public GameObject WindowPanel;
        public ObjectWithText ResultPanel;

        private const int REQUIRED_MONEY = 300;

        public void Initialize()
        {
            AlreadyHeld = false;
        }

        public async void Show()
        {
            WindowPanel.SetActive(true);

            if(AlreadyHeld)
            {
                ResultPanel.text.text = "이미 오디션을 열었습니다.";
                ResultPanel.SetActive(true);
                return;
            }
            if(IngameManager.Instance.Data.Money < REQUIRED_MONEY)
            {
                ResultPanel.text.text = "오디션을 열 돈이 부족합니다.\n\n오디션을 열려면 돈 300이 필요합니다.";
                ResultPanel.SetActive(true);
                return;
            }
            IngameManager.Instance.Data.Money -= REQUIRED_MONEY;
            
            var newIdols = await AuditionPicker.Instance.Show(20);
            AlreadyHeld = true;
            if(newIdols.Count > 0)
            {
                for (int i = 0; i < newIdols.Count; i++)
                {
                    newIdols[i].Index = IngameManager.Instance.Data.CurrentIdolIndex;
                    IngameManager.Instance.Data.Idols.Add(IngameManager.Instance.Data.CurrentIdolIndex++, newIdols[i]);
                    IdolPicker.Instance.LoadCard(newIdols[i]);
                    IngameManager.Instance.IdolList.LoadCard(newIdols[i]);
                }
                ResultPanel.text.text = "아이돌을 성공적으로 모집했습니다.";
                ResultPanel.SetActive(true);
            }
            else
            {
                ResultPanel.text.text = "아이돌을 모집하지 않았습니다.";
                ResultPanel.SetActive(true);
            }
        }
    }
}