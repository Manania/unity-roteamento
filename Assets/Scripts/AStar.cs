using System.Collections.Generic;
using UnityEngine;

// Exemplo adaptado do algoritmo A* do livro 
// AI for Games, de Ian Millington
public class AStar
{
    // Armazena todos os n�s do grid para o algoritmo A*
    public List<Node> squares;
    public AStar(List<Node> squares) 
    { 
        this.squares = squares;
    }

    // Estrutura auxiliar para armazenar informa��es adicionais sobre os n�s durante a busca
    private class NodeRecord
    {
        public Node node; // O pr�prio n�
        public Node connection; // N� anterior no caminho
        public float costSoFar; // Custo acumulado para chegar at� este n�
        public float estimatedTotalCost; // Estimativa do custo total at� o destino

        // Inicializa um novo registro de n� com valores padr�o
        public NodeRecord(Node node)
        {
            this.node = node;
            this.connection = null;
            this.costSoFar = 0;
            this.estimatedTotalCost = Mathf.Infinity;
        }
    }

    // Fun��o heur�stica que estima o custo de um n� at� o destino. Neste caso, usa a dist�ncia euclidiana.
    private float Heuristic(Node a, Node b)
    {
        return Vector2.Distance(a.gridPosition, b.gridPosition);
    }

    // M�todo principal que executa o algoritmo A*
    public List<Node> GetPath(Node startNode, Node destinationNode, List<Node> nodes)
    {
        squares = nodes;

        // Inicializa os registros de n�s, listas aberta e fechada
        Dictionary<Node, NodeRecord> nodeRecords = new Dictionary<Node, NodeRecord>();
        // Lista com n�s a serem explorados
        // Ser�o considerados para expans�o os n�s com menor custo estimado total
        List<NodeRecord> open = new List<NodeRecord>();
        // Lista com n�s que j� foram explorados
        // Ser�o ignorados na expans�o
        List<NodeRecord> closed = new List<NodeRecord>();

        // Prepara o n� de in�cio
        NodeRecord startRecord = new NodeRecord(startNode);
        startRecord.costSoFar = 0;
        // Estimativa do custo total � a dist�ncia euclidiana at� o destino
        startRecord.estimatedTotalCost = Heuristic(startNode, destinationNode);
        open.Add(startRecord);
        nodeRecords[startNode] = startRecord;

        // Enquanto houver n�s na lista aberta, continua a busca
        while (open.Count > 0)
        {
            // Seleciona o n� com o menor custo estimado total
            NodeRecord currentRecord = open[0];
            foreach (var record in open)
            {
                if (record.estimatedTotalCost < currentRecord.estimatedTotalCost)
                {
                    currentRecord = record;
                }
            }

            // Se o n� atual � o destino, termina a busca
            if (currentRecord.node == destinationNode)
            {
                break;
            }

            // Explora os vizinhos do n� atual
            foreach (Node neighbor in currentRecord.node.neighbors)
            {
                // Ignora obst�culos
                if (neighbor.isObstacle) continue;

                float endNodeCost = currentRecord.costSoFar + neighbor.cost;
                NodeRecord endNodeRecord;

                // Atualiza ou cria um novo registro para o vizinho
                if (nodeRecords.ContainsKey(neighbor))
                {
                    endNodeRecord = nodeRecords[neighbor];
                    if (endNodeRecord.costSoFar <= endNodeCost) continue;
                }
                else
                {
                    endNodeRecord = new NodeRecord(neighbor);
                    nodeRecords[neighbor] = endNodeRecord;
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = currentRecord.node;
                endNodeRecord.estimatedTotalCost = endNodeCost + Heuristic(neighbor, destinationNode);

                // Adiciona o vizinho � lista aberta se ainda n�o estiver
                if (!open.Contains(endNodeRecord))
                {
                    open.Add(endNodeRecord);
                }
            }
            currentRecord.node.visited = true;
            // Move o n� atual da lista aberta para a fechada
            open.Remove(currentRecord);
            closed.Add(currentRecord);
        }

        // Reconstr�i o caminho do destino at� o in�cio
        if (nodeRecords.ContainsKey(destinationNode))
        {
            var path = new List<Node>();
            NodeRecord current = nodeRecords[destinationNode];
            while (current.node != startNode)
            {
                path.Add(current.node);
                current = nodeRecords[current.connection];
            }
            return path;
        }
        return null;
    }
}
