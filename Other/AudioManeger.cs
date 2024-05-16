using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    private void Start()
    {
        GameObject[] Objs = GameObject.FindGameObjectsWithTag("AudioManeger");
        if (Objs.Length > 1)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
        // Update is called once per frame
    void Update()
    {
        
    }

}
