using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace MokomoGames.Editor.Debug
{
    public class GenerateAnimatorStateList : EditorWindow
    {
        private UnityEngine.Object _outputDistDirectory;

        [MenuItem("MokomoGames/Animatorのステート名定数クラスを生成")]
        public static void Open()
        {
            GetWindow<GenerateAnimatorStateList>();
        }
        
        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                _outputDistDirectory = EditorGUILayout.ObjectField(
                    "定数クラス書き出し先フォルダ",
                    _outputDistDirectory,
                    typeof(UnityEngine.Object),
                    allowSceneObjects:false);

                var outputDescription = _outputDistDirectory != null
                    ? GetOutputFilePath(_outputDistDirectory, "*")
                    : "設定が不十分です";
                EditorGUILayout.LabelField($"書き出し先→{outputDescription}");
            }
            
            if (GUILayout.Button("全てのAnimatorControllerを定数クラスとして書き出し"))
            {
                if(_outputDistDirectory == null)
                    return;

                var animatorControllers = GetAllAnimatorControllers();

                foreach (var animatorController in animatorControllers)
                {
                    var classBuilder = MakeAnimatorControllerClassBuilder(animatorController);
                    var path = GetOutputFilePath(_outputDistDirectory, classBuilder.ClassName);
                    File.WriteAllText(path,classBuilder.Format(tabNum: 0));
                    AssetDatabase.Refresh();
                }
            }

            if (GUILayout.Button("全てのシーンを定数クラスとして書き出し"))
            {
                var sceneAnimatorController = MakeSceneClassBuilder();
                File.WriteAllText(
                    GetOutputFilePath(_outputDistDirectory,sceneAnimatorController.ClassName),
                    sceneAnimatorController.Format(0)
                    );
                AssetDatabase.Refresh();
            }
        }
        
        private string GetOutputFilePath(UnityEngine.Object outputDirectory,string className)
        {
            var basePath = AssetDatabase.GetAssetPath(outputDirectory);
            return $"{basePath}/{className}.cs";
        }

        private IEnumerable<AnimatorController> GetAllAnimatorControllers()
        {
            return AssetDatabase
                .GetAllAssetPaths()
                .Where(x => Path.GetExtension(x).Equals(".controller"))
                .Select(x => AssetDatabase.LoadAssetAtPath<AnimatorController>(x));
        }

        private ClassBuilder MakeAnimatorControllerClassBuilder(AnimatorController animatorController)
        {
            var classBuilder = new ClassBuilder( animatorController.name );
            foreach (var layer in animatorController.layers)
            {
                var layerClassBuilder = new ClassBuilder(layer.name);
                layerClassBuilder.AddVariables( layer
                    .stateMachine
                    .states
                    .Select(x => new VariableBuilder(x.state.name,x.state.name))
                    .ToList()
                );
                classBuilder.AddClass(layerClassBuilder);
            }

            return classBuilder;
        }

        private ClassBuilder MakeSceneClassBuilder()
        {
            var classBuilder = new ClassBuilder(className: "SceneNames");
            classBuilder.AddVariables(
                AssetDatabase
                    .GetAllAssetPaths()
                    .Where(x => Path.GetExtension(x).Equals(".unity"))
                    .Select(x => 
                        new VariableBuilder(
                            Path.GetFileNameWithoutExtension(x),
                            Path.GetFileNameWithoutExtension(x)))
                    .ToList()
                ); 
            return classBuilder;
        }
    }
}
