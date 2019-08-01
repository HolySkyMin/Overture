using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idol;

namespace Ingame
{
    [Serializable]
    public class WorkData
    {
        public string Title;
        public string Description;
        public IdolPickGroup IdolSlot;
        public bool[] CheckAbility;
        public int[] RequireAbility;
        public int[] RewardCoeff;

        public static List<WorkData> Generate(int amount)
        {
            var pickedWork = new List<WorkData>();
            var chksum = new List<int>();
            for(int i = 0; i < amount; i++)
            {
                int rnd;
                do
                    rnd = UnityEngine.Random.Range(0, IngameManager.Instance.WorkList.Count);
                while (chksum.Contains(rnd));
                pickedWork.Add(new WorkData(IngameManager.Instance.WorkList[rnd]));
                chksum.Add(rnd);
            }
            return pickedWork;
        }

        public WorkData(Dictionary<string, object> csvLine)
        {
            Title = (string)csvLine["title"];
            Description = (string)csvLine["description"];
            IdolSlot = new IdolPickGroup((int)csvLine["idolCount"]);
            CheckAbility = new bool[]
            {
                (int)csvLine["chkVocal"] == 0 ? false : true,
                (int)csvLine["chkDance"] == 0 ? false : true,
                (int)csvLine["chkVisual"] == 0 ? false : true,
                (int)csvLine["chkVariety"] == 0 ? false : true
            };
            RequireAbility = new int[]
            {
                (int)csvLine["reqVocal"], (int)csvLine["reqDance"], (int)csvLine["reqVisual"], (int)csvLine["reqVariety"],
                (int)csvLine["reqMoney"], (int)csvLine["reqHonor"], (int)csvLine["reqFan"]
            };
            RewardCoeff = new int[] { (int)csvLine["rwdMoney"], (int)csvLine["rwdHonor"], (int)csvLine["rwdFan"] };
        }
    }
}