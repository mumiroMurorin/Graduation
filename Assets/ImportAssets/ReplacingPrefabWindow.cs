//  ReplacingPrefabWindow.cs
//  http://kan-kikuchi.hatenablog.com/entry/ReplacingPrefabWindow
//
//  Created by kan.kikuchi on 2020.02.27.

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Prefabを差し替えるウィンドウ
/// </summary>
public class ReplacingPrefabWindow : EditorWindow
{

    //スクロール位置
    private Vector2 _scrollPosition = Vector2.zero;

    //差し替え先のPrefab
    private GameObject _taretPrefab;

    //各値を固定するか
    private bool _isFixPosition = true, _isFixRotation = true, _isFixScale = true;

    //Undo時の名前
    private const string UndoName = " Prefabを差し替え";

    //=================================================================================
    //初期化
    //=================================================================================

    //メニューからウィンドウを表示
    [MenuItem("Tools/Open/ReplacingPrefabWindow")]
    public static void Open()
    {
        ReplacingPrefabWindow.GetWindow(typeof(ReplacingPrefabWindow));
    }

    private void OnEnable()
    {
        //選択されている物が変わったらGUIを更新するように
        Selection.selectionChanged += Repaint;
    }

    //=================================================================================
    //表示するGUIの設定
    //=================================================================================

    private void OnGUI()
    {
        //スクロール位置保存
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUI.skin.scrollView);

        //差し替え先のPrefab選択UI
        EditorGUILayout.LabelField("差し替え先のPrefab");
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        _taretPrefab = (GameObject)EditorGUILayout.ObjectField(_taretPrefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //各値を固定するかを設定するUI
        EditorGUILayout.LabelField("差し替え元の固定する項目");
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        _isFixPosition = EditorGUILayout.ToggleLeft("Position", _isFixPosition, GUILayout.Width(100));
        _isFixRotation = EditorGUILayout.ToggleLeft("Rotation", _isFixRotation, GUILayout.Width(100));
        _isFixScale = EditorGUILayout.ToggleLeft("Scale", _isFixScale);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //選択中(差し替え元)のオブジェクト一覧
        EditorGUILayout.LabelField("差し替える(選択している)オブジェクト一覧");
        EditorGUILayout.BeginVertical(GUI.skin.box);
        var selectingTransforms = Selection.transforms;
        foreach (var selectingTransform in selectingTransforms)
        {
            EditorGUILayout.LabelField($"{selectingTransform.name}");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        //差し替え実行ボタン
        if (_taretPrefab == null)
        {
            EditorGUILayout.HelpBox("差し替え先のPrefabが設定されていません", MessageType.Error);
        }
        else if (selectingTransforms.Length == 0)
        {
            EditorGUILayout.HelpBox("差し替えるオブジェクトが選択されていません", MessageType.Error);
        }
        else if (GUILayout.Button("差し替え実行"))
        {
            Replace(selectingTransforms);
        }

        //描画範囲が足りなかればスクロール
        EditorGUILayout.EndScrollView();
    }

    //=================================================================================
    //差し替え
    //=================================================================================

    private void Replace(Transform[] transforms)
    {
        foreach (var transform in transforms)
        {
            //元のオブジェクトの位置にPrefabのまま設置
            var newObject = (PrefabUtility.InstantiatePrefab(_taretPrefab) as GameObject).transform;
            Undo.RegisterCreatedObjectUndo(newObject.gameObject, UndoName);
            Undo.SetTransformParent(newObject, transform.parent, UndoName);
            newObject.SetSiblingIndex(transform.GetSiblingIndex());
            if (_isFixPosition)
            {
                newObject.position = transform.position;
            }
            if (_isFixRotation)
            {
                newObject.rotation = transform.rotation;
            }
            if (_isFixScale)
            {
                newObject.localScale = transform.localScale;
            }

            //元のオブジェクトをUndoに登録して削除
            Undo.DestroyObjectImmediate(transform.gameObject);
        }
    }

}