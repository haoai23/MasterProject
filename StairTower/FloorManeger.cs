using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManeger : MonoBehaviour
{
    [SerializeField] GameObject[] Prefabs;
    public GameObject _SpawnFloor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StairTower_NormalFloor")
        {
            Spawnfloor();
            SpawnDiamond();
        }

    }
    public void Spawnfloor()
    { 
        GameObject Floor = Instantiate(Prefabs[0],_SpawnFloor.transform);//把物件生成在FloorManeger物件底下      
        Floor.transform.localScale = new Vector3(7.9f, 1.2f, 1);
        Floor.transform.position = new Vector3(0, 11f, 0f);
        
    }
    public void SpawnDiamond() 
    {
        GameObject Diamond = Instantiate(Prefabs[1], _SpawnFloor.transform);
        Diamond.transform.position = new Vector3(Random.Range(-6, 6), 11.5f, 0f);
    }
    

}
 