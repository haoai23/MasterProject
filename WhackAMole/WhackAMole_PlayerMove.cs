using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using static Unity.Collections.AllocatorManager;
using System.Linq;

public class WhackAMole_PlayerMove : MonoBehaviour
{
    public GameObject Tracker1, Tracker2, RightHand, LeftHand;
    GameObject RightHandT, LeftHandT;
    public GameObject DeductBlood;
    public static bool AtOriginalPoint = false;
    bool isReady;
    //public GameObject[] Block;
    public static int i = 0;
    float XProportion = 0;
    float YProportion = 0;
    //public static float DestroyPrefabTime;
    public GameObject WhackAMolePrefab;
    public static bool WhackAMole_StartTimer = false;
    public GameObject GameOver_Panel, ReactionTime_Image, Time_Image, OrignalPoint, MainCamera;

    public static int WhackAMoleSceneTimes = 0;//進入次數
    public GameObject RestartButton;

    List<float> RightHandPX = new List<float>();
    List<float> RightHandPY = new List<float>();
    List<float> RightHandPZ = new List<float>();
    List<float> RightHandRX = new List<float>();
    List<float> RightHandRY = new List<float>();
    List<float> RightHandRZ = new List<float>();

    List<float> LeftHandPX = new List<float>();
    List<float> LeftHandPY = new List<float>();
    List<float> LeftHandPZ = new List<float>();
    List<float> LeftHandRX = new List<float>();
    List<float> LeftHandRY = new List<float>();
    List<float> LeftHandRZ = new List<float>();

    List<float> PunchTime = new List<float>();
    List<float> ClosingTime = new List<float>();
    float AveragePunchTime, AverageClosing;
    
    private void Start()
    {
        WhackAMoleSceneTimes++;
        Debug.Log("WhackAMoleSceneTimes: " + WhackAMoleSceneTimes);
        if (WhackAMoleSceneTimes > 1)
        {
            WhackAMoleSceneTimes = 0;
            WhackAMole_GameController gameController = RestartButton.GetComponent<WhackAMole_GameController>();
            gameController.RestatGame();

        }
    }
    // Update is called once per frame 
    void Update()
    {
        DefineTracker(Tracker1, Tracker2);
        WhackAMolePlayerMove();
        if (isReady & WhackAMole_Timer.WhackAMoleTimer_i != 0)
        {
            RecordPlayerPosition(RightHandT, LeftHandT);
        }
        if (isReady & HAttitudeCorrection & VAttitudeCorrection & WhackAMole_Timer.WhackAMoleTimer_i != 0 )//紀錄數據
        {

            RightHandPX.Add(RightHandT.transform.position.x);
            RightHandPY.Add(RightHandT.transform.position.y);
            RightHandPZ.Add(RightHandT.transform.position.z);
            RightHandRX.Add(RightHandT.transform.eulerAngles.x);
            RightHandRY.Add(RightHandT.transform.eulerAngles.y);
            RightHandRZ.Add(RightHandT.transform.eulerAngles.z);

            LeftHandPX.Add(LeftHandT.transform.position.x);
            LeftHandPY.Add(LeftHandT.transform.position.y);
            LeftHandPZ.Add(LeftHandT.transform.position.z);
            LeftHandRX.Add(LeftHandT.transform.eulerAngles.x);
            LeftHandRY.Add(LeftHandT.transform.eulerAngles.y);
            LeftHandRZ.Add(LeftHandT.transform.eulerAngles.z);

            Debug.Log("WhackAMole遊戲讀取");
        }
        if (WhackAMole_Timer.WhackAMoleTimer_i == 0)
        {
            WhackAMoleSaveCSV();
            Debug.Log("WhackAMole遊戲存檔");
        }
    } 
       
    void DefineTracker(GameObject Tracker1, GameObject Tracker2)
        {
        /*if (Tracker1.transform.position.x < Tracker2.transform.position.x)
        {
            RightHandT = Tracker1;
            LeftHandT = Tracker2;
        }
        else
        {
            RightHandT = Tracker2;
            LeftHandT = Tracker1;
        }*/
        RightHandT = GameObject.FindWithTag("RightHand");
        LeftHandT = GameObject.FindWithTag("LeftHand");
        if (RightHandT & LeftHandT != null)
        {
            isReady = true;
            Debug.Log("isReady: " + isReady);
        }
    }
    void WhackAMolePlayerMove()
    {
        RightHand.transform.position = new Vector3(-RightHandT.transform.position.x * XProportion, RightHandT.transform.position.y * YProportion, RightHandT.transform.position.z);
        LeftHand.transform.position = new Vector3(-LeftHandT.transform.position.x * XProportion, LeftHandT.transform.position.y * YProportion, LeftHandT.transform.position.z);
        
    }
    float PunchSpeed = 0, punchspeed1 = 0, punchspeed0 = 0 ;
    float RPunchTime  , RClosingTime;

