using System.Collections;
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

        [Header("Managers")]
        public TheaterManager Theater;
        public WorkManager Work;
        public LessonManager Lesson;
        public CDManager CD;
        public ConcertManager Concert;
        public AuditionManager Audition;
        public NewSongManager NewSong;
        public ResultManager Result;
        public OpeningManager Opening;

        [Header("UI Elements")]
        public Text MoneyText;
        public Text GroupNameText, DateTimeText;

        [Header("ETC")]
        public IdolListViewer IdolList;
        public SongListViewer SongList;

        public List<Dictionary<string, object>> WorkList;
        [HideInInspector]
        public IngameData Data;

        private void Awake()
        {
            Instance = this;

            Data = new IngameData();
            Data.SongDataPool = CSVReader.Read("Data/SongDataTable");
            Data.Idols = new Dictionary<int, IdolData>();
            Data.Songs = new Dictionary<int, SongData>();
            Data.Month = -1;

            WorkList = CSVReader.Read("Data/WorkDataTable");
        }

        // Start is called before the first frame update
        async void Start()
        {
            await Opening.ShowOpening();
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

            StartNewTurn();
        }

        public void StartNewTurn()
        {
            Data.Month++;
            foreach(var idol in Data.Idols)
            {
                idol.Value.IsWorkLessonPicked = false;
            }
            Work.CreateWorkDatas();
            Work.LoadWorkSlots();
            foreach (var idol in Lesson.Lessons)
                idol.ResetSlot();
            if (Data.CDProcess.IsProcessing)
                Data.CDProcess.ProcessingTurnLeft--;
            if (Data.ConcertProcess.IsProcessing)
                Data.ConcertProcess.ProcessingTurnLeft--;
            Audition.AlreadyHeld = false;
            NewSong.RefreshList();
        }

        void Update()
        {
            MoneyText.text = Data.Money.ToString();
            GroupNameText.text = Data.GroupName;
            DateTimeText.text = $"{Data.Month / 12 + 1}년 {Data.Month % 12 + 1}월";
        }

        public void RunResult()
        {
            Result.gameObject.SetActive(true);
            StartCoroutine(Result.ShowResult());
        }
    }
}