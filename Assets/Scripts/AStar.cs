using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static PriorityQueue openList, closedList;

    private static float HeuristicEstimatedCost(Node currentNode, Node goalNode){
        Vector3 diff = currentNode.position - goalNode.position;
        return diff.magnitude;
    }

    private static ArrayList CalculatePath(Node node){
        ArrayList list = new ArrayList();
        while (node != null){
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }
    private static void AddClosedListNode(Node node, Node neighbor, Node endNode){
        float totalCost = node.totalCost + HeuristicEstimatedCost(node, neighbor);
        neighbor.totalCost = totalCost;
        neighbor.parent = node;
        neighbor.estimatedCost = totalCost + HeuristicEstimatedCost(neighbor, endNode);
        if (!openList.Contains(neighbor))
            openList.Add(neighbor);
    }

    public static ArrayList FindPath(Node startingNode, Node endingNode){
        Node node = null;
        closedList = new PriorityQueue();
        openList = new PriorityQueue();
        startingNode.totalCost = 0f;
        openList.Add(startingNode);
        startingNode.estimatedCost = HeuristicEstimatedCost(startingNode, endingNode);

        

        while (openList.Length != 0){
            node = openList.GetFirst();
            if (node.position == endingNode.position)
                return CalculatePath(node);
            ArrayList neighbors = new ArrayList();
            GridManager.instance.GetNeighbors(node, neighbors);
            for (int i = 0; i < neighbors.Count; i++){
                Node neighbor = (Node)neighbors[i];
                if (!closedList.Contains(neighbor)){
                    AddClosedListNode(node, neighbor, endingNode);
                }
            }
            closedList.Add(node);
            openList.Remove(node);
        }
        if (node.position != endingNode.position){
            Debug.LogError("Goal Not Found!");
            return null;
        }
        return CalculatePath(node);
    }
}
