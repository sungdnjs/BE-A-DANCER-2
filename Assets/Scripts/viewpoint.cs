using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class viewpoint : MonoBehaviour
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;

    public Body[] GetData()
    {
        return _Data;
    }




    void Start()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }

    public static int abcd = 0;
    public static int point = 0;
    static int frame_count1 = 24;// 한 프레임에 24개의 좌표값
    static int frame1 = 0;
    void Update()
    {
       
        abcd++;
       

        if (_Reader != null)
        {

            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)//frame이 읽고 있다면
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];//
                }

                frame.GetAndRefreshBodyData(_Data);//_data의 값을 가져옴


                frame.Dispose();
                frame = null;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }

    public void Click()// 버튼 클릭하면 값이 들어가게 함.
    {
        print("click");
        point = abcd;
    }
}
 

    