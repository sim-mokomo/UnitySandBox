using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugInspector
{
    public void OnGUI ()
    {
        using (new EditorGUILayout.VerticalScope (GUI.skin.window))
        {
            EditorGUILayout.LabelField ("Hello Inspector World");
        }
    }
}