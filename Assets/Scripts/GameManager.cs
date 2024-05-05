using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    List<Node> path;
    GameObject player;
    void Start()
    {
        gridGenerator = new GridGenerator(rows, cols, squarePrefab, obstaclesPercentage, grid);
        squares = gridGenerator.GenerateGrid();
        aStar = new AStar(squares);
        RunPathfinder();
    }
    Node startNode;
    Node endNode;

    void RunPathfinder()
    {
        var startCoord = new Vector2Int(0, 0);
        var endCoord = new Vector2Int(3, 3);
        for (; ; )
        {
            startNode = squares[Random.Range(0, squares.Count)];
            if (!startNode.isObstacle)
            {
                break;
            }
        }
        for (; ; )
        {
            endNode = squares[Random.Range(0, squares.Count)];
            if (!endNode.isObstacle && endNode != startNode)
            {
                break;
            }

        }
        path = aStar.GetPath(startNode, endNode, squares);
        if (path != null)
        {
            foreach (var node in squares)
            {
                if (node.visited)
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
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.transform.position = new Vector3(startNode.gridPosition.x, 0, startNode.gridPosition.y);
        }
    }

    private static void ChangeColor(Node node, Color color)
    {
        node.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }


    IEnumerator currentAnimation;
    public void Update()
    {        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(startNode.gridPosition.x, 0, startNode.gridPosition.y);
        }
        if (path != null && player?.transform.position is Vector3 playerPosition3d)
        {
            var target = path.LastOrDefault();
            if (target == default)
            {
                return;
            }
            var dest3d = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            if (currentAnimation != null)
            {
                currentAnimation.MoveNext();
            }
            else
            {
                currentAnimation = WalkingAnimation(player, dest3d, 0.0001f).GetEnumerator();
            }
            if(player.transform.position == dest3d) 
            {
                path.Remove(target);
            }
        }
    }

    private IEnumerable WalkingAnimation(GameObject player, Vector3 dest, float duracao)
    {
        var s0 = player.transform.position;
        var t = 0f;
        while(t < duracao)
        {
            t += Time.deltaTime;
            player.transform.position = Vector3.Lerp(s0, dest, Mathf.Min(t/duracao, 1f));
            yield return null;
        }
        currentAnimation = null;
    }
}
