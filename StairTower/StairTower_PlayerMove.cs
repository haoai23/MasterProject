using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
//using UnityEngine.Windows;



public class StairTower_PlayerMove : MonoBehaviour
{
    GameObject CurrentFloor;
    public GameObject StairTower_Player;
    bool IsJumping = false;
    public Text Score;
    public GameObject Tracker1, Tracker2, Tracker3;
    GameObject Chest, RightLeg, LeftLeg;
    bool isReady = false;
    public static int StairTower_Score;
    public Text StairTowerScore_Text;
    public static bool StairTowerStartTimer = false;

    public GameObject _Start_Panel,_GameOver_Panel;

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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            StairTower_Player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 2f;
            IsJumping = true;
        }
        if (isReady == false)
        { 
            StairTower_DefinedViveTracker(Tracker1, Tracker2, Tracker3);
        }
        else if (isReady == true)
        {
            RecordPlayerPosture();
            if (RecordSuceesful)
            {
                PlayerMove();
                AnalyzePlayerData();
                StairTowerStartTimer = true;

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


            }   
            if(isGameOver)
            {
                
                _GameOver_Panel.SetActive(true);
            }
        }
    }
    private void PlayerMove() 
    {
        //StairTower_Player.transform.Translate(new Vector2(2.3f * Time.deltaTime * direction , 0));
        if (RightLeg.transform.position.y > LeftLeg.transform.position.y + 0.1 && PersonalStandardHeight != 0)//右腳比左腳高Xf
        {
            StairTower_Player.transform.Translate(new Vector2(0.03f, 0));
        }

        else if (RightLeg.transform.position.y + 0.1 < LeftLeg.transform.position.y && PersonalStandardHeight != 0)//左腳比左腳高Xf
        {
            StairTower_Player.transform.Translate(new Vector2(-0.03f, 0));
        }
        else if (Chest.transform.position.y - ChestNoTiptoes > PersonalStandardHeight * 0.8)//墊腳的高度判斷是否有墊腳尖，高於最大值的九成
        {
            IsJumping = true;
            StairTower_Player.GetComponent<Rigidbody2D>().velocity = Vector2.up *2f;
        }
        else
        {
            IsJumping = false;
        }
        Debug.Log("Isjump:"+IsJumping);
    }
    float RightLegTiptoes, LeftLegTiptoes, ChestTiptoes;
    float RightLegNoTiptoes, LeftLegNoTiptoes, ChestNoTiptoes;

    int i = 0;
    float PersonalStandardHeight = 0;
    bool RecordSuceesful = false;
    void RecordPlayerPosture()
    {
 
        if (Input.GetKeyDown("r"))
        {
            i += 1;
            if (i == 1 && RightLeg != null && LeftLeg != null && Chest != null)//沒有點腳尖
            {
                RightLegNoTiptoes = RightLeg.transform.position.y;
                LeftLegNoTiptoes = LeftLeg.transform.position.y;
                ChestNoTiptoes = Chest.transform.position.y;
                Debug.Log(i + ": OK");                
            }
            if (i == 2 && RightLeg != null && LeftLeg != null && Chest != null)//有點腳尖
            {
                RightLegTiptoes = RightLeg.transform.position.y;
                LeftLegTiptoes = LeftLeg.transform.position.y;
                ChestTiptoes = Chest.transform.position.y;
                RecordSuceesful = true;
                _Start_Panel.SetActive(false);
                Debug.Log(i + ": OK");
            }   
        }
        if (RecordSuceesful)
        {
            PersonalStandardHeight = ((RightLegTiptoes - RightLegNoTiptoes) + (LeftLegTiptoes - LeftLegNoTiptoes) + (ChestTiptoes - ChestNoTiptoes)) / 3;///平均高度
            
            Debug.Log("個人標準高度: "+PersonalStandardHeight);
        }
    }
    public Text CountStep;
    int countStep = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StairTower_NormalFloor")
        {                 
            
            CurrentFloor = collision.gameObject;
            if (IsJumping && collision.contacts[0].normal == new Vector2(0f, -1f))
            {                   
                CurrentFloor.GetComponent<BoxCollider2D>().enabled = false;
                Debug.Log("目前階梯: "+CurrentFloor.GetComponent<BoxCollider2D>().enabled);
                countStep += 1;
                Debug.Log("CountStep: " + countStep);
                CountStep.text = countStep.ToString();
                /*if ( CurrentFloor.GetComponent<BoxCollider2D>().enabled == false)
                 {
                     CurrentFloor.GetComponent<BoxCollider2D>().enabled = true;
                     IsJumping = false;
                 }*/
                StartCoroutine(EnableColliderAfterDelay(CurrentFloor.GetComponent<BoxCollider2D>(), 1f));

            }
            else if (collision.contacts[0].normal == new Vector2(0f, -1f))
            {
                //ModifyHp(1);
            }
        }
        else if (collision.gameObject.tag == "StairTower_NailsFloor")
        {


            if (IsJumping && collision.contacts[0].normal == new Vector2(-1f, 0f))
            {
                CurrentFloor.GetComponent<BoxCollider2D>().enabled = true;
                IsJumping = false; // 玩家落地，重置跳躍狀態
            }
            else if (collision.contacts[0].normal == new Vector2(-1f, 0f))
            {
                //ModifyHp(-1);
            }

        }
        /*else if (collision.gameObject.name == "Wall_Left" || collision.gameObject.name == "Wall_Right")
        {
            direction *= -1;
        }*/
        else if (collision.gameObject.tag == "StairTower_Diamond")
        {
            StairTower_Score += 1;
            StairTowerScore_Text.text = StairTower_Score.ToString();
            Destroy(collision.gameObject);
        }
    }
    public static bool isGameOver = false;//判斷是否遊戲結束了以方便分析
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StairTower_Stairline")
        {
            StairTower_PrefabMove.MoveSpeed = 0.3f + Time.time/100;

        }
        else if (collision.gameObject.name == "Deathline")
        {
            isGameOver = true;
            StairTower_PrefabMove.MoveSpeed = 0f;
            Debug.Log("遊戲結束");
        }
        Debug.Log("開始下降");
        
    }

    private IEnumerator EnableColliderAfterDelay(BoxCollider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

    void StairTower_DefinedViveTracker(GameObject Tracker, GameObject Tracker1, GameObject Tracker2)
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
        }*/


    }
    List<float> AverageRightLeg = new List<float>();
    List<float> AverageLeftLeg = new List<float>();
    List<float> AbdominalXrotation = new List<float>();
    public Text _LeftLegCalfStability, _RightLegCalfStability, _ChestStability, _Score,_Time;
    void AnalyzePlayerData()
    {
        float ChestRightDifference = Chest.transform.position.z - RightLeg.transform.position.z;
        float ChestLefttDifference = Chest.transform.position.z - LeftLeg.transform.position.z;

        _Time.text = StairTower_Timer.StairTower_i.ToString();
        if (!isGameOver)
        { 
            AbdominalXrotation.Add(Chest.transform.position.x);
            _Score.text = StairTower_Score.ToString();
        }
       
        float averageRightLeg, averageLeftLeg;
        if (isReady && RecordSuceesful && LeftLeg.transform.position.y > RightLeg.transform.position.y + 0.1f  && !isGameOver)//左腳抬起的時候
        {
            AverageRightLeg.Add(ChestRightDifference);     
        }
        else if (isReady && RecordSuceesful && RightLeg.transform.position.y > LeftLeg.transform.position.y + 0.1f && !isGameOver) //#需要測試1f是否會太高原始值為0.1f
        {
            AverageLeftLeg.Add(ChestLefttDifference);    
        }
        if (isGameOver)
        {
            float RightVariance, LeftVariance, RightStandardDeviation, LeftStandardDeviation;
            averageRightLeg = AverageRightLeg.Average();
            averageLeftLeg = AverageLeftLeg.Average();

            float AbdominalXrotationMean = AbdominalXrotation.Average();
            // 計算每個數據與平均值的差值的平方相加
            float sumOfSquares = AbdominalXrotation.Sum(x => Mathf.Pow(x - AbdominalXrotationMean, 2));
            float variance = sumOfSquares / AbdominalXrotation.Count;
            float StandardValue = Mathf.Sqrt(variance);
            _ChestStability.text = StandardValue.ToString();
            Debug.Log("Chest標準差" + StandardValue);

            float RightsumOfSquares = AverageRightLeg.Sum(x => Mathf.Pow(x - averageRightLeg, 2));
            RightVariance = RightsumOfSquares / AverageRightLeg.Count;//右腳的變異數
            RightStandardDeviation = (float)Mathf.Sqrt(RightVariance);
            _RightLegCalfStability.text= RightStandardDeviation.ToString();
            Debug.Log("RightVariance: " + RightVariance);
            Debug.Log("RightStandardDeviation: " + RightStandardDeviation);

            float LeftsumOfSquares = AverageLeftLeg.Sum(x => Mathf.Pow(x - averageLeftLeg, 2));
            LeftVariance = LeftsumOfSquares / AverageLeftLeg.Count;//左腳的變異數
            LeftStandardDeviation = (float)Mathf.Sqrt(LeftVariance);
            _LeftLegCalfStability.text = LeftStandardDeviation.ToString();
            Debug.Log("LeftVariance: " + LeftVariance);
            Debug.Log("LeftStandardDeviation: " + LeftStandardDeviation);

            StairTowerpSaveCSV(AbdominalXrotationMean, StandardValue, RightsumOfSquares, RightStandardDeviation, LeftsumOfSquares, LeftStandardDeviation, StairTower_Timer.StairTower_i, countStep, StairTower_Score);
        }
    }
    public void StairTowerpSaveCSV(float ChestXRA, float ChestXRSD, float RightLegCalfStability, float RightLegCalfStabilitySD, float LeftLegCalfStability, float LeftLegCalfStabilityYSD, float time, int stepcount, int score)
    {
        string fileName = "StairTower.csv";
        string timePath = Path.Combine(PlayerPrefs.GetString("timePath"), fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("LeftLegPX,LeftLegPY,LeftLegPZ,LeftLegRX,LeftLegRY,LeftLegRZ,ChestPX,ChestPY,ChestPZ,ChestRX,ChestRY,ChestRZ,RightLegPX,RightLegPY,RightLegPZ,RightLegRX,RightLegRY,RightLegRZ,ChestXRA,ChestXRSD,RightLegCalfStability,RightLegCalfStabilitySD,LeftLegCalfStability,LeftLegCalfStabilityYSD,time,stepcount,score");

        // 確定最大長度
        int maxLength = new int[] { LeftLegPX.Count, LeftLegPY.Count, LeftLegPZ.Count, LeftLegRX.Count, LeftLegRY.Count, LeftLegRZ.Count,
                                    ChestPX.Count, ChestPY.Count, ChestPZ.Count, ChestRX.Count, ChestRY.Count, ChestRZ.Count,
                                    RightLegPX.Count, RightLegPY.Count, RightLegPZ.Count, RightLegRX.Count, RightLegRY.Count, RightLegRZ.Count}.Max();


        // 根據最大長度遍歷
        for (int i = 0; i < maxLength; i++)
        {
            string line = $"{GetValueOrDefault(LeftLegPX, i)},{GetValueOrDefault(LeftLegPY, i)},{GetValueOrDefault(LeftLegPZ, i)},{GetValueOrDefault(LeftLegRX, i)},{GetValueOrDefault(LeftLegRY, i)},{GetValueOrDefault(LeftLegRZ, i)}," +
                          $"{GetValueOrDefault(ChestPX, i)},{GetValueOrDefault(ChestPY, i)},{GetValueOrDefault(ChestPZ, i)},{GetValueOrDefault(ChestRX, i)},{GetValueOrDefault(ChestRY, i)},{GetValueOrDefault(ChestRZ, i)}," +
                          $"{GetValueOrDefault(RightLegPX, i)},{GetValueOrDefault(RightLegPY, i)},{GetValueOrDefault(RightLegPZ, i)},{GetValueOrDefault(RightLegRX, i)},{GetValueOrDefault(RightLegRY, i)},{GetValueOrDefault(RightLegRZ, i)}";

            // 在每一行的末尾添加統計數據
            if (i == 0) // 假設統計數據只需添加一次
            {
                line += $",{ChestXRA},{ChestXRSD},{RightLegCalfStability},{RightLegCalfStabilitySD},{LeftLegCalfStability},{LeftLegCalfStabilityYSD},{time},{stepcount},{score}";
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
