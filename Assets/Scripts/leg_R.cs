using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class leg_R : MonoBehaviour
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




        if (BodySourceView.leg_R == 1)//퍼펙트
        {
            StartCoroutine(leg_stupid2());

        }
        else if (BodySourceView.leg_R == 0)
        {
            StartCoroutine(leg_clear2());
        }



        StopAllCoroutines();

    }

    public IEnumerator leg_stupid2()
    {
        while (true)
        {
            Score.text = "attention! rignt leg!";

            yield return null;
        }
    }
    public IEnumerator leg_clear2()
    {
        while (true)
        {
            Score.text = " ";

            yield return null;
        }
    }
}