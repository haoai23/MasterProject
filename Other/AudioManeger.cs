using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class AudioManager : MonoBehaviour
{
    [Header("背景音樂")]
    public AudioClip GameStartBGM;
    public AudioClip WhackAMoleBGM;
    public AudioClip BowingBGM;
    public AudioClip FlapBGM;
    public AudioClip StepTrainingBGM;
    public AudioClip StairTowerBGM;
    public AudioClip GameOverBGM;

    [Header("物件音效")]
    public AudioClip Jump;
    public AudioClip Coin;
    public AudioClip Click;

    private static AudioSource BGMSource;
    private static AudioSource MusicSource;

    private static AudioManager instance;
    private string currentSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            BGMSource = gameObject.AddComponent<AudioSource>();
            MusicSource = gameObject.AddComponent<AudioSource>();

            BGMSource.loop = true;
            MusicSource.loop = false;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        PlayWhichBGM(currentSceneName);
    }

    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != sceneName)
        {
            Debug.Log("sceneName: " + sceneName);
            PlayWhichBGM(sceneName);
            currentSceneName = sceneName;
            
        }
    }

    public static void PlayWhichBGM(string sceneName)
    {
        AudioClip bgmClip = instance.GameStartBGM;

        switch (sceneName)
        {
            case "WhackAMole":
                bgmClip = instance.WhackAMoleBGM;
                break;
            case "Bowing":
                bgmClip = instance.BowingBGM;
                break;
            case "Flap":
                bgmClip = instance.FlapBGM;
                break;
            case "Step training":
                bgmClip = instance.StepTrainingBGM;
                break;  
            case "UpTheStair_StairTower":
                bgmClip = instance.StairTowerBGM;
                break;
            case "GameOver":
                bgmClip = instance.GameOverBGM;
                break;
        }
        Debug.Log("bgmClip: " + bgmClip);

        if (BGMSource.clip != bgmClip)
        {
            BGMSource.clip = bgmClip;
            BGMSource.Play();
        }
    }

    public static void MusicPlayOneShot(string name)
    {
        AudioClip clipToPlay;
        switch (name)
        {
            case "Jump":
                clipToPlay= instance.Jump;
                break;
            case "Coin":
                clipToPlay = instance.Coin;
                break;
            case "Click":
                clipToPlay = instance.Click;
                break;

            default:
                Debug.LogError("Unsupported audio name: " + name);
                return;
        }

        MusicSource.PlayOneShot(clipToPlay);
    }
}
