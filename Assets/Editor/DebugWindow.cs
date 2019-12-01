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
                    using (new EditorGUILayout.VerticalScope (GUI.skin.box))
                    {
                        EditorGUILayout.LabelField ("プレイヤー詳細情報表示(追従)");
                        Debugger.Config.DisplayPlayerData = EditorGUILayout.Toggle (Debugger.Config.DisplayPlayerData);

                        EditorGUILayout.LabelField ("プレイヤー詳細情報表示(固定)");
                        Debugger.Config.DisplayFixPlayerData = EditorGUILayout.Toggle (Debugger.Config.DisplayFixPlayerData);
                    }
                }
            }
        }
    }
}