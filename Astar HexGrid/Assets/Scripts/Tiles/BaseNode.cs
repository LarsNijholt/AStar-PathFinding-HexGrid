using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Scripts.Hexgrid
{
    public abstract class BaseNode : MonoBehaviour
    {
        [Header("References")]

        [SerializeField] private Color _barrierColor; // Color for non-walkable nodes
        [SerializeField] private Gradient _walkableColor; // Color gradient for walkable nodes
        [SerializeField] protected SpriteRenderer _spriteRenderer; // SpriteRenderer for this node

        // Coordinates of the node
        public ICoords Coordinates; 
        // Method to get distance between two nodes, it delegates to the Coordinates method to get the actual distance
        public float GetDistance(BaseNode other) => Coordinates.GetDistance(other.Coordinates);
        public bool Walkable { get; private set; } // Indicates if this node is walkable
        private bool _selected; // Whether the node is selected
        private Color _defaultColor; // Default color of the node

        // setup the node
        public void Setup(bool walkable, ICoords coordinates)
        {
            Walkable = walkable;
            // Sets color based on whether node is walkable or not
            _spriteRenderer.color = walkable ? _walkableColor.Evaluate(Random.Range(0f, 1f)) : _barrierColor;
            _defaultColor = _spriteRenderer.color; // Save default color

            OnAboveNode += DefinetelyAboveNode; // Subscribe to OnAboveNode event

            Coordinates = coordinates; // Set node's coordinates
            transform.position = Coordinates.Position; // Set the position of the node in the world
        }

        // Declaring a static event, which will be triggered when the mouse is over a node
        public static event Action<BaseNode> OnAboveNode;
        // Subscribing and unsubscribing to the event when the node is enabled or disabled
        private void OnEnable() => OnAboveNode += DefinetelyAboveNode;
        private void OnDisable() => OnAboveNode -= DefinetelyAboveNode;
        // Method to determine if the node is selected
        private void DefinetelyAboveNode(BaseNode selectedNode) => _selected = selectedNode == this;

        // Virtual method that is called when the node is clicked
        protected virtual void OnMouseDown()
        {
            if (!Walkable) return; // If node is not walkable, do nothing
            OnAboveNode?.Invoke(this); // Invoke OnAboveNode event
        }

        // Public properties related to pathfinding
        public List<BaseNode> Neighbours { get; protected set; } // Neighboring nodes
        public BaseNode Connection { get; private set; } // The node's parent in a path
        public float G { get; private set; } // Cost from the start node
        public float H { get; private set; } // Estimated cost to target
        public float F => G + H; // Total cost

        // Abstract method that must be implemented by subclasses to determine neighboring nodes
        public abstract void GetNeighbours();

        // Methods to set the values of Connection, G, and H properties
        public void SetConnection(BaseNode baseNode) => Connection = baseNode;
        public void SetG(float g) => G = g;
        public void SetH(float h) => H = h;

        // Method to set the color of the node
        public void SetColor(Color color) => _spriteRenderer.color = color;

        // Method to revert the node's color to its default
        public void RevertNode() => _spriteRenderer.color = _defaultColor;
    }
}

public interface ICoords
{
    public float GetDistance(ICoords other);
    public Vector2 Position { get; set; }
}