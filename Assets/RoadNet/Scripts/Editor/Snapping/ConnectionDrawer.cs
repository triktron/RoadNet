using RoadNet.Snapping;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoadNet.Editor.Snapping
{
    [CustomPropertyDrawer(typeof(Connection))]
    public class ConnectionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var posRect = position;
            posRect.width = posRect.width / 2;

            var rotRect = posRect;
            rotRect.x = rotRect.x + rotRect.width;

            EditorGUI.PropertyField(posRect, property.FindPropertyRelative("LocalPosition"), GUIContent.none);
            EditorGUI.PropertyField(rotRect, property.FindPropertyRelative("Direction"), GUIContent.none);
        }
    }
}