using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//For pathfinding
public class Astar : MonoBehaviour
{
    private Vector2Int _start;
    private Vector2Int _end;

    //True if is obstacle
    private bool[,] _obstacles;
    private int _rows, _cols;
    
    //Directions right, left, up, down
    private readonly Vector2Int[] _directions = {
        new(1, 0),  
        new(-1, 0),
        new(0, 1),
        new(0, -1) 
    };

    void Awake()
    {
        //Setting AStar
        GameManager.Instance.astar = this;
    }

    //Returns AStar path in List form
    public List<Vector2Int> GetAstarPath()
    {
        var mapTiles = GameManager.Instance.mapDriver.mapTiles;
        _rows = GameManager.Instance.mapDriver.mapTiles.GetLength(0);
        _cols = GameManager.Instance.mapDriver.mapTiles.GetLength(1);
        _obstacles = new bool[_rows, _cols];
        
        //Creating array of "is obstacle" info for board grid
        for (var i = 0; i < _rows; i++)
            for (var j = 0; j < _cols; j++)
                _obstacles[i, j] = mapTiles[i, j].GetComponent<TileObject>().isObstacle;

        _start = GameManager.Instance.mapDriver.startTilePlacement;
        _end = GameManager.Instance.mapDriver.endTilePlacement;
        
        return GetShortestPath();
    }

    List<Vector2Int> GetShortestPath()
    {
        //Heap for nodes than need to be processed
        var binaryHeap = new AstarQueue<AstarNode>();
        
        //Nodes that have been processed
        var visited = new HashSet<Vector2Int>();

        //Start node, no parent, no cost, heuristic cost calculated with Manhattan Distance
        binaryHeap.Enqueue(new AstarNode(_start, null, 0, ManhattanDistance(_start, _end)));

        //Main AStar loop, process until nodes available
        while (binaryHeap.Count > 0)
        {
            //Get node to process from binary heap
            var current = binaryHeap.Dequeue();

            //If we reached end node, reconstruct path and end AStar
            if (current.Position == _end)
                return GetReconstructedPath(current);

            //Node will be processed so it can be marked as done
            visited.Add(current.Position);

            //Check all available directions to move (right, left, up, down)
            foreach (var direction in _directions)
            {
                //Neighbour on the direction side from current node
                var neighbor = current.Position + direction;
                
                //Neighbour might not exist
                if (!(neighbor.x >= 0 && neighbor.x < _rows && neighbor.y >= 0 && neighbor.y < _cols)) 
                    continue;

                //Only search for unvisited neighbours
                if (visited.Contains(neighbor))
                    continue;
                    
                //If neighbor can't be an obstacle
                if (_obstacles[neighbor.x, neighbor.y])
                    continue;

                //New node for neighbor that passed all the checks, incremented cost, new heuristics cost
                var neighborNode = new AstarNode(neighbor, current,  current.StartNodeCost + 1, ManhattanDistance(neighbor, _end));
                
                if (binaryHeap.Contains(neighborNode) && current.StartNodeCost + 1 >= neighborNode.StartNodeCost)
                    continue;
                
                //Neighbor is not in binary heap or shortest path to him was found
                //Set parent, increment cost and set summarized cost
                neighborNode.Parent = current;
                neighborNode.StartNodeCost =  current.StartNodeCost + 1;
                neighborNode.StartEndCost = neighborNode.StartNodeCost + neighborNode.NodeEndCost;

                //Move neighbor to heap if it not exist there
                if (!binaryHeap.Contains(neighborNode))
                    binaryHeap.Enqueue(neighborNode);
            }
        }

        //AStar hasn't found any path (obstacles blocked the way)
        return null;
    }

    //Heuristic calculating method
    float ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    //Makes a reconstruction from end to start
    List<Vector2Int> GetReconstructedPath(AstarNode an)
    {
        var path = new List<Vector2Int>();
        
        //Reconstructing path from current node to parental nodes
        for (; an != null; an = an.Parent)
            path.Add(an.Position);
        path.Reverse();
        return path;
    }
}
