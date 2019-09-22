using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class load_ranking : MonoBehaviour
{

    public Text[] loadName;
    public Text[] loadScore;


    int rank_i;

    //tmp_rank를 index1,2,3중에 가장 큰 값을 갖도록 설정
    int[] tmp_rank = new int[Mathf.Max(save_name_score.index1, save_name_score.index2, save_name_score.index3)];//디폴트 값은 0


    public void Sort()//랭킹을 매길 함수
    {

        if (song_id.song_num == 1)//NONONO일때
        {
            for (int i = 0; i < save_name_score.index1; i++)
            {
                if (PlayerPrefs.HasKey("1name" + i))
                {
                    for (int j = 0; j < save_name_score.index1; j++)
                    {
                        if (PlayerPrefs.GetInt("1score" + i) < PlayerPrefs.GetInt("1score" + j))
                        {
                            ++tmp_rank[i];
                        }
                        else if (PlayerPrefs.GetInt("1score" + i) == PlayerPrefs.GetInt("1score" + j))
                        {
                            if (i >= j)//나 자신이거나 동점자인데 나보나 늦게 춘 사용자는 등수를 ++
                            {
                                ++tmp_rank[i];
                            }
                        }
                    }


                    switch (tmp_rank[i])
                    {
                        case 1:
                            loadName[0].text = PlayerPrefs.GetString("1name" + i);
                            loadScore[0].text = PlayerPrefs.GetInt("1score" + i) + " ";
                            break;
                        case 2:
                            loadName[1].text = PlayerPrefs.GetString("1name" + i);
                            loadScore[1].text = PlayerPrefs.GetInt("1score" + i) + " ";
                            break;
                        case 3:
                            loadName[2].text = PlayerPrefs.GetString("1name" + i);
                            loadScore[2].text = PlayerPrefs.GetInt("1score" + i) + " ";
                            break;
                        case 4:
                            loadName[3].text = PlayerPrefs.GetString("1name" + i);
                            loadScore[3].text = PlayerPrefs.GetInt("1score" + i) + " ";
                            break;
                        case 5:
                            loadName[4].text = PlayerPrefs.GetString("1name" + i);
                            loadScore[4].text = PlayerPrefs.GetInt("1score" + i) + " ";
                            break;
                    }
                }
                else { break; }
            }

        }
        else if (song_id.song_num == 2)
        {
            for (int i = 0; i < save_name_score.index2; i++)
            {
                if (PlayerPrefs.HasKey("2name" + i))
                {
                    for (int j = 0; j < save_name_score.index2; j++)
                    {
                        if (PlayerPrefs.GetInt("2score" + i) < PlayerPrefs.GetInt("2score" + j))
                        {
                            ++tmp_rank[i];
                        }
                        else if (PlayerPrefs.GetInt("2score" + i) == PlayerPrefs.GetInt("2score" + j))
                        {
                            if (i >= j)
                            {
                                ++tmp_rank[i];
                            }
                        }
                    }


                    switch (tmp_rank[i])
                    {
                        case 1:
                            loadName[0].text = PlayerPrefs.GetString("2name" + i);
                            loadScore[0].text = PlayerPrefs.GetInt("2score" + i) + " ";
                            break;
                        case 2:
                            loadName[1].text = PlayerPrefs.GetString("2name" + i);
                            loadScore[1].text = PlayerPrefs.GetInt("2score" + i) + " ";
                            break;
                        case 3:
                            loadName[2].text = PlayerPrefs.GetString("2name" + i);
                            loadScore[2].text = PlayerPrefs.GetInt("2score" + i) + " ";
                            break;
                        case 4:
                            loadName[3].text = PlayerPrefs.GetString("2name" + i);
                            loadScore[3].text = PlayerPrefs.GetInt("2score" + i) + " ";
                            break;
                        case 5:
                            loadName[4].text = PlayerPrefs.GetString("2name" + i);
                            loadScore[4].text = PlayerPrefs.GetInt("2score" + i) + " ";
                            break;
                    }
                }
                else { break; }
            }

        }
        else if (song_id.song_num == 3)
        {
            for (int i = 0; i < save_name_score.index3; i++)
            {
                if (PlayerPrefs.HasKey("3name" + i))
                {
                    for (int j = 0; j < save_name_score.index3; j++)
                    {
                        if (PlayerPrefs.GetInt("3score" + i) < PlayerPrefs.GetInt("3score" + j))
                        {
                            ++tmp_rank[i];
                        }
                        else if (PlayerPrefs.GetInt("3score" + i) == PlayerPrefs.GetInt("3score" + j))
                        {
                            if (i >= j)
                            {
                                ++tmp_rank[i];
                            }
                        }
                    }


                    switch (tmp_rank[i])
                    {
                        case 1:
                            loadName[0].text = PlayerPrefs.GetString("3name" + i);
                            loadScore[0].text = PlayerPrefs.GetInt("3score" + i) + " ";
                            break;
                        case 2:
                            loadName[1].text = PlayerPrefs.GetString("3name" + i);
                            loadScore[1].text = PlayerPrefs.GetInt("3score" + i) + " ";
                            break;
                        case 3:
                            loadName[2].text = PlayerPrefs.GetString("3name" + i);
                            loadScore[2].text = PlayerPrefs.GetInt("3score" + i) + " ";
                            break;
                        case 4:
                            loadName[3].text = PlayerPrefs.GetString("3name" + i);
                            loadScore[3].text = PlayerPrefs.GetInt("3score" + i) + " ";
                            break;
                        case 5:
                            loadName[4].text = PlayerPrefs.GetString("3name" + i);
                            loadScore[4].text = PlayerPrefs.GetInt("3score" + i) + " ";
                            break;
                    }
                }
                else { break; }

            }
        }


    }

}
