using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackAMole_PositionOccupied : MonoBehaviour
{
    public static bool[] PositionOccupied;
    public  GameObject[] Block;

    private void Start()
    {
        // 初始化 PositionOccupied 
       PositionOccupied = new bool[Block.Length];
    }
    private void Update()
    {   
      //  Debug.Log(string.Join(", ", PositionOccupied));
    }

    /*private void OnTriggerEnter(Collider other)
    {
       
            for (int i = 0; i < Block.Length; i++)
            {
                if (other.gameObject.transform != Block[i].transform)
                {
                    PositionOccupied[i] = false;
                    // 如果找到匹配的 Block，就退出循环 
                }
            }
        
    }*/
   
    
}