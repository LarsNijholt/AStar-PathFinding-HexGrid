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

        [Header("Pathfinding")]
        [SerializeField] private TextMeshPro _fCostText;

        [SerializeField] private TextMeshPro _gCostText, _hCostText;

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
            SetText();
        }
        public void SetH(float h)
        {
            H = h;
            SetText();
        }

        private void SetText()
        {
            if (_selected) return;
            _gCostText.text = G.ToString();
            _hCostText.text = H.ToString();
            _fCostText.text = F.ToString();
        }

        public void SetColor(Color color) => _spriteRenderer.color = color;

        public void RevertNode()
        {
            _spriteRenderer.color = _defaultColor;
            _gCostText.text = "";
            _hCostText.text = "";
            _fCostText.text = "";
        }
    }
}

public interface Coords
{
    public float GetDistance(Coords other);
    public Vector2 Position { get; set; }
}