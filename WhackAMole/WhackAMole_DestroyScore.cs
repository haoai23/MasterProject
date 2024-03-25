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
        if (collision.gameObject.tag == "WhackAMole_Number")//����i�H�[�W�H�K�s�W�����Q�Ʀr�����F
        { 
            Destroy(collision.gameObject);
        }
        
    }
}
