using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BuildingIDAttribute))]
public class BuildingIDDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string[] allIDs = BuildingID.All;
        int currentIndex = System.Array.IndexOf(allIDs, property.stringValue);
        if (currentIndex < 0) {
            currentIndex = 0; // Default to the first item if not found
        }

        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, allIDs);
        property.stringValue = allIDs[selectedIndex];
    }
}