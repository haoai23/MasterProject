using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp.Tracking;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Text;

public  class Bowing_PlayerMove : MonoBehaviour
{
    bool isReady = false;
    public GameObject Tracker, Tracker1, Tracker2;
    public GameObject LeftLeg, RightLeg, Chest;
    public GameObject Whale_LowPoly;
    SplineFollower _SplineFollower;
    int ViveNumber =0;
    float MoveSpeed;
    public GameObject Up_Arrow, DownArrow, OK;
    float RightLegEulerAnglesValue, LeftLegEulerAnglesValue, ChestEulerAnglesValue;//用來記錄第一次的原始數據
    float RightLegYEulerAnglesValue, LeftLegYEulerAnglesValue;//用來記錄第一次的原始數據
    float RightLegFirstXPosition, LeftLegFitstXPosition;
    public static bool isGameOver = false;

    List<float> LeftLegPX = new List<float>();
    List<float> LeftLegPY = new List<float>(); 
    List<float> LeftLegPZ = new List<float>();
    List<float> LeftLegRX = new List<float>();
    List<float> LeftLegRY = new List<float>();
    List<float> LeftLegRZ = new List<float>();

    List<float> ChestPX = new List<float>();
    List<float> ChestPY = new List<float>();
    List<float> ChestPZ = new List<float>();
    List<float> ChestRX = new List<float>();
    List<float> ChestRY = new List<float>();
    List<float> ChestRZ = new List<float>();

    List<float> RightLegPX = new List<float>();
    List<float> RightLegPY = new List<float>();
    List<float> RightLegPZ = new List<float>();
    List<float> RightLegRX = new List<float>();
    List<float> RightLegRY = new List<float>();
    List<float> RightLegRZ = new List<float>();
    /*  public Text _GameSpendTime, _MovingAnegle, _AverageSpeed, _RightLeg, _LeftLeg;*/
    // Start is called before the first frame update
    public static int BowingSceneTimes = 0;
    public GameObject RestartButton;

