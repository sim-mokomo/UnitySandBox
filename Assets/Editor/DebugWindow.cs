using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugWindow : EditorWindow
{
    [MenuItem ("MokomoGames/DebugWindow")]
    public static void Open ()
    {
        EditorWindow.CreateWindow<DebugWindow> ();
    }
}