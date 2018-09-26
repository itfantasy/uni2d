using UnityEngine;
using System.Collections;

namespace itfantasy.unii
{
    /// <summary>
    /// 2D视口摄影机
    /// </summary>
    public class Camera2D : Display2D
    {
        public static Camera2D curCamera2D;
        
        Camera _mainCamera;
        public Camera mainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = this.gameObject.GetComponent<Camera>();
                }
                return _mainCamera;
            }
        }

        [HideInInspector]
        [SerializeField]
        Vector2 _viewSize = new Vector2(800, 480);
        
        public Vector2 viewSize
        {
            get
            {
                return _viewSize;
            }
            set
            {
                _viewSize = value;
                Reset();
            }
        }

        public Color color
        {
            get
            {
                return mainCamera.backgroundColor;
            }
            set
            {
                mainCamera.backgroundColor = value;
            }
        }

        void Reset()
        {
            curCamera2D = this;
            mainCamera.orthographicSize = _viewSize.y / 200.0f;
            UpdateTransform();
        }

        void Awake()
        {
            mainCamera.orthographicSize = _viewSize.y / 200.0f;
            UpdateTransform();
        }

        public Sprite2D RayHitSprite()
        {
            Sprite2D target = null;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.collider.gameObject.GetComponent<Sprite2D>();
            }
            return target;
        }

        #region -----------------> 牵引式摄影机实现

        public bool draging = false;
        public Sprite2D mainSprite;
        public float areaWidth;
        public float areaHeight;

        void LateUpdate()
        {
            if (draging && mainSprite != null)
            {
                if (!CheckSideY())
                {
                    this.y = mainSprite.y;
                }
                if (!CheckSideX())
                {
                    this.x = mainSprite.x;
                }
            }
        }

        private bool CheckSideX()
        {

            if (mainSprite.x < -areaWidth / 2 + viewSize.x / 2)
            {
                this.x = -areaWidth / 2 + viewSize.x / 2;
                return true;
            }
            else if (mainSprite.x > areaWidth / 2 - viewSize.x / 2)
            {
                this.x = areaWidth / 2 - viewSize.x / 2;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckSideY()
        {

            if (mainSprite.y < -areaHeight / 2 + viewSize.y / 2)
            {
                this.y = -areaHeight / 2 + viewSize.y / 2;
                return true;
            }
            else if (mainSprite.y > areaHeight / 2 - viewSize.y / 2)
            {
                this.y = areaHeight / 2 - viewSize.y / 2;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}


