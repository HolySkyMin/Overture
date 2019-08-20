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

        // Start is called before the first frame update
        void Start()
        {
            LoadLessonData();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowLessonPanel()
        {
            Window.SetActive(true);
        }

        public void HideLessonPanel()
        {
            Window.SetActive(false);
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