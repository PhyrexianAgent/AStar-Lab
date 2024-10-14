using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] private int rowCount, columnCount;
    [SerializeField] private float cellSize;
    [SerializeField] private bool showGrid = true, showObstacles = true;
    [SerializeField] private string obstacleTagName = "Obstacles";
    [SerializeField] private string mudTagName = "Mud";
    [SerializeField] private float mudWeight = 5;
    [SerializeField] private Color mudColor = Color.green;
    [SerializeField] private Color gridColor = Color.blue;
    [SerializeField] private Color obstacleColor = Color.grey;

    private GameObject[] obstacles;
    private GameObject[] mudSpots;
    private Node[,] nodes;

    public int GetRow(int index) => index / columnCount;
    public int GetColumn(int index) => index % columnCount;
    public int GetGridIndex(Vector3 position){
        if (!IsInBounds(position))
            return -1;
        position -= Vector3.zero;
        int col = (int)(position.x / cellSize);
        int row = (int)(position.z / cellSize);
        return row * columnCount + col;
    }
    public bool IsInBounds(Vector3 position){
        float width = columnCount * cellSize, height = rowCount * cellSize;
        return position.x >= Vector3.zero.x && position.x <= Vector3.zero.x + width && position.z >= Vector3.zero.z && position.z <= Vector3.zero.z + width;
    }
    public Vector3 GetCellCenter(int index){
        Vector3 cellPos = GetCellPosition(index);
        cellPos.x += (cellSize / 2.0f);
        cellPos.z += (cellSize / 2.0f);
        return cellPos;
    }
    public Vector3 GetCellPosition(int index) => Vector3.zero + new Vector3(GetColumn(index) * cellSize, 0, GetRow(index) * cellSize);

    private void CalculateObstacles(){
        foreach(GameObject obstacle in obstacles){
            int cellIndex = GetGridIndex(obstacle.transform.position);
            nodes[GetColumn(cellIndex), GetRow(cellIndex)].MakeObstacle();
        }
    }
    private void CalculateMudSpots(){
        foreach(GameObject mudSpot in mudSpots){
            int cellIndex = GetGridIndex(mudSpot.transform.position);
            nodes[GetColumn(cellIndex), GetRow(cellIndex)].totalCost = mudWeight;
        }
    }
    private void MakeCells(){
        int index = 0;
        nodes = new Node[columnCount, rowCount];
        for (int row = 0; row < rowCount; row++){
            for (int col = 0; col < columnCount; col++){
                nodes[col, row] = new Node(GetCellCenter(index));
                index++;
            }
        }
    }
    private void AddNode(int row, int col, int index){
        Vector3 cellPos = GetCellCenter(index);
        nodes[col, row] = new Node(cellPos);
    }
    public void GetNeighbors(Node node, ArrayList neighbors){
        int index = GetGridIndex(node.position);
        int row = GetRow(index), col = GetColumn(index);
        AddNeighbor(col, row - 1, neighbors);
        AddNeighbor(col, row + 1, neighbors);
        AddNeighbor(col - 1, row, neighbors);
        AddNeighbor(col + 1, row, neighbors);
    }
    private void AddNeighbor(int col, int row, ArrayList neighbors){
        if (col > -1 && col < columnCount && row > -1 && row < rowCount && !nodes[col, row].IsObstacle()){
            neighbors.Add(nodes[col, row]);
        }
    }
    private void DebugDrawGrid(){
        float width = columnCount * cellSize, height = rowCount * cellSize;
        for (int i = 0; i <= rowCount; i++){
            Vector3 startPos = transform.position + new Vector3(0.0f, 0.0f, 1.0f) * i * cellSize;
            Vector3 endPos = startPos + new Vector3(1.0f, 0.0f, 0.0f) * width;
            Debug.DrawLine(startPos, endPos, gridColor);
        }
        for (int i = 0; i <= columnCount; i++){
            Vector3 startPos = transform.position + new Vector3(1.0f, 0.0f, 0.0f) * i * cellSize;
            Vector3 endPos = startPos + new Vector3(0.0f, 0.0f, 1.0f) * height;
            Debug.DrawLine(startPos, endPos, gridColor);
        }
    }
    private void DrawObstacles(){
        Gizmos.color = obstacleColor;
        foreach(GameObject obstacle in obstacles){
            Gizmos.DrawCube(GetCellCenter(GetGridIndex(obstacle.transform.position)), new Vector3(cellSize, 1.0f, cellSize));
        }
    }
    private void DrawMud(){
        Gizmos.color = mudColor;
        foreach(GameObject mudSpot in mudSpots){
            Gizmos.DrawCube(GetCellCenter(GetGridIndex(mudSpot.transform.position)), new Vector3(cellSize, 1.0f, cellSize));
        }
    }


    void Awake(){
        instance = this;
        obstacles = GameObject.FindGameObjectsWithTag(obstacleTagName);
        mudSpots = GameObject.FindGameObjectsWithTag(mudTagName);
        MakeCells();
        CalculateObstacles();
        CalculateMudSpots();
    }
    void OnDrawGizmos(){
        if (showGrid)
            DebugDrawGrid();
        Gizmos.DrawSphere(transform.position, 0.5f);
        if (showObstacles && obstacles != null){
            if (obstacles != null)
                DrawObstacles();
            if (mudSpots != null)
                DrawMud();
        }
    }
}
