using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{


    public AudioClip MusicClip;

    public AudioSource MusicSource; // 먼저 만들어 놓고, unity에서 연결함.

    

    void Start()
    {
        MusicSource.clip = MusicClip;// 시작이 되면 연결되고,

    }
    public void button_click()
    {

        MusicSource.Play();// 클릭하면 시작함.
    }
    /*
       void Start()
       {
           MusicSource.clip = MusicClip;

       }

      void Update()
       {
           if (Input.GetKeyDown(KeyCode.Space))
               MusicSource.Play();
       }
     */
}
