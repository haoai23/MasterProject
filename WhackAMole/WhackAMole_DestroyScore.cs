using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackAMole_DestroyScore : MonoBehaviour
{
    Rigidbody WhackAMoleGameObjectScore;
    
    private void Update()
    {
        WhackAMoleGameObjectScore = this.GetComponent<Rigidbody>();
        WhackAMoleGameObjectScore.constraints = RigidbodyConstraints.FreezeAll;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WhackAMole_Number")//之後可以加上以免新增物件後被數字消除了
        { 
            Destroy(collision.gameObject);
        }
        
    }
}
