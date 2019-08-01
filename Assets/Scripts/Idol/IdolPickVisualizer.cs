using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Idol
{
    public class IdolPickVisualizer : MonoBehaviour
    {
        public Text IdolCount;
        public IdolPickVisualElement[] IdolSlotVisual;

        public void Visualize(IdolPickGroup slot)
        {
            IdolCount.text = $"{slot.Count}/{slot.Capacity}";
            for (int i = 0; i < IdolSlotVisual.Length; i++)
            {
                if (i < slot.Capacity)
                {
                    IdolSlotVisual[i].gameObject.SetActive(true);
                    if (slot.IdolIndices[i] != -1)
                        IdolSlotVisual[i].SetActive(true);
                    else
                        IdolSlotVisual[i].SetActive(false);
                }
                else
                    IdolSlotVisual[i].gameObject.SetActive(false);
            }
        }
    }
}