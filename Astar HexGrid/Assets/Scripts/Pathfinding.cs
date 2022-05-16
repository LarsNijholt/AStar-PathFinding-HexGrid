using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Hexgrid;
using UnityEngine;

public static class Pathfinding
{
    private static readonly Color pathColor = new Color(0.65f, 0.35f, 0.35f);
    private static readonly Color OpenColor = new Color(0.4f, 0.6f, 0.4f);
    private static readonly Color ClosedColor = new Color(0.35f, 0.4f, 0.5f);

    public static List<BaseNode> FindPath(BaseNode startNode, BaseNode targetNode)
    {
        var toSearch = new List<BaseNode>() { startNode };
        var processed = new List<BaseNode>();

        while(toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            current.SetColor(ClosedColor);

            if (current == targetNode)
            {
                var currentPathNode = targetNode;
                var path = new List<BaseNode>();
                var count = 100;
                while(currentPathNode != startNode)
                {
                    path.Add(currentPathNode);
                    currentPathNode = currentPathNode.Connection;
                    count--;
                    if (count < 0) throw new Exception();
                    Debug.Log("Exception thrown");
                }

                foreach (var node in path) node.SetColor(pathColor);
                startNode.SetColor(pathColor);
                Debug.Log(path.Count);
                return path;
            }

            foreach(var neighbour in current.Neighbours.Where(t=> t.Walkable && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbour);

                var CostToNeighbour = current.G + current.GetDistance(neighbour);

                if (!inSearch || CostToNeighbour < neighbour.G)
                {
                    neighbour.SetG(CostToNeighbour);
                    neighbour.SetConnection(current);

                    if(!inSearch)
                    {
                        neighbour.SetH(neighbour.GetDistance(targetNode));
                        toSearch.Add(neighbour);
                        neighbour.SetColor(OpenColor);
                    }
                }
            }
        }
        return null;
            
    }
}
