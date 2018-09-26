using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace itfantasy.unii
{
    public class Sprite2D : Display2D
    {
        
        [HideInInspector]
        [SerializeField] 
        Vector2 _size;
        
        public Vector2 size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                if (sprite != null)
                {
                    scale = new Vector2(value.x / sprite.rect.width, value.y / sprite.rect.height);
                }
            }
        }

        public float width
        {
            get
            {
                return size.x;
            }
            set
            {
                size = new Vector2(value, size.y);
            }
        }

        public float height
        {
            get
            {
                return size.y;
            }
            set
            {
                size = new Vector2(size.x, value);
            }
        }

        SpriteRenderer _renderer;
        public SpriteRenderer renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = this.GetComponent<SpriteRenderer>();
                }
                return _renderer;
            }
        }
        public Sprite sprite
        {
            get
            {
                return renderer.sprite;
            }
            set
            {
                renderer.sprite = value;
            }
        }

        public int sortingOrder
        {
            get { return (renderer != null) ? renderer.sortingOrder : 0; }
            set { if (renderer != null && renderer.sortingOrder != value) renderer.sortingOrder = value; }
        }

        public string sortingLayer
        {
            get { return (renderer != null) ? renderer.sortingLayerName : "default"; }
            set
            {
                if (renderer != null && renderer.sortingLayerName != value)
                {
                    renderer.sortingLayerName = value;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
                }
            }
        }

        // Use this for initialization
        void Reset()
        {
            UpdateSize();
            UpdateTransform();
        }

        void Awake()
        {
            Reset();
        }

        public void UpdateSize()
        {
            Vector3 localScale = this.gameObject.transform.localScale;
            if (sprite != null)
            {
                size = new Vector2(localScale.x * sprite.rect.width, localScale.y * sprite.rect.height);
            }
        }
    }

    public enum Pivot
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight,
    }
}
