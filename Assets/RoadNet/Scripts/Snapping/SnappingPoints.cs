using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoadNet.Snapping
{
    public class SnappingPoints : MonoBehaviour
    {
        public Connection[] Connections = new Connection[0];

        public IEnumerable<Connection> GetConnections()
        {
            return Connections.Select(c => {
                c.WorldPos = transform.TransformPoint(c.LocalPosition);
                c.WorldDir = transform.TransformDirection(c.Direction);
                c.WorldOffset = transform.TransformDirection(c.LocalPosition);
                return c;
            });
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            foreach (var point in GetConnections())
            {
                Gizmos.DrawSphere(point.WorldPos, .5f);
                Gizmos.DrawRay(point.WorldPos, point.WorldDir);
            }
        }
#endif
    }
}