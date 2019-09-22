using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nowscore_hart : MonoBehaviour
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
            StartCoroutine(hart());

        }

        if (BodySourceView.love == 2)//굿
        {
            StartCoroutine(hart());
        }

        if (BodySourceView.love == 3)//배드
        {
            StartCoroutine(hart());
        }

        if (BodySourceView.love == 4)//컴페어가 스튜핏
        {
            StartCoroutine(hart());
        }

        //stupid일때도 점수(하트)는 떠야하므로!
        if (BodySourceView.love == 5 || BodySourceView.love == 6 || BodySourceView.love == 7 || BodySourceView.love == 8 || BodySourceView.love == 9)
        {
            StartCoroutine(hart());
        }
        StopAllCoroutines();

    }





    public IEnumerator hart()
    {
        while (true)
        {
            if (BodySourceView.avg > 100)
                Score.text = "♥♥♥♥♥♥♥♥♥♥";
            else if (BodySourceView.avg > 90)
                Score.text = "♥♥♥♥♥♥♥♥♥";
            else if (BodySourceView.avg > 80)
                Score.text = "♥♥♥♥♥♥♥♥";
            else if (BodySourceView.avg > 70)
                Score.text = "♥♥♥♥♥♥♥";
            else if (BodySourceView.avg > 60)
                Score.text = "♥♥♥♥♥♥";
            else if (BodySourceView.avg > 50)
                Score.text = "♥♥♥♥♥";
            else if (BodySourceView.avg > 40)
                Score.text = "♥♥♥♥";
            else if (BodySourceView.avg > 30)
                Score.text = "♥♥";
            else if (BodySourceView.avg > 20)
                Score.text = "♥";
            else
                Score.text = "♥";
            yield return null;
        }
    }





}