/*
Find Nearest Neighbors
Sarper Soher - https://www.sarpersoher.com
Mar 04 2021 - 02:46
*/

using UnityEngine;
using UnityEditor;

/* NOTE(sarper Mar 04 21):
    Draws the DefaultPoolItem instances in a single inspector line, side by side, and fills the whole available space on that inspector row
    Default generic list inspector is too crowded without a good reason, this is simplified
*/
[CustomPropertyDrawer(typeof(DefaultPoolItem))]
public sealed class DefaultPoolItemEditor : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        int indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float fieldSpacing = 10f;

        // NOTE(sarper Mar 04 21): EditorGUIUtility.currentViewWidth gives us the width of the inspector here. Screen.width did the same but it doesn't factor in the vertical scrollbar
        float prefabFieldWidth = EditorGUIUtility.currentViewWidth - 70f;
        float countFieldWidth = EditorGUIUtility.currentViewWidth - prefabFieldWidth - 35f;

        Rect prefabRect = new Rect(position.x, position.y, prefabFieldWidth, position.height);
        Rect countRect = new Rect(position.x + prefabFieldWidth + fieldSpacing, position.y, countFieldWidth, position.height);

        EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("Prefab"), label);
        EditorGUI.PropertyField(countRect, property.FindPropertyRelative("Count"), GUIContent.none);

        EditorGUI.indentLevel = indentLevel;

        EditorGUI.EndProperty();
    }
}