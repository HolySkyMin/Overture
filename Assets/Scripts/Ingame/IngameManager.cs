﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Idol;
using Song;

namespace Ingame
{
    public class IngameManager : MonoBehaviour
    {
        public static IngameManager Instance { get; private set; }

        [Header("UI Elements")]
        public Text MoneyText;

        [Header("ETC")]
        public IdolListViewer IdolList;
        public SongListViewer SongList;

        public List<Dictionary<string, object>> WorkList;
        [HideInInspector]
        public IngameData Data;

        private void Awake()
        {
            Instance = this;

            // Debug 190719
            Data = new IngameData();
            Data.SongDataPool = CSVReader.Read("Data/SongDataTable");
            Data.Idols = new Dictionary<int, IdolData>();

            var idols = IdolData.Generate(20);
            Debug.Log(idols.Length);
            for (int i = 0; i < idols.Length; i++)
            {
                idols[i].Index = i;
                Data.Idols.Add(i, idols[i]);
            }
            Data.CurrentIdolIndex = 20;

            Data.Songs = SongData.GetAll();
            Data.Money = 10000;

            WorkList = CSVReader.Read("Data/WorkDataTable");
        }

        // Start is called before the first frame update
        void Start()
        {
            // Debug 190719
            foreach (var idol in Data.Idols)
            {
                IdolPicker.Instance.LoadCard(idol.Value);
                IdolList.LoadCard(idol.Value);
            }
            foreach (var song in Data.Songs)
            {
                SongPicker.Instance.LoadCard(song.Value);
                SongList.LoadCard(song.Value);
            }
        }

        // Update is called once per frame
        void Update()
        {
            MoneyText.text = Data.Money.ToString();
        }

        public async void IdolPickerTest()
        {
            var pickRes = await IdolPicker.Instance.Show(5);
            Debug.Log(pickRes.ToString());
        }
    }
}