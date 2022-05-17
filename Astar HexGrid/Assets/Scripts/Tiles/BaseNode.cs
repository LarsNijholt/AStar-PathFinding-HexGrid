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
        [SerializeField] private Color _barrierColor;
        [SerializeField] private Gradient _walkableColor;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        public Coords Coordinates;
        public float GetDistance(BaseNode other) => Coordinates.GetDistance(other.Coordinates); // Helper to reduce noise in pathfinding.
        public bool Walkable { get; private set; }
        private bool _selected;
        private Color _defaultColor;

        public virtual void Setup(bool walkable, Coords coordinates)
        {
            Walkable = walkable;
            _spriteRenderer.color = walkable ? _walkableColor.Evaluate(Random.Range(0f, 1f)) : _barrierColor;
            _defaultColor = _spriteRenderer.color;

            OnAboveNode += DefinetelyAboveNode;

            Coordinates = coordinates;
            transform.position = Coordinates.Position;
        }

        public static event Action<BaseNode> OnAboveNode;
        private void OnActivate() => OnAboveNode += DefinetelyAboveNode;
        private void OnDeactivate() => OnAboveNode -= DefinetelyAboveNode;
        private void DefinetelyAboveNode(BaseNode selectedNode) => _selected = selectedNode == this;

        protected virtual void OnMouseDown()
        {
            if (!Walkable) return;
            OnAboveNode?.Invoke(this);
        }

        public List<BaseNode> Neighbours { get; protected set; }
        public BaseNode Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public abstract void GetNeighbours();

        public void SetConnection(BaseNode baseNode)
        {
            Connection = baseNode;
        }

        public void SetG(float g)
        {
            G = g;
        }
        public void SetH(float h)
        {
            H = h;
        }

        public void SetColor(Color color) => _spriteRenderer.color = color;

        public void RevertNode()
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}

public interface Coords
{
    public float GetDistance(Coords other);
    public Vector2 Position { get; set; }
}