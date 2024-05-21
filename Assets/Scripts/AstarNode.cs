using System;
using UnityEngine;

//For storing node data
class AstarNode : IComparable<AstarNode>
{
    public Vector2Int Position;
    public AstarNode Parent;
    public float StartNodeCost; // Cost from start to current node
    public float NodeEndCost; // Cost from this node to end (of course heuristic)
    public float StartEndCost; // Whole cost from start to end

    //Store and calculate values on creation
    public AstarNode(Vector2Int position, AstarNode parent, float startNodeCost, float nodeEndCost)
    {
        Position = position;
        Parent = parent;
        StartNodeCost = startNodeCost;
        NodeEndCost = nodeEndCost;
        StartEndCost = startNodeCost + nodeEndCost;
    }

    //Compare final cost
    public int CompareTo(AstarNode other)
    {
        return StartEndCost.CompareTo(other.StartEndCost);
    }
}