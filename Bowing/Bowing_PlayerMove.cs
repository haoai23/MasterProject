using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp.Tracking;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public  class Bowing_PlayerMove : MonoBehaviour
{
    bool isReady = false;
    public GameObject Tracker, Tracker1, Tracker2;
    static public GameObject LeftLeg, RightLeg, Chest;
    public GameObject Whale_LowPoly;
    SplineFollower _SplineFollower;
    int ViveNumber =0;
    float MoveSpeed;
    public GameObject Up_Arrow, DownArrow, OK;
    float RightLegEulerAnglesValue, LeftLegEulerAnglesValue, ChestEulerAnglesValue;//用來記錄第一次的原始數據
    float RightLegYEulerAnglesValue, LeftLegYEulerAnglesValue;//用來記錄第一次的原始數據
    public static bool isGameOver = false;
  /*  public Text _GameSpendTime, _MovingAnegle, _AverageSpeed, _RightLeg, _LeftLeg;*/
    // Start is called before the first frame update
    void Start()
    {
        _SplineFollower = this.gameObject.GetComponent<SplineFollower>();
        RightLeg = GameObject.FindWithTag("RightLeg");
        LeftLeg = GameObject.FindWithTag("LeftLeg");
        Chest = GameObject.FindWithTag("Chest");
    }
    float RecordLastRYValue = 0, RecordNewRYValue;
    float RecordLastLYValue = 0, RecordNewLYValue;
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
            PlayerMove(PlayerMoveSpeed());
            Debug.Log("IsStart");
        }
        
        if (isReady & !isGameOver)
        {
            RightLegMoveAngle.Add(RightLeg.transform.eulerAngles.x - RightLegEulerAnglesValue);
            LeftLegMoveAngle.Add(LeftLeg.transform.eulerAngles.x - LeftLegEulerAnglesValue);
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
        
        float leftLegAngularSpeed;
        float currentLeftLegRotation = LeftLeg.transform.eulerAngles.x;
        

        leftLegAngularSpeed = Mathf.Abs(currentLeftLegRotation - previousLeftLegRotation) / Time.deltaTime ;

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
    void RecordMoveAngle()
    {
        RecordNewRYValue = RightLeg.transform.eulerAngles.y - RightLegYEulerAnglesValue;
        RecordNewLYValue = LeftLeg.transform.eulerAngles.y - LeftLegYEulerAnglesValue;
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
        else { RecordLastRYValue = RecordNewLYValue; }
        Debug.Log("IsRecord");
    }
    List<float> RightLegValue = new List<float>();
    List<float> LeftLegValue = new List<float>();
    List<float> RightLegMoveAngle = new List<float>();//受測者的移動角度
    List<float> LeftLegMoveAngle = new List<float>();//受測者的移動角度
    List<float> AverageSpeed = new List<float>();
    public Text RightLegSandardValue, LeftLegSandardValue, _AverageSpeed, _RightMoveAngle, _LeftMoveAngle;
    void Bowing_DataAnalysis()
    {
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
        //平均速度
        _AverageSpeed.text = AverageSpeed.Average().ToString();
        Debug.Log("移動速度: " + _AverageSpeed.text);
        //平均移動角度
        _LeftMoveAngle.text = LeftLegMoveAngle.Average().ToString();
        _RightMoveAngle.text = RightLegMoveAngle.Average().ToString();


    }    
}
