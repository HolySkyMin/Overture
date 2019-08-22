using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    public Animator Motion;

    private void Awake()
    {
        Instance = this;
    }

    public async void ChangeScene(string sceneName)
    {
        Motion.Play("SceneFadeIn");
        await new WaitForSeconds(0.33f);
        await SceneManager.LoadSceneAsync(sceneName);
    }
}
