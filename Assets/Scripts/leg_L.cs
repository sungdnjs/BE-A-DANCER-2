using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class leg_L : MonoBehaviour
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
            StartCoroutine(leg_stupid());

        }
        else if (BodySourceView.leg_L == 0)
        {
            StartCoroutine(leg_clear());
        }



        StopAllCoroutines();

    }

    public IEnumerator leg_stupid()
    {
        while (true)
        {
            Score.text = "attention! left leg!";

            yield return null;
        }
    }
    public IEnumerator leg_clear()
    {
        while (true)
        {
            Score.text = " ";

            yield return null;
        }
    }
}