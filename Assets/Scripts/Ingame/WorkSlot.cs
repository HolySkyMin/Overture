using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Idol;

namespace Ingame
{
    public class WorkSlot : MonoBehaviour
    {
        public Text TitleText, DescriptionText;
        public IdolPickVisualizer PickVisualizer;
        public GameObject[] MoneyDisp, HonorDisp, FanDisp;
        public GameObject[] ChkAbilDisp;
        public ObjectWithText[] ReqAbilDisp;
        [HideInInspector]
        public WorkData Data;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetWork(WorkData data)
        {
            Data = data;

            TitleText.text = data.Title;
            DescriptionText.text = data.Description;
            for(int i = 0; i < 5; i++)
            {
                if (i < Data.RewardCoeff[0])
                    MoneyDisp[i].SetActive(true);
                if (i < Data.RewardCoeff[1])
                    HonorDisp[i].SetActive(true);
                if (i < Data.RewardCoeff[2])
                    FanDisp[i].SetActive(true);
            }
            for (int i = 0; i < Data.CheckAbility.Length; i++)
                if (Data.CheckAbility[i] == true)
                    ChkAbilDisp[i].SetActive(true);
            for(int i = 0; i < Data.RequireAbility.Length; i++)
            {
                if(Data.RequireAbility[i] > 0)
                {
                    ReqAbilDisp[i].text.text = Data.RequireAbility[i].ToString();
                    ReqAbilDisp[i].SetActive(true);
                }
            }
        }

        private void Update()
        {
            PickVisualizer.Visualize(Data.IdolSlot);
        }

        public async void SetIdol()
        {
            Data.IdolSlot = await IdolPicker.Instance.Show(Data.IdolSlot, true);
        }
    }
}