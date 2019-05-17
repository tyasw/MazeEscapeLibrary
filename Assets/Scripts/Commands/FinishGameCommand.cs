﻿using System.Collections.Generic;

namespace Assets.Scripts.Commands {
    public class FinishGameCommand : Command {
        private void Awake() {
            ClassFactory classFactory = ClassFactory.GetInstance();
            Subjects = new List<Subject>();
            Subject Event = classFactory.GetWonGameEvent();
            Subjects.Add(Event);
        }

        public override void Run() {
            base.Run();
        }

        public override string ToString() {
            return "WonGameCommand";
        }
    }
}