﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Events;

public class MainMenuController : MonoBehaviour {
    public CustomEventSystem EventSystem;

    void Awake() {
        EventSystem = GameObject.FindObjectOfType<CustomEventSystem>();
        InitializeEvents();
    }

    private void InitializeEvents() {
        EventSystem.RegisterListener(typeof(StartGameEvent), StartNewGame);
    }

    public void StartNewGame() {
        SceneManager.LoadScene("Game");
    }
}