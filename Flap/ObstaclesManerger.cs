using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManerger : MonoBehaviour
{
    [SerializeField] GameObject[] ObstaclesPrefabs;
    GameObject Obj;
    private Rigidbody ObjRB;
    public static int Range;
    public void SpawnObstacles()
    {
        int ObstaclesType = Random.Range(0, ObstaclesPrefabs.Length);
        Range = Random.Range(4, 9);

        GameObject Obj = Instantiate(ObstaclesPrefabs[ObstaclesType], transform);//把物件生成在ObstaclesManeger物件底下

        while (RandomValue.Contains(Range)) 
        { 
            Range = Random.Range(4, 9); 

        }
        Debug.Log("間隔大小: " + Range);
        RandomValue.Add(Range);
        RandomValue2.Add(Range);
        if (RandomValue.Count >= 5)
        {
            RandomValue.Clear();
        }
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

