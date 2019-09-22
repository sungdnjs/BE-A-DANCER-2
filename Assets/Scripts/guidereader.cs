using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class guidereader : MonoBehaviour //이부분은 viewpoint에서 guide 그냥 저장된거 불러온거 써먹을려고 한 부분이다.
{
    public static float[,] guide1 = new float[120, 30];
    string[] c1 = new string[30]; //c1은 string으로 해놓으면 안되는 부분이었다.
    public TextAsset g1txt;
    public TextAsset g2txt;
    public TextAsset g3txt;


    // Use this for initialization
    void Start()
    {
        read();
    }


    public void read()
    {

        if (song_id.song_num == 1)
        {
            string[] records1 = g1txt.text.Split('\n');

            for (int i = 0; i < records1.Length; i++)
            {
                c1 = records1[i].Split(',');

                for (int k = 0; k < c1.Length - 1; k++)////////////////////////////////////////-1을 꼭 해주어야해.
                {
                    guide1[i, k] = float.Parse(c1[k]);


                }

            }
        }
        else if (song_id.song_num == 2)
        {
            string[] records1 = g2txt.text.Split('\n');


            for (int i = 0; i < records1.Length; i++)
            {

                c1 = records1[i].Split(',');

                for (int k = 0; k < c1.Length - 1; k++)////////////////////////////////////////-1을 꼭 해주어야해.
                {
                    guide1[i, k] = float.Parse(c1[k]);

                }

            }
        }
        else if (song_id.song_num == 3)
        {
            string[] records1 = g3txt.text.Split('\n');


            for (int i = 0; i < records1.Length; i++)
            {

                c1 = records1[i].Split(',');

                for (int k = 0; k < c1.Length - 1; k++)////////////////////////////////////////-1을 꼭 해주어야해.
                {
                    guide1[i, k] = float.Parse(c1[k]);

                }

            }
        }


    }
}




