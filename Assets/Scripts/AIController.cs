using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private float elapsedTime, intervalTime = 1.0f;
    [SerializeField] private Color pathColor = Color.magenta;

    private Node startNode, endNode;
    private ArrayList pathArray = new ArrayList();

    private void FindPath(){
        startNode = new Node(GridManager.instance.GetCellCenter(GridManager.instance.GetGridIndex(transform.position)));
        endNode = new Node(GridManager.instance.GetCellCenter(GridManager.instance.GetGridIndex(destination.position)));
        pathArray = AStar.FindPath(startNode, endNode);
    }
    void Start()
    {
        FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > intervalTime){
            elapsedTime = 0f;
            FindPath();
        }
    }
    void OnDrawGizmos(){
        if (pathArray == null)
            return;
        if (pathArray.Count > 0){
            int index = 1;
            foreach (Node node in pathArray){
                if (index < pathArray.Count){
                    Node nextNode = (Node)pathArray[index++];
                    Debug.DrawLine(node.position, nextNode.position, pathColor);
                }
            }
        }
    }
}
