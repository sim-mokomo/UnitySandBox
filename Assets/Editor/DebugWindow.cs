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

                [MenuItem ("MokomoGames/DebugWindow")]
                public static void Open ()
                {
                    EditorWindow.CreateWindow<DebugWindow> ();
                }

                public void OnGUI ()
                {
                    using (new EditorGUILayout.VerticalScope (GUI.skin.box))
                    {
                        foreach (var debugFlag in Debugger.config.DebugFlags)
                        {
                            DisplayDebugFlagToggle (debugFlag.Value);
                        }
                    }
                }

                void DisplayDebugFlagToggle (DebugFlag debugFlag)
                {
                    using (new EditorGUILayout.HorizontalScope (GUI.skin.box))
                    {
                        EditorGUILayout.LabelField (debugFlag.Description);
                        debugFlag.Active = EditorGUILayout.Toggle (debugFlag.Active);
                    }
                }
            }
        }
    }
}