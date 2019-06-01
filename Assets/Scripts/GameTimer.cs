﻿using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
    public Text GameTimeText;
    public float timeStartedMaze;
    public float elapsedTime;

    private bool timerShouldRun;

    private void Awake() {
        GameTimeText = GetComponent<Text>();
    }

    private void Start() {
        timeStartedMaze = Time.time;
        timerShouldRun = true;
    }

    private void Update() {
        if (timerShouldRun) {
            elapsedTime = Time.time - timeStartedMaze;
            GameTimeText.text = "Time: " + elapsedTime.ToString().Substring(0, 4); 
        }
    }

    public void StopTimer() {
        timerShouldRun = false;
    }
}
