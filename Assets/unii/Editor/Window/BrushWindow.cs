using UnityEngine;
using UnityEditor;
using System.Collections;

namespace itfantasy.unii
{
    public class BrushWindow : EditorWindow
    {
        private static BrushWindow _instance;
        public static BrushWindow GetInstance()
        {
            if (_instance == null)
            {
                _instance = (BrushWindow)EditorWindow.GetWindow(typeof(BrushWindow));
            }
            _instance.minSize = new Vector2(300, 200);
            _instance.maxSize = new Vector2(300, 250);
            return _instance;
        }

        BrushVo vo = new BrushVo();

        void OnInspectorUpdate() 
        {
		    Repaint();
	    }

	    void OnGUI() 
        {    
            vo.name = EditorGUILayout.TextField("名 称", vo.name);
            vo.color = EditorGUILayout.ColorField("颜 色", vo.color);
            vo.type = EditorGUILayout.IntField("类 型", vo.type);
            vo.data = EditorGUILayout.TextField("数 据", vo.data);
            if (GUILayout.Button("确 定"))
            {
                if (mod == 0)
                {
                    vo.id = Coord2D.maxBrushIndex++;
                    Coord2D.AddBrush(Output());
                    CoordEditor.UpdateBrushOptions();
                }
                else
                {
                    CoordEditor.UpdateBrushOptions();
                }
                Coord2D.SaveBrushInfo();
                this.Close();
            }
        }

        public int mod = 0;

        public void Input(BrushVo vo)
        {
            this.vo = vo;
        }

        public BrushVo Output()
        {
            return vo;
        }
    }
}
