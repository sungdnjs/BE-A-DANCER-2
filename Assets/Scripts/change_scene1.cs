using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Scene 관리

public class change_scene1 : MonoBehaviour
{

    public string sceneName;

    public void change_scene()
    {
        SceneManager.LoadScene(sceneName);
        //SceneName의 scene으로 이동한다.
    }
}