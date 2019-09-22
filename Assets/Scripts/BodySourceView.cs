using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.IO;
using UnityEngine.UI;
using System;


public class BodySourceView : MonoBehaviour
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    static int frame_count = 47;// 한 프레임에 24개의 좌표값인데 앞프레임부터 가져오기 위해서 24에서 47로 수정.
    static int frame = 0;//프레임 수
    static double range1 = 2.0;
   // static double st_range = 50;// 완전히 틀렸을 때, 비교기준값(머리)
   // static double st_rangeal = 20;//왼팔
  //  static double st_rangear = 20;//오른팔
    static double st_rangell = 15;//왼다리
    static double st_rangelr = 15;//오른다리

    static float[,] jarray = new float[15, 2];///쓰는 조인트가15개
    static int i = 0;
    static int j = 0;
    static int pare = 0;
    static int pare1 = 0;
    static int pare2 = 0;
    public static double sum, good = 0;
    public static int avg= 0;
    static int total = 0;//frame 확인용
    public static int love = 0;
    public static int arm_L, arm_R, leg_L, leg_R, head = 0;
    public static int SM = 0;
    public static int st_love0 = 0;
    public static int st_love1 = 0;
    public static int st_love2 = 0;
    public static int st_love3 = 0;
    public static int st_love4 = 0;
    public static int st_love5 = 0;
    public static int st_love6 = 0;
    public static int st_love7 = 0;
    public static int st_love8 = 0;
    public static int st_love9 = 0;

    // 완전히 틀렸을 때




    void Update()
    {
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }

    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.5f, 0.5f);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));




                if (viewpoint.abcd > viewpoint.point && viewpoint.point != 0)// 이부분으로 클릭했을 때, 적용되도록 함
                {
                    if (frame_count <= 47 || (frame_count >= 696 && frame_count <= 719))
                    {

                        if (jt == Kinect.JointType.SpineBase || jt == Kinect.JointType.Neck || jt == Kinect.JointType.ElbowLeft || jt == Kinect.JointType.WristLeft || jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.ElbowRight || jt == Kinect.JointType.WristRight || jt == Kinect.JointType.HandRight || jt == Kinect.JointType.KneeLeft || jt == Kinect.JointType.AnkleLeft || jt == Kinect.JointType.FootLeft || jt == Kinect.JointType.KneeRight || jt == Kinect.JointType.AnkleRight || jt == Kinect.JointType.FootRight || jt == Kinect.JointType.SpineShoulder)
                        {//조인트 추가


                            jarray[i, j] = (int)hart(targetJoint.Value).x; ///////
                            jarray[i, j + 1] = (int)hart(targetJoint.Value).y;
                            // jarray[i, j + 2] = (int)hart(targetJoint.Value).z;


                            i++;

                        }

                        if (frame_count == 0)//여기서 비교해야 됨.(정확)
                        {
                            print("frame= " + frame);
                            frame_count = 720;
                            frame++;

                            //////////////////

                            //비교부분**(왼팔어깨 부분),정확할 때,

                            float g_sho_L = (guidereader.guide1[pare, 5] - guidereader.guide1[pare, 7]) / (guidereader.guide1[pare, 4] - guidereader.guide1[pare, 6]);//가이드
                            float p_sho_L = (jarray[2, 1] - jarray[3, 1]) / (jarray[2, 0] - jarray[3, 0]);//플레이어
                                                                                                          /* print("pare : " + pare);
                                                                                                           print("g_sho_L" + g_sho_L + "p_sho_L" + p_sho_L);//확인
                                                                                                           print("왼팔어깨 가이드 " + guidereader.guide1[pare, 5] + "헤헤" + guidereader.guide1[pare, 7] + "헤헤" + guidereader.guide1[pare, 4] + "헤헤" + guidereader.guide1[pare, 6]);//확인
                                                                                                           print("왼팔어깨 플레이어 " + jarray[2, 1] + "요기 " + jarray[3, 1] + "요기 " + jarray[2, 0] + "요기 " + jarray[3, 0]);//확인
                                                                                                           print("g_sho_L/p_sho_L : " + Math.Abs(g_sho_L / p_sho_L));
                                                                               */
                            if (Math.Abs(g_sho_L / p_sho_L) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_sho_L / p_sho_L) > st_rangeal)
                             {
                                 st_love0++;
                                 total++;
                                 print("stupid!!!!!");

                             }*/
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(오른팔어깨 부분),정확할 때,

                            float g_sho_R = (guidereader.guide1[pare, 11] - guidereader.guide1[pare, 13]) / (guidereader.guide1[pare, 10] - guidereader.guide1[pare, 12]);//가이드
                            float p_sho_R = (jarray[5, 1] - jarray[6, 1]) / (jarray[5, 0] - jarray[6, 0]);//플레이어
                                                                                                          /*    print("pare : " + pare);
                                                                                                              print("g_sho_R" + g_sho_R + "p_sho_R" + p_sho_R);//확인
                                                                                                              print("왼팔 가이드 " + guidereader.guide1[pare, 11] + "헤헤" + guidereader.guide1[pare, 13] + "헤헤" + guidereader.guide1[pare, 10] + "헤헤" + guidereader.guide1[pare, 12]);//확인
                                                                                                              print("왼팔 플레이어 " + jarray[5, 1] + "요기 " + jarray[6, 1] + "요기 " + jarray[5, 0] + "요기 " + jarray[6, 0]);//확인
                                                                                                              print("g_sho_R/p_sho_R : " + Math.Abs(g_sho_R / p_sho_R));
                                                                                                              */

                            if (Math.Abs(g_sho_R / p_sho_R) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_sho_R / p_sho_R) > st_rangear)
                             {
                                 st_love1++;
                                 total++;
                                 print("stupid!!!!!");
                             }*/
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(왼다리힙 부분),정확할 때,

                            float g_hip_L = (guidereader.guide1[pare, 17] - guidereader.guide1[pare, 19]) / (guidereader.guide1[pare, 16] - guidereader.guide1[pare, 18]);//가이드
                            float p_hip_L = (jarray[8, 1] - jarray[9, 1]) / (jarray[8, 0] - jarray[9, 0]);//플레이어
                                                                                                          /*  print("pare : " + pare);
                                                                                                            print("g_hip_L" + g_hip_L + "p_hip_L" + p_hip_L);//확인
                                                                                                            print("왼팔 가이드 " + guidereader.guide1[pare, 17] + "헤헤" + guidereader.guide1[pare, 19] + "헤헤" + guidereader.guide1[pare, 16] + "헤헤" + guidereader.guide1[pare, 18]);//확인
                                                                                                            print("왼팔 플레이어 " + jarray[8, 1] + "요기 " + jarray[9, 1] + "요기 " + jarray[8, 0] + "요기 " + jarray[9, 0]);//확인
                                                                                                            print("g_hip_L/p_hip_L : " + Math.Abs(g_hip_L / p_hip_L));
                                                                                                            */

                            if (Math.Abs(g_hip_L / p_hip_L) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_hip_L / p_hip_L) > st_rangell)
                            {
                                st_love2++;
                                total++;
                                print("stupid!!!!!");
                            }
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(오른다리힙 부분),정확할 때,

                            float g_hip_R = (guidereader.guide1[pare, 23] - guidereader.guide1[pare, 25]) / (guidereader.guide1[pare, 22] - guidereader.guide1[pare, 24]);//가이드
                            float p_hip_R = (jarray[11, 1] - jarray[12, 1]) / (jarray[11, 0] - jarray[12, 0]);//플레이어
                                                                                                              /*  print("pare : " + pare);
                                                                                                                print("g_hip_R" + g_hip_R + "p_hip_R" + p_hip_R);//확인
                                                                                                                print("왼팔 가이드 " + guidereader.guide1[pare, 23] + "헤헤" + guidereader.guide1[pare, 25] + "헤헤" + guidereader.guide1[pare, 22] + "헤헤" + guidereader.guide1[pare, 24]);//확인
                                                                                                                print("왼팔 플레이어 " + jarray[11, 1] + "요기 " + jarray[12, 1] + "요기 " + jarray[11, 0] + "요기 " + jarray[12, 0]);//확인
                                                                                                                print("g_hip_R/p_hip_R : " + Math.Abs(g_hip_R / p_hip_R));
                                                                                                                */

                            if (Math.Abs(g_hip_R / p_hip_R) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_hip_R / p_hip_R) > st_rangelr)
                            {
                                st_love3++;
                                total++;
                                print("stupid!!!!!");
                            }
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(머리와목 부분),정확할 때,////////틀림

                            float g_h_n = (guidereader.guide1[pare, 3] - guidereader.guide1[pare, 29]) / (guidereader.guide1[pare, 2] - guidereader.guide1[pare, 28]);//가이드
                            float p_h_n = (jarray[1, 1] - jarray[14, 1]) / (jarray[1, 0] - jarray[14, 0]);//플레이어
                                                                                                          /*   print("pare : " + pare);
                                                                                                             print("g_h_n" + g_h_n + "p_h_n" + p_h_n);//확인
                                                                                                             print("왼팔 가이드 " + guidereader.guide1[pare, 1] + "헤헤" + guidereader.guide1[pare, 29] + "헤헤" + guidereader.guide1[pare, 0] + "헤헤" + guidereader.guide1[pare, 28]);//확인
                                                                                                             print("왼팔 플레이어 " + jarray[1, 1] + "요기 " + jarray[14, 1] + "요기 " + jarray[1, 0] + "요기 " + jarray[14, 0]);//확인
                                                                                                             print("g_h_n/p_h_n : " + Math.Abs(g_h_n / p_h_n));
                                                                                                             */
                            if (Math.Abs(g_h_n / p_h_n) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_h_n / p_h_n) > st_range)
                             {
                                 st_love4++;
                                 total++;
                                 print("stupid!!!!!");
                             }*/
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분 목과 스파인미드
                            float g_n_m = (guidereader.guide1[pare, 29] - guidereader.guide1[pare, 1]) / (guidereader.guide1[pare, 28] - guidereader.guide1[pare, 0]);//가이드
                            float p_n_m = (jarray[14, 1] - jarray[0, 1]) / (jarray[14, 0] - jarray[0, 0]);//플레이어

                            /*  print("pare : " + pare);
                              print("g_n_m" + g_n_m + "p_h_m" + p_n_m);
                              print("머리 가이드 " + guidereader.guide1[pare, 29] + "헤헤" + guidereader.guide1[pare, 1] + "헤헤" + guidereader.guide1[pare, 28] + "헤헤" + guidereader.guide1[pare, 0]);
                              print("머리 플레이어 " + jarray[14, 1] + "요기 " + jarray[0, 1] + "요기 " + jarray[14, 0] + "요기 " + jarray[0, 0]);
                              print("g_n_m / p_n_m : " + Math.Abs(g_n_m / p_n_m));
                              */
                            if (Math.Abs(g_n_m / p_n_m) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_n_m / p_n_m) > st_range)
                             {
                                 st_love5++;
                                 total++;
                                 print("stupid!!!!!");
                             }*/
                            else
                            {
                                print("not");

                                total++;
                            }



                            //비교부분**(왼팔 부분),정확할 때,

                            float g_arm_L = (guidereader.guide1[pare, 7] - guidereader.guide1[pare, 9]) / (guidereader.guide1[pare, 6] - guidereader.guide1[pare, 8]);//가이드
                            float p_arm_L = (jarray[3, 1] - jarray[4, 1]) / (jarray[3, 0] - jarray[4, 0]);//플레이어
                                                                                                          /*  print("pare : " + pare);
                                                                                                            print("g_arm_L" + g_arm_L + "p_arm_L" + p_arm_L);//확인
                                                                                                            print("왼팔 가이드 " + guidereader.guide1[pare, 7] + "헤헤" + guidereader.guide1[pare, 9] + "헤헤" + guidereader.guide1[pare, 6] + "헤헤" + guidereader.guide1[pare, 8]);//확인
                                                                                                            print("왼팔 플레이어 " + jarray[3, 1] + "요기 " + jarray[4, 1] + "요기 " + jarray[3, 0] + "요기 " + jarray[4, 0]);//확인
                                                                                                            print("g_arm_L/p_arm_L : " + Math.Abs(g_arm_L / p_arm_L));
                                                                                                            */

                            if (Math.Abs(g_arm_L / p_arm_L) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_arm_L / p_arm_L) > st_rangeal)
                             {
                                 st_love6++;
                                 total++;
                                 print("stupid!!!!!");
                             }*/
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++; 마지막에 했음


                            //비교부분**(오른팔 부분), 정확할 때,
                            float g_arm_R = (guidereader.guide1[pare, 13] - guidereader.guide1[pare, 15]) / (guidereader.guide1[pare, 12] - guidereader.guide1[pare, 14]);
                            float p_arm_R = (jarray[6, 1] - jarray[7, 1]) / (jarray[6, 0] - jarray[7, 0]);
                            /*print("pare : " + pare);
                            print("g_arm_R" + g_arm_R + "p_arm_R" + p_arm_R);
                            print("오른팔 가이드 " + guidereader.guide1[pare, 13] + "헤헤" + guidereader.guide1[pare, 15] + "헤헤" + guidereader.guide1[pare, 12] + "헤헤" + guidereader.guide1[pare, 14]);
                            print("오른팔 플레이어 " + jarray[6, 1] + "요기 " + jarray[7, 1] + "요기 " + jarray[6, 0] + "요기 " + jarray[7, 0]);
                            print("g_arm_R / p_arm_R : " + Math.Abs(g_arm_R / p_arm_R));
                            */

                            if (Math.Abs(g_arm_R / p_arm_R) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_arm_R / p_arm_R) > st_rangear)
                             {
                                 st_love7++;
                                 total++;
                                 print("stupid!!!!!");
                             }*/
                            else
                            {
                                print("not");
                                total++;
                            }


                            //pare++;


                            //비교부분**(왼쪽 다리), 정확할 때,
                            float g_leg_l = (guidereader.guide1[pare, 19] - guidereader.guide1[pare, 21]) / (guidereader.guide1[pare, 18] - guidereader.guide1[pare, 20]);//가이드
                            float p_leg_l = (jarray[9, 1] - jarray[10, 1]) / (jarray[9, 0] - jarray[10, 0]);//플레이어
                                                                                                            /*  print("pare : " + pare);
                                                                                                              print("g_leg_l" + g_leg_l + "p_leg_l" + p_leg_l);
                                                                                                              print("왼쪽다리 가이드 " + guidereader.guide1[pare, 19] + "헤헤" + guidereader.guide1[pare, 21] + "헤헤" + guidereader.guide1[pare, 18] + "헤헤" + guidereader.guide1[pare, 20]);
                                                                                                              print("왼쪽다리 플레이어 " + jarray[9, 1] + "요기 " + jarray[10, 1] + "요기 " + jarray[9, 0] + "요기 " + jarray[10, 0]);
                                                                                                              print("g_leg_l / p_leg_l : " + Math.Abs(g_leg_l / p_leg_l));
                                                                                                              */
                            if (Math.Abs(g_leg_l / p_leg_l) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_leg_l / p_leg_l) > st_rangell)
                            {
                                st_love8++;
                                total++;
                                print("stupid!!!!!");
                            }
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++;


                            //비교부분**(오른쪽 다리), 정확할 때
                            float g_leg_R = (guidereader.guide1[pare, 25] - guidereader.guide1[pare, 27]) / (guidereader.guide1[pare, 24] - guidereader.guide1[pare, 26]);//가이드
                            float p_leg_R = (jarray[12, 1] - jarray[13, 1]) / (jarray[12, 0] - jarray[13, 0]);//플레이어
                                                                                                              /*  print("pare : " + pare);
                                                                                                                print("g_leg_R" + g_leg_R + "p_leg_R" + p_leg_R);
                                                                                                                print("오른쪽다리 가이드 " + guidereader.guide1[pare, 25] + "헤헤" + guidereader.guide1[pare, 27] + "헤헤" + guidereader.guide1[pare, 24] + "헤헤" + guidereader.guide1[pare, 26]);
                                                                                                                print("오른쪽다리 플레이어 " + jarray[12, 1] + "요기 " + jarray[13, 1] + "요기 " + jarray[12, 0] + "요기 " + jarray[13, 0]);
                                                                                                                print("g_leg_R / p_leg_R : " + Math.Abs(g_leg_R / p_leg_R));
                                                                                                                */
                            if (Math.Abs(g_leg_R / p_leg_R) < range1)
                            {
                                print("good");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_leg_R / p_leg_R) > st_rangelr)
                            {
                                st_love9++;
                                total++;
                                print("stupid!!!!!");
                            }
                            else
                            {
                                print("not");
                                total++;
                            }

                            //pare++;

                            pare++;//여기서 한번만.







                            var fileName = "test1.txt";///aaaa의 txt파일을 만듬

                            if (File.Exists(fileName))
                            {
                                using (StreamWriter sr = File.AppendText(fileName))// 두번째부터는 존재하니까 그거에다가 씀.
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr.Write(jarray[l, k]);
                                            sr.Write(", ");
                                        }

                                    }
                                    sr.Write("\n");
                                    sr.Close();



                                }
                            }
                            else
                            {

                                using (StreamWriter sr = File.CreateText(fileName))// 처음에만 실행됨!
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr.Write(jarray[l, k]);
                                            sr.Write(", ");
                                        }

                                    }
                                    sr.Write("\n");
                                    sr.Close();
                                }

                            }

                            //i = 0;
                            // j = 0;// 이렇게 초기화를 해주어야 다시 덮어쓸 수 있음. =>>가이드 만드느라 가이드 뒷부분에 해줄겡



                            //**가이드 안무는 이부분만 따로 저장
                            var fileName2 = "gtxt.txt"; // guide 안무
                            if (File.Exists(fileName))
                            {
                                using (StreamWriter sr2 = File.AppendText(fileName2))// 두번째부터는 존재하니까 그거에다가 씀.
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr2.Write(jarray[l, k]);
                                            sr2.Write(", ");
                                        }

                                    }
                                    sr2.Write("\n");
                                    sr2.Close();



                                }
                            }
                            else
                            {

                                using (StreamWriter sr2 = File.CreateText(fileName2))// 처음에만 실행됨!
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr2.Write(jarray[l, k]);
                                            sr2.Write(", ");
                                        }

                                    }
                                    sr2.Write("\n");
                                    sr2.Close();
                                }

                            }

                            i = 0;
                            j = 0;// 이렇게 초기화를 해주어야 다시 덮어쓸 수 있음.
                        }

                        //여기시작

                        if (frame_count == 696)//기준 프레임 뒤의 한 프레임의 정보를 다 가져왔을 때 txt파일에 쓰고 배열 비워주기
                        {
                            ////  print("frame= " + frame);                      




                            //비교부분**(왼팔어깨 부분)

                            float g_sho_L = (guidereader.guide1[pare, 5] - guidereader.guide1[pare, 7]) / (guidereader.guide1[pare, 4] - guidereader.guide1[pare, 6]);//가이드
                            float p_sho_L = (jarray[2, 1] - jarray[3, 1]) / (jarray[2, 0] - jarray[3, 0]);//플레이어
                                                                                                          /*  print("pare1 : " + pare1);
                                                                                                            print("g_sho_L" + g_sho_L + "p_sho_L" + p_sho_L);//확인
                                                                                                            print("왼팔어깨 가이드 1 " + guidereader.guide1[pare, 5] + "헤헤" + guidereader.guide1[pare, 7] + "헤헤" + guidereader.guide1[pare, 4] + "헤헤" + guidereader.guide1[pare, 6]);//확인
                                                                                                            print("왼팔어깨 플레이어 1 " + jarray[2, 1] + "요기 " + jarray[3, 1] + "요기 " + jarray[2, 0] + "요기 " + jarray[3, 0]);//확인
                                                                                                            print("g_sho_L/p_sho_L 1 : " + Math.Abs(g_sho_L / p_sho_L));
                                                                                                            */

                            if (Math.Abs(g_sho_L / p_sho_L) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_sho_L / p_sho_L) > st_rangeal)
                             {
                                 st_love0++;
                                 total++;
                                 print("1stupid!!!!!");
                             }*/
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(오른팔어깨 부분)

                            float g_sho_R = (guidereader.guide1[pare, 11] - guidereader.guide1[pare, 13]) / (guidereader.guide1[pare, 10] - guidereader.guide1[pare, 12]);//가이드
                            float p_sho_R = (jarray[5, 1] - jarray[6, 1]) / (jarray[5, 0] - jarray[6, 0]);//플레이어
                                                                                                          /* print("pare1 : " + pare1);
                                                                                                           print("g_sho_R" + g_sho_R + "p_sho_R" + p_sho_R);//확인
                                                                                                           print("왼팔 가이드 1 " + guidereader.guide1[pare, 11] + "헤헤" + guidereader.guide1[pare, 13] + "헤헤" + guidereader.guide1[pare, 10] + "헤헤" + guidereader.guide1[pare, 12]);//확인
                                                                                                           print("왼팔 플레이어 1 " + jarray[5, 1] + "요기 " + jarray[6, 1] + "요기 " + jarray[5, 0] + "요기 " + jarray[6, 0]);//확인
                                                                                                           print("g_sho_R/p_sho_R 1 : " + Math.Abs(g_sho_R / p_sho_R));
                                                                                                           */

                            if (Math.Abs(g_sho_R / p_sho_R) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_sho_R / p_sho_R) > st_rangear)
                             {
                                 st_love1++;
                                 total++;
                                 print("1stupid!!!!!");
                             }*/
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(왼다리힙 부분)

                            float g_hip_L = (guidereader.guide1[pare, 17] - guidereader.guide1[pare, 19]) / (guidereader.guide1[pare, 16] - guidereader.guide1[pare, 18]);//가이드
                            float p_hip_L = (jarray[8, 1] - jarray[9, 1]) / (jarray[8, 0] - jarray[9, 0]);//플레이어
                                                                                                          /* print("pare1 : " + pare1);
                                                                                                           print("g_hip_L" + g_hip_L + "p_hip_L" + p_hip_L);//확인
                                                                                                           print("왼팔 가이드 1 " + guidereader.guide1[pare, 17] + "헤헤" + guidereader.guide1[pare, 19] + "헤헤" + guidereader.guide1[pare, 16] + "헤헤" + guidereader.guide1[pare, 18]);//확인
                                                                                                           print("왼팔 플레이어 1 " + jarray[8, 1] + "요기 " + jarray[9, 1] + "요기 " + jarray[8, 0] + "요기 " + jarray[9, 0]);//확인
                                                                                                           print("g_hip_L/p_hip_L 1 : " + Math.Abs(g_hip_L / p_hip_L));
                                                                                                           */

                            if (Math.Abs(g_hip_L / p_hip_L) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_hip_L / p_hip_L) > st_rangell)
                            {
                                st_love2++;
                                total++;
                                print("1stupid!!!!!");
                            }
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(오른다리힙 부분)

                            float g_hip_R = (guidereader.guide1[pare, 23] - guidereader.guide1[pare, 25]) / (guidereader.guide1[pare, 22] - guidereader.guide1[pare, 24]);//가이드
                            float p_hip_R = (jarray[11, 1] - jarray[12, 1]) / (jarray[11, 0] - jarray[12, 0]);//플레이어
                                                                                                              /*    print("pare1 : " + pare1);
                                                                                                                  print("g_hip_R" + g_hip_R + "p_hip_R" + p_hip_R);//확인
                                                                                                                  print("왼팔 가이드 1 " + guidereader.guide1[pare, 23] + "헤헤" + guidereader.guide1[pare, 25] + "헤헤" + guidereader.guide1[pare, 22] + "헤헤" + guidereader.guide1[pare, 24]);//확인
                                                                                                                  print("왼팔 플레이어 1 " + jarray[11, 1] + "요기 " + jarray[12, 1] + "요기 " + jarray[11, 0] + "요기 " + jarray[12, 0]);//확인
                                                                                                                  print("g_hip_R/p_hip_R 1 : " + Math.Abs(g_hip_R / p_hip_R));
                                                                                                                  */

                            if (Math.Abs(g_hip_R / p_hip_R) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_hip_R / p_hip_R) > st_rangelr)
                            {
                                st_love3++;
                                total++;
                                print("1stupid!!!!!");
                            }
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(머리와목 부분)

                            float g_h_n = (guidereader.guide1[pare, 3] - guidereader.guide1[pare, 29]) / (guidereader.guide1[pare, 2] - guidereader.guide1[pare, 28]);//가이드
                            float p_h_n = (jarray[1, 1] - jarray[14, 1]) / (jarray[1, 0] - jarray[14, 0]);//플레이어
                                                                                                          /*  print("pare1 : " + pare1);
                                                                                                            print("g_h_n" + g_h_n + "p_h_n" + p_h_n);//확인
                                                                                                            print("왼팔 가이드 1 " + guidereader.guide1[pare, 1] + "헤헤" + guidereader.guide1[pare, 29] + "헤헤" + guidereader.guide1[pare, 0] + "헤헤" + guidereader.guide1[pare, 28]);//확인
                                                                                                            print("왼팔 플레이어 1 " + jarray[1, 1] + "요기 " + jarray[14, 1] + "요기 " + jarray[1, 0] + "요기 " + jarray[14, 0]);//확인
                                                                                                            print("g_h_n/p_h_n 1 : " + Math.Abs(g_h_n / p_h_n));
                                                                                                            */

                            if (Math.Abs(g_h_n / p_h_n) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            /*else if (Math.Abs(g_h_n / p_h_n) > st_range)
                            {
                                st_love4++;
                                total++;
                                print("1stupid!!!!!");
                            }*/
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분 목과 스파인미드
                            float g_n_m = (guidereader.guide1[pare, 29] - guidereader.guide1[pare, 1]) / (guidereader.guide1[pare, 28] - guidereader.guide1[pare, 0]);//가이드
                            float p_n_m = (jarray[14, 1] - jarray[0, 1]) / (jarray[14, 0] - jarray[0, 0]);//플레이어

                            /*  print("pare1 : " + pare1);
                              print("g_n_m" + g_n_m + "p_h_m" + p_n_m);
                              print("머리 가이드 1 " + guidereader.guide1[pare, 29] + "헤헤" + guidereader.guide1[pare, 1] + "헤헤" + guidereader.guide1[pare, 28] + "헤헤" + guidereader.guide1[pare, 0]);
                              print("머리 플레이어 1 " + jarray[14, 1] + "요기 " + jarray[0, 1] + "요기 " + jarray[14, 0] + "요기 " + jarray[0, 0]);
                              print("g_n_m / p_n_m 1 : " + Math.Abs(g_n_m / p_n_m));
                              */
                            if (Math.Abs(g_n_m / p_n_m) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            /*else if (Math.Abs(g_n_m / p_n_m) > st_range)
                            {
                                st_love5++;
                                total++;
                                print("1stupid!!!!!");
                            }*/
                            else
                            {
                                print("not1");

                                total++;
                            }




                            //비교부분**(왼팔 부분)

                            float g_arm_L = (guidereader.guide1[pare, 7] - guidereader.guide1[pare, 9]) / (guidereader.guide1[pare, 6] - guidereader.guide1[pare, 8]);//가이드
                            float p_arm_L = (jarray[3, 1] - jarray[4, 1]) / (jarray[3, 0] - jarray[4, 0]);//플레이어
                                                                                                          /* print("pare1 : " + pare1);
                                                                                                           print("g_arm_L" + g_arm_L + "p_arm_L" + p_arm_L);//확인
                                                                                                           print("왼팔 가이드 1 " + guidereader.guide1[pare, 7] + "헤헤" + guidereader.guide1[pare, 9] + "헤헤" + guidereader.guide1[pare, 6] + "헤헤" + guidereader.guide1[pare, 8]);//확인
                                                                                                           print("왼팔 플레이어 1 " + jarray[3, 1] + "요기 " + jarray[4, 1] + "요기 " + jarray[3, 0] + "요기 " + jarray[4, 0]);//확인
                                                                                                           print("g_arm_L/p_arm_L 1 : " + Math.Abs(g_arm_L / p_arm_L));
                                                                                                           */

                            if (Math.Abs(g_arm_L / p_arm_L) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            /*else if (Math.Abs(g_arm_L / p_arm_L) > st_rangeal)
                            {
                                st_love6++;
                                total++;
                                print("1stupid!!!!!");
                            }*/
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++; 마지막에 했음


                            //비교부분**(오른팔 부분)
                            float g_arm_R = (guidereader.guide1[pare, 13] - guidereader.guide1[pare, 15]) / (guidereader.guide1[pare, 12] - guidereader.guide1[pare, 14]);
                            float p_arm_R = (jarray[6, 1] - jarray[7, 1]) / (jarray[6, 0] - jarray[7, 0]);
                            /*   print("pare1 : " + pare1);
                               print("g_arm_R" + g_arm_R + "p_arm_R" + p_arm_R);
                               print("오른팔 가이드 1 " + guidereader.guide1[pare, 13] + "헤헤" + guidereader.guide1[pare, 15] + "헤헤" + guidereader.guide1[pare, 12] + "헤헤" + guidereader.guide1[pare, 14]);
                               print("오른팔 플레이어 1 " + jarray[6, 1] + "요기 " + jarray[7, 1] + "요기 " + jarray[6, 0] + "요기 " + jarray[7, 0]);
                               print("g_arm_R / p_arm_R 1 : " + Math.Abs(g_arm_R / p_arm_R));
                               */

                            if (Math.Abs(g_arm_R / p_arm_R) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            /*else if (Math.Abs(g_arm_R / p_arm_R) > st_rangear)
                            {
                                st_love7++;
                                total++;
                                print("1stupid!!!!!");
                            }*/
                            else
                            {
                                print("not1");
                                total++;
                            }


                            //pare++;


                            //비교부분**(왼쪽 다리)
                            float g_leg_l = (guidereader.guide1[pare, 19] - guidereader.guide1[pare, 21]) / (guidereader.guide1[pare, 18] - guidereader.guide1[pare, 20]);//가이드
                            float p_leg_l = (jarray[9, 1] - jarray[10, 1]) / (jarray[9, 0] - jarray[10, 0]);//플레이어
                                                                                                            /*   print("pare1 : " + pare1);
                                                                                                               print("g_leg_l" + g_leg_l + "p_leg_l" + p_leg_l);
                                                                                                               print("왼쪽다리 가이드 1 " + guidereader.guide1[pare, 19] + "헤헤" + guidereader.guide1[pare, 21] + "헤헤" + guidereader.guide1[pare, 18] + "헤헤" + guidereader.guide1[pare, 20]);
                                                                                                               print("왼쪽다리 플레이어 1 " + jarray[9, 1] + "요기 " + jarray[10, 1] + "요기 " + jarray[9, 0] + "요기 " + jarray[10, 0]);
                                                                                                               print("g_leg_l / p_leg_l 1 : " + Math.Abs(g_leg_l / p_leg_l));
                                                                                                               */
                            if (Math.Abs(g_leg_l / p_leg_l) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_leg_l / p_leg_l) > st_rangell)
                            {
                                st_love8++;
                                total++;
                                print("1stupid!!!!!");
                            }
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //pare++;


                            //비교부분**(오른쪽 다리)
                            float g_leg_R = (guidereader.guide1[pare, 25] - guidereader.guide1[pare, 27]) / (guidereader.guide1[pare, 24] - guidereader.guide1[pare, 26]);//가이드
                            float p_leg_R = (jarray[12, 1] - jarray[13, 1]) / (jarray[12, 0] - jarray[13, 0]);//플레이어
                                                                                                              /* print("pare1 : " + pare1);
                                                                                                               print("g_leg_R" + g_leg_R + "p_leg_R" + p_leg_R);
                                                                                                               print("오른쪽다리 가이드 1 " + guidereader.guide1[pare, 25] + "헤헤" + guidereader.guide1[pare, 27] + "헤헤" + guidereader.guide1[pare, 24] + "헤헤" + guidereader.guide1[pare, 26]);
                                                                                                               print("오른쪽다리 플레이어 1 " + jarray[12, 1] + "요기 " + jarray[13, 1] + "요기 " + jarray[12, 0] + "요기 " + jarray[13, 0]);
                                                                                                               print("g_leg_R / p_leg_R 1 : " + Math.Abs(g_leg_R / p_leg_R));
                                                                                                               */
                            if (Math.Abs(g_leg_R / p_leg_R) < range1)
                            {
                                print("good1");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_leg_R / p_leg_R) > st_rangelr)
                            {
                                st_love9++;
                                total++;
                                print("1stupid!!!!!");
                            }
                            else
                            {
                                print("not1");
                                total++;
                            }

                            //*951fjfj
                            pare1++;//여기서 한번만.
                                    //점수비교 부분

                            print(st_love0);
                            print(st_love1);
                            print(st_love2);
                            print(st_love3);
                            print(st_love4);
                            print(st_love5);
                            print(st_love6);
                            print(st_love7);
                            print(st_love8);
                            print(st_love9);
                            //초기화 시점 이쪽으로 바꿈!!!!!!!!!111ㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠ
                            arm_L = 0;
                            arm_R = 0;
                            leg_L = 0;
                            leg_R = 0;
                            head = 0;

                            print(pare1 + "만큼 비교한다.");
                            print("total 개수 : " + total); //total 30개가 프레임당 잘 들어갔는지 비교

                            //stupid 표시 (
                            if (st_love8 > 2 || st_love9 > 2)
                            //  if (st_love6 > 2 || st_love7 > 2 || st_love8 > 2 || st_love9 > 2)
                            {
                                print("들어왔다");
                                if (pare1 >= 1 && pare1 <= 3)
                                { SM++; }
                                else
                                {
                                    SM = 100;
                                    sum += 0;// stupid는 0점을 줌   

                                   if(sum / (pare1 - 3)>100)
                                    {
                                        avg = 100;
                                    }
                                    else
                                    {
                                        avg = (int)(sum / (pare1 - 3));// avg 각 프레임당 평균점수로 내기 위해서 avg 변수에 저장해줌
                                    }

                                    if (st_love8 >= 2)
                                    {
                                        love = 7;
                                        leg_L = 1;
                                    }
                                    if (st_love9 >= 2)
                                    {
                                        love = 8;
                                        leg_R = 1;
                                    }

                                }
                            }
                            else
                            {
                                if (good >= 0 && good <= 3)
                                {
                                    print(good + "  oops");

                                    if (pare1 >= 1 && pare1 <= 3)
                                    { SM++; }
                                    else
                                    {
                                        sum += good * 4;// sum이라는 점수에 good의 개수당 4점을 처리해줌

                                        if (sum / (pare1 - 3) > 100)
                                        {
                                            avg = 100;
                                        }
                                        else
                                        {
                                            avg = (int)sum / (pare1 - 3);// avg 각 프레임당 평균점수로 내기 위해서 avg 변수에 저장해줌
                                        }

                                        SM = 100;
                                        love = 4;
                                    }

                                }
                                else if (good >= 4 && good <= 11)
                                {
                                    print(good + "   bad");

                                    if (pare1 >= 1 && pare1 <= 3)
                                    { SM++; }
                                    else
                                    {
                                        sum += good * 4;

                                        if (sum / (pare1 - 3) > 100)
                                        {
                                            avg = 100;
                                        }
                                        else
                                        {
                                            avg = (int)sum / (pare1 - 3);// avg 각 프레임당 평균점수로 내기 위해서 avg 변수에 저장해줌
                                        }

                                        SM = 100;
                                        love = 3;
                                    }
                                }
                                else if (good >= 12 && good <= 20)
                                {
                                    print(good + "   good");

                                    if (pare1 >= 1 && pare1 <= 3)
                                    { SM++; }
                                    else
                                    {
                                        sum += good * 4.5;

                                        if (sum / (pare1 - 3) > 100)
                                        {
                                            avg = 100;
                                        }
                                        else
                                        {
                                            avg = (int)sum / (pare1 - 3);// avg 각 프레임당 평균점수로 내기 위해서 avg 변수에 저장해줌
                                        }

                                        SM = 100;
                                        love = 2;
                                    }
                                }
                                else if (good >= 21 && good <= 30)
                                {
                                    print(good + "   perfect");

                                    if (pare1 >= 1 && pare1 <= 3)
                                    { SM++; }
                                    else
                                    {
                                        sum += good * 4.5;

                                        if (sum / (pare1 - 3) > 100)
                                        {
                                            avg = 100;
                                        }
                                        else
                                        {
                                            avg = (int)sum / (pare1 - 3);// avg 각 프레임당 평균점수로 내기 위해서 avg 변수에 저장해줌
                                        }

                                        SM = 100;
                                        love = 1;
                                    }
                                }
                            }
                            good = 0;//good의 개수를 초기화 함
                            total = 0;// total의 개수를 초기화 함
                                      //비교 종료
                                      //초기화

                            //비교 종료

                            st_love0 = 0;
                            st_love1 = 0;
                            st_love2 = 0;
                            st_love3 = 0;
                            st_love4 = 0;
                            st_love5 = 0;
                            st_love6 = 0;
                            st_love7 = 0;
                            st_love8 = 0;
                            st_love9 = 0;




                            var fileName = "test1.txt";///aaaa의 txt파일을 만듬
                            if (File.Exists(fileName))
                            {
                                using (StreamWriter sr = File.AppendText(fileName))// 두번째부터는 존재하니까 그거에다가 씀.
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr.Write(jarray[l, k]);
                                            sr.Write(", ");
                                        }

                                    }
                                    //sr.Write("frame_count= " + frame_count);
                                    //sr.Write("frame= " + (frame - 1));
                                    sr.Write("\n");
                                    sr.Close();



                                }
                            }
                            else
                            {


                                using (StreamWriter sr = File.CreateText(fileName))// 처음에만 실행됨!
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr.Write(jarray[l, k]);
                                            sr.Write(", ");
                                        }

                                    }
                                    //sr.Write("frame_count= " + frame_count);
                                    //sr.Write("frame= " + (frame - 1));
                                    sr.Write("\n");
                                    sr.Close();
                                }

                            }

                            i = 0;
                            j = 0;// 이렇게 초기화를 해주어야 다시 덮어쓸 수 있음.
                        }

                        if (frame_count == 24)//기준 프레임 앞의 한 프레임의 정보를 다 가져왔을 때 txt파일에 쓰고 배열 비워주기
                        {
                            //// print("frame= " + frame);
                            /////비교부분 3


                            //비교부분**(왼팔어깨 부분),정확할 때,

                            float g_sho_L = (guidereader.guide1[pare, 5] - guidereader.guide1[pare, 7]) / (guidereader.guide1[pare, 4] - guidereader.guide1[pare, 6]);//가이드
                            float p_sho_L = (jarray[2, 1] - jarray[3, 1]) / (jarray[2, 0] - jarray[3, 0]);//플레이어
                                                                                                          /*  print("pare2 : " + pare2);
                                                                                                            print("g_sho_L" + g_sho_L + "p_sho_L" + p_sho_L);//확인
                                                                                                            print("왼팔어깨 가이드 2 " + guidereader.guide1[pare, 5] + "헤헤" + guidereader.guide1[pare, 7] + "헤헤" + guidereader.guide1[pare, 4] + "헤헤" + guidereader.guide1[pare, 6]);//확인
                                                                                                            print("왼팔어깨 플레이어 2 " + jarray[2, 1] + "요기 " + jarray[3, 1] + "요기 " + jarray[2, 0] + "요기 " + jarray[3, 0]);//확인
                                                                                                            print("g_sho_L/p_sho_L 2 : " + Math.Abs(g_sho_L / p_sho_L));
                                                                                                            */

                            if (Math.Abs(g_sho_L / p_sho_L) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_sho_L / p_sho_L) > st_rangeal)
                             {
                                 st_love0++;
                                 total++;
                                 print("2stupid!!!!!");
                             }*/
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(오른팔어깨 부분),정확할 때,

                            float g_sho_R = (guidereader.guide1[pare, 11] - guidereader.guide1[pare, 13]) / (guidereader.guide1[pare, 10] - guidereader.guide1[pare, 12]);//가이드
                            float p_sho_R = (jarray[5, 1] - jarray[6, 1]) / (jarray[5, 0] - jarray[6, 0]);//플레이어
                                                                                                          /*   print("pare2: " + pare2);
                                                                                                             print("g_sho_R" + g_sho_R + "p_sho_R" + p_sho_R);//확인
                                                                                                             print("왼팔 가이드 2 " + guidereader.guide1[pare, 11] + "헤헤" + guidereader.guide1[pare, 13] + "헤헤" + guidereader.guide1[pare, 10] + "헤헤" + guidereader.guide1[pare, 12]);//확인
                                                                                                             print("왼팔 플레이어 2 " + jarray[5, 1] + "요기 " + jarray[6, 1] + "요기 " + jarray[5, 0] + "요기 " + jarray[6, 0]);//확인
                                                                                                             print("g_sho_R/p_sho_R 2 : " + Math.Abs(g_sho_R / p_sho_R));
                                                                                                             */

                            if (Math.Abs(g_sho_R / p_sho_R) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_sho_R / p_sho_R) > st_rangear)
                             {
                                 st_love1++;
                                 total++;
                                 print("2stupid!!!!!");
                             }*/
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(왼다리힙 부분),정확할 때,

                            float g_hip_L = (guidereader.guide1[pare, 17] - guidereader.guide1[pare, 19]) / (guidereader.guide1[pare, 16] - guidereader.guide1[pare, 18]);//가이드
                            float p_hip_L = (jarray[8, 1] - jarray[9, 1]) / (jarray[8, 0] - jarray[9, 0]);//플레이어
                                                                                                          /* print("pare2 : " + pare2);
                                                                                                           print("g_hip_L" + g_hip_L + "p_hip_L" + p_hip_L);//확인
                                                                                                           print("왼팔 가이드 2 " + guidereader.guide1[pare, 17] + "헤헤" + guidereader.guide1[pare, 19] + "헤헤" + guidereader.guide1[pare, 16] + "헤헤" + guidereader.guide1[pare, 18]);//확인
                                                                                                           print("왼팔 플레이어 2 " + jarray[8, 1] + "요기 " + jarray[9, 1] + "요기 " + jarray[8, 0] + "요기 " + jarray[9, 0]);//확인
                                                                                                           print("g_hip_L/p_hip_L 2 : " + Math.Abs(g_hip_L / p_hip_L));
                                                                                                           */

                            if (Math.Abs(g_hip_L / p_hip_L) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_hip_L / p_hip_L) > st_rangell)
                            {
                                st_love2++;
                                total++;
                                print("2stupid!!!!!");
                            }
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(오른다리힙 부분),정확할 때,

                            float g_hip_R = (guidereader.guide1[pare, 23] - guidereader.guide1[pare, 25]) / (guidereader.guide1[pare, 22] - guidereader.guide1[pare, 24]);//가이드
                            float p_hip_R = (jarray[11, 1] - jarray[12, 1]) / (jarray[11, 0] - jarray[12, 0]);//플레이어
                                                                                                              /*  print("pare2 : " + pare2);
                                                                                                                print("g_hip_R" + g_hip_R + "p_hip_R" + p_hip_R);//확인
                                                                                                                print("왼팔 가이드 2 " + guidereader.guide1[pare, 23] + "헤헤" + guidereader.guide1[pare, 25] + "헤헤" + guidereader.guide1[pare, 22] + "헤헤" + guidereader.guide1[pare, 24]);//확인
                                                                                                                print("왼팔 플레이어 2 " + jarray[11, 1] + "요기 " + jarray[12, 1] + "요기 " + jarray[11, 0] + "요기 " + jarray[12, 0]);//확인
                                                                                                                print("g_hip_R/p_hip_R 2 : " + Math.Abs(g_hip_R / p_hip_R));
                                                                                                                */

                            if (Math.Abs(g_hip_R / p_hip_R) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_hip_R / p_hip_R) > st_rangelr)
                            {
                                st_love3++;
                                total++;
                                print("2stupid!!!!!");
                            }
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분**(머리와목 부분),정확할 때,

                            float g_h_n = (guidereader.guide1[pare, 3] - guidereader.guide1[pare, 29]) / (guidereader.guide1[pare, 2] - guidereader.guide1[pare, 28]);//가이드
                            float p_h_n = (jarray[1, 1] - jarray[14, 1]) / (jarray[1, 0] - jarray[14, 0]);//플레이어
                            /*print("pare2 : " + pare2);
                            print("g_h_n" + g_h_n + "p_h_n" + p_h_n);//확인
                            print("왼팔 가이드 2 " + guidereader.guide1[pare, 1] + "헤헤" + guidereader.guide1[pare, 29] + "헤헤" + guidereader.guide1[pare, 0] + "헤헤" + guidereader.guide1[pare, 28]);//확인
                            print("왼팔 플레이어 2 " + jarray[1, 1] + "요기 " + jarray[14, 1] + "요기 " + jarray[1, 0] + "요기 " + jarray[14, 0]);//확인
                            print("g_h_n/p_h_n 2 : " + Math.Abs(g_h_n / p_h_n));
                            */

                            if (Math.Abs(g_h_n / p_h_n) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_h_n / p_h_n) > st_range)
                             {
                                 st_love4++;
                                 total++;
                                 print("2stupid!!!!!");
                             }*/
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++; 마지막에 했음

                            //비교부분 목과 스파인미드
                            float g_n_m = (guidereader.guide1[pare, 29] - guidereader.guide1[pare, 1]) / (guidereader.guide1[pare, 28] - guidereader.guide1[pare, 0]);//가이드
                            float p_n_m = (jarray[14, 1] - jarray[0, 1]) / (jarray[14, 0] - jarray[0, 0]);//플레이어

                            /*  print("pare2 : " + pare2);
                              print("g_n_m" + g_n_m + "p_h_m" + p_n_m);
                              print("머리 가이드 2 " + guidereader.guide1[pare, 29] + "헤헤" + guidereader.guide1[pare, 1] + "헤헤" + guidereader.guide1[pare, 28] + "헤헤" + guidereader.guide1[pare, 0]);
                              print("머리 플레이어 2 " + jarray[14, 1] + "요기 " + jarray[0, 1] + "요기 " + jarray[14, 0] + "요기 " + jarray[0, 0]);
                              print("g_n_m / p_n_m 2 : " + Math.Abs(g_n_m / p_n_m));
                              */
                            if (Math.Abs(g_n_m / p_n_m) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            /*else if (Math.Abs(g_n_m / p_n_m) > st_range)
                            {
                                st_love5++;
                                total++;
                                print("2stupid!!!!!");
                            }*/
                            else
                            {
                                print("not2");

                                total++;
                            }



                            /////////////////////////////

                            //비교부분**(왼팔 부분),정확할 때,

                            float g_arm_L = (guidereader.guide1[pare, 7] - guidereader.guide1[pare, 9]) / (guidereader.guide1[pare, 6] - guidereader.guide1[pare, 8]);//가이드
                            float p_arm_L = (jarray[3, 1] - jarray[4, 1]) / (jarray[3, 0] - jarray[4, 0]);//플레이어
                                                                                                          /*  print("pare2 : " + pare2);
                                                                                                            print("g_arm_L" + g_arm_L + "p_arm_L" + p_arm_L);//확인
                                                                                                            print("왼팔 가이드 2 " + guidereader.guide1[pare, 7] + "헤헤" + guidereader.guide1[pare, 9] + "헤헤" + guidereader.guide1[pare, 6] + "헤헤" + guidereader.guide1[pare, 8]);//확인
                                                                                                            print("왼팔 플레이어 2 " + jarray[3, 1] + "요기 " + jarray[4, 1] + "요기 " + jarray[3, 0] + "요기 " + jarray[4, 0]);//확인
                                                                                                            print("g_arm_L/p_arm_L 2 : " + Math.Abs(g_arm_L / p_arm_L));
                                                                                                            */

                            if (Math.Abs(g_arm_L / p_arm_L) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_arm_L / p_arm_L) > st_rangeal)
                             {
                                 st_love6++;
                                 total++;
                                 print("2stupid!!!!!");
                             }*/
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++; 마지막에 했음


                            //비교부분**(오른팔 부분), 정확할 때,
                            float g_arm_R = (guidereader.guide1[pare, 13] - guidereader.guide1[pare, 15]) / (guidereader.guide1[pare, 12] - guidereader.guide1[pare, 14]);
                            float p_arm_R = (jarray[6, 1] - jarray[7, 1]) / (jarray[6, 0] - jarray[7, 0]);
                            /*  print("pare2 : " + pare2);
                              print("g_arm_R" + g_arm_R + "p_arm_R" + p_arm_R);
                              print("오른팔 가이드 2 " + guidereader.guide1[pare, 13] + "헤헤" + guidereader.guide1[pare, 15] + "헤헤" + guidereader.guide1[pare, 12] + "헤헤" + guidereader.guide1[pare, 14]);
                              print("오른팔 플레이어 2 " + jarray[6, 1] + "요기 " + jarray[7, 1] + "요기 " + jarray[6, 0] + "요기 " + jarray[7, 0]);
                              print("g_arm_R / p_arm_R 2 : " + Math.Abs(g_arm_R / p_arm_R));
                              */

                            if (Math.Abs(g_arm_R / p_arm_R) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            /* else if (Math.Abs(g_arm_R / p_arm_R) > st_rangear)
                             {
                                 st_love7++;
                                 total++;
                                 print("2stupid!!!!!");
                             }*/
                            else
                            {
                                print("not2");
                                total++;
                            }


                            //pare++;


                            //비교부분**(왼쪽 다리), 정확할 때,
                            float g_leg_l = (guidereader.guide1[pare, 19] - guidereader.guide1[pare, 21]) / (guidereader.guide1[pare, 18] - guidereader.guide1[pare, 20]);//가이드
                            float p_leg_l = (jarray[9, 1] - jarray[10, 1]) / (jarray[9, 0] - jarray[10, 0]);//플레이어
                                                                                                            /*   print("pare2 : " + pare2);
                                                                                                               print("g_leg_l" + g_leg_l + "p_leg_l" + p_leg_l);
                                                                                                               print("왼쪽다리 가이드 2 " + guidereader.guide1[pare, 19] + "헤헤" + guidereader.guide1[pare, 21] + "헤헤" + guidereader.guide1[pare, 18] + "헤헤" + guidereader.guide1[pare, 20]);
                                                                                                               print("왼쪽다리 플레이어 2 " + jarray[9, 1] + "요기 " + jarray[10, 1] + "요기 " + jarray[9, 0] + "요기 " + jarray[10, 0]);
                                                                                                               print("g_leg_l / p_leg_l 2 : " + Math.Abs(g_leg_l / p_leg_l));
                                                                                                               */
                            if (Math.Abs(g_leg_l / p_leg_l) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_leg_l / p_leg_l) > st_rangell)
                            {
                                st_love8++;
                                total++;
                                print("2stupid!!!!!");
                            }
                            else
                            {
                                print("not2");
                                total++;
                            }

                            //pare++;


                            //비교부분**(오른쪽 다리), 정확할 때
                            float g_leg_R = (guidereader.guide1[pare, 25] - guidereader.guide1[pare, 27]) / (guidereader.guide1[pare, 24] - guidereader.guide1[pare, 26]);//가이드
                            float p_leg_R = (jarray[12, 1] - jarray[13, 1]) / (jarray[12, 0] - jarray[13, 0]);//플레이어
                                                                                                              /* print("pare2 : " + pare2);
                                                                                                               print("g_leg_R" + g_leg_R + "p_leg_R" + p_leg_R);
                                                                                                               print("오른쪽다리 가이드 2 " + guidereader.guide1[pare, 25] + "헤헤" + guidereader.guide1[pare, 27] + "헤헤" + guidereader.guide1[pare, 24] + "헤헤" + guidereader.guide1[pare, 26]);
                                                                                                               print("오른쪽다리 플레이어 2 " + jarray[12, 1] + "요기 " + jarray[13, 1] + "요기 " + jarray[12, 0] + "요기 " + jarray[13, 0]);
                                                                                                               print("g_leg_R / p_leg_R 2 : " + Math.Abs(g_leg_R / p_leg_R));
                                                                                                               */
                            if (Math.Abs(g_leg_R / p_leg_R) < range1)
                            {
                                print("good2");
                                good++;
                                total++;
                            }
                            else if (Math.Abs(g_leg_R / p_leg_R) > st_rangelr)
                            {
                                st_love9++;
                                total++;
                                print("2stupid!!!!!");
                            }
                            else
                            {
                                print("not2");
                                total++;
                            }

                            pare2++;//여기서 한번만.




                            var fileName = "test1.txt";///aaaa의 txt파일을 만듬
                            if (File.Exists(fileName))
                            {
                                using (StreamWriter sr = File.AppendText(fileName))// 두번째부터는 존재하니까 그거에다가 씀.
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr.Write(jarray[l, k]);
                                            sr.Write(", ");
                                        }

                                    }
                                    //sr.Write("frame_count= " + frame_count);
                                    //sr.Write("frame= " + (frame - 1));
                                    sr.Write("\n");
                                    sr.Close();



                                }
                            }
                            else
                            {


                                using (StreamWriter sr = File.CreateText(fileName))// 처음에만 실행됨!
                                {
                                    for (int l = 0; l < jarray.GetLength(0); l++)
                                    {
                                        for (int k = 0; k < jarray.GetLength(1); k++)
                                        {
                                            //float[,] jarray = new float[10, 3];
                                            sr.Write(jarray[l, k]);
                                            sr.Write(", ");
                                        }

                                    }
                                    //sr.Write("frame_count= " + frame_count);
                                    //sr.Write("frame= " + (frame - 1));
                                    sr.Write("\n");
                                    sr.Close();
                                }

                            }


                            i = 0;
                            j = 0;// 이렇게 초기화를 해주어야 다시 덮어쓸 수 있음.
                        }
                    }
                    frame_count--;
                }
            }



            else
            {
                lr.enabled = false;
            }

        }


    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    //    public void Jointprint(
    private static Vector3 hart(Kinect.Joint joint)
    {

        /*double x = System.Math.Round(joint.Position.X, 4;
        double y = System.Math.Round(joint.Position.Y, 4) * 1000;
        double z = System.Math.Round(joint.Position.Z, 4) * 1000;
        return new Vector3((float)x,(float)y,(float)z);
        */

        return new Vector3(joint.Position.X * 1000, joint.Position.Y * 1000, joint.Position.Z * 1000);// 가이드를 정수로 저장시킴.

    }



}
