using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

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
                AllowOutput ? GetOutputFilePath(_outputDistDirectory, _targetAnimatorController) : "設定が不十分です";
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
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"public static class {_targetAnimatorController.name}");
            builder.AppendLine("{");

            foreach (var layer in _targetAnimatorController.layers)
            {
                builder.AppendLine($"public static class {layer.name}");
                builder.AppendLine("{");
                foreach (var state in layer.stateMachine.states)
                {
                    builder.AppendLine($"public static readonly string {state.state.name};");
                }

                builder.AppendLine("}");
            }
            
            builder.AppendLine("}");

            var path = GetOutputFilePath(_outputDistDirectory, _targetAnimatorController);
            File.WriteAllText(path,builder.ToString());
            AssetDatabase.Refresh();
        }
    }
}