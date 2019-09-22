using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;



public class love1 : MonoBehaviour
{
  
 
    public void Click(string tag)
    {

       /* BodySourceView ddd = GameObject.Find("int").GetComponent<BodySourceView>();
        ddd.frame_count = 0;
        */

      //필요  Debug.Log(tag + "222:" + DateTime.Now.ToString("hh:mm:ss:fff"));// tag는 뭘까..

        

        var fileName = "test1.txt";///aaaa의 txt파일을 만듬
        if (File.Exists(fileName))
        {
            using (StreamWriter sr = File.AppendText(fileName))// 두번째부터는 존재하니까 그거에다가 씀.
            {
                       
                        sr.Write("love ");
                  
               
                sr.Close();

            }
        }
        else
        {


            using (StreamWriter sr = File.CreateText(fileName))// 처음에만 실행됨!
            {
                sr.Write("love ");
                sr.Close();
            }

        }


    }

}
