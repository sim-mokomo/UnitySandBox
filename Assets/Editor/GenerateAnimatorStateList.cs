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
        private AnimatorController _targetAnimatorController;
        private UnityEngine.Object _outputDistDirectory;

        [MenuItem("MokomoGames/Animatorのステート名定数クラスを生成")]
        public static void Open()
        {
            GetWindow<GenerateAnimatorStateList>();
        }

        public bool AllowOutput => _targetAnimatorController != null && _targetAnimatorController != null;

        private string GetOutputFilePath(UnityEngine.Object outputDirectory,AnimatorController animatorController)
        {
            var basePath = AssetDatabase.GetAssetPath(_outputDistDirectory);
            var fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(animatorController));
            return $"{basePath}/{fileName}.cs";
        }

        private void OnGUI()
        {
            _targetAnimatorController = EditorGUILayout.ObjectField(
                "ステート名を定数クラスに抽出するAnimator",
                _targetAnimatorController,
                typeof(AnimatorController),
                allowSceneObjects: false) as AnimatorController;

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                _outputDistDirectory = EditorGUILayout.ObjectField(
                    "定数クラス書き出し先フォルダ",
                    _outputDistDirectory,
                    typeof(UnityEngine.Object),
                    allowSceneObjects:false);

                var outputDescription =
                    AllowOutput ? 
                        GetOutputFilePath(_outputDistDirectory, _targetAnimatorController) :
                        "設定が不十分です";
                EditorGUILayout.LabelField($"書き出し先→{outputDescription}");
            }
            
            EditorGUILayout.LabelField("↓抽出されるステート、レイヤーごとに分割される。");
            using(new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                if(_targetAnimatorController == null)
                    return;
                foreach (var layer in _targetAnimatorController.layers)
                {
                    EditorGUILayout.LabelField($"レイヤー名:{layer.name}",EditorStyles.boldLabel);
                    foreach (var state in layer.stateMachine.states)
                    {
                        EditorGUILayout.LabelField(state.state.name);
                    }
                }
            }

            if (GUILayout.Button("抽出開始"))
            {
                if(_targetAnimatorController == null)
                    return;

                var classBuilder = new ClassBuilder( _targetAnimatorController.name );
                foreach (var layer in _targetAnimatorController.layers)
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
                 
                var path = GetOutputFilePath(_outputDistDirectory, _targetAnimatorController);
                File.WriteAllText(path,classBuilder.Format());
                AssetDatabase.Refresh();
            }
        }
    }
}
