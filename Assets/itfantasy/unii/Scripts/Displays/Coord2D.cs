using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using itfantasy.lmjson;
using itfantasy.unii.utils;

namespace itfantasy.unii
{
    /// <summary>
    /// 2D场景
    /// 
    /// 2D场景变成2D坐标系
    /// 剔除有关sprite的相关属性，不再关注图像处理
    /// 拥有独立的摄影机，仅用于计算坐标转换使用（某些属性需要与主2d摄影机联动）
    /// 
    /// </summary>
    public class Coord2D : Display2D
    {
        public static Coord2D curCoord2D;

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
        float _gridSize;

        public float gridSize
        {
            get
            {
                return _gridSize;
            }
            set
            {
                _gridSize = value;
            }
        }

        [HideInInspector]
        [SerializeField]
        float _gridAngel;

        public float gridAngel
        {
            get
            {
                return _gridAngel;
            }
            set
            {
                _gridAngel = value;
            }
        }

        [HideInInspector]
        [SerializeField]
        float _gridRotation;

        public float gridRotation
        {
            get
            {
                return _gridRotation;
            }
            set
            {
                _gridRotation = value;
                this.gameObject.transform.rotation = new Quaternion(
                    this.gameObject.transform.rotation.x, 
                    this.gameObject.transform.rotation.y,
                    (float)(_gridRotation * System.Math.PI / 360.0f), 
                    this.gameObject.transform.rotation.w);
            }
        }

        [HideInInspector]
        [SerializeField]
        float _gridArea;

        public float gridArea
        {
            get
            {
                return _gridArea;
            }
            set
            {
                _gridArea = value;
            }
        }

        [HideInInspector]
        [SerializeField]
        string _terrainText = "";

        public bool needSaveTerrain = false;
        Dictionary<Vector2, int> _terrain = null;

        public Dictionary<Vector2, int> terrain
        {
            get
            {
                if (_terrain == null)
                {
                    _terrain = new Dictionary<Vector2, int>();
                    if (_terrainText != "")
                    {
                        JsonData json = JsonUtil.Decode(_terrainText);
                        foreach (string key in json.Keys)
                        {
                            _terrain.Add(Comm.Vector2Parse(key), json[key].ToInt());
                        }
                    }
                }
                return _terrain;
            }
        }

        public BrushVo GetTerrain(Vector2 coord)
        {
            int id = _terrain[coord];
            if (_brushDict.ContainsKey(id))
            {
                return _brushDict[id];
            }
            return null;
        }

        public void SetTerrain(Vector2 coord)
        {
            if (terrain.ContainsKey(coord))
            {
                if (terrain[coord] != curBrush.id)
                {
                    terrain[coord] = curBrush.id;
                }
                else
                {
                    terrain.Remove(coord);
                }
            }
            else
            {
                terrain.Add(coord, curBrush.id);
            }
            needSaveTerrain = true;
        }

        public void SaveTerrain()
        {
            if (needSaveTerrain)
            {
                if (_terrain != null && _terrain.Count > 0)
                {
                    _terrainText = JsonUtil.Encode(_terrain);
                }
                else
                {
                    _terrainText = "{}";
                }
                Debug.Log("地形数据已保存:" + _terrainText);
                needSaveTerrain = false;
            }
        }

        public void ClearTerrain()
        {
            _terrain.Clear();
            needSaveTerrain = true;
        }

        #region =============> 2D场景地形刷

        static List<BrushVo> _brushList;
        static Dictionary<int, BrushVo> _brushDict = new Dictionary<int, BrushVo>();

        public static void AddBrush(BrushVo brush)
        {
            _brushDict.Add(brush.id, brush);
            _brushList.Add(brush);
            curBrushIndex = _brushList.Count - 1;
        }

        public static void DelBrush()
        {
            _brushDict.Remove(curBrush.id);
            _brushList.RemoveAt(curBrushIndex);
            curBrushIndex = 0;
        }
        
        public static List<BrushVo> brushList
        {
            get
            {
                if (_brushList == null || _brushList.Count < 2)
                {
                    _brushList = new List<BrushVo>();
                    LoadBrushInfo();
                    if (_brushList.Count == 0)
                    {
                        BrushVo block = new BrushVo();
                        block.id = 1;
                        block.name = "障碍";
                        block.color = Color.red;
                        block.type = 1;
                        _brushList.Add(block);
                        _brushDict.Add(block.id, block);

                        BrushVo mask = new BrushVo();
                        mask.id = 2;
                        mask.name = "遮挡";
                        mask.color = Color.white;
                        mask.type = 2;
                        _brushList.Add(mask);
                        _brushDict.Add(mask.id, mask);

                        maxBrushIndex = 3;
                    }
                }

                return _brushList;
            }
        }
        public static int curBrushIndex = 0;
        public static int maxBrushIndex = 0;
        public static BrushVo curBrush
        {
            get
            {
                return brushList[curBrushIndex];
            }
        }

