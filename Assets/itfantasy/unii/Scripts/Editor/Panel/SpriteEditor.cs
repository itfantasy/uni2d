using UnityEngine;
using UnityEditor;
using System.Collections;

namespace itfantasy.unii
{
    [CustomEditor(typeof(Sprite2D))]
    [ExecuteInEditMode]
    public class SpriteEditor : Editor 
    {

        public override void OnInspectorGUI()
        {
            Sprite2D sprite = (Sprite2D)target;
            sprite.UpdateTransform();
            sprite.UpdateSize();
            sprite.position = EditorGUILayout.Vector3Field("位 置", sprite.position);
            sprite.size = EditorGUILayout.Vector2Field("尺 寸", sprite.size);
            sprite.alpha = EditorGUILayout.Slider("透明度", sprite.alpha, 0.0f, 1.0f);

            sprite.udata = EditorGUILayout.TextField("用户数据", sprite.udata);
        }

    }
}
