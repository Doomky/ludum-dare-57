using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.DataStructures
{
    [Serializable]
    public abstract class Path<TNumeric>
    {
        [SerializeField]
        protected List<TNumeric> _positions = new();

        public List<TNumeric> Positions => _positions;

        public abstract TNumeric GetSegment(int i);

        public Path()
        {

        }

        public Path(List<TNumeric> positions)
        {
            this._positions = new List<TNumeric>(positions);
        }

        public Path(params TNumeric[] positions)
        {
            this._positions = new List<TNumeric>(positions);
        }
    }
}
