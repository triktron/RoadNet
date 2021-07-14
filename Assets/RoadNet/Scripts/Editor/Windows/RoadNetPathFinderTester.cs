using RoadNet.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoadNet.Editor.Windows
{
    public class RoadNetPathFinderTester : EditorWindow
    {
        RoadNet Net;

        Vector3 startPos;
        Vector3 endPos = Vector3.left;

        int startingNode;
        int edningNode;

        List<int> path = new List<int>();

        [MenuItem("Tools/Road Net/Path Finder")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            RoadNetPathFinderTester window = (RoadNetPathFinderTester)EditorWindow.GetWindow(typeof(RoadNetPathFinderTester));
            window.Show();
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }


        private void OnSceneGUI(SceneView sceneView)
        {
            if (Net == null) return;

            EditorGUI.BeginChangeCheck();
            startPos = Handles.PositionHandle(startPos, Quaternion.identity);
            Handles.Label(startPos, "Starting Pos");
            if (EditorGUI.EndChangeCheck())
            {
                startingNode = GetClosestSegment(startPos);
                UpdatePath();
            }

            EditorGUI.BeginChangeCheck();
            endPos = Handles.PositionHandle(endPos, Quaternion.identity);
            Handles.Label(endPos, "Edning Pos");
            if (EditorGUI.EndChangeCheck())
            {
                edningNode = GetClosestSegment(endPos);
                UpdatePath();
            }


            Handles.color = Color.gray;
            Handles.DrawLine(startPos, (Net.Segments[startingNode].Start + Net.Segments[startingNode].End) * .5f);
            Handles.DrawLine(endPos, (Net.Segments[edningNode].Start + Net.Segments[edningNode].End) * .5f);

            foreach (var i in path)
            {
                var offset = Vector3.up * .2f;
                DrawTools.ArrowHandle(Net.Segments[i].Start + offset, Net.Segments[i].End + offset, Color.magenta);
            }
        }

        int GetClosestSegment(Vector3 pos)
        {
            int closest = 0;
            float closestDist = float.MaxValue;

            for (int i = 0; i < Net.Segments.Length; i++)
            {
                var center = (Net.Segments[i].Start + Net.Segments[i].End) * .5f;
                var dist = Vector3.Distance(pos, center);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = i;
                }
            }

            return closest;
        }

        void UpdatePath()
        {
            path = Net.FindPath(startingNode, edningNode);
        }

        void OnGUI()
        {
            Net = EditorGUILayout.ObjectField("Road Net Object", Net, typeof(RoadNet), false) as RoadNet;
        }
    }
}
