using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTower_PrefabMove : MonoBehaviour
{
    public static float MoveSpeed = 0f;
    void Update()
    {
        transform.Translate(0, -MoveSpeed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
