using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioManeger : MonoBehaviour
{
    public AudioClip FlapBGM;
    public AudioClip StairTowerBGM;
    public AudioClip WhackAMolwBGM;
    public AudioClip BowingBGM;
    public AudioClip StepTrainingBGM;

    List< AudioSource> audios = new List<AudioSource>();
    private void Awake()
    {
        GameObject[] Objs = GameObject.FindGameObjectsWithTag("AudioManeger");
        if (Objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        Debug.Log(null);
        DontDestroyOnLoad(this.gameObject);
        
        for(int i = 0; i<8; i++) 
        { 
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }
    private void Update()
    {
    }
    public void PlayAudio(int index,string name, bool isloop)
    {
        var clip = GetAudio(name);
        if(clip != null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isloop;
            audio.Play();
        }

    }
    private AudioClip GetAudio(string name)
    {
        switch (name) 
        {
            case "FlapBGM":
                return FlapBGM;
            case "StairTowerBGM":
                return FlapBGM;
            case "WhackAMoleBGM":
                return FlapBGM;
            case "BowingBGM":
                return FlapBGM;
            case "StepTrainingBGM":
                return FlapBGM;
        }
        return null;

    }
}
