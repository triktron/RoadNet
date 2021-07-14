using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadNet.Utils
{
    public class AStarPathFinding<TPosition>
    {
        public delegate List<TPosition> NeighbourStrategy(TPosition from);
        public delegate float DistanceStrategy(TPosition from, TPosition to);
        public delegate float HeuristicStrategy(TPosition from, TPosition to);


        private readonly NeighbourStrategy _neighbours;
        private readonly DistanceStrategy _distance;
        private readonly HeuristicStrategy _heuristic;

        public AStarPathFinding(NeighbourStrategy neighbour, DistanceStrategy distance, HeuristicStrategy heuristic)
        {
            _neighbours = neighbour;
            _distance = distance;
            _heuristic = heuristic;
        }

        public List<TPosition> Path(TPosition from, TPosition to)
        {
            var openSet = new List<TPosition>() { from };
            var cameFrom = new Dictionary<TPosition, TPosition>();
            var gScores = new Dictionary<TPosition, float>() { { from, 0 } };
            var fScores = new Dictionary<TPosition, float>() { { from, _heuristic(from, to) } };

            while (openSet.Count > 0)
            {
                TPosition current = FindLowestFScore(fScores, openSet);

                if (current.Equals(to))
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);

                var neigbours = _neighbours(current);

                foreach (var neigbour in neigbours)
                {
                    var tentiveGScore = gScores[current] + _distance(current, neigbour);

                    if (tentiveGScore < gScores.GetValueOrDefault(neigbour, float.PositiveInfinity))
                    {
                        cameFrom[neigbour] = current;
                        gScores[neigbour] = tentiveGScore;
                        fScores[neigbour] = gScores[neigbour] + _heuristic(neigbour, to);

                        if (!openSet.Contains(neigbour)) openSet.Add(neigbour);
                    }
                }
            }

            return new List<TPosition>(0);
        }

        private List<TPosition> ReconstructPath(Dictionary<TPosition, TPosition> cameFrom, TPosition current)
        {
            var path = new List<TPosition>() { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];

                path.Insert(0, current);
            }

            return path;
        }

        public TPosition FindLowestFScore(Dictionary<TPosition, float> fScores, List<TPosition> openSet)
        {
            TPosition currentNode = openSet[0];

            foreach (var node in openSet)
            {
                var currentFScore = fScores.GetValueOrDefault(currentNode, float.PositiveInfinity);
                var fScore = fScores.GetValueOrDefault(node, float.PositiveInfinity);
                if (fScore < currentFScore)
                    currentNode = node;
            }

            return currentNode;
        }
    }
}