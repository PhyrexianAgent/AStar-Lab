using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Node : IComparable
{
    private bool isObstacle = false;
    public Node parent = null;
    public float totalCost = 1, estimatedCost;
    public Vector3 position {get; private set;}

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
    public void MakeObstacle(){
        isObstacle = true;
    }
}
