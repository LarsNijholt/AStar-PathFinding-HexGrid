using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Hexgrid;
using Scripts.Scriptables;
using Scripts.gridsprite;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Grid
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        [SerializeField] private Sprite _playerSprite, _goalSprite;
        [SerializeField] private GridSprite _spritePrefab;
        [SerializeField] private ScriptableBaseGrid _grid;
        [SerializeField] private bool _drawConnections;

        public Dictionary<Vector2, BaseNode> Nodes { get; private set; }

        private BaseNode _playerBaseNode, _goalNode;
        private GridSprite _player, _goal;

        private void Awake() => Instance = this;

        private void Start()
        {
            Nodes = _grid.GenerateGrid();

            foreach (var node in Nodes.Values) node.GetNeighbours();
            SpawnGridSprites();
            BaseNode.OnAboveNode += OnAboveNode;
        }

        private void OnDestroy() => BaseNode.OnAboveNode -= OnAboveNode;
        

        private void OnAboveNode(BaseNode baseNode)
        {
            _goalNode = baseNode;
            _goal.transform.position = _goalNode.Coordinates.Position;

            foreach (var n in Nodes.Values) n.RevertNode();

            var path = Pathfinding.FindPath(_playerBaseNode, _goalNode);
        }

        void SpawnGridSprites()
        {
            _playerBaseNode = Nodes.Where(t => t.Value.Walkable).OrderBy(t => Random.value).First().Value;
            _player = Instantiate(_spritePrefab, _playerBaseNode.Coordinates.Position, Quaternion.identity);
            _player.Setup(_playerSprite);

            _goal = Instantiate(_spritePrefab, new Vector3(50,50,50), Quaternion.identity);
            _goal.Setup(_goalSprite);
        }

        public BaseNode GetNodeAtLocation(Vector2 position) => Nodes.TryGetValue(position, out var node) ? node : null;

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying || !_drawConnections) return;
            Gizmos.color = Color.red;
            foreach (var node in Nodes)
            {
                if(node.Value.Connection == null) continue;
                Gizmos.DrawLine((Vector3)node.Key + new Vector3(0, 0, -1), (Vector3)node.Value.Connection.Coordinates.Position + new Vector3(0,0,-1));
            }
        }
    }
}
