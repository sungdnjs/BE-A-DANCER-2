using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class manager : MonoBehaviour
{

    public Text Score;//대문자 주의
    string score;//소문자

    //public static manager instance;

    //public float miTime = 5f;



    private void Start()
    {
        Score = GetComponent<Text>();


    }
    private void Update()
    {


        if (BodySourceView.SM >= 1 && BodySourceView.SM <= 3)
        {
            StartCoroutine(loading());
            StopAllCoroutines();
        }

        if (BodySourceView.love == 1)//퍼펙트
        {
            StartCoroutine(perfectscore());

        }

        if (BodySourceView.love == 2)//굿
        {
            StartCoroutine(goodscore());
        }

        if (BodySourceView.love == 3)//배드
        {
            StartCoroutine(badscore());
        }

        if (BodySourceView.love == 4)//컴페어가 스튜핏
        {
            StartCoroutine(stupidscore());
        }
        
        if (BodySourceView.love == 5 || BodySourceView.love == 6 || BodySourceView.love == 7 || BodySourceView.love == 8 || BodySourceView.love == 9)
        {
            StartCoroutine(verystupidscore());
        }


        StopAllCoroutines();

    }





    public IEnumerator perfectscore()
    {
        while (true)
        {
            try
            {
                Score.text = " PERFECT ";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Text was not set in the inspector");
            }

            yield return null;
        }
    }

    public IEnumerator goodscore()
    {
       
        while (true)
        {
            try
            {
                Score.text = " GOOD ";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Text was not set in the inspector");
            }

            yield return null;
        }
    }

    public IEnumerator badscore()
    {
        while (true)
        {
            try
            {
                Score.text = " BAD ";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Text was not set in the inspector");
            }

            yield return null;
        }
    }

    public IEnumerator stupidscore()
    {
        while (true)
        {
            try
            {
                Score.text = " OOPS!  ";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Text was not set in the inspector");
            }

            yield return null;
        }
    }

    public IEnumerator verystupidscore()
    {
        while (true)
        {
            try
            {
                Score.text = " STUPID ";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Text was not set in the inspector");
            }

            yield return null;
        }
    }

    public IEnumerator loading()
    {
        while (true)
        {
            try
            {
                Score.text = "loading ";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Text was not set in the inspector");
            }

            yield return null;
        }
    }


}