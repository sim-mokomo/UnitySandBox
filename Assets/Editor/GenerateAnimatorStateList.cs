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
                    ? GetOutputFilePath(_outputDistDirectory, null)
                    : "設定が不十分です";
                EditorGUILayout.LabelField($"書き出し先→{outputDescription}");
            }
            
            if (GUILayout.Button("抽出開始"))
            {
                if(_outputDistDirectory == null)
                    return;

                var animatorControllers = GetAllAnimatorControllers();

                foreach (var animatorController in animatorControllers)
                {
                    var classBuilder = MakeAnimatorControllerClassBuilder(animatorController);
                    var path = GetOutputFilePath(_outputDistDirectory, animatorController);
                    File.WriteAllText(path,classBuilder.Format(tabNum: 0));
                    AssetDatabase.Refresh();
                }
            }
        }
        
        private string GetOutputFilePath(UnityEngine.Object outputDirectory,AnimatorController animatorController)
        {
            var basePath = AssetDatabase.GetAssetPath(outputDirectory);
            var fileName = animatorController != null
                ? Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(animatorController))
                : "*";
            return $"{basePath}/{fileName}.cs";
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
                classBuilder.AddClasses(layerClassBuilder);
            }

            return classBuilder;
        }
    }
}
