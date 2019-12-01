using System;
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
                Filter filter = new Filter ();
                DebugInspector inspector = new DebugInspector ();

                [MenuItem ("MokomoGames/DebugWindow")]
                public static void Open ()
                {
                    EditorWindow.CreateWindow<DebugWindow> ();
                }

                public void OnGUI ()
                {
                    using (new EditorGUILayout.HorizontalScope (GUI.skin.box))
                    {
                        using (new EditorGUILayout.VerticalScope (GUI.skin.window))
                        {
                            using (new EditorGUILayout.VerticalScope (GUI.skin.box))
                            {
                                using (new EditorGUILayout.HorizontalScope (GUI.skin.box))
                                {
                                    EditorGUILayout.LabelField ("フィルター");
                                    filter.Name = EditorGUILayout.TextField (filter.Name);
                                }
                            }
                            using (new EditorGUILayout.VerticalScope (GUI.skin.box))
                            {
                                foreach (var debugFlag in Debugger.config.DebugFlags.Values)
                                {
                                    if (filter.IsFilter (debugFlag.Description))
                                        DisplayDebugFlagToggle (debugFlag);
                                }
                            }
                        }
                        inspector.OnGUI ();
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

                public class Filter
                {
                    string preFilter = "";
                    string name = "";
                    public string Name
                    {
                        get
                        {
                            return name;
                        }
                        set
                        {
                            preFilter = name;
                            name = value;
                            if (!preFilter.Equals (name))
                            {
                                OnUpdateName?.Invoke (name);
                            }
                        }
                    }

                    public bool IsFilter (string otherName)
                    {
                        return otherName.Contains (name) || string.IsNullOrEmpty (name);
                    }

                    public event Action<string> OnUpdateName;

                }
            }
        }
    }
}