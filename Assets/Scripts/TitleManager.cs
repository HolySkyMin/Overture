using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayTitle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneChanger.Instance.ChangeScene("IngameMainScene");
    }

    public void ToggleFullscreen()
    {
        if (Screen.fullScreen)
            Screen.SetResolution(1600, 900, false);
        else
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