        public static void SaveBrushInfo()
        {
            JsonData array = new JsonData();
            foreach (BrushVo brush in _brushList)
            {
                array.Add(brush.Serialize());
            }
            string path = Application.dataPath + "/Resources/Brushs.bytes";
            FileIO.CreateFile(path, array.ToJson());
        }

        public static void LoadBrushInfo()
        {
            TextAsset bytes = (TextAsset)Resources.Load("Brushs");
            if (bytes != null)
            {
                JsonData array = JsonUtil.Decode(bytes.text);
                for (int i = 0; i < array.Count; i++)
                {
                    BrushVo brush = new BrushVo();
                    brush.Initialize(array[i]);
                    _brushList.Add(brush);
                    if (!_brushDict.ContainsKey(brush.id))
                    {
                        _brushDict.Add(brush.id, brush);
                        if (maxBrushIndex < brush.id)
                        {
                            maxBrushIndex = brush.id;
                        }
                    }
                }
            }
        }

        #endregion

        private Vector2 _viewSize = new Vector2(1280, 720);

        /// <summary>
        /// 屏幕坐标转2D坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2 ScreenToP2DPoint(Vector3 pos)
        {
            Vector3 vp = mainCamera.ScreenToViewportPoint(pos);
            return new Vector2(
                vp.x * _viewSize.x - _viewSize.x / 2,
                vp.y * _viewSize.y - _viewSize.y / 2
            );
        }

        /// <summary>
        /// 2D坐标系到屏幕坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 P2DToScreenPoint(Vector2 pos, float z)
        {
            Vector3 vp = new Vector3((pos.x + _viewSize.x / 2) / _viewSize.x, (pos.y + _viewSize.y / 2) / _viewSize.y, z);
            Vector3 des = mainCamera.ViewportToScreenPoint(vp);
            des.z = z;
            return des;
        }

        /// <summary>
        /// 世界坐标转2D坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 WorldToP2DPoint(Vector3 pos)
        {
            Vector3 vp = mainCamera.WorldToViewportPoint(pos);
            return new Vector2(
                vp.x * _viewSize.x - _viewSize.x / 2,
                vp.y * _viewSize.y - _viewSize.y / 2
            );
        }

        /// <summary>
        /// 2D坐标系到世界坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 P2DToWorldPoint(Vector2 pos, float z)
        {
            Vector3 vp = new Vector3((pos.x + _viewSize.x / 2) / _viewSize.x, (pos.y + _viewSize.y / 2) / _viewSize.y, z);
            Vector3 des = mainCamera.ViewportToWorldPoint(vp);
            des.z = z;
            return des;
        }

        /// <summary>
        /// 2D坐标系到游戏坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="gridSize"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Vector2 P2DToCoordPoint(Vector2 pos, float gridSize, float angle)
        {
            Vector2 ret;
            if (angle == 0)
            {
                ret = new Vector2((int)(pos.x / gridSize), (int)(pos.y / gridSize));
            }
            else
            {
                float radian = GetRadian(angle);
                ret = new Vector2(
                    Mathf.Round((float)((pos.y / (2 * System.Math.Cos(radian)) + pos.x / (2 * System.Math.Sin(radian))) / gridSize)),
                    Mathf.Round((float)((pos.y / (2 * System.Math.Cos(radian)) - pos.x / (2 * System.Math.Sin(radian))) / gridSize))
                );
            }
            return ret;
        }

        /// <summary>
        /// 游戏坐标到2D坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="gridSize"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Vector2 CoordToP2DPoint(Vector2 pos, float gridSize, float angle)
        {
            Vector2 ret;
            if (angle == 0)
            {
                ret = new Vector2(pos.x * gridSize, pos.y * gridSize);
            }
            else
            {
                double radian = GetRadian(angle);
                ret = new Vector2(
                    (float)((pos.x - pos.y) * System.Math.Sin(radian) * gridSize),
                    (float)((pos.x + pos.y) * System.Math.Cos(radian) * gridSize)
                );
            }
            return ret;
        }

        /// <summary>
        /// 游戏坐标到世界坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="gridSize"></param>
        /// <param name="angle"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector3 CoordToWorldPoint(Vector2 pos, float gridSize, float angle, float z)
        {
            return P2DToWorldPoint(CoordToP2DPoint(pos, gridSize, angle), z);
        }

        /// <summary>
        /// 世界坐标系到游戏坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="gridSize"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Vector2 WorldToCoordPoint(Vector3 pos, float gridSize, float angle)
        {
            return P2DToCoordPoint(WorldToP2DPoint(pos), gridSize, angle);
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float GetRadian(float angle)
        {
            return (float)(angle * System.Math.PI / 180.0f);
        }

        // Use this for initialization
        void Reset()
        {
            curCoord2D = this;
            UpdateTransform();
            mainCamera.orthographicSize = _viewSize.y / 200.0f;
        }

        void Awake()
        {
            _brushList = new List<BrushVo>();
            LoadBrushInfo();
            Reset();
        }

        public void UpdateRotation()
        {
            _gridRotation = (float)(this.gameObject.transform.rotation.z * 360.0f / System.Math.PI);
        }

    }
}
