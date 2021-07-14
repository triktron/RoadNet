using RoadNet.Snapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RoadNet.Editor.Snapping
{
    [CustomEditor(typeof(SnappingPoints))]
    public class RoadSnapping : UnityEditor.Editor
    {
        private Vector3 _lastPos;

        private void OnEnable()
        {
            var snappingPoints = target as SnappingPoints;
            var snappingTransform = snappingPoints.transform;

            _lastPos = snappingTransform.position;

            EditorApplication.update += Update;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        void Update()
        {
            var snappingPoints = target as SnappingPoints;
            if (snappingPoints == null || !snappingPoints.isActiveAndEnabled) return;

            var snappingTransform = snappingPoints.transform;

            var currentPos = snappingTransform.position;

            if (_lastPos != currentPos)
            {
                _lastPos = currentPos;

                var allOthers = FindObjectsOfType<SnappingPoints>().Where(sp => sp != snappingPoints).SelectMany(sp => sp.GetConnections()).ToArray();
                var ownPoints = snappingPoints.GetConnections().ToArray();

                TrySnap(allOthers, ownPoints, snappingTransform);
            }
        }

        private void TrySnap(Connection[] othersPoints, Connection[] ownPoints, Transform connectionTransform)
        {
            float closestDist = -1;
            Connection closestConnOwn = null;
            Connection closestConnOther = null;

            foreach (var own in ownPoints)
            {
                var closest = own.GetClossetPoint(othersPoints);

                if (closest == null) continue;

                var dist = Vector3.Distance(own.WorldPos, closest.WorldPos);
                if (closestDist < 0 || closestDist > dist)
                {
                    closestDist = dist;
                    closestConnOther = closest;
                    closestConnOwn = own;
                }
            }

            if (closestDist >= 0)
            {
                connectionTransform.position = closestConnOther.WorldPos - closestConnOwn.WorldOffset;
            }
        }
    }
}
