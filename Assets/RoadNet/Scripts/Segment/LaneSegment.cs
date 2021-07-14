using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadNet.Segment
{
    [System.Serializable]
    public struct LaneSegment
    {
        [System.Flags]
        public enum EndStates
        {
            ConnectebleEnd = (1 << 0),
            ConnectebleStart = (1 << 1),
            OpenEnd = (1 << 2),
            OpenStart = (1 << 3),
        }

        public Vector3 Start;
        public Vector3 End;

        [SerializeField]
        public EndStates EndState;

        public int[] Next;
        public int[] Prev;
    }

}