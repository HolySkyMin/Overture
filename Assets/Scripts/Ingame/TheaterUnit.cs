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

        void Start()
        {
            if (!Unlocked)
                gameObject.SetActive(false);
        }

        public void Activate()
        {
            Unlocked = true;
            gameObject.SetActive(true);
        }
    }
}
