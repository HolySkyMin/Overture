using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Song;

namespace Ingame
{
    public class NewSongObject : MonoBehaviour
    {
        public GameObject PurchasedPanel;
        public Button CostButton;
        public SongCard Card;
        public Text CostText;
        public NewSongManager Manager;

        public void Set(SongData data)
        {
            Card.SetSong(data);
            CostText.text = data.Cost.ToString();
        }

        public void Click()
        {
            var res = Manager.TryPurchase(Card.LinkedSong);
            if(res == SongPurchaseState.Succeed)
            {
                CostButton.interactable = false;
                PurchasedPanel.SetActive(true);
            }
            else if(res == SongPurchaseState.RequireBattle)
            {

            }
        }
    }
}