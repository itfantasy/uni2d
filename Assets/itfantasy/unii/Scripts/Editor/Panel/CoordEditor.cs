using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace itfantasy.unii
{
    [CustomEditor(typeof(Coord2D))]
    [ExecuteInEditMode]
    public class CoordEditor : Editor
    {

        #region ==================> 编辑面板

        public override void OnInspectorGUI()
        {
            Coord2D coord = (Coord2D)target;
            coord.UpdateTransform();
            coord.UpdateRotation();

            coord.position = EditorGUILayout.Vector3Field("位 置", coord.position);
            //coord.size = EditorGUILayout.Vector2Field("尺 寸", coord.size);
            coord.alpha = EditorGUILayout.Slider("透明度", coord.alpha, 0.0f, 1.0f);

            if (NGUIEditorTools.DrawHeader("单 元 格 设 定"))
            {
                NGUIEditorTools.BeginContents();
                GUILayout.BeginVertical();
                
                coord.gridSize = EditorGUILayout.FloatField("尺 寸", coord.gridSize);
                coord.gridAngel = EditorGUILayout.FloatField("角 度", coord.gridAngel);
                coord.gridRotation = EditorGUILayout.FloatField("旋 转", coord.gridRotation);
                coord.gridArea = EditorGUILayout.FloatField("范 围", coord.gridArea);
                
                GUILayout.EndVertical();
                NGUIEditorTools.EndContents();
            }

            if (NGUIEditorTools.DrawHeader("地 形 绘 制"))
            {
                NGUIEditorTools.BeginContents();

                GUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("选择笔刷");

                Coord2D.curBrushIndex = EditorGUILayout.Popup(Coord2D.curBrushIndex, options);

                EditorGUILayout.ColorField(Coord2D.curBrush.color);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("新 增"))
                {
                    BrushWindow window = BrushWindow.GetInstance();
                    window.title = "新增笔刷";
                    window.mod = 0;
                    window.Input(new BrushVo());
                    window.ShowPopup();
                }
                if (GUILayout.Button("编 辑"))
                {
                    BrushWindow window = BrushWindow.GetInstance();
                    window.title = "编辑笔刷";
                    window.mod = 1;
                    window.Input(Coord2D.curBrush);
                    window.ShowPopup();
                }
                if (GUILayout.Button("删 除"))
                {
                    Coord2D.DelBrush();
                    UpdateBrushOptions();
                    Coord2D.SaveBrushInfo();
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();

                NGUIEditorTools.EndContents();
            }

            EditorGUILayout.LabelField("场景视图按Q键进入拖动状态：");
            EditorGUILayout.LabelField("【单击鼠标左键编辑地形】【单击鼠标右键编辑事件】【再次单击取消】");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清空地形数据"))
            {
                if (EditorUtility.DisplayDialog("清空地形数据", "确定清空全部地形数据吗？（该操作不可撤销！！）", "确定", "取消"))
                {
                    coord.ClearTerrain();
                }
            }

            if (GUILayout.Button("清空事件数据"))
            {
                if (EditorUtility.DisplayDialog("清空事件数据", "确定清空全部事件数据吗？（该操作不可撤销！！）", "确定", "取消"))
                {
                    coord.ClearTerrain();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            coord.udata = EditorGUILayout.TextField("用户数据", coord.udata);

            if (coord.needSaveTerrain)
            {
                if (GUILayout.Button("保存场景"))
                {
                    coord.SaveTerrain();
                    EditorApplication.SaveScene();
                }
            }

        }

        #endregion

        #region ==================> 场景扩展

        Vector2 mousePos = Vector2.zero;
        void OnSceneGUI()
        {
            Coord2D coord = (Coord2D)target;
            DrawGrid();
            DrawTerrains();
            Event e = Event.current;
            if (e.isMouse)
            {
                if (e.button == 0 && e.clickCount == 1)
                {
                    if (e.type == EventType.mouseDown)
                    {
                        mousePos = e.mousePosition;
                    }
                    if (e.type == EventType.mouseUp)
                    {
                        if (mousePos == e.mousePosition)
                        {
                            // 左键单击设定或者取消地形
                            coord.SetTerrain(currentCoord);
                        }
                    }
                }

                if (e.button == 1 && e.clickCount == 1 && e.type == EventType.mouseUp)
                {
                    // 右键单击设定事件
                    Debug.Log("设定事件!");
                }
            }
            if (e.isKey)
            {
                if (e.control)
                {
                    if (e.type == EventType.keyUp && e.keyCode == KeyCode.S)
                    {
                        coord.SaveTerrain();
                        EditorApplication.SaveScene();
                    }
                }
            }

        }

        /// <summary>
        /// 场景编辑视窗中的游戏坐标
        /// </summary>
        Vector2 currentCoord
        {
            get
            {
                Coord2D coord = (Coord2D)target;
                Vector2 pos = Event.current.mousePosition;
                return coord.WorldToCoordPoint(
                    HandleUtility.GUIPointToWorldRay(pos).GetPoint(10),
                    coord.gridSize,
                    coord.gridAngel
                );
            }
        }
        
        void DrawGrid()
        {
            Coord2D coord = (Coord2D)target;
            Color color = Color.blue;
            color.a = 0.8f;
            Handles.color = color;
            float gridMaxSize = coord.gridArea * 2 - 1;
            for (int i = 0; i <= gridMaxSize; i++)
            {
                Vector3 horizontalStart = coord.CoordToWorldPoint(
                    new Vector2(i - gridMaxSize / 2, -gridMaxSize/2),
                    coord.gridSize, coord.gridAngel, -9
                );
                Vector3 horizontalEnd = coord.CoordToWorldPoint(
                    new Vector2(i - gridMaxSize / 2, gridMaxSize/2),
                    coord.gridSize, coord.gridAngel, -9
                );
                Handles.DrawLine(horizontalStart, horizontalEnd);

                Vector3 VerticalStart = coord.CoordToWorldPoint(
                    new Vector2(-gridMaxSize / 2, i - gridMaxSize/2),
                    coord.gridSize, coord.gridAngel, -9
                );
                Vector3 VerticalEnd = coord.CoordToWorldPoint(
                    new Vector2(gridMaxSize / 2, i - gridMaxSize/2),
                    coord.gridSize, coord.gridAngel, -9
                );
                Handles.DrawLine(VerticalStart, VerticalEnd);

            }
        }

        List<Vector2> dirtyCoords = new List<Vector2>();

        void DrawTerrains()
        {
            dirtyCoords.Clear();
            Coord2D coord = (Coord2D)target;
            foreach (KeyValuePair<Vector2, int> kv in coord.terrain)
            {
                BrushVo vo = coord.GetTerrain(kv.Key);
                if (vo != null)
                {
                    DrawTerrain(kv.Key, vo.color);
                }
                else
                {
                    dirtyCoords.Add(kv.Key);
                }
            }
            foreach (Vector2 v2 in dirtyCoords)
            {
                coord.terrain.Remove(v2);
            }
        }

        void DrawTerrain(Vector2 v2, Color color)
        {
            Handles.color = Color.white;
            Coord2D coord = (Coord2D)target;
            Color face = new Color(color.r, color.g, color.b, color.a * 0.5f);
            float x = v2.x; float y = v2.y;
            Vector3[] verts = new Vector3[] { 
                coord.CoordToWorldPoint(new Vector2(x-0.5f,y-0.5f),coord.gridSize,coord.gridAngel,-9),
                coord.CoordToWorldPoint(new Vector2(x+0.5f,y-0.5f),coord.gridSize,coord.gridAngel,-9),
                coord.CoordToWorldPoint(new Vector2(x+0.5f,y+0.5f),coord.gridSize,coord.gridAngel,-9),
                coord.CoordToWorldPoint(new Vector2(x-0.5f,y+0.5f),coord.gridSize,coord.gridAngel,-9)
            };
            Handles.DrawSolidRectangleWithOutline(verts, face, color);
        }

        #endregion

        static string[] _options;
        static string[] options
        {
            get
            {
                if (_options == null)
                {
                    _options = GetBrushName().ToArray();
                }
                return _options;
            }
        }

        static List<string> GetBrushName()
        {
            List<string> ret = new List<string>();
            foreach (BrushVo brush in Coord2D.brushList)
            {
                ret.Add(brush.name);
            }
            return ret;
        }

        public static void UpdateBrushOptions()
        {
            _options = GetBrushName().ToArray();
        }
        
    }
}

