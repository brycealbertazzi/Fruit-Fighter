﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameStates {
        PreGame,
        GameOn,
        GameOver
    }

    public GameStates state;

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

        //Sets this to not be destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }

    public Transform[] spawnPoints;
    public int maxEnemiesAllowedOnMap;
    public int totalEnemiesOnMap = 0;
    public GameObject playerCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;
    private AudioSource audioSource;
    private Text score, time;
    private int playerScore;
    [SerializeField] private GameObject GameCanvas, GameOverCanvas, PregameCanvas;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCamera = player.transform.Find("Droid").transform.Find("Head").transform.Find("PlayerCamera").gameObject;
        score = GameCanvas.transform.Find("Score").GetComponent<Text>();
        time = GameCanvas.transform.Find("Time").GetComponent<Text>();

        PreGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            audioSource.mute = !audioSource.mute; //If true make false, if false make truett
        }
    }

    private int pointsPerKill = 10;
    public void UpdateScore() {
        playerScore += pointsPerKill;
        score.text = playerScore.ToString();
    }

    private int seconds, minutes, hours = 0;
    void UpdateTime() {
        if (minutes >= 59)
        {
            minutes = seconds = 0;
            hours++;
        }
        else
        {
            if (seconds >= 59)
            {
                seconds = 0;
                minutes++;
            }
            else
            {
                seconds++;
            }
        }
        if (hours <= 0) //Under an hour
        {
            if (seconds < 10)
            {
                time.text = minutes.ToString() + ":0" + seconds.ToString();
            }
            else {
                time.text = minutes.ToString() + ":" + seconds.ToString();
            }
        }
        else { //Over an hour
            if (seconds < 10 && minutes < 10)
            {
                time.text = hours.ToString() + ":0" + minutes.ToString() + ":0" + seconds.ToString();
            }
            else if (seconds < 10 && minutes >= 10) {
                time.text = hours.ToString() + ":" + minutes.ToString() + ":0" + seconds.ToString();
            }
            else if (seconds >= 10 && minutes < 10) {
                time.text = hours.ToString() + ":0" + minutes.ToString() + ":" + seconds.ToString();
            }
            else
            {
                time.text = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();
            }
        }
    }

    public void PreGame() {
        state = GameStates.PreGame;
        PregameCanvas.GetComponent<Canvas>().enabled = true;
        GameCanvas.GetComponent<Canvas>().enabled = false;
        GameOverCanvas.GetComponent<Canvas>().enabled = false; ;
        mainCamera.GetComponent<Camera>().enabled = true; //Show game through main camera
        ResetGame();
        player.GetComponent<Player>().ResetPlayer();
    }

    public void GameOn() {
        state = GameStates.GameOn;
        GameCanvas.GetComponent<Canvas>().enabled = true;
        PregameCanvas.GetComponent<Canvas>().enabled = false;
        GameOverCanvas.GetComponent<Canvas>().enabled = false;
        mainCamera.GetComponent<Camera>().enabled = false; //Show game through player camera
        InvokeRepeating("UpdateTime", 1, 1); //Update the time the game has been going on for once per second
        foreach (SpawnPoints point in FindObjectsOfType<SpawnPoints>()) {
            point.InvokeRepeating("SpawnEnemy", point.initialRoundSpawnDelay, point.timeBetweenSpawns);
        }
    }

    public void GameOver() {
        state = GameStates.GameOver;
        GameOverCanvas.GetComponent<Canvas>().enabled = true;
        PregameCanvas.GetComponent<Canvas>().enabled = false;
        GameCanvas.GetComponent<Canvas>().enabled = false;
        mainCamera.GetComponent<Camera>().enabled = true; //Show game through main camera
        foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
            Destroy(enemy.gameObject);
        }
        foreach (SpawnPoints point in FindObjectsOfType<SpawnPoints>())
        {
            point.CancelInvoke("SpawnEnemy");
        }
    }

    public void ResetGame() {
        hours = minutes = seconds = 0;
        playerScore = 0;
        score.text = playerScore.ToString();
        time.text = "0:00";
        totalEnemiesOnMap = 0;
    }

}
