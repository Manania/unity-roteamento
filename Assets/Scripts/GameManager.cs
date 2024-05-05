using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridGenerator gridGenerator;
    List<Node> squares;
    public GameObject squarePrefab;
    public GameObject grid;
    [Header("Grid Settings - Cuidado ao colocar valores muito altos!")]
    [Range(10, 200)]
    public int rows;
    [Range(10, 200)]
    public int cols;
    [Range(0, 50)]
    public int obstaclesPercentage;
    public Color pathColor = Color.red;
    AStar aStar;
    void Start()
    {
        gridGenerator = new GridGenerator(rows, cols, squarePrefab, obstaclesPercentage, grid);
        squares = gridGenerator.GenerateGrid();
        aStar = new AStar(squares);
        RunPathfinder();
    }

    void RunPathfinder()
    {
        var startCoord = new Vector2Int(0, 0);
        var endCoord = new Vector2Int(3, 3);
        Node startNode;
        for (;;)
        {
            startNode = squares[Random.Range(0, squares.Count)];
            if (!startNode.isObstacle)
            {
                break;
            }
        }
        Node endNode;
        for (;;)
        {
            endNode = squares[Random.Range(0, squares.Count)];
            if (!endNode.isObstacle && endNode != startNode)
            {
                break;
            }

        }
        var path = aStar.GetPath(startNode, endNode, squares);
        if (path != null)
        {
            foreach (var node in squares)
            {
                if(node.visited)
                {
                    ChangeColor(node, Color.yellow);
                }
            }
            var pathColor = Color.red;
            foreach (var node in path)
            {
                ChangeColor(node, pathColor);
            }
        }
        Node.ChangeColor(startNode, Color.blue);
        Node.ChangeColor(endNode, Color.magenta);
    }

    private static void ChangeColor(Node node, Color color)
    {
        node.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }
}
