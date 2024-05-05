using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridGenerator
{
    int rows;
    int cols;
    GameObject squarePrefab;
    int obstaclesPercentage;
    List<Node> squares = new List<Node>();
    GameObject grid;

    public GridGenerator(int rows, int cols, GameObject squarePrefab, int obstaclesPercentage, GameObject grid)
    {
        this.rows = rows;
        this.cols = cols;
        this.squarePrefab = squarePrefab;
        this.obstaclesPercentage = obstaclesPercentage;
        this.grid = grid;
    }

    // Gera a grid
    public List<Node> GenerateGrid()
    {
        float startX = grid.transform.position.x - (cols / 2);
        float startY = grid.transform.position.y - (rows / 2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var position = new Vector3(startX + j, 0, startY + i);
                var square = GameObject.Instantiate(squarePrefab, position, Quaternion.identity);
                square.transform.parent = grid.transform;
                var squareNode = square.GetComponent<Node>();
                squareNode.gridPosition = new Vector2Int(j, i);

                var isObstacle = Random.Range(0, 100) < obstaclesPercentage;
                if (isObstacle)
                {
                    var tile = squareNode.gameObject.GetComponent<Tile>();
                    var cubeSize = 10f;
                    tile.transform.localScale = new Vector3(square.transform.localScale.x, cubeSize, square.transform.localScale.x);
                    Node.ChangeColor(squareNode, Color.black);
                    //square.transform.tag = "Obstacle";
                    squareNode.isObstacle = true;
                }
                squares.Add(squareNode);
            }
        }

        for (var i = 0; i < squares.Count; i++)
        {
            var node1 = squares[i];
            var textObject = node1.gameObject.GetComponentInChildren<TextMeshPro>();
            textObject.text = node1.gridPosition.x + "," + node1.gridPosition.y;
            for (var j = i + 1; j < squares.Count; j++)
            {
                var node2 = squares[j];
                if ((node2.gridPosition - node1.gridPosition).magnitude == 1)
                {
                    node1.AddNeighbor(node2);
                    node2.AddNeighbor(node1);
                }
            }
        }
        return squares;
    }
}
