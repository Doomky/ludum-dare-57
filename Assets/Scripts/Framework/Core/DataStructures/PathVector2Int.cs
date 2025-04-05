using Framework.Extensions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.DataStructures
{
    [Serializable]
    public class PathVector2Int : Path<Vector2Int>
    {
        [ReadOnly]
        [ShowInInspector]
        public int Length
        {
            get
            {
                int length = 0;
                int keyPositionsCount = this._positions.Count;
                for (int i = 0; i < keyPositionsCount - 1; i++)
                {
                    length += this._positions[i].ManhattanDistance(this._positions[i + 1]);
                }

                return length;
            }
        }

        public PathVector2Int() : base()
        {
        }

        public PathVector2Int(params Vector2Int[] positions) : base(positions)
        {
        }

        public PathVector2Int(List<Vector2Int> positions) : base(positions)
        {
            int positionsCount = this._positions?.Count ?? 0;
            for (int i = 0; i < positionsCount - 1; i++)
            {
                Vector2Int segment = this.GetSegment(i);
                Debug.Assert(segment.x * segment.y == 0, "PathVector2Int: Diagonal movement is not allowed.");
            }
        }

        public PathVector2Int(PathVector2Int pathVector) : base(pathVector.Positions)
        {

        }
        public override Vector2Int GetSegment(int i)
        {
            Debug.Assert(i > -1);
            Debug.Assert(i < this._positions.Count - 1);

            return this._positions[i + 1] - this._positions[i];
        }

        public bool Contains(Vector2Int pathPosition)
        {
            int positionsCount = this._positions.Count;
            for (int i = 0; i < positionsCount - 1; i++)
            {
                Vector2Int segment = this.GetSegment(i);
                Vector2Int segmentDirection = segment.Normalized();

                int segmentLength = segment.ManhattanMagnitude() + 1;
                for (int j = 0; j < segmentLength; j++)
                {
                    Vector2Int segmentPosition = this._positions[i] + j * segmentDirection;

                    if (segmentPosition == pathPosition)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
