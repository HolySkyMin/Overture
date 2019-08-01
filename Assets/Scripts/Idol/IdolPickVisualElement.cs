using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idol
{
    public class IdolPickVisualElement : MonoBehaviour
    {
        public GameObject FillObject;

        public void SetActive(bool flag)
        {
            FillObject.SetActive(flag);
        }
    }
}