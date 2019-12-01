using System.Collections;
using System.Collections.Generic;
using MokomoGames.Debug;
using UnityEditor;
using UnityEngine;

namespace MokomoGames
{
    namespace Editor
    {
        namespace Debug
        {
            public class DebugWindow : EditorWindow
            {
                static bool displayDebugModel;

                [MenuItem ("MokomoGames/DebugWindow")]
                public static void Open ()
                {
                    EditorWindow.CreateWindow<DebugWindow> ();
                }

                public void OnGUI ()
                {
                    using (new EditorGUILayout.HorizontalScope (GUI.skin.box))
                    {
                        EditorGUILayout.LabelField ("プレイヤー詳細情報表示");
                        Debugger.Config.DisplayPlayerData = EditorGUILayout.Toggle (Debugger.Config.DisplayPlayerData);
                    }
                }
            }
        }
    }
}