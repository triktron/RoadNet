using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace RoadNet.Editor
{
    [CustomEditor(typeof(RoadNet))]
    public class RoadNetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Build")) (target as RoadNet).Build();
        }
    }

}