using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoremanager : MonoBehaviour
{

    public Text A;//대문자 주의
    int a = 0;//소문자

    public static scoremanager instance;

    void Awake()
    {
        if(scoremanager.instance == null)
        {
            scoremanager.instance = this;
        }

        A.text = "Total Score =" + BodySourceView.avg.ToString();
    }



   



}