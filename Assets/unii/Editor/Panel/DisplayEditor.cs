using UnityEngine;
using UnityEditor;
using System.Collections;

namespace itfantasy.unii
{
    [CustomEditor(typeof(Display2D))]
    [ExecuteInEditMode]
    public class DisplayEditor : Editor 
    {

        public override void OnInspectorGUI()
        {
            Display2D display = (Display2D)target;
            display.UpdateTransform();
            
            display.position = EditorGUILayout.Vector3Field("位 置", display.position);
            display.scale = EditorGUILayout.Vector2Field("缩 放", display.scale);
            display.rotation = EditorGUILayout.Vector3Field("旋 转", display.rotation);

            display.udata = EditorGUILayout.TextField("用户数据", display.udata);
        }

    }
}
