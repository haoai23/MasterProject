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
    public static Text UserID, UserWeight, UserBithday_Date;
    public static string userID, userWeight, userBithday_Date;

    private void Update()
    {
        if (UserID != null && UserWeight != null !&& UserBithday_Date != null)
        {
            Debug.Log("UserID: " + UserID.text);
            Debug.Log("UserWeight: " + UserWeight.text);
            Debug.Log("UserWeight: " + UserBithday_Date.text);
            
        }
        if (Input.GetKeyDown("space"))//可以加個防呆以免按錯
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
    }
    
    public static string UserInfomationCeateFile(Text userID, Text userBithday_Date)
    {
        DateTime localDate = DateTime.Now;
        string fileName = userID.text + "_" + userBithday_Date.text;
        string filePath = Path.Combine(Application.dataPath, fileName);
        string timePath = Path.Combine(filePath, localDate.ToString("yyyyMMdd-HH-mm-ss"));

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
        return timePath;
    }
    void dataTable(string filePath)
    {
        DataTable dt = new DataTable("Sheet1");
        dt.Columns.Add("體重");

        DataRow dr = dt.NewRow();
        dr["體重"] = UserWeight.text;
        dt.Rows.Add(dr);
        filePath = filePath + "\\test.csv";
        saveCsv(filePath, dt);

    }
    public void saveCsv(string filePath, DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create,FileAccess.Write))
        {
            using (TextWriter textWrite = new StreamWriter(fileStream))
            {
                textWrite.Write(stringBuilder.ToString());
            }
        }
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

    public void Gmaelist()
    {
        _UserInterface.SetActive(false);
        _GameList.SetActive(true);
    }
}
