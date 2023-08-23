using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Grid;

namespace Scripts.Hexgrid
{
    public class HexNode : BaseNode
    {
        // Override the GetNeighbours method from the BaseNode class
        public override void GetNeighbours()
        {
            // Retrieves all nodes from the GridManager singleton instance where the distance to the current node is 1.
            Neighbours = GridManager.Instance.Nodes
                            .Where(t => Coordinates.GetDistance(t.Value.Coordinates) == 1)
                            .Select(t => t.Value)
                            .ToList();
        }
    }

    // Defining HexGridCoordinates struct that implements ICoords interface.
    // The struct defines coordinates for a HexGrid system, which are often defined using axial or cubic systems.
    public struct HexGridCoordinates : ICoords
    {
        // The axial coordinates q and r
        private readonly int _q;
        private readonly int _r;

        // Constructor for the HexGridCoordinates struct
        public HexGridCoordinates(int q, int r)
        {
            _q = q;
            _r = r;
            
            // Calculate the position using the axial coordinates
            Position = _q * new Vector2(sqrt3, 0) + _r * new Vector2(sqrt3 / 2, 1.5f);
        }

        // Implementing the GetDistance method from the ICoords interface
        public float GetDistance(ICoords other) => (this - (HexGridCoordinates)other).AxialLength();

        // Cached value for the square root of 3 to avoid recalculating it multiple times.
        private static readonly float sqrt3 = Mathf.Sqrt(3);

        // Property to get and set the position of the coordinates.
        public Vector2 Position { get; set; }

        // Method to calculate the axial length which helps in finding the distance between two hexes.
        private int AxialLength()
        {
            // Various conditions to compute the axial length based on q and r values.
            if (_q == 0 && _r == 0) return 0;
            if (_q > 0 && _r >= 0) return _q + _r;
            if(_q <= 0 && _r > 0) return -_q < _r ? _r : -_q;
            if (_q < 0) return -_q - _r;
            return -_r > _q ? -_r : _q;
        }

        // Operator overloading for subtraction to find the difference between two HexGridCoordinates.
        public static HexGridCoordinates operator -(HexGridCoordinates a, HexGridCoordinates b)
        {
            return new HexGridCoordinates(a._q - b._q, a._r - b._r);
        }
    }
}
