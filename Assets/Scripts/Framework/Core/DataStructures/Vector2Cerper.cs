using UnityEngine;

namespace Framework.DataStructures
{
    public class Vector2Cerper : Cerper<Vector2>
    {
        protected override Vector2 Lerp(float t)
        {
            return Vector2.Lerp(this._origin, this._target, t);
        }
    }
}
