using RoadNet.Flags;
using RoadNet.Segment;
using RoadNet.Utils;
using UnityEditor;
using UnityEngine;

namespace RoadNet.Editor.Windows
{
    public class RoadNetVizualizer : EditorWindow
    {
        RoadNet Net;

        bool showSegments;
        float offsetBetweenSegements = 0;
        float connectionSize = .35f;

        [MenuItem("Tools/Road Net/Vizualizer")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            RoadNetVizualizer window = (RoadNetVizualizer)EditorWindow.GetWindow(typeof(RoadNetVizualizer));
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
            if (Net == null || !showSegments) return;


            foreach (var laneSegment in Net.Segments)
            {
                var dir = Vector3.Normalize(laneSegment.End - laneSegment.Start);

                // draw connection to next segment
                foreach (var next in laneSegment.Next)
                {
                    var dirNext = Vector3.Normalize(Net.Segments[next].End - Net.Segments[next].Start);

                    Handles.color = Color.red;
                    Handles.DrawLine(laneSegment.End - dir * offsetBetweenSegements, Net.Segments[next].Start + dirNext * offsetBetweenSegements);
                }

                // draw segment
                DrawTools.ArrowHandle(laneSegment.Start + dir * offsetBetweenSegements, laneSegment.End - dir * offsetBetweenSegements, Color.green);

                // draw connecteble ends
                Handles.color = Color.gray;
                if (FlagsHelper.IsSet(laneSegment.EndState, LaneSegment.EndStates.ConnectebleStart)) Handles.DrawSphere(0, laneSegment.Start, Quaternion.identity, connectionSize / 2);
                if (FlagsHelper.IsSet(laneSegment.EndState, LaneSegment.EndStates.ConnectebleEnd)) Handles.DrawSphere(0, laneSegment.End, Quaternion.identity, connectionSize / 2);

                // draw open ends
                Handles.color = Color.blue;
                if (FlagsHelper.IsSet(laneSegment.EndState, LaneSegment.EndStates.OpenEnd)) Handles.DrawSphere(0, laneSegment.End - dir * offsetBetweenSegements, Quaternion.identity, connectionSize);
                Handles.color = Color.red;
                if (FlagsHelper.IsSet(laneSegment.EndState, LaneSegment.EndStates.OpenStart)) Handles.DrawSphere(0, laneSegment.Start + dir * offsetBetweenSegements, Quaternion.identity, connectionSize);
            }

        }

        void OnGUI()
        {
            Net = EditorGUILayout.ObjectField("Road Net Object", Net, typeof(RoadNet), false) as RoadNet;

            EditorGUI.BeginChangeCheck();
            showSegments = GUILayout.Toggle(showSegments, "Show Segments");
            offsetBetweenSegements = EditorGUILayout.FloatField("Offset between segments", offsetBetweenSegements);
            connectionSize = EditorGUILayout.FloatField("Connection size", connectionSize);
            if (EditorGUI.EndChangeCheck()) SceneView.RepaintAll();
        }
    }
}
