using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.volume = 0.21f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            audioSource.mute = !audioSource.mute; //If true make false, if false make truett
        }
    }
}
