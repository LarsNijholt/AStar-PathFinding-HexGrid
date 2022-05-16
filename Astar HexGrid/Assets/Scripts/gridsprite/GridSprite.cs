using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.gridsprite
{
    public class GridSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Setup(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}