using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class ConcertManager : MonoBehaviour
    {
        public GameObject MasterPanel;
        public GameObject WindowPanel;
        public ObjectWithText ProcessingPanel;

        public Text CurMoney, AllFan, ConcertScale, SpendMoney;
        public ConcertStageObj[] Stages;
        [HideInInspector]
        public ConcertData Data;

        // Start is called before the first frame update
        void Start()
        {
            Data = IngameManager.Instance.Data.ConcertProcess;

        }

        private void Update()
        {
            CurMoney.text = IngameManager.Instance.Data.Money.ToString();
            int fanCount = 0;
            foreach (var idol in IngameManager.Instance.Data.Idols)
                fanCount += idol.Value.Fan;
            AllFan.text = fanCount.ToString();
        }

        public void Refresh()
        {
            foreach (var stage in Stages)
                stage.CheckBtn.isOn = false;
        }

        public void ShowWindow()
        {
            MasterPanel.SetActive(true);

            if (Data.IsProcessing)
            {
                ProcessingPanel.text.text = $"콘서트 준비가 한창 진행되고 있으며,\n{Data.ProcessingTurnLeft}달 뒤에 콘서트가 열립니다!";
                ProcessingPanel.SetActive(true);
            }
            else
            {
                WindowPanel.SetActive(true);
                UpdateInfo();
            }
        }

        public void UpdateInfo()
        {
            var stageClass = new Dictionary<ConcertStageType, int>();
            int spendMoney = 0;
            foreach (var stage in Stages)
            {
                if(stage.IsSelected)
                {
                    if (stageClass.ContainsKey(stage.StageType))
                        stageClass[stage.StageType]++;
                    else
                        stageClass.Add(stage.StageType, 1);
                    spendMoney += stage.MoneyCount;
                }
            }
            var highest = new KeyValuePair<ConcertStageType, int>(ConcertStageType.Arena, 0);
            foreach(var value in stageClass)
            {
                if (highest.Value < value.Value)
                    highest = value;
            }
            if(highest.Key == ConcertStageType.Hall)
            {
                if (highest.Value >= 3)
                    ConcertScale.text = "홀 투어";
                else
                    ConcertScale.text = "홀 규모";
            }
            if (highest.Key == ConcertStageType.Arena)
            {
                if (highest.Value >= 3)
                    ConcertScale.text = "아레나 투어";
                else
                    ConcertScale.text = "아레나 규모";
            }
            if (highest.Key == ConcertStageType.Dome)
            {
                if (highest.Value >= 3)
                    ConcertScale.text = "돔 투어";
                else
                    ConcertScale.text = "돔 규모";
            }
            Data.Scale = ConcertScale.text;
            SpendMoney.text = spendMoney.ToString();
        }

        public void StartWork()
        {
            int spendMoney = 0;
            foreach (var stage in Stages)
            {
                if (stage.IsSelected)
                {
                    spendMoney += stage.MoneyCount;
                    Data.StageNames.Add(stage.StageName);
                    Data.SeatCounts.Add(stage.SeatCount);
                    Data.SoldoutCoeffs.Add(stage.SoldoutThreshold);
                }
            }
            if(IngameManager.Instance.Data.Money < spendMoney)
            {
                ProcessingPanel.text.text = "돈이 부족합니다!";
                ProcessingPanel.SetActive(true);
                Data.StageNames.Clear();
                Data.SeatCounts.Clear();
                Data.SoldoutCoeffs.Clear();
            }
            else
            {
                IngameManager.Instance.Data.Money -= spendMoney;
                Data.IsProcessing = true;
                Data.ProcessingTurnLeft = 3;
                WindowPanel.SetActive(false);
                ProcessingPanel.text.text = $"콘서트 준비를 시작했습니다.\n{Data.ProcessingTurnLeft}달 뒤에 콘서트가 열립니다!";
                ProcessingPanel.SetActive(true);
            }
        }

        public (bool, int, int[], string, bool) ApplySendResultData()
        {
            if (Data.IsProcessing)
            {
                if (Data.ProcessingTurnLeft == 0)
                {
                    int fan = 0, money = 0, reqSeat = 0, fillSeat = 0;
                    foreach (var idol in IngameManager.Instance.Data.Idols)
                        fan += idol.Value.Fan;
                    for (int i = 0; i < Data.StageNames.Count; i++)
                    {
                        reqSeat += Mathf.RoundToInt(Data.SeatCounts[i] * Data.SoldoutCoeffs[i]);
                        if (Data.SeatCounts[i] * Data.SoldoutCoeffs[i] <= fan)
                            fillSeat += Mathf.RoundToInt(Data.SeatCounts[i] * Data.SoldoutCoeffs[i]);
                        else
                            fillSeat += fan;
                    }
                    money = Mathf.RoundToInt(fillSeat * 1.1f);
                    return (true, 0, new[] { money, reqSeat, fillSeat }, Data.Scale, reqSeat == fillSeat);
                }
                else
                    return (true, Data.ProcessingTurnLeft, new[] { 0 }, "", false);
            }
            else
                return (false, 0, new[] { 0 }, "", false);
        }
    }
}