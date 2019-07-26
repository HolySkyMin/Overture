using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class TheaterManager : MonoBehaviour
    {
        public GameObject Window;
        public RectTransform ObjMasterParent;
        public TheaterUnit[] Units;

        private void Start()
        {
            for(int i = 0; i < 5; i++)
            {
                if(Units[i] != null)
                {
                    Units[i].Data = IngameManager.Instance.Data.Theater.Units[i];
                    for(int j = 0; j < 4; j++)
                    {
                        if(Units[i].Slots[j] != null)
                            Units[i].Slots[j].Data = IngameManager.Instance.Data.Theater.Units[i].Slots[j];
                    }
                    if (Units[i].Data.Unlocked)
                        Units[i].Activate();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int CalculateMoney()
        {
            return 0;
        }

        public void ApplyFanChange()
        {

        }

        public void SaveTheater()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Units[i] != null)
                {
                    IngameManager.Instance.Data.Theater.Units[i] = Units[i].Data;
                    for (int j = 0; j < 4; j++)
                    {
                        if (Units[i].Slots[j] != null)
                            IngameManager.Instance.Data.Theater.Units[i].Slots[j] = Units[i].Slots[j].Data;
                    }
                }
            }
        }
    }
}