using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public enum ConcertStageType { Hall, Arena, Dome }

    public class ConcertStageObj : MonoBehaviour
    {
        public bool IsSelected { get { return CheckBtn.isOn; } }

        public string StageName;
        public ConcertStageType StageType;
        public int SeatCount, MoneyCount;
        public float SoldoutThreshold;
        public Text StageText, SeatText, MoneyText;
        public Toggle CheckBtn;

        private void Start()
        {
            StageText.text = StageName;
            SeatText.text = SeatCount.ToString();
            MoneyText.text = MoneyCount.ToString();
        }
    }
}