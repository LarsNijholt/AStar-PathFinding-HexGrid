using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Hexgrid;
using UnityEngine;

public static class Pathfinding
{
    private static readonly Color pathColor = Color.red;
    private static readonly Color OpenColor = Color.green;
    private static readonly Color ClosedColor = Color.blue;

    /// This method finds the shortest path between a start and a target node
    public static List<BaseNode> FindPath(BaseNode startNode, BaseNode targetNode)
    {
        // Initialize the open list with the start node
        var toSearch = new List<BaseNode>() { startNode };
        // Initialize the closed list
        var processed = new List<BaseNode>();

        // Continue searching as long as there are nodes in the open list
        while(toSearch.Any())
        {
            // Start by considering the first node in the open list
            var current = toSearch[0];

            // Find the node in the open list with the lowest F cost (or H cost in the event of a tie)
            foreach (var t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

            // Move the current node from the open list to the closed list
            processed.Add(current);
            toSearch.Remove(current);

            // Color the current node to indicate that it's been processed
            current.SetColor(ClosedColor);

            // If the current node is the target node, we've found our path
            if (current == targetNode)
            {
                // Backtrack from the target node to the start node to create the final path
                var currentPathNode = targetNode;
                var path = new List<BaseNode>();
                var count = 100;

                // Continue backtracking until we reach the start node
                while(currentPathNode != startNode)
                {
                    path.Add(currentPathNode);
                    currentPathNode = currentPathNode.Connection;
                    count--;

                    // Safety check to prevent infinite loops
                    if (count < 0) throw new Exception();
                    Debug.Log("Exception thrown");
                }

                // Color the final path nodes
                foreach (var node in path) node.SetColor(pathColor);
                startNode.SetColor(pathColor);

                // Output the number of nodes in the path
                Debug.Log(path.Count);

                // Return the final path
                return path;
            }

            // For each walkable and unprocessed neighboring node of the current node
            foreach(var neighbour in current.Neighbours.Where(t=> t.Walkable && !processed.Contains(t)))
            {
                // Check if the neighbor is in the open list
                var inSearch = toSearch.Contains(neighbour);

                // Calculate the new G cost for this neighbor
                var CostToNeighbour = current.G + current.GetDistance(neighbour);

                // If the neighbor wasn't in the open list, or we found a shorter path to it
                if (!inSearch || CostToNeighbour < neighbour.G)
                {
                    neighbour.SetG(CostToNeighbour);            // Update its G cost
                    neighbour.SetConnection(current);          // Set its parent to the current node

                    // If the neighbor wasn't in the open list, calculate its H cost and add it to the open list
                    if(!inSearch)
                    {
                        neighbour.SetH(neighbour.GetDistance(targetNode));
                        toSearch.Add(neighbour);
                        neighbour.SetColor(OpenColor);         // Color the node to indicate that it's in the open list
                    }
                }
            }
        }
        // If no path is found, return null
        return null;
    }
}
