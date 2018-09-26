using UnityEngine;
using UnityEditor;
using System.Collections;

namespace itfantasy.unii
{
    [CustomEditor(typeof(Camera2D))]
    [ExecuteInEditMode]
    public class CameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Camera2D camera = (Camera2D)target;
            camera.UpdateTransform();
            camera.viewSize = EditorGUILayout.Vector2Field("视口尺寸", camera.viewSize);
            camera.position = EditorGUILayout.Vector3Field("位 置", camera.position);

            camera.draging = EditorGUILayout.Toggle("牵引式摄影机", camera.draging);
            if (camera.draging)
            {
                //camera.mainScene = EditorGUILayout.ObjectField("主场景", camera.mainScene, typeof(Scene2D)) as Scene2D;
                //camera.mainSprite = EditorGUILayout.ObjectField("聚焦物体", camera.mainSprite, typeof(Sprite2D)) as Sprite2D;
            }
        }
    }
}
