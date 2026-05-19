using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    [HideInInspector] public static soundManager Instance;
    [Header("AudioSource")]
    public AudioSource BGMSource;
    public AudioSource shortSource;

    [Header("AudioClip")]
    public AudioClip[] bgmClips;
    private AudioClip currentBGM;
    public AudioClip[] shortClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playBGM(int index)
    {
        if(currentBGM != bgmClips[index])
        {
            BGMSource.clip = bgmClips[index];
            BGMSource.Play();
            currentBGM = bgmClips[index];
        }
        
    }
    public void playshotMuisc(int index) 
    { 
        shortSource.PlayOneShot(shortClips[index]);
    }
}
