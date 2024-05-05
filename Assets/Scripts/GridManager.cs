using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public void Start()
    {
        var nodes = gameObject.GetComponentsInChildren<Node>();
        for(var i = 0; i < nodes.Length; i++)
        {
            var node1 = nodes[i];
            grid.Add(node1.gridPosition, node1);
            var textObject = node1.gameObject.GetComponentInChildren<TextMeshPro>();
            textObject.text = node1.gridPosition.x + "," + node1.gridPosition.y;
            for(var j = i+1; j < nodes.Length; j++)
            {
                var node2 = nodes[j];
                if ((node2.gridPosition - node1.gridPosition).magnitude == 1)
                {
                    node1.AddNeighbor(node2);
                    node2.AddNeighbor(node1);
                }
            }
        }
        var pathfinder = new AStar(nodes.ToList());
        var start = grid[new Vector2Int(0, 0)];
        var end = grid[new Vector2Int(3, 3)];
        var path = pathfinder.GetPath(start, end, nodes.ToList());
        if(path != null)
        {
            var pathColor = Color.red;
            ChangeColor(start, pathColor);
            foreach (var node in path)
            {
                ChangeColor(node, pathColor);
            }
        }
    }

    private static void ChangeColor(Node node, Color color)
    {
        node.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }
}