    void Start()
    {
        _SplineFollower = this.gameObject.GetComponent<SplineFollower>();
        RightLeg = GameObject.FindWithTag("RightLeg");
        LeftLeg = GameObject.FindWithTag("LeftLeg");
        Chest = GameObject.FindWithTag("Chest");

        BowingSceneTimes++;
        Debug.Log("BowingSceneTimes" + BowingSceneTimes);
        if (BowingSceneTimes > 1)
        {
            Bowing_GameControl gameController = RestartButton.GetComponent<Bowing_GameControl>();
            gameController.RestartGame();
            BowingSceneTimes = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Bowing_GameControl.isStart) 
        {
            RightLegEulerAnglesValue = RightLeg.transform.eulerAngles.x;
            LeftLegEulerAnglesValue = LeftLeg.transform.eulerAngles.x; 
            ChestEulerAnglesValue = Chest.transform.eulerAngles.x;

            RightLegYEulerAnglesValue = RightLeg.transform.eulerAngles.y;
            LeftLegYEulerAnglesValue = LeftLeg.transform.eulerAngles.y;

            RightLegFirstXPosition = RightLeg.transform.position.x;
            LeftLegFitstXPosition = LeftLeg.transform.position.x;
            PlayerMove(PlayerMoveSpeed());
            Debug.Log("IsStart");
        }
        
        if (isReady & !isGameOver)
        {
            /*RightLegMoveAngle.Add(RightLeg.transform.eulerAngles.x - RightLegEulerAnglesValue);
            LeftLegMoveAngle.Add(LeftLeg.transform.eulerAngles.x - LeftLegEulerAnglesValue);*/

            LeftLegPX.Add(LeftLeg.transform.position.x);
            LeftLegPY.Add(LeftLeg.transform.position.y);
            LeftLegPZ.Add(LeftLeg.transform.position.z);
            LeftLegRX.Add(LeftLeg.transform.eulerAngles.x);
            LeftLegRY.Add(LeftLeg.transform.eulerAngles.y);
            LeftLegRZ.Add(LeftLeg.transform.eulerAngles.z);

            ChestPX.Add(Chest.transform.position.x);
            ChestPY.Add(Chest.transform.position.y);
            ChestPZ.Add(Chest.transform.position.z);
            ChestRX.Add(Chest.transform.eulerAngles.x);
            ChestRY.Add(Chest.transform.eulerAngles.y);
            ChestRZ.Add(Chest.transform.eulerAngles.z);

            RightLegPX.Add(LeftLeg.transform.position.x);
            RightLegPY.Add(LeftLeg.transform.position.y);
            RightLegPZ.Add(LeftLeg.transform.position.z);
            RightLegRX.Add(LeftLeg.transform.eulerAngles.x);
            RightLegRY.Add(LeftLeg.transform.eulerAngles.y);
            RightLegRZ.Add(LeftLeg.transform.eulerAngles.z);
            RecordMoveAngle();
        }
        if (isGameOver)
        {
            Bowing_DataAnalysis();
            Debug.Log("isGameOver");
        }
         DefinedViveTracker(/*Tracker, Tracker1, Tracker2*/);


    }
    void DefinedViveTracker(/*GameObject Tracker, GameObject Tracker1, GameObject Tracker2*/)
    {
        RightLeg = GameObject.FindWithTag("RightLeg");
        LeftLeg = GameObject.FindWithTag("LeftLeg");
        Chest = GameObject.FindWithTag("Chest");
        if (LeftLeg != null & RightLeg != null & Chest != null)
        {
            isReady = true;
        }
        /*if (Tracker.transform.position.y > Tracker1.transform.position.y &&
            Tracker.transform.position.y > Tracker2.transform.position.y)
        {
            Chest = Tracker;
            if (Tracker1.transform.position.x > Tracker2.transform.position.x)
            {
                RightLeg = Tracker1;
                LeftLeg = Tracker2;                           
            }
            else 
            {
                RightLeg = Tracker2;
                LeftLeg = Tracker1;          
           }
           isReady = true;
       }
        if (Tracker1.transform.position.y > Tracker.transform.position.y &&
            Tracker1.transform.position.y > Tracker2.transform.position.y)
        {
            Chest = Tracker1;
            if (Tracker.transform.position.x > Tracker2.transform.position.x)
            {
                RightLeg = Tracker;
                LeftLeg = Tracker2;              
            }
            else
            {
                RightLeg = Tracker2;
                LeftLeg = Tracker;              
            }
           isReady = true;
       }
        if (Tracker2.transform.position.y > Tracker.transform.position.y &&
            Tracker2.transform.position.y > Tracker1.transform.position.y)
        {
            Chest = Tracker2;
            if (Tracker.transform.position.x > Tracker1.transform.position.x)
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
        }*/


    }
    void PlayerMove(float MoveSpeed)
    {
        //Debug.Log(Mathf.Abs(LeftLeg.transform.eulerAngles.x - LeftLegEulerAnglesValue));
        if (isReady)
        {           
            if (/*Mathf.Abs(LeftLeg.transform.eulerAngles.x - LeftLegEulerAnglesValue) > 40 && */
                RightLeg.transform.position.y - LeftLeg.transform.position.y < 1)    
            {
                 _SplineFollower.followSpeed = MoveSpeed;
            }
            else
            {
                _SplineFollower.followSpeed = 5f;
                _SplineFollower.followSpeed = Mathf.Lerp(_SplineFollower.followSpeed, 0f, 1f * Time.deltaTime);                
            }
         

            //_SplineFollower.followSpeed = MoveSpeed;
        }        
    }
    float previousLeftLegRotation = 0;
    float PlayerMoveSpeed()//可以再寫防呆的部分左右腳的不對稱動作
    {
        float currentLeftLegRotation = LeftLeg.transform.eulerAngles.x;
        float leftLegAngularSpeed = Mathf.Abs(currentLeftLegRotation - previousLeftLegRotation) / Time.deltaTime ;

        previousLeftLegRotation = currentLeftLegRotation;
        Debug.Log("角速度: "+ leftLegAngularSpeed);
        if ( leftLegAngularSpeed > 25 ) 
        {
            leftLegAngularSpeed = 25;
        }
        AverageSpeed.Add(leftLegAngularSpeed);
        return leftLegAngularSpeed; // 返回角速度
    }
    int GOL = 0;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Bowing_Whale" & 
            Chest.transform.eulerAngles.x - LeftLeg.transform.eulerAngles.x < 45) 
        {
            //StartCoroutine(StartCountdown());
            OK.SetActive(true);
            DownArrow.SetActive(false);
            Up_Arrow.SetActive(false);
            Debug.Log("c8 c8 有啦");

        }
        else if (Chest.transform.eulerAngles.x - LeftLeg.transform.eulerAngles.x > 55)
        {
            OK.SetActive(false);
            DownArrow.SetActive(false);
            Up_Arrow.SetActive(true);
            Debug.Log("腳抬高一些");
        }
        else if(Chest.transform.eulerAngles.x - LeftLeg.transform.eulerAngles.x > 35) 
        {
            OK.SetActive(false);
            DownArrow.SetActive(true);
            Up_Arrow.SetActive(false);
            Debug.Log("腳抬低一些");
        }
        if(collision.gameObject.tag == "Bowing_GameOverLine")
        {          
            
            GOL += 1;
            if (GOL > 1) 
            {
                isGameOver = true;
                _SplineFollower.followSpeed = 0f;
                Bowing_DataAnalysis();
            }
            Debug.Log(GOL);
            Debug.Log("isgameover" + isGameOver);
        }
    }
    IEnumerator StartCountdown()
    {
        float CountdownTime = 10f;
        while(CountdownTime > 0f) 
        {
            yield return new WaitForSeconds(1);
            CountdownTime -= Time.deltaTime;
        }
        
    }
    float RecordLastRYValue = 0, RecordNewRYValue;
    float RecordLastLYValue = 0, RecordNewLYValue;

    float RecordLastRXPValue = 0, RecordNewRXPValue;
    float RecordLastLXPValue = 0, RecordNewLXPValue;

    void RecordMoveAngle()
    {
        RecordNewRYValue = RightLeg.transform.eulerAngles.x - RightLegEulerAnglesValue;
        RecordNewLYValue = LeftLeg.transform.eulerAngles.x - LeftLegEulerAnglesValue;
        if (RecordNewRYValue > RecordLastRYValue)
        {
            RightLegMoveAngle.Add(RecordNewRYValue);
            RecordLastRYValue = RecordNewRYValue;
        }
        else { RecordLastRYValue = RecordNewRYValue; }

        if (RecordNewLYValue > RecordLastLYValue)
        {
            LeftLegMoveAngle.Add(RecordNewLYValue);
            RecordLastLYValue = RecordNewLYValue;
        }
        else { RecordLastLYValue = RecordNewLYValue; }

        RecordNewRXPValue = RightLeg.transform.position.x - RightLegFirstXPosition;
        RecordNewLXPValue = LeftLeg.transform.position.x - LeftLegFitstXPosition;
        if (RecordNewRXPValue > RecordLastRXPValue)
        {
            UPRightLegMoveXPosition.Add(RecordNewRXPValue);
            RecordLastRXPValue = RecordNewRXPValue;
        }
        else 
        { 
            RecordLastRXPValue = RecordNewRXPValue;
            DownRightLegMoveXPosition.Add(Mathf.Abs(RecordNewRXPValue));

        }

        if (RecordNewLXPValue > RecordLastLXPValue)
        {
            UPLeftLegMoveXposition.Add(RecordNewLXPValue);
            RecordLastLXPValue = RecordNewLXPValue;
        }
        else { RecordLastLXPValue = RecordNewLXPValue;
            DownLeftMoveXPosition.Add(RecordNewLXPValue);
        }
        Debug.Log("IsRecord");
    }
    List<float> RightLegValue = new List<float>();
    List<float> LeftLegValue = new List<float>();
    List<float> RightLegMoveAngle = new List<float>();//受測者的移動角度
    List<float> LeftLegMoveAngle = new List<float>();//受測者的移動角度
    List<float> UPRightLegMoveXPosition = new List<float>();//向上右腳受測者X位置的移動軌跡
    List<float> UPLeftLegMoveXposition = new List<float>();//向下受測者X位置的移動軌跡
    List<float> DownRightLegMoveXPosition = new List<float>();//向上右腳受測者X位置的移動軌跡
    List<float> DownLeftMoveXPosition = new List<float>();//向下受測者X位置的移動軌跡

    List<float> AverageSpeed = new List<float>();
    public Text RightLegSandardValue, LeftLegSandardValue, _AverageSpeed, _RightMoveAngle, _LeftMoveAngle;
    void Bowing_DataAnalysis()
    {
        /*//雙腳各自的分析
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
        Debug.Log("左腳標準差" + LeftLegZRotationStandardValue);*/
        //平均速度
        _AverageSpeed.text = AverageSpeed.Average().ToString();
        Debug.Log("移動速度: " + _AverageSpeed.text);
        //平均移動角度                                                      
        _LeftMoveAngle.text = LeftLegMoveAngle.Average().ToString();
        _RightMoveAngle.text = RightLegMoveAngle.Average().ToString();

        BowingSaveCSV();
    }
    public void BowingSaveCSV()
    {
        string fileName = "Bowing.csv";
        string timePath = Path.Combine(PlayerPrefs.GetString("timePath"), fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("LeftLegPX,LeftLegPY,LeftLegPZ,LeftLegRX,LeftLegRY,LeftLegRZ,ChestPX,ChestPY,ChestPZ,ChestRX,ChestRY,ChestRZ,RightLegPX,RightLegPY,RightLegPZ,RightLegRX,RightLegRY,RightLegRZ,UPRightLegMoveXPosition,UPLeftLegMoveXposition,DownRightLegMoveXPosition,DownLeftLegMoveXposition,AverageSpeed,LeftMoveAngle,RightMoveAngle");

        // 確定最大長度
        int maxLength = new int[] { LeftLegPX.Count, LeftLegPY.Count, LeftLegPZ.Count, LeftLegRX.Count, LeftLegRY.Count, LeftLegRZ.Count,
                                    ChestPX.Count, ChestPY.Count, ChestPZ.Count, ChestRX.Count, ChestRY.Count, ChestRZ.Count,
                                    RightLegPX.Count, RightLegPY.Count, RightLegPZ.Count, RightLegRX.Count, RightLegRY.Count, RightLegRZ.Count,
                                    UPRightLegMoveXPosition .Count,UPLeftLegMoveXposition.Count,DownRightLegMoveXPosition.Count,DownLeftMoveXPosition.Count}.Max();


        // 根據最大長度遍歷
        for (int i = 0; i < maxLength; i++)
        {
            string line = $"{GetValueOrDefault(LeftLegPX, i)},{GetValueOrDefault(LeftLegPY, i)},{GetValueOrDefault(LeftLegPZ, i)},{GetValueOrDefault(LeftLegRX, i)},{GetValueOrDefault(LeftLegRY, i)},{GetValueOrDefault(LeftLegRZ, i)}," +
                          $"{GetValueOrDefault(ChestPX, i)},{GetValueOrDefault(ChestPY, i)},{GetValueOrDefault(ChestPZ, i)},{GetValueOrDefault(ChestRX, i)},{GetValueOrDefault(ChestRY, i)},{GetValueOrDefault(ChestRZ, i)}," +
                          $"{GetValueOrDefault(RightLegPX, i)},{GetValueOrDefault(RightLegPY, i)},{GetValueOrDefault(RightLegPZ, i)},{GetValueOrDefault(RightLegRX, i)},{GetValueOrDefault(RightLegRY, i)},{GetValueOrDefault(RightLegRZ, i)},"+
                          $"{GetValueOrDefault(UPRightLegMoveXPosition, i)},{GetValueOrDefault(UPLeftLegMoveXposition, i)},{GetValueOrDefault(DownRightLegMoveXPosition, i)},{GetValueOrDefault(DownLeftMoveXPosition, i)}";

            // 在每一行的末尾添加統計數據
            if (i == 0) // 假設統計數據只需添加一次
            {
                line += $",{AverageSpeed.Average()},{LeftLegMoveAngle.Average()},{RightLegMoveAngle.Average()}";
            }
            sb.AppendLine(line);
        }

        // 使用 FileStream 和 StreamWriter 寫入文件
        using (FileStream fs = new FileStream(timePath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.Write(sb.ToString());
        }
    }

    // 輔助方法來處理可能的索引越界問題
    private string GetValueOrDefault(List<float> list, int index)
    {
        if (index < list.Count)
        {
            return list[index].ToString();
        }
        return "N/A"; // 或者您可以選擇返回空字符串 ""
    }
}
