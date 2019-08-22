using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Song;

namespace Ingame
{
    public enum SongPurchaseState { Failed, RequireBattle, Succeed }

    public class NewSongManager : MonoBehaviour
    {
        public GameObject MasterPanel, WindowPanel, AlertPanel;
        public RectTransform SongObjParent;
        public GameObject SongObjTemplate;

        private List<GameObject> songObjs;

        void Start()
        {
            songObjs = new List<GameObject>();
        }

        public void RefreshList()
        {
            while(songObjs.Count > 0)
            {
                Destroy(songObjs[0]);
                songObjs.RemoveAt(0);
            }

            var newSongData = SongData.Get(3);
            for(int i = 0; i < newSongData.Length; i++)
            {
                var newObj = Instantiate(SongObjTemplate);
                newObj.GetComponent<NewSongObject>().Set(newSongData[i]);
                newObj.transform.SetParent(SongObjParent);
                newObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                newObj.SetActive(true);
                songObjs.Add(newObj);
            }
        }

        public void ShowWindow()
        {
            MasterPanel.SetActive(true);
        }

        public void HideWindow()
        {
            MasterPanel.SetActive(false);
        }

        public SongPurchaseState TryPurchase(SongData data)
        {
            if(IngameManager.Instance.Data.Money < data.Cost)
            {
                AlertPanel.SetActive(true);
                return SongPurchaseState.Failed;
            }
            else
            {
                IngameManager.Instance.Data.Money -= data.Cost;
                data.Index = IngameManager.Instance.Data.CurrentSongIndex;
                IngameManager.Instance.Data.Songs.Add(IngameManager.Instance.Data.CurrentSongIndex++, data);
                data.SetAsEarned();

                SongPicker.Instance.LoadCard(IngameManager.Instance.Data.Songs[data.Index]);
                IngameManager.Instance.SongList.LoadCard(IngameManager.Instance.Data.Songs[data.Index]);
                return SongPurchaseState.Succeed;
            }
        }

        // Battle code
    }
}