using System.Collections.Generic;
using UnityEngine;

// Exemplo adaptado do algoritmo A* do livro 
// AI for Games, de Ian Millington
public class AStar
{
    // Armazena todos os nós do grid para o algoritmo A*
    public List<Node> squares;
    public AStar(List<Node> squares) 
    { 
        this.squares = squares;
    }

    // Estrutura auxiliar para armazenar informações adicionais sobre os nós durante a busca
    private class NodeRecord
    {
        public Node node; // O próprio nó
        public Node connection; // Nó anterior no caminho
        public float costSoFar; // Custo acumulado para chegar até este nó
        public float estimatedTotalCost; // Estimativa do custo total até o destino

        // Inicializa um novo registro de nó com valores padrão
        public NodeRecord(Node node)
        {
            this.node = node;
            this.connection = null;
            this.costSoFar = 0;
            this.estimatedTotalCost = Mathf.Infinity;
        }
    }

    // Função heurística que estima o custo de um nó até o destino. Neste caso, usa a distância euclidiana.
    private float Heuristic(Node a, Node b)
    {
        return Vector2.Distance(a.gridPosition, b.gridPosition);
    }

    // Método principal que executa o algoritmo A*
    public List<Node> GetPath(Node startNode, Node destinationNode, List<Node> nodes)
    {
        squares = nodes;

        // Inicializa os registros de nós, listas aberta e fechada
        Dictionary<Node, NodeRecord> nodeRecords = new Dictionary<Node, NodeRecord>();
        // Lista com nós a serem explorados
        // Serão considerados para expansão os nós com menor custo estimado total
        List<NodeRecord> open = new List<NodeRecord>();
        // Lista com nós que já foram explorados
        // Serão ignorados na expansão
        List<NodeRecord> closed = new List<NodeRecord>();

        // Prepara o nó de início
        NodeRecord startRecord = new NodeRecord(startNode);
        startRecord.costSoFar = 0;
        // Estimativa do custo total é a distância euclidiana até o destino
        startRecord.estimatedTotalCost = Heuristic(startNode, destinationNode);
        open.Add(startRecord);
        nodeRecords[startNode] = startRecord;

        // Enquanto houver nós na lista aberta, continua a busca
        while (open.Count > 0)
        {
            // Seleciona o nó com o menor custo estimado total
            NodeRecord currentRecord = open[0];
            foreach (var record in open)
            {
                if (record.estimatedTotalCost < currentRecord.estimatedTotalCost)
                {
                    currentRecord = record;
                }
            }

            // Se o nó atual é o destino, termina a busca
            if (currentRecord.node == destinationNode)
            {
                break;
            }

            // Explora os vizinhos do nó atual
            foreach (Node neighbor in currentRecord.node.neighbors)
            {
                // Ignora obstáculos
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

                // Adiciona o vizinho à lista aberta se ainda não estiver
                if (!open.Contains(endNodeRecord))
                {
                    open.Add(endNodeRecord);
                }
            }
            currentRecord.node.visited = true;
            // Move o nó atual da lista aberta para a fechada
            open.Remove(currentRecord);
            closed.Add(currentRecord);
        }

        // Reconstrói o caminho do destino até o início
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
