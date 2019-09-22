using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class textread : MonoBehaviour
{

    public GameObject spinebase;
    public GameObject spinemid;
    public GameObject neck;
    public GameObject head;
    public GameObject shoulderleft;
    public GameObject elbowleft;
    public GameObject wristleft;
    public GameObject handleft;
    public GameObject shoulderright;
    public GameObject elbowright;
    public GameObject wristright;
    public GameObject handright;
    public GameObject hipleft;
    public GameObject kneeleft;
    public GameObject ankleleft;
    public GameObject footleft;
    public GameObject hipright;
    public GameObject kneeright;
    public GameObject ankleright;
    public GameObject footright;
    public GameObject spineshoulder;
    public GameObject handtipleft;
    public GameObject thumbleft;
    public GameObject handtipright;
    public GameObject thumbright;


    public TextAsset AT = Resources.Load("AT") as TextAsset;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(move());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator move()
    {


        string[] records = AT.text.Split('\n');

        for (int i = 0; i < records.Length; i++)
        {
            string[] c = records[i].Split(',');
            for (int k = 0; k < 75; k++)
            {
                //  print(float.Parse(c[k]));
            }


            spinebase.transform.position = new Vector3(float.Parse(c[0]), float.Parse(c[1]), float.Parse(c[2]));
            spinemid.transform.position = new Vector3(float.Parse(c[3]), float.Parse(c[4]), float.Parse(c[5]));
            neck.transform.position = new Vector3(float.Parse(c[6]), float.Parse(c[7]), float.Parse(c[8]));
            head.transform.position = new Vector3(float.Parse(c[9]), float.Parse(c[10]), float.Parse(c[11]));
            shoulderleft.transform.position = new Vector3(float.Parse(c[12]), float.Parse(c[13]), float.Parse(c[14]));
            elbowleft.transform.position = new Vector3(float.Parse(c[15]), float.Parse(c[16]), float.Parse(c[17]));
            wristleft.transform.position = new Vector3(float.Parse(c[18]), float.Parse(c[19]), float.Parse(c[20]));
            handleft.transform.position = new Vector3(float.Parse(c[21]), float.Parse(c[22]), float.Parse(c[23]));
            shoulderright.transform.position = new Vector3(float.Parse(c[24]), float.Parse(c[25]), float.Parse(c[26]));
            elbowright.transform.position = new Vector3(float.Parse(c[27]), float.Parse(c[28]), float.Parse(c[29]));
            wristright.transform.position = new Vector3(float.Parse(c[30]), float.Parse(c[31]), float.Parse(c[32]));
            handright.transform.position = new Vector3(float.Parse(c[33]), float.Parse(c[34]), float.Parse(c[35]));
            hipleft.transform.position = new Vector3(float.Parse(c[36]), float.Parse(c[37]), float.Parse(c[38]));
            kneeleft.transform.position = new Vector3(float.Parse(c[39]), float.Parse(c[40]), float.Parse(c[41]));
            ankleleft.transform.position = new Vector3(float.Parse(c[42]), float.Parse(c[43]), float.Parse(c[44]));
            footleft.transform.position = new Vector3(float.Parse(c[45]), float.Parse(c[46]), float.Parse(c[47]));
            hipright.transform.position = new Vector3(float.Parse(c[48]), float.Parse(c[49]), float.Parse(c[50]));
            kneeright.transform.position = new Vector3(float.Parse(c[51]), float.Parse(c[52]), float.Parse(c[53]));
            ankleright.transform.position = new Vector3(float.Parse(c[54]), float.Parse(c[55]), float.Parse(c[56]));
            footright.transform.position = new Vector3(float.Parse(c[57]), float.Parse(c[58]), float.Parse(c[59]));
            spineshoulder.transform.position = new Vector3(float.Parse(c[60]), float.Parse(c[61]), float.Parse(c[62]));
            handtipleft.transform.position = new Vector3(float.Parse(c[63]), float.Parse(c[64]), float.Parse(c[65]));
            thumbleft.transform.position = new Vector3(float.Parse(c[66]), float.Parse(c[67]), float.Parse(c[68]));
            handtipright.transform.position = new Vector3(float.Parse(c[69]), float.Parse(c[70]), float.Parse(c[71]));
            thumbright.transform.position = new Vector3(float.Parse(c[72]), float.Parse(c[73]), float.Parse(c[74]));

            yield return new WaitForSeconds(0.1F);
            if (i >= records.Length)
            {
                break;
            }

        }






    }
}