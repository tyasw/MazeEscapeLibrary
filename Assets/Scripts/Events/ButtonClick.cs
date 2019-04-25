﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Events {
    public class ButtonClick : MonoBehaviour {
        public Button Button;
        public GameEvent Event;
        public GameController GameController;

        private void Start() {
            GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            Button.onClick.AddListener(OnClick);
            Event = GetComponent<GameEvent>();
        }

        private void OnClick() {
            Event.Trigger(GameController);
        }
    }
}
