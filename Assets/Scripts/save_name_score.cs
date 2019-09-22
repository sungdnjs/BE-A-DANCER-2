using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class save_name_score : MonoBehaviour
{

    public InputField inputName;
    public int total_score;

    static public int index1 = 0;//몇명 저장되어있는지 초기값. 현재 저장된 사용자수 0명, nonono
    static public int index2 = 0;//국민체조
    static public int index3 = 0;//3번째노래

    //만약 index값이 4면 현재 저장된 사용자 4명

    public int index_v = 0;//인덱스를 +1로 바꿔야하냐 말아야하냐, 이 값이 1이면 바꿔라, 0이면 안바꾸기

    public int save_i1;//nonono
    public int save_i2;//국민체조
    public int save_i3;

    public void Save()
    {
        ////몇번째 노래냐 정보 받음
        if (song_id.song_num == 1)//NONONO일때
        {
            index1 = PlayerPrefs.GetInt("index1");//현재 몇명 저장되어있는지 가져옴


            for (save_i1 = 0; save_i1 <= index1; save_i1++)//나랑 같은 이름이 있는지 확인하자
            {
                //print(inputName.text);
                //print(PlayerPrefs.GetString("1name" + save_i1));

                if ((inputName.text == PlayerPrefs.GetString("1name" + save_i1)) && (save_i1 < index1))//랭킹중에 나랑 이름이 같은 게 나오면 
                {
                    PlayerPrefs.SetInt("1score" + save_i1, total_score);//점수 갱신
                    break;
                }


                else if (save_i1 == index1) //랭킹 중에 이름이 나랑 다 다른 경우 새로 저장
                {
                    index_v = 1;
                    total_score = BodySourceView.avg;
                    PlayerPrefs.SetString("1name" + (index1), inputName.text);
                    PlayerPrefs.SetInt("1score" + (index1), total_score);


                }

            }
            if (index_v == 1)
            {
                index1++;
                PlayerPrefs.SetInt("index1", index1);
                index_v = 0;
            }
        }

        else if (song_id.song_num == 2) //국민체조일때
        {
            index2 = PlayerPrefs.GetInt("index2");//현재 몇명 저장되어있는지 가져옴

            for (save_i2 = 0; save_i2 <= index2; save_i2++)//나랑 같은 이름이 있는지 확인하자
            {
                //print(inputName.text);
                //print(PlayerPrefs.GetString("2name" + save_i2));

                if ((inputName.text == PlayerPrefs.GetString("2name" + save_i2)) && (save_i2 < index2))//랭킹중에 나랑 이름이 같은 게 나오면 
                {
                    PlayerPrefs.SetInt("2score" + save_i2, total_score);//점수 갱신
                    break;
                }

                else if (save_i2 == index2) //랭킹 중에 이름이 나랑 다 다른 경우 새로 저장
                {
                    index_v = 1;
                    total_score = BodySourceView.avg;
                    PlayerPrefs.SetString("2name" + (index2), inputName.text);
                    PlayerPrefs.SetInt("2score" + (index2), total_score);

                }

            }
            if (index_v == 1)
            {
                index2++;
                PlayerPrefs.SetInt("index2", index2);
                index_v = 0;
            }


        }


        else if (song_id.song_num == 3) //세번째 노래일때
        {
            index3 = PlayerPrefs.GetInt("index3");//현재 몇명 저장되어있는지 가져옴

            for (save_i3 = 0; save_i3 <= index3; save_i3++)//나랑 같은 이름이 있는지 확인하자
            {
                //print(inputName.text);
                //print(PlayerPrefs.GetString("3name" + save_i3));

                if ((inputName.text == PlayerPrefs.GetString("3name" + save_i3)) && (save_i3 < index3))//랭킹중에 나랑 이름이 같은 게 나오면 
                {
                    PlayerPrefs.SetInt("3score" + save_i3, total_score);//점수 갱신
                    break;
                }


                else if (save_i3 == index3) //랭킹 중에 이름이 나랑 다 다른 경우 새로 저장
                {
                    index_v = 1;
                    total_score = BodySourceView.avg;
                    PlayerPrefs.SetString("3name" + (index3), inputName.text);
                    PlayerPrefs.SetInt("3score" + (index3), total_score);
                }

            }
            if (index_v == 1)
            {
                index3++;
                PlayerPrefs.SetInt("index3", index3);
                index_v = 0;
            }

        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("index1", index1);
        PlayerPrefs.SetInt("index2", index2);
        PlayerPrefs.SetInt("index3", index3);


    }
    
    /* void Start()
     {
         PlayerPrefs.DeleteAll();
         PlayerPrefs.SetInt("index1", index1);
         PlayerPrefs.SetInt("index2", index2);
         PlayerPrefs.SetInt("index3", index3);
     }*/

     
}
