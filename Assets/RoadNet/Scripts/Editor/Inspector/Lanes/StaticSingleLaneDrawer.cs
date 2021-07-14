using RoadNet.Lanes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace RoadNet.Editor.Lanes
{
    [CustomPropertyDrawer(typeof(StaticSingleLane))]
    public class StaticSingleLaneDrawer : PropertyDrawer
    {
        SerializedProperty _points;

        ReorderableList _list;

        public override void OnGUI(Rect rect, SerializedProperty serializedProperty, GUIContent label)
        {
            SerializedProperty listProperty = GetSerializedProperty(serializedProperty);
            ReorderableList list = GetList(listProperty);

            //float height = 0f;
            //for (var i = 0; i < listProperty.arraySize; i++)
            //{
            //    height = Mathf.Max(height, EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i)));
            //}
            //list.elementHeight = height;
            list.DoList(rect);
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            SerializedProperty listProperty = GetSerializedProperty(serializedProperty);
            return GetList(listProperty).GetHeight();
        }

        private ReorderableList GetList(SerializedProperty serializedProperty)
        {
            if (_list == null)
            {
                _list = new ReorderableList(serializedProperty.serializedObject, serializedProperty);

                _list.drawElementCallback = DrawListItems;
                _list.drawHeaderCallback = DrawHeader;
            }

            return _list;
        }

        private void DrawHeader(Rect rect)
        {
            string name = "Points";
            EditorGUI.LabelField(rect, name);
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            element,
            GUIContent.none
        );
        }

        private SerializedProperty GetSerializedProperty(SerializedProperty serializedProperty)
        {
            //if (_points == null)
            {
                _points = serializedProperty.FindPropertyRelative("Points");
            }

            return _points;
        }
    }
}
