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

            navCamera.transform.position = new Vector3(0.0f, 0.0f, -1024.0f);
            navCamera.clearFlags = CameraClearFlags.Skybox;
            navCamera.backgroundColor = new Color(100.0f / 255.0f, 149.0f / 255.0f, 237.0f / 255.0f, 1);
            navCamera.cullingMask = setNav2dLayer(camera);
            navCamera.orthographic = true;
            navCamera.farClipPlane = 2048.0f;
        }

        [MenuItem("GameObject/2D Object/Coord2D - 坐标系")]
        static void CreateCoord2D()
        {
            GameObject go = Selection.activeGameObject;
            GameObject coord2D = new GameObject("Coord2D");
            Camera navCamera = coord2D.AddComponent<Camera>();

            navCamera.clearFlags = CameraClearFlags.Nothing;
            navCamera.transform.position = Vector3.zero;
            navCamera.backgroundColor = new Color(100.0f / 255.0f, 149.0f / 255.0f, 237.0f / 255.0f, 0);
            navCamera.cullingMask = setNav2dLayer(coord2D);
            navCamera.orthographic = true;
            navCamera.nearClipPlane = 0.3f;
            navCamera.farClipPlane = 0.35f;
            coord2D.AddComponent<Coord2D>();

            if (go != null)
            {
                coord2D.transform.parent = go.transform;
            }
        }

        [MenuItem("GameObject/2D Object/Sprite2D - 精灵")]
        static void CreateSprite2D()
        {
            GameObject go = Selection.activeGameObject;
            GameObject sprite2D = new GameObject("Sprite2D");

            sprite2D.AddComponent<SpriteRenderer>();
            sprite2D.AddComponent<BoxCollider2D>();
            sprite2D.AddComponent<Sprite2D>();
            setNav2dLayer(sprite2D);

            if (go != null)
            {
                sprite2D.transform.parent = go.transform;
            }
        }

        [MenuItem("GameObject/2D Object/Display2D - 2D节点")]
        static void CreateDisplay2D()
        {
            GameObject go = Selection.activeGameObject;
            GameObject display2D = new GameObject("Display2D");

            display2D.AddComponent<Display2D>();
            setNav2dLayer(display2D);

            if (go != null)
            {
                display2D.transform.parent = go.transform;
            }
        }

        static int setNav2dLayer(GameObject go)
        {
            if (!EditorTool.isHasLayer(nav2dLayer))
            {
                EditorTool.AddLayer(nav2dLayer);
            }
            go.layer = LayerMask.NameToLayer(nav2dLayer);
            return 1 << go.layer;
        }
    }
}
