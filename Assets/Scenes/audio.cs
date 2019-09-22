using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{


    public AudioClip MusicClip;

    public AudioSource MusicSource; // 먼저 만들어 놓고, unity에서 연결함.

  
       void Start()
       {
           MusicSource.clip = MusicClip;

       }

      void Update()
       {
           if (Input.GetKeyDown(KeyCode.Space))
               MusicSource.Play();
       }
    
}

