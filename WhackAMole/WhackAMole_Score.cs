using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhackAMole_Score : MonoBehaviour
{
    public GameObject[] ScoreNumberImage;
    public GameObject[] SpawnScorePosition;
    public static int Score;
    public static int thousands_digit_1 = 0, hundreds_digit_1 = 0,tens_digit_1 = 0, units_digit_1 = 0;
    public Text Score_Text;
    // Update is called once per frame
    void Update()
    {
        Caculate_Score();
        //Debug.Log(Score);
    }
    void Caculate_Score()
    {
        int thousands_digit = Score / 1000;
        int hundreds_digit = (Score / 100) % 10;
        int tens_digit = (Score / 10) % 10;
        int units_digit = Score % 10;
        Score_Text.text = Score.ToString();

        if (thousands_digit_1 != thousands_digit)
        {
            thousands_digit_1 = thousands_digit;
            Instantiate(ScoreNumberImage[thousands_digit],
            SpawnScorePosition[0].transform.position,SpawnScorePosition[0].transform.rotation);
            
        }
        else if (hundreds_digit_1 != hundreds_digit)
        {
            hundreds_digit_1 = hundreds_digit;
            Instantiate(ScoreNumberImage[hundreds_digit],
            SpawnScorePosition[1].transform.position,SpawnScorePosition[1].transform.rotation);

        }
        else if (tens_digit_1 != tens_digit)
        {
            tens_digit_1 = tens_digit;
            Instantiate(ScoreNumberImage[tens_digit],
            SpawnScorePosition[2].transform.position, SpawnScorePosition[2].transform.rotation);

        }
        else if (units_digit_1 !=  units_digit)
        {
            units_digit_1 = units_digit;
            Instantiate(ScoreNumberImage[units_digit],
            SpawnScorePosition[3].transform.position, SpawnScorePosition[3].transform.rotation);

        }
        else 
        {
            Instantiate(ScoreNumberImage[thousands_digit],SpawnScorePosition[0].transform.position, SpawnScorePosition[0].transform.rotation);
            Instantiate(ScoreNumberImage[hundreds_digit],SpawnScorePosition[1].transform.position, SpawnScorePosition[1].transform.rotation);
            Instantiate(ScoreNumberImage[tens_digit],SpawnScorePosition[2].transform.position, SpawnScorePosition[2].transform.rotation);
            Instantiate(ScoreNumberImage[units_digit],SpawnScorePosition[3].transform.position, SpawnScorePosition[3].transform.rotation);
        }

    }
   
}
