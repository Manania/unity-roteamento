using System.Collections;
using System.Collections.Generic;
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
                    square.GetComponent<SpriteRenderer>().color = Color.black;
                    square.transform.tag = "Obstacle";
                    squareNode.isObstacle = true;
                }
                squares.Add(squareNode);
            }
        }

        // Adiciona os vizinhos de cada quadrado
        foreach (var square in squares)
        {
            var x = square.gridPosition.x;
            var y = square.gridPosition.y;

            if (x > 0)
            {
                square.AddNeighbor(GetNodeAtPosition(x - 1, y));
            }
            if (x < cols - 1)
            {
                square.AddNeighbor(GetNodeAtPosition(x + 1, y));
            }
            if (y > 0)
            {
                square.AddNeighbor(GetNodeAtPosition(x, y - 1));
            }
            if (y < rows - 1)
            {
                square.AddNeighbor(GetNodeAtPosition(x, y + 1));
            }
        }


        return squares;
    }

    private Node GetNodeAtPosition(float x, float y)
    {
        // Aqui estamos assumindo que cada GameObject tem um componente Node anexado
        return squares.Find(n => n.gridPosition.x == x && n.gridPosition.y == y);
    }

    // Muda a cor de um quadrado posteriormente
    public void ChangeSquareColor(int index, Color color, List<Node> objects)
    {
        if (index >= 0 && index < objects.Count)
        {
            objects[index].GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void ClearGrid()
    {
        foreach (Node square in squares)
        {
            if (!square.isObstacle)
            {
                square.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
