using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhackAMole_GameController : MonoBehaviour
{
    public static bool WhackAMoleIsGameStarted = false;
    public GameObject  StartButton;
    public GameObject MainCamera, ReactionTime_Image, Time_Image, GameOver_Panel, WhackAMolePrefabs, Score, OriginalPoint;

    private void Update()
    {
        
    }
    // Start is called before the first frame update
    public void StartGame()
    {
        WhackAMole_PlayerMove.WhackAMole_StartTimer = true;
        WhackAMolePrefabs.SetActive(true);
        GameOver_Panel.SetActive(false);
        ReactionTime_Image.SetActive(true);
        Time_Image.SetActive(true);
        OriginalPoint.SetActive(true);
        MainCamera.SetActive(true);
    }
    public void RestatGame()
    {
        SceneManager.LoadScene("WhackAMole");
        WhackAMoleIsGameStarted = false;
        WhackAMole_PlayerMove.WhackAMole_StartTimer = false;
        WhackAMole_SpawnPrefab.AverageReactionTime.Clear();
        WhackAMole_SpawnPrefab.ActuallyFirstQuadraScore = 0;
        WhackAMole_SpawnPrefab.ActuallySecondQuadranScore = 0;
        WhackAMole_SpawnPrefab.ActuallyThirdQuadranScore = 0;
        WhackAMole_SpawnPrefab.ActuallyFourthQuadranScore= 0;
        WhackAMole_Timer.WhackAMoleTimer_i = 90;
        WhackAMolePrefabs.SetActive(true);
        GameOver_Panel.SetActive(false);
        ReactionTime_Image.SetActive(true);
        Time_Image.SetActive(true);
        OriginalPoint.SetActive(true);


    }
}
