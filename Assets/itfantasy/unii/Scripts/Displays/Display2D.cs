using UnityEngine;
using System.Collections;

namespace itfantasy.unii
{
    public class Display2D : MonoBehaviour, IDisplay2D
    {
        [HideInInspector]
        [SerializeField]
        Vector3 _position;

        public Vector3 position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                this.gameObject.transform.position = new Vector3(value.x / 100, value.y / 100, value.z);
            }
        }

        public Vector2 position2D
        {
            get
            {
                return new Vector2(position.x, position.y);
            }
            set
            {
                position = new Vector3(value.x, value.y, position.z);
            }
        }

        public float x
        {
            get
            {
                return position.x;
            }
            set
            {
                position = new Vector3(value, position.y, position.z);
            }
        }

        public float y
        {
            get
            {
                return position.y;
            }
            set
            {
                position = new Vector3(position.x, value, position.z);
            }
        }

        public float z
        {
            get
            {
                return position.z;
            }
            set
            {
                position = new Vector3(position.x, position.y, value);
            }
        }

        [HideInInspector]
        [SerializeField]
        Vector2 _scale;

        public Vector2 scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                this.gameObject.transform.localScale = new Vector3(_scale.x, _scale.y, this.gameObject.transform.localScale.z);
            }
        }

        public float scaleX
        {
            get
            {
                return scale.x;
            }
            set
            {
                scale = new Vector2(value, scale.y);
            }
        }

        public float scaleY
        {
            get
            {
                return scale.y;
            }
            set
            {
                scale = new Vector2(scale.x, value);
            }
        }

        [HideInInspector]
        [SerializeField]
        Vector3 _rotation;

        public Vector3 rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                this.gameObject.transform.rotation
                    = new Quaternion(_rotation.x, _rotation.y, _rotation.z,
                        this.gameObject.transform.rotation.w);
            }
        }

        public float rotationX
        {
            get
            {
                return rotation.x;
            }
            set
            {
                rotation = new Vector3(value, rotation.y, rotation.z);
            }
        }

        public float rotationY
        {
            get
            {
                return rotation.y;
            }
            set
            {
                rotation = new Vector3(rotation.x, value, rotation.z);
            }
        }

        public float rotationZ
        {
            get
            {
                return rotation.z;
            }
            set
            {
                rotation = new Vector3(rotation.x, rotation.y, value);
            }
        }

        public Color color { get; set; }
       
        public float alpha { get; set; }
        
        public string udata = "";

        void Reset()
        {
            UpdateTransform();
        }

        void Awake()
        {
            Reset();
        }

        public void UpdateTransform()
        {
            Vector3 pos = this.gameObject.transform.position;
            position = new Vector3(pos.x * 100, pos.y * 100, pos.z);
            Vector3 sc = this.gameObject.transform.localScale;
            scale = new Vector2(sc.x, sc.y);
            Quaternion rt = this.gameObject.transform.rotation;
            rotation = new Vector3(rt.x, rt.y, rt.z);
        }
    }
}
