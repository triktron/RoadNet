using RoadNet.Flags;
using RoadNet.Segment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadNet.Lanes
{
    public abstract class Lane : MonoBehaviour
    {
        public abstract LaneSegment[] GetLanes();

        public LaneSegment[] BuildLanesFromPoints(List<Vector3> points)
        {
            if (points.Count <= 1) return new LaneSegment[0];

            var lanes = new LaneSegment[points.Count - 1];

            for (int i = 0; i < points.Count - 1; i++)
            {
                lanes[i] = new LaneSegment()
                {
                    Start = transform.TransformPoint(points[i]),
                    End = transform.TransformPoint(points[i + 1])
                };

                FlagsHelper.SetTo(ref lanes[i].EndState, LaneSegment.EndStates.ConnectebleEnd | LaneSegment.EndStates.OpenEnd, i == points.Count - 2);
                FlagsHelper.SetTo(ref lanes[i].EndState, LaneSegment.EndStates.ConnectebleStart | LaneSegment.EndStates.OpenStart, i == 0);
            }

            return lanes;
        }
    }
}