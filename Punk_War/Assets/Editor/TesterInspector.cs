using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Tester))]
public class TesterInspector : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("integers"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vectors"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("colors"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("objects"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
