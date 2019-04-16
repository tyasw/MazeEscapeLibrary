﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiView {
    public GameModel GameModel { get; set; }
    public MazeTransformer MazeTransformer { get; set; }
    public int MazeWidth { get; set; }
    public int MazeHeight { get; set; }

    private Cell[,] Maze { get; set; }  // row x col

    public GuiView(GameModel gameModel) {
        GameModel = gameModel;
        MazeTransformer = new MazeTransformer(GameModel.MazeModel);
        MazeWidth = GameModel.GetMazeWidth();
        MazeHeight = GameModel.GetMazeHeight();
    }

    public void ShowMaze() {
        MazeWidth = GameModel.GetMazeWidth();
        MazeHeight = GameModel.GetMazeHeight();
        Maze = MazeTransformer.CreateMazeMatrix();
        //DrawMazeWallsTest();
        DrawMazeWalls();
    }

    private void DrawMazeWallsTest() {
        if (MazeWidth > 0 && MazeHeight > 0) {
            float wallWidth = GameModel.MazeModel.CellSize;
            float wallThickness = GameModel.MazeModel.CellWallThickness;
            Wall verticalTemplateWall = createVerticalTemplateWall(wallWidth, wallThickness);
            Wall horizontalTemplateWall = createHorizontalTemplateWall(wallWidth, wallThickness);
            drawTopWall(horizontalTemplateWall);
            drawLeftEdge(0,verticalTemplateWall);
            drawLeftEdge(1,verticalTemplateWall);
            drawBottomWall(horizontalTemplateWall);

            verticalTemplateWall.Instance.SetActive(false);
            horizontalTemplateWall.Instance.SetActive(false);
        }
    }

    private void DrawMazeWalls() {
        if (MazeWidth > 0 && MazeHeight > 0) {
            float wallWidth = GameModel.MazeModel.CellSize;
            float wallThickness = GameModel.MazeModel.CellWallThickness;
            Wall verticalTemplateWall = createVerticalTemplateWall(wallWidth, wallThickness);
            Wall horizontalTemplateWall = createHorizontalTemplateWall(wallWidth, wallThickness);
            drawTopWall(horizontalTemplateWall);
            for (int row = 0; row < MazeHeight; row++) {
                drawLeftEdge(row, verticalTemplateWall);
                drawInteriorRow(row, verticalTemplateWall, horizontalTemplateWall);
            }
            drawBottomWall(horizontalTemplateWall);

            verticalTemplateWall.Instance.SetActive(false);
            horizontalTemplateWall.Instance.SetActive(false);
        }
    }

    private Wall createVerticalTemplateWall(float wallWidth, float wallThickness) {
        float totalWidth = wallWidth + 2 * wallThickness;
        float cellHeight = 2.0f;        // TODO: should be the same as cell size

        GameObject exampleWall = GameObject.FindGameObjectWithTag("Wall");

        float xPos = (cellHeight + wallThickness) / 2;
        float yPos = cellHeight / 2;
        float zPos = 0.0f;
        Vector3 position = new Vector3(xPos, yPos, zPos);
        Quaternion rotation = exampleWall.transform.rotation;
        GameObject templateWall = GameObject.Instantiate(exampleWall, position, rotation);
        templateWall.name = "Vertical Template";

        templateWall.transform.localScale = new Vector3(totalWidth, cellHeight, wallThickness);
        Wall template = new Wall(wallThickness, wallWidth, templateWall);

        return template;
    }

    private Wall createHorizontalTemplateWall(float wallWidth, float wallThickness) {
        float totalWidth = wallWidth + 2 * wallThickness;
        float cellHeight = 2.0f;

        GameObject exampleWall = GameObject.FindGameObjectWithTag("Wall");

        float xPos = 0.0f;
        float yPos = cellHeight / 2;
        float zPos = (cellHeight + wallThickness) / 2;
        Vector3 position = new Vector3(xPos, yPos, zPos);
        Quaternion rotation = exampleWall.transform.rotation;
        GameObject templateWall = GameObject.Instantiate(exampleWall, position, rotation);
        templateWall.name = "Horizontal Template";

        templateWall.transform.localScale = new Vector3(wallThickness, cellHeight, totalWidth);

        Wall template = new Wall(wallThickness,wallWidth,templateWall);

        return template;
    }

    private void drawTopWall(Wall templateWall) {
        GameObject template = templateWall.Instance;
        float xPos = template.transform.position.x;
        float yPos = template.transform.position.y;
        float zPos = template.transform.position.z;
        float wallWidth = templateWall.Width;
        float wallThickness = templateWall.Thickness;
        for (int col = 0; col < MazeWidth; col++) {
            Vector3 wallPosition = new Vector3(xPos, yPos, zPos);
            Quaternion wallRotation = template.transform.rotation;
            GameObject wall = GameObject.Instantiate(template, wallPosition, wallRotation);
            zPos += wallWidth + wallThickness;
        }
    }

    private void drawBottomWall(Wall templateWall) {
        GameObject template = templateWall.Instance;
        int numRows = Maze.GetLength(0);
        float wallWidth = templateWall.Width;
        float wallThickness = templateWall.Thickness;
        float xPos = (wallWidth + wallThickness) * numRows + template.transform.position.x;
        float yPos = template.transform.position.y;
        float zPos = template.transform.position.z;
        for (int col = 0; col < (MazeWidth - 1); col++) {       // Leave bottom right open for exit
            //Debug.Log("Intended = (" + xPos + ", 0.0f, " + zPos + ")");
            Vector3 wallPosition = new Vector3(xPos, yPos, zPos);
            Quaternion wallRotation = template.transform.rotation;
            GameObject wall = GameObject.Instantiate(template, wallPosition, wallRotation);
            //Debug.Log("Actual = (" + wall.transform.position.x + ", " + wall.transform.position.y + ", " + wall.transform.position.z + ")");
            zPos += wallWidth + wallThickness;
        }
    }

    private void drawLeftEdge(int row, Wall templateWall) {
        if (row != 0) {         // Leave upper right open for entrance
            GameObject template = templateWall.Instance;
            float wallWidth = templateWall.Width;
            float wallThickness = templateWall.Thickness;
            Quaternion wallRotation = template.transform.rotation;
            float xPos = row * (wallWidth + wallThickness) + template.transform.position.x;
            float yPos = template.transform.position.y;
            float zPos = template.transform.position.z;
            Vector3 wallPosition = new Vector3(xPos, yPos, zPos);
            GameObject wall = GameObject.Instantiate(template, wallPosition, wallRotation);
        }
    }

    private void drawInteriorRow(int row, Wall verticalTemplateWall, Wall horizontalTemplateWall) {
        GameObject verticalWall = verticalTemplateWall.Instance;
        GameObject horizontalWall = horizontalTemplateWall.Instance;
        float wallWidth = verticalTemplateWall.Width;
        float wallThickness = verticalTemplateWall.Thickness;
        for (int col = 0; col < MazeWidth; col++) {
            if (Maze[row, col].BottomWall) {    // Draw horizontal wall
                float xPos = (row + 1) * (wallWidth + wallThickness) + horizontalWall.transform.position.x;
                float yPos = horizontalWall.transform.position.y;
                float zPos = col * (wallWidth + wallThickness) + horizontalWall.transform.position.z;
                Vector3 wallPosition = new Vector3(xPos, yPos, zPos);
                Quaternion wallRotation = horizontalWall.transform.rotation;
                GameObject wall = GameObject.Instantiate(horizontalWall, wallPosition, wallRotation);
            }

            if (Maze[row, col].RightWall) {     // Draw vertical wall
                float xPos = row * (wallWidth + wallThickness) + verticalWall.transform.position.x;
                float yPos = verticalWall.transform.position.y;
                float zPos = (col + 1) * (wallWidth + wallThickness) + verticalWall.transform.position.z;
                Vector3 wallPosition = new Vector3(xPos, yPos, zPos);
                Quaternion wallRotation = verticalWall.transform.rotation;
                GameObject wall = GameObject.Instantiate(verticalWall, wallPosition, wallRotation);
            }
        }
    }
}