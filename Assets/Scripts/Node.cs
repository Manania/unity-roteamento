using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2Int gridPosition;
    public float cost = 1f;
    public float distance = Mathf.Infinity;
    public bool visited = false;
    public Node previousNode = null;
    public bool isObstacle = false;
    public List<Node> neighbors = new List<Node>();

    public void AddNeighbor(Node neighbor)
    {
        neighbors.Add(neighbor);
        
        // -----------------------------------------------------------
        // ativar a linha abaixo para setar o cost randomicamente.
        // Só vai funcionar no Dijkistra e no A*
        // assim o Dijkistra encontra o menor caminho baseado no custo
        // Se não tiver essa linha, o Dijkistra vai se comportar como o BFS
        
        cost = Random.Range(0.0f, 3.0f);
        
        // -----------------------------------------------------------
    }
    private void Start()
    {
        var pos = gameObject.transform.position;
        this.gridPosition = new Vector2Int((int)pos.x, (int)pos.z);
    }

    public static void ChangeColor(Node node, Color color)
    {
        node.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
    }
}
