using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptedAnimation;

namespace Idol
{
    public class IdolCard : MonoBehaviour
    {
        [Header("Name and Image")]
        public Text NameText;
        public Image IdolImage;
        [Header("Main Properties")]
        public Text VocalText;
        public Text DanceText, VisualText, VarietyText;
        public Slider VocalSlider, DanceSlider, VisualSlider, VarietySlider;
        [Header("Sub Properties")]
        public Text CostText;
        public Text HonorText, FanText, PersonaText;
        [Header("Animating")]
        public ScriptAnimation Motion;

        [HideInInspector]
        public IdolData LinkedIdol;

        private void OnEnable()
        {
            if(Motion != null)
                Motion.Appear();

            CostText.text = LinkedIdol.Cost.ToString();
            NameText.text = LinkedIdol.Name;
            IdolImage.sprite = Resources.Load<Sprite>(LinkedIdol.ImageKey);
            VocalSlider.value = LinkedIdol.Vocal;
            VocalText.text = LinkedIdol.Vocal.ToString();
            DanceSlider.value = LinkedIdol.Dance;
            DanceText.text = LinkedIdol.Dance.ToString();
            VisualSlider.value = LinkedIdol.Visual;
            VisualText.text = LinkedIdol.Visual.ToString();
            VarietySlider.value = LinkedIdol.Variety;
            VarietyText.text = LinkedIdol.Variety.ToString();
            HonorText.text = LinkedIdol.Honor.ToString();
            FanText.text = LinkedIdol.Fan.ToString();
            PersonaText.text = IdolData.PersonaStringDic[LinkedIdol.Personality];
        }

        public void SetIdol(IdolData data)
        {
            LinkedIdol = data;

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
            PersonaText.text = IdolData.PersonaStringDic[LinkedIdol.Personality];
        }
    }
}