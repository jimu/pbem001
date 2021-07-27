using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CommandSet)), CanEditMultipleObjects]
public class CommandSetEditor : Editor
{

    public SerializedProperty commandSetProp;
    void OnEnable()
    {
        commandSetProp = serializedObject.FindProperty("commandSet");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        commandSetProp.stringValue = EditorGUILayout.TextArea(commandSetProp.stringValue, GUILayout.MaxHeight(500));
        serializedObject.ApplyModifiedProperties();
    }
}
