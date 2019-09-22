using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class left1 : MonoBehaviour
{

    public Text Score;//대문자 주의
    string score;//소문자


    private void Start()
    {
        Score = GetComponent<Text>();


    }
    // Use this for initialization
    private void Update()
    {



        if (BodySourceView.leg_L == 1)//퍼펙트
        {
            StartCoroutine(stupid());

        }

        else if (BodySourceView.leg_L == 0)
        {
            StartCoroutine(clear());
        }



        StopAllCoroutines();
    }

    public IEnumerator stupid()
    {
        while (true)
        {
            Score.text = "CAUTION!";

            yield return null;
        }
    }

    public IEnumerator clear()
    {
        while (true)
        {
            Score.text = " ";

            yield return null;
        }
    }

}