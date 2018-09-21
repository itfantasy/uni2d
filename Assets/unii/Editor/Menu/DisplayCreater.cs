using UnityEngine;
using UnityEditor;
using System.Collections;

namespace itfantasy.unii
{
    public class DisplayCreater
    {
        static string nav2dLayer = "Native2d";

        [MenuItem("GameObject/2D Object/Camera2D - 摄影机")]
        static void CreateCamera2D()
        {
            GameObject camera = new GameObject("Camera2D");
            Camera navCamera = camera.AddComponent<Camera>();
            Camera2D camera2d = camera.AddComponent<Camera2D>();
            navCamera.transparencySortMode = TransparencySortMode.Orthographic;
            navCamera.cullingMask = setNav2dLayer(camera);
        }

        [MenuItem("GameObject/2D Object/Display2D - 2D节点")]
        static void CreateDisplay2D()
        {
            GameObject go = Selection.activeGameObject;
            GameObject display2D = new GameObject("Display2D");
            display2D.AddComponent<Display2D>();
            if (go != null)
            {
                display2D.transform.parent = go.transform;
            }
            setNav2dLayer(display2D);
        }

        [MenuItem("GameObject/2D Object/Sprite2D - 精灵")]
        static void CreateSprite2D()
        {
            GameObject go = Selection.activeGameObject;
            GameObject sprite2D = new GameObject("Sprite2D");
            sprite2D.AddComponent<SpriteRenderer>();
            sprite2D.AddComponent<BoxCollider2D>();
            sprite2D.AddComponent<Sprite2D>();
            if (go != null)
            {
                sprite2D.transform.parent = go.transform;
            }
            setNav2dLayer(sprite2D);
        }

        [MenuItem("GameObject/2D Object/Coord2D - 坐标系")]
        static void CreateCoord2D()
        {
            GameObject go = Selection.activeGameObject;
            GameObject coord2D = new GameObject("Coord2D");
            Camera navCamera = coord2D.AddComponent<Camera>();
            navCamera.transparencySortMode = TransparencySortMode.Orthographic;
            navCamera.cullingMask = setNav2dLayer(coord2D);
            coord2D.AddComponent<Coord2D>();
            if (go != null)
            {
                coord2D.transform.parent = go.transform;
            }
            setNav2dLayer(coord2D);
        }

        static int setNav2dLayer(GameObject go)
        {
            if (!EditorTool.isHasLayer(nav2dLayer))
            {
                EditorTool.AddLayer(nav2dLayer);
            }
            go.layer = LayerMask.NameToLayer(nav2dLayer);
            return go.layer;
        }
    }
}
