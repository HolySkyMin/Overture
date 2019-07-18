using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idol;

namespace Ingame
{
    public class IngameManager : MonoBehaviour
    {
        public static IngameManager Instance { get; private set; }

        [HideInInspector]
        public IngameData Data;

        private void Awake()
        {
            Instance = this;

            // Debug 190719
            Data = new IngameData();
            Data.Idols = new List<IdolData>();

            var idols = IdolData.Generate(20);
            Debug.Log(idols.Length);
            foreach (var idol in idols)
                Data.Idols.Add(idol);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Debug 190719
            foreach (var idol in Data.Idols)
                IdolPicker.Instance.LoadCard(idol);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public async void IdolPickerTest()
        {
            var pickRes = await IdolPicker.Instance.Show(5);
            Debug.Log(pickRes.ToString());
        }
    }
}