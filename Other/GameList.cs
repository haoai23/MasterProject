using OpenCvSharp.Tracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameList : MonoBehaviour
{
    public GameObject _UserInterface, _GameList, DefindTracker;
    public Text UserID, UserWeight, UserBithday_Date;
    public static string userID, userWeight, userBithday_Date;
    private void Update()
    {
        /*if ()
        {
            Debug.Log("UserID: " + UserID.text);
            Debug.Log("UserWeight: " + UserWeight.text);
            Debug.Log("UserBithday_Date: " + UserBithday_Date.text);
            
        }*/
        if (Input.GetKeyDown("space") /*& UserID != null && UserWeight != null && UserBithday_Date != null*/)//可以加個防呆以免按錯
        {
            _UserInterface.SetActive(false);
            _GameList.SetActive(true);
            DefindTracker.SetActive(false);
        }
    }
    public void UserInterface()
    {
        _UserInterface.SetActive(false);
        _GameList.SetActive(false);
        DefindTracker.SetActive(true);
        if (UserID != null && UserWeight != null && UserBithday_Date != null)
        {
            userID = UserID.text;
            userWeight = UserWeight.text;
            userBithday_Date = UserBithday_Date.text;
            UserInfomationCeateFile(UserID, UserBithday_Date);
            
        }
        if(UserID != null && UserWeight != null && PlayerPrefs.GetString(timePath)!= timePath)
        {
            TrackerManeger.Tracker.Clear();
            TrackerManeger.tagIndex = 0;
        }
    }
    public static string filePath, timePath;
    void  UserInfomationCeateFile(Text userID, Text userBithday_Date)
    {
        DateTime localDate = DateTime.Now;
        string fileName = userID.text + "_" + userBithday_Date.text;
        filePath = Path.Combine(Application.dataPath, fileName);
        timePath = Path.Combine(filePath, localDate.ToString("yyyyMMdd-HH-mm-ss"));

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            Debug.Log("已創建母資料夾: " + filePath);
        }

        if (!Directory.Exists(timePath))
        {
            Directory.CreateDirectory(timePath);
            Debug.Log("已創建子資料夾: " + timePath);           
        }
        else
        {
            Debug.Log("子資料夾已存在: " + timePath);
            //dataTable(timePath);
        }    
        PlayerPrefs.SetString("timePath", timePath);
        PlayerPrefs.SetString("filename", fileName);
    }
    public void Game_Bowing()
    {
        SceneManager.LoadScene("Bowing");
    } 
    public void Game_Flap()
    {
        SceneManager.LoadScene("Flap");
    } 
    public void Game_Step_Training()
    {
        SceneManager.LoadScene("Step training");
    } 
    public void Game_WhackAMole()
    {
        SceneManager.LoadScene("WhackAMole");
    } 
    public void Game_StairTower()
    {
        SceneManager.LoadScene("UpTheStair_StairTower");
    }
    public void User_Interface()
    {
        SceneManager.LoadScene("UserInterface");
    }
    public static bool OIP = false;//OptionIsPress
    public void Gamelist()
    {       
        OIP = true;
        _UserInterface.SetActive(false);
        _GameList.SetActive(true);
        
    }
}
