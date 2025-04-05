using System;
using UnityEngine;

namespace Framework.DataStructures
{
    public class FloatCerper : Cerper<float>
    {
        protected override float Lerp(float t)
        {
            return Mathf.Lerp(this._origin, this._target, t);
        }
    }
}
