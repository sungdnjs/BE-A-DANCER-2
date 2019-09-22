using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class arm_L : MonoBehaviour
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




        if (BodySourceView.arm_L == 1)//퍼펙트
        {
            StartCoroutine(arm_stupid());

        }

        else if(BodySourceView.arm_L == 0)
        {
            StartCoroutine(arm_clear());
        }


     
            StopAllCoroutines();
    }

    public IEnumerator arm_stupid()
    {
        while (true)
        {
            Score.text = "attention! left arm";

            yield return null;
        }
    }

    public IEnumerator arm_clear()
    {
        while (true)
        {
            Score.text = " ";

            yield return null;
        }
    }

}