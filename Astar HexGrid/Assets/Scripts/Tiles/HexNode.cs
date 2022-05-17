using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Grid;

namespace Scripts.Hexgrid
{
    public class HexNode : BaseNode
    {
        public override void GetNeighbours()
        {
            Neighbours = GridManager.Instance.Nodes.Where(t => Coordinates.GetDistance(t.Value.Coordinates) == 1).Select(t=> t.Value).ToList();
        }
    }

    public struct HexGridCoordinates : Coords
    {
        private readonly int _q;
        private readonly int _r;
        public HexGridCoordinates(int q, int r)
        {
            _q = q;
            _r = r;
            Position = _q * new Vector2(sqrt3, 0) + _r * new Vector2(sqrt3 / 2, 1.5f);
        }

        public float GetDistance(Coords other) => (this - (HexGridCoordinates)other).AxialLength();

        private static readonly float sqrt3 = Mathf.Sqrt(3);

        public Vector2 Position { get; set; }

        private int AxialLength()
        {
            if (_q == 0 && _r == 0) return 0;
            if (_q > 0 && _r >= 0) return _q + _r;
            if(_q <= 0 && _r > 0) return -_q < _r ? _r : -_q;
            if (_q < 0) return -_q - _r;
            return -_r > _q ? -_r : _q;
        }

        public static HexGridCoordinates operator -(HexGridCoordinates a, HexGridCoordinates b)
        {
            return new HexGridCoordinates(a._q - b._q, a._r - b._r);
        }
    }
}
