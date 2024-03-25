using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WhackAMole_SpawnPrefab : MonoBehaviour
{
    public GameObject[] SpawnPrefab;
    public GameObject[] SpawnPosition;
    bool[] IsOccupied;
    GameObject[] SpawnObject;
    GameObject Mole;
    float Timer_i = 0;
    float LastSpawnTime;
    public Text ShowReactionTime;
    public List<float> AverageReactionTime;
    public Text AverageReactionTime_Text;
    public Text GameReviews_Text;

    // Start is called before the first frame update
    void Start()
    {
        IsOccupied = new bool[SpawnPosition.Length];
        SpawnObject = new GameObject[SpawnPosition.Length];
    }

    // Update is called once per frame
    void Update()
    {
        Spawnprefab();
        //Debug.Log(string.Join(", ", IsOccupied));
        Timer_i += Time.deltaTime;
        if (Timer_i > 1)
        {
            StartCoroutine(SpawnWhackAMolePrefabs1s());
        }

    }
    void Spawnprefab()
    {
        int PositionIndex = Random.Range(0, SpawnPosition.Length);
        bool isoccupied = PositionIsOcupied(PositionIndex);

        if (!isoccupied & Mole == null && WhackAMole_PlayerMove.AtOriginalPoint)//在這邊新增位置的起始點
        {
            if (PositionIndex != 1 && PositionIndex != 2 && PositionIndex != 5 && PositionIndex != 6)
            {
                Mole = Instantiate(SpawnPrefab[1], SpawnPosition[PositionIndex].transform.position, SpawnPosition[PositionIndex].transform.rotation);
                SpawnObject[PositionIndex] = Mole;
                float spawntime = Time.time;
                WhackAMole_ReactionTime(spawntime, PositionIndex);
            }
            else
            {
                Mole = Instantiate(SpawnPrefab[2], SpawnPosition[PositionIndex].transform.position, SpawnPosition[PositionIndex].transform.rotation);

                SpawnObject[PositionIndex] = Mole;
                float spawntime = Time.time;
                WhackAMole_ReactionTime(spawntime, PositionIndex);
            }
        }

    }

    IEnumerator SpawnWhackAMolePrefabs1s()
    {
        int PositionIndex = Random.Range(0, SpawnPosition.Length);
        bool isoccupied = PositionIsOcupied(PositionIndex);

        if (!isoccupied && WhackAMole_PlayerMove.AtOriginalPoint)
        {
            if (PositionIndex != 1 && PositionIndex != 2 && PositionIndex != 5 && PositionIndex != 6)
            {
                GameObject Flower = Instantiate(SpawnPrefab[0], SpawnPosition[PositionIndex].transform.position, SpawnPosition[PositionIndex].transform.rotation);
                SpawnObject[PositionIndex] = Flower;
            }
            else
            {
                GameObject Flower = Instantiate(SpawnPrefab[3], SpawnPosition[PositionIndex].transform.position, SpawnPosition[PositionIndex].transform.rotation);
                SpawnObject[PositionIndex] = Flower;
            }
        }
        Timer_i = 0;
        yield return new WaitForSeconds(1);
    }
    bool PositionIsOcupied(int PositionIndex)//檢查位置有沒有被占用
    {
        if (PositionIndex >= 0 && PositionIndex < SpawnObject.Length)
        {
            if (SpawnObject[PositionIndex] != null)
            {
                IsOccupied[PositionIndex] = true;

                return true;
            }
        }
        return false;

    }



    void WhackAMole_ReactionTime(float spawnTime, int PositionIndex)
    {

        if (LastSpawnTime > 0)
        {
            float ReactionTime = spawnTime - LastSpawnTime;
            //Debug.Log("Time Difference: " + ReactionTime);
            QuadrantScore(ReactionTime, PositionIndex);//各象限的分數
            AverageReactionTime.Add(ReactionTime);
            float averageReactionTime = AverageReactionTime.Average();
            ShowReactionTime.text = ReactionTime.ToString();
            AverageReactionTime_Text.text = averageReactionTime.ToString();
            Debug.Log("averageReactionTime: " + averageReactionTime);
        }

        LastSpawnTime = spawnTime;
    }

    public static int TotalFirstQuadranScore = 0;//理論上要獲得的分數
    public static int TotalSecondQuadranScore = 0;
    public static int TotalThirdQuadranScore = 0;
    public static int TotalFourthQuadranScore = 0;
    public static int FirstQuadranScore = 0;//實際上的分數
    public static int SecondQuadranScore = 0;
    public static int ThirdQuadranScore = 0;
    public static int FourthQuadranScore = 0;
    public static float ActuallyFirstQuadraScore = 0;
    public static float ActuallySecondQuadranScore = 0;
    public static float ActuallyThirdQuadranScore = 0;
    public static float ActuallyFourthQuadranScore = 0;

    public Text ActuallyFirstQuadraScore_Text;
    public Text ActuallySecondQuadraScore_Text;
    public Text ActuallyThirdQuadranScore_Text;
    public Text ActuallyFourthQuadranScore_Text;
    void QuadrantScore(float ReactionTime, int PositionIndex)//出現的次數要除上打到的次數
    {
        if (PositionIndex < 2)
        {
            TotalFirstQuadranScore += 1;

            if (ReactionTime < 3f)
            {
                FirstQuadranScore += 1;//實際上獲得的分數                               
            }
            ActuallyFirstQuadraScore = (float)FirstQuadranScore / TotalFirstQuadranScore;
            Debug.Log("Time Difference: " + ReactionTime);

            //Debug.Log("ActuallyFirstQuadranScore: " + ActuallyFirstQuadraScore);
        }
        else if (PositionIndex >= 2 && PositionIndex < 4)
        {
            TotalSecondQuadranScore += 1;
            if (ReactionTime < 3f)
            {
                SecondQuadranScore += 1;//實際上獲得的分數                               
            }
            ActuallySecondQuadranScore = (float)SecondQuadranScore / TotalSecondQuadranScore;

        }
        else if (PositionIndex >= 4 && PositionIndex < 6)
        {
            TotalThirdQuadranScore += 1;
            if (ReactionTime < 3f)
            {
                ThirdQuadranScore += 1;//實際上獲得的分數                               
            }
            ActuallyThirdQuadranScore = (float)ThirdQuadranScore / TotalThirdQuadranScore;
        }
        else if (PositionIndex >= 6 && PositionIndex < 8)
        {
            TotalFourthQuadranScore += 1;
            if (ReactionTime < 3f)
            {
                FourthQuadranScore += 1;//實際上獲得的分數                               
            }
            ActuallyFourthQuadranScore = (float)FourthQuadranScore / TotalFourthQuadranScore;
        }
        Debug.Log("ActuallyFirstQuadranScore: " + ActuallyFirstQuadraScore);
        Debug.Log("ActuallySecondQuadranScore: " + ActuallySecondQuadranScore);
        Debug.Log("ActuallyThirdQuadranScore: " + ActuallyThirdQuadranScore);
        Debug.Log("ActuallyForthQuadranScore: " + ActuallyFourthQuadranScore);
        WhackAMole_GameReviews();
    }
    void WhackAMole_GameReviews()
    {
        ActuallyFirstQuadraScore_Text.text = ActuallyFirstQuadraScore.ToString();
        ActuallySecondQuadraScore_Text.text = ActuallySecondQuadranScore.ToString();
        ActuallyThirdQuadranScore_Text.text = ActuallyThirdQuadranScore.ToString();
        ActuallyFourthQuadranScore_Text.text = ActuallyFourthQuadranScore.ToString();


    }
}