    public static float LeaveAPTime= 0;//離開原始點的時間
    void OnCollisionEnter(Collision collision)        
    {
        
        if (collision.gameObject.tag == "WhackAMole_Mole")
        {
            Destroy(collision.gameObject); 
            WhackAMole_Score.Score = WhackAMole_Score.Score + 10;
            AtOriginalPoint = false;
            if (AtOriginalPoint == false)//紀錄回來時間
            {
                punchspeed0 = Time.time;
            }
            RPunchTime = punchspeed0 - LeaveAPTime;//出拳速度
            if(RPunchTime > 0) {  PunchTime.Add(RPunchTime);}
           
            //DestroyPrefabTime = Time.time;
            Debug.Log(" PunchTime: " + RPunchTime);
            Debug.Log(" PunchTime數列長度: " + PunchTime.Count);

        }
        else if (collision.gameObject.tag == "WhackAMole_Flower")
        {
            DeductBlood.SetActive(true);
            StartCoroutine(DeactivateDeductBlood(0.3f));
            //WhackAMole_Score.Score = WhackAMole_Score.Score - 1;
        }
        else if (collision.gameObject.name == "OriginalPoint")
        {
            if (RightHand.transform.position.x - LeftHand.transform.position.x <6)
            {
                AtOriginalPoint = true;
                if (AtOriginalPoint == true)
                {
                    punchspeed1 = Time.time;
                }
                RClosingTime = punchspeed1 - punchspeed0;//收拳速度
                if(RClosingTime >0) { ClosingTime.Add(RClosingTime);}
                
                //punchspeed1 = punchspeed0;
                Debug.Log("ClosingTime" + RClosingTime);
                Debug.Log("ClosingTime數列長度" + ClosingTime.Count);
            }
        }
        IEnumerator DeactivateDeductBlood(float delay)
        {
            yield return new WaitForSeconds(delay);
            DeductBlood.SetActive(false);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "OriginalPoint")
        {
            LeaveAPTime = Time.time;
            Debug.Log("LeaveAPTime" + LeaveAPTime);
        }
    }
    bool HAttitudeCorrection = false;//水平校正
    bool VAttitudeCorrection = false;//垂直校正

    void RecordPlayerPosition(GameObject RightHand, GameObject LeftHand)
    {

        if (Input.GetKeyDown("s"))
        {
            float RecordXPosition = Mathf.Abs(RightHand.transform.position.x - LeftHand.transform.position.x);
            XProportion = 23 / RecordXPosition;
            HAttitudeCorrection = true;
            Debug.Log(XProportion);
        }
        if (Input.GetKeyDown("a"))
        {
            float RecordYPosition = Mathf.Abs(RightHand.transform.position.y - LeftHand.transform.position.y);
            YProportion = 11 / RecordYPosition;
            VAttitudeCorrection = true;
            Debug.Log(YProportion);
            //i = 0;
        }
        if (XProportion > 10 && YProportion > 5 && WhackAMole_Timer.WhackAMoleTimer_i > 0)
        {
            WhackAMole_GameController.WhackAMoleIsGameStarted = true;
            WhackAMole_StartTimer = true;
            WhackAMolePrefab.SetActive(true);
            GameOver_Panel.SetActive(false);
            ReactionTime_Image.SetActive(true) ;
            Time_Image.SetActive(true) ; 
            OrignalPoint.SetActive(true);
            MainCamera.SetActive(true);

        }
    }
    public void WhackAMoleSaveCSV()
    {
        string fileName = "WhackAMole.csv";
        string timePath = Path.Combine(PlayerPrefs.GetString("timePath"), fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("RightHandPX,RightHandPY,RightHandPZ,RightHandRX,RightHandRY,RightHandRZ,LeftHandPX,LeftHandPY,LeftHandPZ,LeftHandRX,LeftHandRY,LeftHandRZ,PunchTime,ClosingTime,RecordMolePosition,ReactionTime,ActuallyFirstQuadraScore,ActuallySecondQuadranScore,ActuallyThirdQuadranScore,ActuallyFourthQuadranScore");

        // 確定最大長度
        int maxLength = new int[] { RightHandPX.Count, RightHandPY.Count, RightHandPZ.Count, RightHandRX.Count, RightHandRY.Count, RightHandRZ.Count, 
            LeftHandPX.Count, LeftHandPY.Count, LeftHandPZ.Count, LeftHandRX.Count, LeftHandRY.Count, LeftHandRZ.Count,PunchTime.Count,ClosingTime.Count,
            WhackAMole_SpawnPrefab.RecordMolePosition.Count,WhackAMole_SpawnPrefab.AverageReactionTime.Count}.Max();


        // 根據最大長度遍歷
        for (int i = 0; i < maxLength; i++)
        {
            string line = $"{GetValueOrDefault(RightHandPX, i)},{GetValueOrDefault(RightHandPY, i)},{GetValueOrDefault(RightHandPZ, i)},{GetValueOrDefault(RightHandRX, i)},{GetValueOrDefault(RightHandRY, i)},{GetValueOrDefault(RightHandRZ, i)}," +
                          $"{GetValueOrDefault(LeftHandPX, i)},{GetValueOrDefault(LeftHandPY, i)},{GetValueOrDefault(LeftHandPZ, i)},{GetValueOrDefault(LeftHandRX, i)},{GetValueOrDefault(LeftHandRY, i)},{GetValueOrDefault(LeftHandRZ, i)},"+
                          $"{GetValueOrDefault(PunchTime, i)},{GetValueOrDefault(ClosingTime, i)},{GetValueOrDefault(WhackAMole_SpawnPrefab.RecordMolePosition, i)},{GetValueOrDefault(WhackAMole_SpawnPrefab.AverageReactionTime, i)}";


            // 在每一行的末尾添加統計數據
            if (i == 0) // 假設統計數據只需添加一次
            {
                line += $",{WhackAMole_SpawnPrefab.ActuallyFirstQuadraScore},{WhackAMole_SpawnPrefab.ActuallySecondQuadranScore},{WhackAMole_SpawnPrefab.ActuallyThirdQuadranScore},{WhackAMole_SpawnPrefab.ActuallyFourthQuadranScore}" ;
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
