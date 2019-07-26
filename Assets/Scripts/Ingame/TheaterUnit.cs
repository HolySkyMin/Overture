using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class TheaterUnit : MonoBehaviour
    {
        public string UnitName;
        public bool Unlocked;
        public int RequiredMoney;
        public TheaterSlot[] Slots;

        public TheaterUnitData Data;

        void Start()
        {
            if (!Data.Unlocked)
                gameObject.SetActive(false);
        }

        public void Activate()
        {
            Data.Unlocked = true;
            gameObject.SetActive(true);
        }
    }
}
