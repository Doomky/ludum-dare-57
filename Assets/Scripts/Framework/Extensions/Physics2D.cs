using Game.Layers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Extensions
{
    public static class Physics2D<T>
    {
        static readonly List<T> _results = new();

        public static bool TryGetOverlapCircle<TLayer>(Vector2 origin, float radius, TLayer layerMask, out IReadOnlyList<T> result) where TLayer : Enum
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, radius, PhysicalLayerHelpers.GetLayerMaskFromFlags(layerMask));

            _results.Clear();

            int count = colliders?.Length ?? 0;
            for (int i = 0; i < count; i++)
            {
                Collider2D collider2D = colliders[i];
                if (collider2D.TryGetComponent(out T component))
                {
                    _results.Add(component);
                }
            }

            result = _results;

            return result.Count > 0;
        }

        public static bool TryRaycast<TLayer>(Vector2 origin, Vector2 direction, TLayer layerMask, out T result, float distance = float.PositiveInfinity) where TLayer : Enum
        {
            result = default;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, distance, PhysicalLayerHelpers.GetLayerMaskFromFlags(layerMask));
            Rigidbody2D rb = raycastHit2D.rigidbody;
            return rb && rb.TryGetComponent(out result);
        }
    }
}