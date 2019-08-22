using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTitleManager : MonoBehaviour
{
    public void GoToTitle()
    {
        SceneChanger.Instance.ChangeScene("TitleScene");
    }
}
