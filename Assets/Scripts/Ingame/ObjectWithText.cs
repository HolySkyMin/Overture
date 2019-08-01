using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class ObjectWithText : MonoBehaviour
    {
        public Text text;

        public void SetActive(bool flag)
        {
            gameObject.SetActive(flag);
        }
    }
}