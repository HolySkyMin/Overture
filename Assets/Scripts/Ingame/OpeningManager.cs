using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ScriptedAnimation;

namespace Ingame
{
    public class OpeningManager : MonoBehaviour
    {
        public GameObject OpeningPanel;
        public ScriptAnimation Panel1, Panel2, Panel3;
        public InputField NameInput;
        public Text NameDisplayText;

        private bool nextClicked = false;

        public async Task ShowOpening()
        {
            OpeningPanel.SetActive(true);
            await Panel1.Appear_C();
            await new WaitUntil(() => nextClicked);
            nextClicked = false;
            IngameManager.Instance.Data.GroupName = NameInput.text;
            await Panel1.Disappear_C();
            await Panel2.Appear_C();
            await new WaitUntil(() => nextClicked);
            nextClicked = false;
            var idolList = await AuditionPicker.Instance.Show(20, 10);
            for(int i = 0; i < idolList.Count; i++)
            {
                idolList[i].Index = IngameManager.Instance.Data.CurrentIdolIndex;
                IngameManager.Instance.Data.Idols.Add(IngameManager.Instance.Data.CurrentIdolIndex++, idolList[i]);
            }
            await Panel2.Disappear_C();
            var songData = Song.SongData.Get(3);
            foreach(var song in songData)
            {
                song.Index = IngameManager.Instance.Data.CurrentSongIndex;
                IngameManager.Instance.Data.Songs.Add(IngameManager.Instance.Data.CurrentSongIndex++, song);
                song.SetAsEarned();
            }
            IngameManager.Instance.Data.Money += 50;
            await Panel3.Appear_C();
            await new WaitUntil(() => nextClicked);
            nextClicked = false;
            OpeningPanel.SetActive(false);
        }

        private void Update()
        {
            NameDisplayText.text = $"\"{NameInput.text}\"";
        }

        public void Click()
        {
            nextClicked = true;
        }
    }
}