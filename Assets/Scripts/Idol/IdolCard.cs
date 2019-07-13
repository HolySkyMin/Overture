using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Idol
{
    public class IdolCard : MonoBehaviour
    {
        [Header("Name and Image")]
        public Text NameText;
        public Image IdolImage;
        [Header("Main Properties")]
        public Text VocalText, DanceText, VisualText, VarietyText;
        public Slider VocalSlider, DanceSlider, VisualSlider, VarietySlider;
        [Header("Sub Properties")]
        public Text CostText, HonorText, FanText, PersonaText;
        [HideInInspector]
        private IdolData linkedIdol;

        public void SetIdol(IdolData data)
        {
            linkedIdol = data;

            CostText.text = data.Cost.ToString();
            NameText.text = data.Name;
            IdolImage.sprite = Resources.Load<Sprite>(data.ImageKey);
            VocalSlider.value = data.Vocal;
            VocalText.text = data.Vocal.ToString();
            DanceSlider.value = data.Dance;
            DanceText.text = data.Dance.ToString();
            VisualSlider.value = data.Visual;
            VisualText.text = data.Visual.ToString();
            VarietySlider.value = data.Variety;
            VarietyText.text = data.Variety.ToString();
            HonorText.text = data.Honor.ToString();
            FanText.text = data.Fan.ToString();
            // Personality
        }
    }
}