using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Idol;
using Song;

namespace Ingame
{
    public class CDManager : MonoBehaviour
    {
        public static readonly int[] SongRankMoneyTable = { 0, 30, 70, 120, 180, 250 };

        [Header("Big Panels")]
        public GameObject MasterPanel;
        public GameObject WindowPanel;
        public ObjectWithText ProcessingPanel;

        [Header("Elements in Window Panel")]
        public RectTransform SongVisualizeParent;
        public Text CDCount,  SongCount, IdolCount, AppealCount, MoneyCount;
        public GameObject SongVisualizeTemplate;
        public IdolPickVisualizer IdolVisualizer;
        [HideInInspector]
        public CDData Data;

        private List<GameObject> songVisuals;

        private void Start()
        {
            songVisuals = new List<GameObject>();

            Data = IngameManager.Instance.Data.CDProcess;
        }

        public void Save()
        {
            IngameManager.Instance.Data.CDProcess = Data;
        }

        public void ShowWindow()
        {
            MasterPanel.SetActive(true);

            if (Data.IsProcessing)
            {
                ProcessingPanel.text.text = $"음반 작업이 한창 진행되고 있으며,\n{Data.ProcessingTurnLeft}달 뒤에 음반이 발매됩니다!";
                ProcessingPanel.SetActive(true);
            }
            else
            {
                Data.Songs = new List<int>();
                Data.Idols = new IdolPickGroup(12);
                while (songVisuals.Count > 0)
                {
                    Destroy(songVisuals[0]);
                    songVisuals.RemoveAt(0);
                }
                WindowPanel.SetActive(true);
                UpdateInfo();
            }
        }

        private void Update()
        {
            IdolVisualizer.Visualize(Data.Idols);
        }

        public async void SetSong()
        {
            Data.Songs = await SongPicker.Instance.Show(Data.Songs);
            VisualizeSong();
            UpdateInfo();
        }

        public async void SetIdol()
        {
            Data.Idols = await IdolPicker.Instance.Show(Data.Idols);
            UpdateInfo();
        }

        public void VisualizeSong()
        {
            while(songVisuals.Count > 0)
            {
                Destroy(songVisuals[0]);
                songVisuals.RemoveAt(0);
            }

            for(int i = 0; i < Data.Songs.Count; i++)
            {
                var newObj = Instantiate(SongVisualizeTemplate);
                newObj.GetComponent<CDSongVisualizeObj>().Set(IngameManager.Instance.Data.Songs[Data.Songs[i]]);
                newObj.transform.SetParent(SongVisualizeParent);
                newObj.transform.localScale = Vector3.one;
                newObj.SetActive(true);
                songVisuals.Add(newObj);
            }
            LayoutRebuilder.MarkLayoutForRebuild(SongVisualizeParent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(SongVisualizeParent);
        }

        public void UpdateInfo()
        {
            CDCount.text = $"{IngameManager.Instance.Data.GroupName}의 {Data.CDCount}번째 음반";
            SongCount.text = Data.Songs.Count.ToString();
            IdolCount.text = Data.Idols.Count.ToString();
            float appeal = 0;
            for(int i = 0; i < Data.Songs.Count; i++)
                appeal += Data.Idols.CalculateAppeal(IngameManager.Instance.Data.Songs[Data.Songs[i]]).Item1;
            AppealCount.text = appeal.ToString("N0");
            MoneyCount.text = CalculateSpendMoney().ToString();
        }

        public int CalculateSpendMoney()
        {
            int res = 0;
            foreach (var index in Data.Idols.IdolIndices)
                if(index != -1)
                    res += IngameManager.Instance.Data.Idols[index].Cost * 3;
            foreach (var index in Data.Songs)
                res += SongRankMoneyTable[IngameManager.Instance.Data.Songs[index].Rank];
            return res;
        }

        public int CalculateGetMoney(float appeal)
        {
            return Mathf.RoundToInt(appeal * 0.07f);
        }

        public int CalculateFan(float appeal)
        {
            return Mathf.RoundToInt(appeal * 0.75f);
        }

        public void StartWork()
        {
            int money = CalculateSpendMoney();
            if(IngameManager.Instance.Data.Money < money)
            {
                ProcessingPanel.text.text = "돈이 부족합니다!";
                ProcessingPanel.SetActive(true);
            }
            else
            {
                Data.IsProcessing = true;
                Data.ProcessingTurnLeft = 3;
                WindowPanel.SetActive(false);
                ProcessingPanel.text.text = $"음반 작업이 시작되었습니다.\n{Data.ProcessingTurnLeft}달 뒤에 음반이 발매됩니다!";
                ProcessingPanel.SetActive(true);
            }
        }

        public (bool, float, int, int, int) ApplySendResultData()
        {
            if (Data.IsProcessing)
            {
                if (Data.ProcessingTurnLeft == 0)
                {
                    float appeal = 0;
                    int totalMoney = 0, totalFan = 0;
                    for (int i = 0; i < Data.Songs.Count; i++)
                    {
                        var pair = Data.Idols.CalculateAppeal(IngameManager.Instance.Data.Songs[Data.Songs[i]]);
                        for (int j = 0; j < pair.Item2.Length; j++)
                        {
                            IngameManager.Instance.Data.Idols[Data.Idols.IdolIndices[j]].Fan += CalculateFan(pair.Item2[j]);
                            totalFan += CalculateFan(pair.Item2[j]);
                        }
                        appeal += pair.Item1;
                        totalMoney += CalculateGetMoney(pair.Item1);
                        IngameManager.Instance.Data.Money += CalculateGetMoney(pair.Item1);
                    }
                    Data.IsProcessing = false;
                    return (true, appeal, totalMoney, totalFan, 0);
                }
                else
                    return (true, 0, 0, 0, Data.ProcessingTurnLeft);
            }
            else
                return (false, 0, 0, 0, 0);
        }
    }
}