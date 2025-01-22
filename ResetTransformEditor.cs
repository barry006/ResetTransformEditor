#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Git.Tools
{
 public class ResetTransformEditor : EditorWindow
 {
     [MenuItem("Tools/Reset Transform")]
     static void ShowWindow() => GetWindow<ResetTransformEditor>("Reset Transform");
     KeyCode _customKey = KeyCode.F1; Event _e = null; bool _resetPosition = true, _resetRotation = true, _resetScale = true;
     void OnEnable() => EditorApplication.update += EditorUpdate;
     void OnDisable() => EditorApplication.update -= EditorUpdate;
     void ResetTransform(Transform _transform) { if (_resetPosition) _transform.position = Vector3.zero; if (_resetRotation) _transform.rotation = Quaternion.identity; if (_resetScale) _transform.localScale = Vector3.one; }
     void ResetTransformAllChild(Transform parent) { foreach (Transform child in parent) { RecordUndoAndMarkDirty(child); ResetTransform(child); ResetTransformAllChild(child); } }
     void RecordUndoAndMarkDirty(Transform t) { Undo.RecordObject(t, "Reset Transform"); EditorUtility.SetDirty(t); }
     void EditorUpdate()
     {
         if (!Selection.activeTransform) return;
         if (_e.keyCode == _customKey) foreach (Transform t in Selection.transforms) { RecordUndoAndMarkDirty(t); ResetTransform(t); }
         if (_e.control && _e.keyCode == _customKey) ResetTransformAllChild(Selection.activeTransform);
     }
     void OnGUI()
     {
         _e = Event.current;
         GUILayout.Label("Assign Shortcut", EditorStyles.boldLabel);
         DrawRect(10); GUILayout.Space(10); GUILayout.BeginVertical("box");
         _customKey = (KeyCode)EditorGUILayout.EnumPopup("Shortcut Key", _customKey);
         GUILayout.EndVertical(); DrawRect(1); GUILayout.Space(10); GUILayout.BeginVertical("box");
         _resetPosition = EditorGUILayout.Toggle("Reset Position", _resetPosition);
         _resetRotation = EditorGUILayout.Toggle("Reset Rotation", _resetRotation);
         _resetScale = EditorGUILayout.Toggle("Reset Scale", _resetScale);
         GUILayout.EndVertical(); DrawRect(25);
     }
     void DrawRect(float height) { Rect rect = EditorGUILayout.GetControlRect(false, 1); rect.height = height; EditorGUI.DrawRect(rect, Color.green); }
 }
}
#endif
