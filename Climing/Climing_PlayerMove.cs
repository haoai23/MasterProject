using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Climing_PlayerMove : MonoBehaviour
{
    public GameObject Tracker1, Tracker2, Tracker3;
    GameObject Chest, RightLeg, LeftLeg;
    public static bool isReady = false;
    float ChestTransformY, RightLegTransformY, LeftLegTransformY;
    float Blinktime;
    double StandardValue;
    int R = 0, L = 0;
    Rigidbody PlayerRigiBody;
    int State = 0;
    bool isEnter = false;
    public static bool Climing_StartTimer = false;//開始計時
    Climing_GameControl climing_GameControl;
    private void Start()
    {
        PlayerRigiBody = GetComponent<Rigidbody>();
        climing_GameControl = GetComponent<Climing_GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        /*LeftLeg = GameObject.FindWithTag("LeftLeg");
        RightLeg = GameObject.FindWithTag("RightLeg");
        Chest = GameObject.FindWithTag("Chest");
        if (LeftLeg != null & RightLeg != null & Chest != null)
        {
            if(Input.GetKeyDown(KeyCode.R))
            { isReady = true; }
            
        }
        Debug.Log(isReady);*/
        KeyboradControlPlayer();//測試用
        if (isReady == false)
        {
            StairTower_DefinedViveTracker(Tracker1, Tracker2, Tracker3);

            ChestTransformY = Chest.transform.position.y;
            RightLegTransformY = RightLeg.transform.position.y;
            LeftLegTransformY = LeftLeg.transform.position.y;
            Judgment_Criteria(ChestTransformY, RightLegTransformY, LeftLegTransformY);
        }
        else if (isReady == true)
        {
            StairTower_DefinedViveTracker(Tracker1, Tracker2, Tracker3);
            TrackerControlPlayer();
            Climing_StartTimer = true;
            if (isEnter == false)
            {
                newR_OR_L = -1;
            }
            if (Climing_Timer.ClimingTimer == 0)
            {
                isGameOver = true;
                Climing_StartTimer = false;
                isReady = false;
                DataAnalysis();
            }
        }
    }
    /*int R_OR_L= 0;
    /*void KeyboradControlPlayer()
    {
        if (Input.GetKey("right"))
        {
            R += 1;
            R_OR_L = 0;
            Debug.Log("R: " + R);
            PlayerMoveRightOrLeft(R_OR_L);
        }
        else if (Input.GetKey("left"))
        {
            L += 1;
            R_OR_L = 1;
            Debug.Log("L: " + L);
            PlayerMoveRightOrLeft(R_OR_L);
        }
    }*/
    int previousR_OR_L = -1;
    void KeyboradControlPlayer()
    {
        int newR_OR_L = -1; // 初始化为 -1，表示没有按下左或右

        if (Input.GetKey("right"))
        {
            newR_OR_L = 0;

        }
        else if (Input.GetKey("left"))
        {
            newR_OR_L = 1;
        }
        // 判断 R_OR_L 是否发生变化
        if (newR_OR_L != previousR_OR_L)
        {
            // 执行处理
            PlayerMoveRightOrLeft(newR_OR_L);

            // 更新 previousR_OR_L
            previousR_OR_L = newR_OR_L;
        }
        //Debug.Log("newR_OR_L: " + newR_OR_L);
    }
    bool PlayerRightLegIsUp = false;
    bool PlayerLeftLegIsUp = false;
    int RightLegNumber = 0;
    int LeftLegNumber = 0;
    int RightLegNumber_i = 0;
    int LeftLegNumber_i = 0;
    int newR_OR_L = -1;
    List<float> RightLegValue = new List<float>();
    List<float> LeftLegValue = new List<float>();
    List<float> AbdominalXrotation = new List<float>();//姿態穩定
    List<float> WhenRightLegMove = new List<float>();//紀錄腳移動的時間
    List<float> WhenLeftLegMove = new List<float>();
    List<int> R_OR_L = new List<int>();
    float RTimer = 0;//右腳抬起
    float LTimer;//左腳抬起
    List<float> RightLegTimer = new List<float>();
    List<float> LeftLegTimer = new List<float>();
    int RightStepCount = 0, LeftStepCount = 0;
    void TrackerControlPlayer()
    {
        isEnter = true;
        AbdominalXrotation.Add(Chest.transform.eulerAngles.x);
        if (LeftLeg.transform.position.y < StandardValue && RightLeg.transform.position.y > StandardValue)//右腳抬起
        {
            //PlayerRightLegIsUp = true;
            RightLegNumber_i = +1;
            RightLegNumber = RightLegNumber_i % 2;
            //Debug.Log("RightLegNumber_i: " + RightLegNumber_i);

            /*數據紀錄*/
            RightLegValue.Add(RightLeg.transform.eulerAngles.z);

            /*計算時間長度 用來計算速率*/
            RTimer += Time.deltaTime;
            Debug.Log("RTimer" + RTimer);
            /*計算右腳步數*/
            RightStepCount++;
            Debug.Log("RightStepCount: " + RightStepCount);

            if (/*PlayerRightLegIsUp && PlayerLeftLegIsUp == false && */RightLegNumber == 1)//用數字算i=+1 算進來幾次
            {
                ImageBlink(1010, true);
                newR_OR_L = 1;
                R_OR_L.Add(newR_OR_L);
            }
            else
            {
                ImageBlink(1010, false);
            }
        }
        else if (LeftLeg.transform.position.y > StandardValue && RightLeg.transform.position.y < StandardValue)
        {
            //PlayerLeftLegIsUp = true;
            LeftLegNumber_i = +1;
            LeftLegNumber = LeftLegNumber_i % 2;
            Debug.Log("LeftLegNumber_i: " + LeftLegNumber_i);

            /*數據紀錄*/
            LeftLegValue.Add(LeftLeg.transform.eulerAngles.z);


            /*計算時間長度 用來計算速率*/
            LTimer += Time.deltaTime;
            Debug.Log("LTimer: " + LTimer);
            /*計算右腳步數*/
            LeftStepCount++;
            Debug.Log("LeftStepCount: " + LeftStepCount);
            if (/*PlayerRightLegIsUp == false && PlayerLeftLegIsUp &&*/ LeftLegNumber == 1)
            {
                ImageBlink(0101, true);
                newR_OR_L = 0;
                R_OR_L.Add(newR_OR_L);
            }
            else
            {
                ImageBlink(0101, false);
            }
        }
        else if (LeftLeg.transform.position.y > StandardValue && RightLeg.transform.position.y > StandardValue)
        {
            ImageBlink(1100, true);
            PlayerRightLegIsUp = false;
            PlayerLeftLegIsUp = false;
            RightLegTimer.Add(RTimer);//站在台階上
            RTimer = 0;
            LeftLegTimer.Add(LTimer);
            LTimer = 0;
        }
        else
        {
            ImageBlink(0011, true);
            if (newR_OR_L != previousR_OR_L && newR_OR_L == 0 || newR_OR_L == 1)
            {
                // 執行
                PlayerMoveRightOrLeft(R_OR_L[0]);
                // 更新 previousR_OR_L
                previousR_OR_L = newR_OR_L;
                R_OR_L.Clear();


                //紀錄時間
                RightLegTimer.Add(RTimer);//站在地上
                RTimer = 0;
                LeftLegTimer.Add(LTimer);
                LTimer = 0;
            }
            R_OR_L.Clear();
            isEnter = false;
        }
        Debug.Log("newR_OR_L: " + newR_OR_L);
    }
    void Judgment_Criteria(float ChestTransformY, float RightLegTransformY, float LeftLegTransformY)
    {
        List<float> TotalData = new List<float> { RightLegTransformY, LeftLegTransformY };
        /*/float Mean = TotalData.Average();
        // 計算每個數據與平均值的差值的平方相加
        float sumOfSquares = TotalData.Sum(x => Mathf.Pow(x - Mean, 2));
        float variance = sumOfSquares / TotalData.Count;
        //StandardValue = Mathf.Sqrt(variance);*/
        StandardValue = TotalData.Average() + 0.1;
        if (StandardValue < 0.45)
        {
            isReady = false;
            Debug.Log("標準值過小");
        }
    }
    void ImageBlink(int State, bool IsUp)//控制提示圖片每0.5秒閃爍
    {
        if (State == 1010)//以圖片逆時間開始算 Ex:右腳先踏, 白色255 黑色0，透明度0看不到 透明度255看的到
        {
            if (IsUp == true)
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 0, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 0, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 255, 1);
                if (Time.time % 2 > 0.2)
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 255, 1);
                }
                else
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 255, 0);
                }
            }
            else if (IsUp == false)
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 0, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 255, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 0, 1);
                if (Time.time % 2 > 0.2)
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 255, 1);
                }
                else
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 255, 0);
                }
            }
        }
        if (State == 0101)// 白色255 黑色0，透明度0看不到 透明度255看的到
        {
            if (IsUp == true)
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 0, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 255, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 0, 1);
                if (Time.time % 2 > 0.5)
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 255, 1);
                }
                else
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 255, 0);
                }
            }
            else if (IsUp == false)
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 255, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 0, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 0, 1);
                if (Time.time % 2 > 0.5)
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 255, 1);
                }
                else
                {
                    Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 255, 0);
                }
            }
        }
        if (State == 1100)//以圖片逆時間開始算 Ex:右腳先踏, 白色255 黑色0，透明度0看不到 透明度255看的到
        {
            Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 0, 1);
            Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 0, 1);
            if (Time.time % 2 > 0.5)
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 255, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 255, 1);
            }
            else
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 255, 0);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 255, 0);
            }
        }
        if (State == 0011)//以圖片逆時間開始算 Ex:右腳先踏, 白色255 黑色0，透明度0看不到 透明度255看的到
        {
            Climing_ImageBlink.ClimingImageBlink.ImageBlink(2, 0, 1);
            Climing_ImageBlink.ClimingImageBlink.ImageBlink(3, 0, 1);

            if (Time.time % 2 > 0.5)
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 255, 1);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 255, 1);
            }
            else
            {
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(0, 255, 0);
                Climing_ImageBlink.ClimingImageBlink.ImageBlink(1, 255, 0);
            }

        }
    }
    //bool isMoving = false;
    void PlayerMoveRightOrLeft(int MoveR_OR_L)
    {

        if (MoveR_OR_L == 0 && State == MoveR_OR_L)
        {
            transform.position = transform.position + transform.forward + transform.up * 2;
        }
        else if (MoveR_OR_L == 1 && State == MoveR_OR_L)
        {
            transform.position = transform.position + transform.forward + transform.up * 2;
        }
        else if (MoveR_OR_L == 0 && State != MoveR_OR_L)
        {
            transform.Rotate(0f, 90f, 0f, Space.Self);
            transform.position = transform.position + transform.forward + transform.up * 2;
            State = MoveR_OR_L;
        }
        else if (MoveR_OR_L == 1 && State != MoveR_OR_L)
        {
            transform.Rotate(0f, -90f, 0f, Space.Self);
            transform.position = transform.position + transform.forward + transform.up * 2;
            State = MoveR_OR_L;
        }
    }


    int CoinNumber = 0;
    public Text Coin;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Climing_Coin")
        {
            Destroy(other.gameObject);
            CoinNumber++;
            //Debug.Log("金幣數量: " + CoinNumber);
            Coin.text = CoinNumber.ToString();
        }
    }
    public static bool isGameOver = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Climing_Castle")
        {
            isGameOver = true;
            Climing_StartTimer = false;
            DataAnalysis();
            //climing_GameControl.Climing_GameOver();
            Debug.Log("遊戲結束");
        }
    }
    public Text WhenGameOverShowCoin, ChestSandardValue, RightLegSandardValue, LeftLegSandardValue,
        AverageSpeed, RightLegAverageSpeed, LeftLegAverageSpeed;

    void DataAnalysis()//整個遊戲的分析
    {
        if (isGameOver)
        {
            WhenGameOverShowCoin.text = CoinNumber.ToString();
            float AbdominalXrotationMean = AbdominalXrotation.Average();
            // 計算每個數據與平均值的差值的平方相加
            float sumOfSquares = AbdominalXrotation.Sum(x => Mathf.Pow(x - AbdominalXrotationMean, 2));
            float variance = sumOfSquares / AbdominalXrotation.Count;
            float StandardValue = Mathf.Sqrt(variance);
            ChestSandardValue.text = StandardValue.ToString();
            Debug.Log("Chest標準差" + StandardValue);
            //雙腳各自的分析
            float RightLegZRotationValueAverage = RightLegValue.Average();
            float RightLegZRotationSumOfSquares = RightLegValue.Sum(x => Mathf.Pow(x - RightLegZRotationValueAverage, 2));
            float RightLegZRotationvariance = RightLegZRotationSumOfSquares / RightLegValue.Count;
            float RightLegZRotationStandardValue = Mathf.Sqrt(RightLegZRotationvariance);
            RightLegSandardValue.text = RightLegZRotationStandardValue.ToString();
            Debug.Log("右腳標準差" + RightLegZRotationStandardValue);
            float LeftLegZRotationValueAverage = LeftLegValue.Average();
            float LeftLegZRotationSumOfSquares = LeftLegValue.Sum(x => Mathf.Pow(x - LeftLegZRotationValueAverage, 2));
            float LeftLegZRotationvariance = LeftLegZRotationSumOfSquares / LeftLegValue.Count;
            float LeftLegZRotationStandardValue = Mathf.Sqrt(LeftLegZRotationvariance);
            LeftLegSandardValue.text = LeftLegZRotationStandardValue.ToString();
            Debug.Log("左腳標準差" + LeftLegZRotationStandardValue);
            //遊戲均速
            float AverageStepTime = 90 / (RightStepCount + LeftStepCount);//平均每步所需要的時間
            AverageSpeed.text = AverageStepTime.ToString();
            Debug.Log("平均每步所需要的時間" + AverageStepTime);
            float rightLegAverageSpeed = RightLegTimer.Sum() / RightStepCount;//右腳的均速
            RightLegAverageSpeed.text = rightLegAverageSpeed.ToString();
            Debug.Log("右腳的均速" + rightLegAverageSpeed);
            float leftLegAverageSpeed = LeftLegTimer.Sum() / LeftStepCount;//左腳的均速
            LeftLegAverageSpeed.text = leftLegAverageSpeed.ToString();
            Debug.Log("左腳的均速" + leftLegAverageSpeed);
        }
    }
    void ClimingSave()
    {
        string fileName = GameList.userID + "_" + GameList.userBithday_Date;
        string filePath = Path.Combine(Application.dataPath, fileName);

        /*using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Variable,Value"); // Writing headers

            // Writing existing data
            writer.WriteLine("WhenGameOverShowCoin," + CoinNumber.ToString());
            writer.WriteLine("ChestStandardValue," + StandardValue.ToString());
            writer.WriteLine("RightLegStandardValue," + RightLegZRotationStandardValue.ToString());
            writer.WriteLine("LeftLegStandardValue," + LeftLegZRotationStandardValue.ToString());
            writer.WriteLine("AverageStepTime," + AverageStepTime.ToString());
            writer.WriteLine("RightLegAverageSpeed," + rightLegAverageSpeed.ToString());
            writer.WriteLine("LeftLegAverageSpeed," + leftLegAverageSpeed.ToString());

            // Writing sensor values
            writer.WriteLine("LeftLeg.transform," + LeftLeg.transform.ToString());
            writer.WriteLine("RightLeg.transform," + RightLeg.transform.ToString());
            writer.WriteLine("Chest.transform," + Chest.transform.ToString());

            // Writing list data
            writer.WriteLine("List1Data," + string.Join(",", List1.Select(x => x.ToString()).ToArray()));
            writer.WriteLine("List2Data," + string.Join(",", List2.Select(x => x.ToString()).ToArray()));
        }*/

        Debug.Log("Data saved to CSV file.");
    }

    void StairTower_DefinedViveTracker(GameObject Tracker, GameObject Tracker1, GameObject Tracker2)
    {
        if (Tracker.transform.position.y > Tracker1.transform.position.y &&
            Tracker.transform.position.y > Tracker2.transform.position.y)
        {
            Chest = Tracker;
            Debug.Log("1");
            if (Tracker2.transform.position.x > Tracker1.transform.position.x &&
                Tracker.transform.position.x > Tracker1.transform.position.x)
            {
                RightLeg = Tracker2;
                LeftLeg = Tracker1;
            }
            else
            {
                RightLeg = Tracker1;
                LeftLeg = Tracker2;
            }
            isReady = true;
        }
        if (Tracker1.transform.position.y > Tracker.transform.position.y &&
            Tracker1.transform.position.y > Tracker2.transform.position.y)
        {
            Chest = Tracker1;
            Debug.Log("2");
            if (Tracker2.transform.position.x > Tracker.transform.position.x &&
                Tracker1.transform.position.x > Tracker.transform.position.x)
            {
                RightLeg = Tracker2;
                LeftLeg = Tracker;
            }
            else
            {
                RightLeg = Tracker;
                LeftLeg = Tracker2;
            }
            isReady = true;
        }
        if (Tracker2.transform.position.y > Tracker.transform.position.y &&
            Tracker2.transform.position.y > Tracker1.transform.position.y)
        {
            Chest = Tracker2;
            Debug.Log("3");
            if (Tracker.transform.position.x > Tracker1.transform.position.x &&
                Tracker2.transform.position.x > Tracker1.transform.position.x)
            {
                RightLeg = Tracker;
                LeftLeg = Tracker1;
            }
            else
            {
                RightLeg = Tracker1;
                LeftLeg = Tracker;
            }
            isReady = true;
        }


    }

}
