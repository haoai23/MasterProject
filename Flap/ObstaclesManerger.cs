using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManerger : MonoBehaviour
{
    [SerializeField] GameObject[] ObstaclesPrefabs;
    GameObject Obj;
    private Rigidbody ObjRB;

    public void SpawnObstacles()
    {
        int ObstaclesType = Random.Range(0, ObstaclesPrefabs.Length);
        GameObject Obj = Instantiate(ObstaclesPrefabs[ObstaclesType], transform);//把物件生成在ObstaclesManeger物件底下
        Obj.transform.position = new Vector3(-20f, Random.Range(3,8), 0f);
        Obj.transform.Rotate(0, 0, 90);
        ObjRB = Obj.GetComponent<Rigidbody>();
        ObjRB.constraints = RigidbodyConstraints.FreezePositionY;
        ObjRB.constraints = RigidbodyConstraints.FreezePositionZ;
        ObjRB.constraints = RigidbodyConstraints.FreezeRotationX;
        ObjRB.constraints = RigidbodyConstraints.FreezeRotationY;
        ObjRB.constraints = RigidbodyConstraints.FreezeRotationZ;       
    }
}

