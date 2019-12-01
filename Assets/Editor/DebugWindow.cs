using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MokomoGames
{
    namespace Debug
    {
        public class DebugWindow : EditorWindow
        {
            [MenuItem ("MokomoGames/DebugWindow")]
            public static void Open ()
            {
                EditorWindow.CreateWindow<DebugWindow> ();
            }
        }
    }
}