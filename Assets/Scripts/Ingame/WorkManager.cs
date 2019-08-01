using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class WorkManager : MonoBehaviour
    {
        public GameObject Window;
        public RectTransform SlotParent;
        public GameObject SlotTemplate;

        private List<GameObject> slotObj = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            // DEBUG
            CreateWorkDatas();
            LoadWorkSlots();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowWorkPanel()
        {
            Window.SetActive(true);
        }

        public void HideWorkPanel()
        {
            Window.SetActive(false);
        }

        public void CreateWorkDatas()
        {
            IngameManager.Instance.Data.Work = WorkData.Generate(3).ToArray();
        }

        public void SaveWorkDatas()
        {
            for(int i = 0; i < slotObj.Count; i++)
                IngameManager.Instance.Data.Work[i] = slotObj[i].GetComponent<WorkSlot>().Data;
        }

        public void LoadWorkSlots()
        {
            while(slotObj.Count > 0)
            {
                Destroy(slotObj[0]);
                slotObj.RemoveAt(0);
            }

            for(int i = 0; i < IngameManager.Instance.Data.Work.Length; i++)
            {
                var newObj = Instantiate(SlotTemplate);
                var newSlot = newObj.GetComponent<WorkSlot>();
                newSlot.SetWork(IngameManager.Instance.Data.Work[i]);
                newObj.transform.SetParent(SlotParent);
                newObj.transform.localScale = Vector3.one;
                newObj.SetActive(true);
                slotObj.Add(newObj);
            }
        }
    }
}