using System.Collections;
using Scripts.Hexgrid;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Scriptables
{
    public abstract class ScriptableBaseGrid : ScriptableObject
    {
        [SerializeField] protected BaseNode baseNodeprefab;
        [SerializeField, Range(0, 6)] private int _barrierWeight = 3;
        public abstract Dictionary<Vector2, BaseNode> GenerateGrid();

        protected bool MakeObstacle() => Random.Range(1, 20) > _barrierWeight;
    } 
}
