using System.Collections;
using System.Collections.Generic;
using Scripts.Hexgrid;
using UnityEngine;

namespace Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Add Scriptable Hex Grid")]
    public class ScriptableHexGrid : ScriptableBaseGrid
    {
        [SerializeField, Range(1, 50)] private int _gridWidth = 16;
        [SerializeField, Range(1, 50)] private int _gridHeight = 9;

        public override Dictionary<Vector2, BaseNode> GenerateGrid()
        {
            var nodes = new Dictionary<Vector2, BaseNode>();
            var grid = new GameObject
            {
                name = "Grid"
            };
            for (int r = 0; r < _gridHeight; r++)
            {
                var rOffset = r >> 1;
                for(var q = -rOffset; q < _gridWidth - rOffset; q++)
                {
                    var node = Instantiate(baseNodeprefab, grid.transform);
                    node.Setup(MakeObstacle(), new HexGridCoordinates(q, r));
                    nodes.Add(node.Coordinates.Position, node);
                }
            }

            return nodes;
        }
    }
}
