using RoadNet.Flags;
using RoadNet.Lanes;
using RoadNet.Segment;
using RoadNet.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace RoadNet
{
    [CreateAssetMenu(fileName = "Road Net", menuName = "Road Net")]
    public class RoadNet : ScriptableObject
    {
        public LaneSegment[] Segments = new LaneSegment[0];

        public void Build()
        {
            var lanes = FindObjectsOfType<Lane>();

            Reset();

            foreach (var lane in lanes)
            {
                AddLane(lane);
            }

#if UNITY_EDITOR
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public void Reset(int expectedLength = 0)
        {
            Segments = new LaneSegment[expectedLength];
        }

        public void AddLane(Lane lane)
        {
            var segments = lane.GetLanes();

            // add segments to the list
            Segments = Segments.Concat(segments).ToArray();

            // get indexes
            var startIndex = Segments.Length - segments.Length;
            var endIndex = Segments.Length - 1;

            // get end positions
            var startPos = Segments[startIndex].Start;
            var endPos = Segments[endIndex].End;

            // connect inner segments
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i != endIndex)
                    Segments[i].Next = new int[1] { i + 1 };
                else
                    Segments[i].Next = new int[0];

                if (i != startIndex)
                    Segments[i].Prev = new int[1] { i - 1 };
                else
                    Segments[i].Prev = new int[0];
            }

            //connect ends
            for (int i = 0; i < startIndex; i++)
            {
                if (FlagsHelper.IsSet(Segments[i].EndState, LaneSegment.EndStates.ConnectebleEnd))
                {
                    if (Vector3.Distance(startPos, Segments[i].End) < 0.1f)
                    {
                        Segments[i].Next = Segments[i].Next.Append(startIndex).ToArray();
                        Segments[startIndex].Prev = Segments[startIndex].Prev.Append(i).ToArray();

                        FlagsHelper.Unset(ref Segments[i].EndState, LaneSegment.EndStates.OpenEnd);
                        FlagsHelper.Unset(ref Segments[startIndex].EndState, LaneSegment.EndStates.OpenStart);
                    }
                }

                if (FlagsHelper.IsSet(Segments[i].EndState, LaneSegment.EndStates.ConnectebleStart))
                {
                    if (Vector3.Distance(endPos, Segments[i].Start) < 0.1f)
                    {
                        Segments[i].Prev = Segments[i].Next.Append(endIndex).ToArray();
                        Segments[endIndex].Next = Segments[endIndex].Next.Append(i).ToArray();

                        FlagsHelper.Unset(ref Segments[i].EndState, LaneSegment.EndStates.OpenStart);
                        FlagsHelper.Unset(ref Segments[endIndex].EndState, LaneSegment.EndStates.OpenEnd);
                    }
                }
            }
        }

        public List<int> FindPath(int fromIndex, int toIndex)
        {
            List<int> NeighbourStrategy(int i) => Segments[i].Next.ToList();
            float Distance(int from, int to) => Vector3.Distance(Segments[to].Start, Segments[to].End);

            var pf = new AStarPathFinding<int>(NeighbourStrategy, Distance, Distance);

            return pf.Path(fromIndex, toIndex);
        }
    }

}