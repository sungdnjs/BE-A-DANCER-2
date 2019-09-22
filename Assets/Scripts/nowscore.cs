using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nowscore : MonoBehaviour
{

    public Text Score;//대문자 주의
    string score;//소문자





    private void Start()
    {
        Score = GetComponent<Text>();


    }
    private void Update()
    {




        if (BodySourceView.love == 1)//퍼펙트
        {
            StartCoroutine(nowplay());

        }

        if (BodySourceView.love == 2)//굿
        {
            StartCoroutine(nowplay());
        }

        if (BodySourceView.love == 3)//배드
        {
            StartCoroutine(nowplay());
        }

        if (BodySourceView.love == 4)//컴페어가 스튜핏
        {
            StartCoroutine(nowplay());
        }

        //stupid일때도 점수는 떠야하므로!
        
                if (BodySourceView.love == 5 || BodySourceView.love == 6 || BodySourceView.love == 7 || BodySourceView.love == 8 || BodySourceView.love == 9)
                {
                    StartCoroutine(nowplay());
                }
        StopAllCoroutines();

    }





    public IEnumerator nowplay()
    {
        while (true)
        {
            Score.text = " " + BodySourceView.avg;

            yield return null;
        }
    }





}