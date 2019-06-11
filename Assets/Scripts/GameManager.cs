using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
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

    public Transform[] spawnPoints;
    public int enemiesLeftInRound;
    public GameObject playerCamera;
    [SerializeField] private Transform playerStartLocation;
    [SerializeField] private GameObject player;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCamera = player.transform.Find("Droid").transform.Find("Head").transform.Find("PlayerCamera").gameObject;

        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.volume = 0.21f;
        audioSource.Play();

        Instantiate(player, playerStartLocation.position, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            audioSource.mute = !audioSource.mute; //If true make false, if false make truett
        }
    }
}
