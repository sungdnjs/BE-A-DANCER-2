using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class head : MonoBehaviour
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

        /*


        if (BodySourceView.head == 1)//퍼펙트
        {
            StartCoroutine(head_stupid());

        }
        else if (BodySourceView.head == 0)
        {
            StartCoroutine(head_clear());
        }
        */


        StopAllCoroutines();

    }

    public IEnumerator head_stupid()
    {
        while (true)
        {
            Score.text = "attention! head!";

            yield return null;
        }
    }
    public IEnumerator head_clear()
    {
        while (true)
        {
            Score.text = " ";

            yield return null;
        }
    }
}