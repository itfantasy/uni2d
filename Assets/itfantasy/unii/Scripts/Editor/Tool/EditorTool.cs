using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace itfantasy.unii
{
    public class EditorTool
    {

        public static void AddTag(string tag)
        {
            if (!isHasTag(tag))
            {
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty it = tagManager.GetIterator();
                while (it.NextVisible(true))
                {
                    if (it.name == "tags")
                    {
                        for (int i = 0; i < it.arraySize; i++)
                        {
                            SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                            if (string.IsNullOrEmpty(dataPoint.stringValue))
                            {
                                dataPoint.stringValue = tag;
                                tagManager.ApplyModifiedProperties();
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static void AddLayer(string layer)
        {
            if (!isHasLayer(layer))
            {
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty it = tagManager.GetIterator();
                while (it.NextVisible(true))
                {
                    if (it.name.StartsWith("layers"))
                    {
                        if (it.type == "string")
                        {
                            if (string.IsNullOrEmpty(it.stringValue))
                            {
                                it.stringValue = layer;
                                tagManager.ApplyModifiedProperties();
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static bool isHasTag(string tag)
        {
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
            {
                if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                    return true;
            }
            return false;
        }

        public static bool isHasLayer(string layer)
        {
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
            {
                if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                    return true;
            }
            return false;
        }

        static bool Editor__getGameViewSizeError = false;
        public static bool Editor__gameViewReflectionError = false;

        public static bool GetGameViewSize(out float width, out float height, out float aspect)
        {
            try
            {
                Editor__gameViewReflectionError = false;

                System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                System.Reflection.MethodInfo GetMainGameView = gameViewType.GetMethod("GetMainGameView", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                object mainGameViewInst = GetMainGameView.Invoke(null, null);
                if (mainGameViewInst == null)
                {
                    width = height = aspect = 0;
                    return false;
                }
                System.Reflection.FieldInfo s_viewModeResolutions = gameViewType.GetField("s_viewModeResolutions", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (s_viewModeResolutions == null)
                {
                    System.Reflection.PropertyInfo currentGameViewSize = gameViewType.GetProperty("currentGameViewSize", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    object gameViewSize = currentGameViewSize.GetValue(mainGameViewInst, null);
                    System.Type gameViewSizeType = gameViewSize.GetType();
                    int gvWidth = (int)gameViewSizeType.GetProperty("width").GetValue(gameViewSize, null);
                    int gvHeight = (int)gameViewSizeType.GetProperty("height").GetValue(gameViewSize, null);
                    int gvSizeType = (int)gameViewSizeType.GetProperty("sizeType").GetValue(gameViewSize, null);
                    if (gvWidth == 0 || gvHeight == 0)
                    {
                        width = height = aspect = 0;
                        return false;
                    }
                    else if (gvSizeType == 0)
                    {
                        width = height = 0;
                        aspect = (float)gvWidth / (float)gvHeight;
                        return true;
                    }
                    else
                    {
                        width = gvWidth; height = gvHeight;
                        aspect = (float)gvWidth / (float)gvHeight;
                        return true;
                    }
                }
                else
                {
                    Vector2[] viewModeResolutions = (Vector2[])s_viewModeResolutions.GetValue(null);
                    float[] viewModeAspects = (float[])gameViewType.GetField("s_viewModeAspects", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
                    string[] viewModeStrings = (string[])gameViewType.GetField("s_viewModeAspectStrings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
                    if (mainGameViewInst != null
                        && viewModeStrings != null
                        && viewModeResolutions != null && viewModeAspects != null)
                    {
                        int aspectRatio = (int)gameViewType.GetField("m_AspectRatio", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(mainGameViewInst);
                        string thisViewModeString = viewModeStrings[aspectRatio];
                        if (thisViewModeString.Contains("Standalone"))
                        {
                            width = UnityEditor.PlayerSettings.defaultScreenWidth; height = UnityEditor.PlayerSettings.defaultScreenHeight;
                            aspect = width / height;
                        }
                        else if (thisViewModeString.Contains("Web"))
                        {
                            width = UnityEditor.PlayerSettings.defaultWebScreenWidth; height = UnityEditor.PlayerSettings.defaultWebScreenHeight;
                            aspect = width / height;
                        }
                        else
                        {
                            width = viewModeResolutions[aspectRatio].x; height = viewModeResolutions[aspectRatio].y;
                            aspect = viewModeAspects[aspectRatio];
                            // this is an error state
                            if (width == 0 && height == 0 && aspect == 0)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            catch (System.Exception e)
            {
                if (Editor__getGameViewSizeError == false)
                {
                    Debug.LogError("GameCamera.GetGameViewSize - has a Unity update broken this?\nThis is not a fatal error !\n" + e.ToString());
                    Editor__getGameViewSizeError = true;
                }
                Editor__gameViewReflectionError = true;
            }
            width = height = aspect = 0;
            return false;
        }
    }
}
