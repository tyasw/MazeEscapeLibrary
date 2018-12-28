﻿using System;
using System.Collections;
using System.Collections.Generic;

/** TextController.cs
 * 
 * Controls the creation of the maze, and the view presented to the user for the
 * text version of the game.
 */
namespace MazeEscape.src
{
    public class TextController : GameController
    {
        public GameModel GameModel { get; set; }
        public GameView GameView { get; set; }
        public CommandParser CmdParser { get; set; }

        public TextController()
        {
            GameModel = new GameModel();
            GameView = new TextView(GameModel);
            CmdParser = new CommandParser();
        }

        public void Start()
        {
            AddCommand(new ShowNewGameOptionsCommand(this));
            AddCommand(new BeginGameCommand(this, GameModel));

            while (true)
            {
                RunNextCommand();
            }

            //...
        }

        public void ShowMainMenu()
        {
            throw new NotImplementedException();
        }

        public void AddCommand(Command command)
        {
            CmdParser.AddCommand(command);
        }

        public void ShowGameOptions()
        {
            try
            {
                GetMazeWidthAndHeightFromUser();
            } catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void LogError(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private void GetMazeWidthAndHeightFromUser()
        {
            Console.WriteLine("What size of maze do you want?");
            Console.Write("Width: ");
            int width = Int32.Parse(Console.ReadLine().Trim());
            Console.Write("Height: ");
            int height = Int32.Parse(Console.ReadLine().Trim());

            GameModel.SetMazeWidth(width);
            GameModel.SetMazeHeight(height);
        }

        public void FireDrawMaze()
        {
            GameView.ShowMaze();
        }

        public void FireDrawWorld()
        {
            //...
            FireDrawMaze();
        }

        public void RunNextCommand()
        {
            CmdParser.RunNextCommand();
        }
    }
}
