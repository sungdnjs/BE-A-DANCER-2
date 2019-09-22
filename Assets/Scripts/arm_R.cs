using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class arm_R : MonoBehaviour
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




        if (BodySourceView.arm_R == 1)//퍼펙트
        {
            StartCoroutine(arm_stupid2());

        }
        else if (BodySourceView.arm_R == 0)
        {
            StartCoroutine(arm_clear2());
        }



        StopAllCoroutines();

    }

    public IEnumerator arm_stupid2()
    {
        while (true)
        {
            Score.text = "attention! rignt arm!";

            yield return null;
        }
    }
    public IEnumerator arm_clear2()
    {
        while (true)
        {
            Score.text = " ";

            yield return null;
        }
    }
}