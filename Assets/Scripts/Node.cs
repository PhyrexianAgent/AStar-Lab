using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Node : IComparable
{
    private bool isObstacle = false;
    public Node parent {get; private set;} = null;
    public float totalCost {get; private set;} = 1;
    public float estimatedCost {get; private set;} = 0;
    private Vector3 position;

    public Node(Vector3 position){
        this.position = position;
    }

    public bool IsObstacle() => isObstacle;

    public int CompareTo(object obj){
        Node node = (Node)obj;
        if (estimatedCost < node.estimatedCost)
            return -1;
        else if (estimatedCost > node.estimatedCost)
            return 1;
        return 0;
    }
}
