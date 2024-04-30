using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesManerger : MonoBehaviour
{
    [SerializeField] GameObject[] ObstaclesPrefabs;
    GameObject Obj;
    private Rigidbody ObjRB;
    public static float Range;
    List<float> WallHeight = new List<float>() { 3,4,5};
    public void SpawnObstacles()
    {
        int ObstaclesType = Random.Range(0, ObstaclesPrefabs.Length);
        // Range = Random.Range(4, 9);//隨機產生牆的高度
        float SDH = Flap_PlayerMove.RecordYPotation / 7;//根據使用者腹部到地板之間的高度除了最矮高度換算成牆的高度 SDF=標準高度
        Range = WallHeight.First()+SDH;//提取第一個元素
        Debug.Log("WallHeight[0]" + WallHeight[0]);
        if (WallHeight.Count > 1)//當牆是最後一個數字時就回到最一開始所定義的數列
        {
            WallHeight.RemoveAt(0);
        }
        else
        {
            WallHeight = new List<float>() { 3, 4, 5 };
        }
        GameObject Obj = Instantiate(ObstaclesPrefabs[ObstaclesType], transform);//把物件生成在ObstaclesManeger物件底下

        /*while (RandomValue.Contains(Range)) 
        { 
            Range = Random.Range(4, 9); 

        }*/
        Debug.Log("間隔大小: " + Range);
       /* RandomValue.Add(Range);
        RandomValue2.Add(Range);
        if (RandomValue.Count >= 5)
        {
            RandomValue.Clear();
        }*/
        Obj.transform.position = new Vector3(-20f, Range, 0f);
        Obj.transform.Rotate(0, 0, 90);
        ObjRB = Obj.GetComponent<Rigidbody>();
        ObjRB.constraints = RigidbodyConstraints.FreezePositionY;
        ObjRB.constraints = RigidbodyConstraints.FreezePositionZ;
        ObjRB.constraints = RigidbodyConstraints.FreezeRotationX;
        ObjRB.constraints = RigidbodyConstraints.FreezeRotationY;
        ObjRB.constraints = RigidbodyConstraints.FreezeRotationZ;       
    }
    List<int> RandomValue = new List<int>();//紀錄產生過的數字
    public static List<float> RandomValue2 = new List<float>();//紀錄產生過的數字，用於跟分數做比較
    

}

