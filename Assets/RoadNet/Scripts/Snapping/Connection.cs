using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoadNet.Snapping
{
    [System.Serializable]
    public class Connection
    {
        public Vector3 LocalPosition;
        public Vector3 Direction;

        public Vector3 WorldPos;
        public Vector3 WorldDir;
        public Vector3 WorldOffset;

        public Connection GetClossetPoint(Connection[] others, float maxDist = 2)
        {
            float min = maxDist;
            Connection conn = null;

            foreach (var other in others)
            {
                if (Vector3.Dot(WorldDir, other.WorldDir) > 0) continue;

                var dist = Vector3.Distance(WorldPos, other.WorldPos);
                if (dist < min)
                {
                    min = dist;
                    conn = other;
                }
            }

            return conn;
        }
    }
}