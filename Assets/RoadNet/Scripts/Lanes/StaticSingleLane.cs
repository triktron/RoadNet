using RoadNet.Segment;
using RoadNet.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadNet.Lanes
{
    public class StaticSingleLane : Lane
    {
        public List<Vector3> Points = new List<Vector3>();
#if UNITY_EDITOR
        public bool ShowInInspector = false;
#endif
        public override LaneSegment[] GetLanes()
        {
            return BuildLanesFromPoints(Points);
        }


#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            if (ShowInInspector)
            {
                for (int i = 1; i < Points.Count; i++)
                {
                    var posA = transform.TransformPoint(Points[i - 1]);
                    var posB = transform.TransformPoint(Points[i]);

                    DrawTools.Arrow(posA, posB, Color.white);
                }
            }
        }
#endif
    }
}