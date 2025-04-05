using UnityEngine;
using Framework.UI;
using Framework.Managers;

namespace Framework.Helpers
{

    public static class AnchorHelpers
    {
        public static Vector2 GetAnchor(this Anchor anchor)
        {
            switch (anchor)
            {
                case Anchor.LeftTop:
                    {
                        return new Vector2(0, 1);
                    }

                case Anchor.LeftMiddle:
                    {
                        return new Vector2(0, 0.5f);
                    }

                case Anchor.LeftBottom:
                    {
                        return new Vector2(0, 0);
                    }

                case Anchor.TopLeft:
                    {
                        return new Vector2(0, 1);
                    }

                case Anchor.TopMiddle:
                    {
                        return new Vector2(0.5f, 1);
                    }

                case Anchor.TopRight:
                    {
                        return new Vector2(1, 1);
                    }

                case Anchor.RightTop:
                    {
                        return new Vector2(1, 1);
                    }

                case Anchor.RightMiddle:
                    {
                        return new Vector2(1, 0.5f);
                    }

                case Anchor.RightBottom:
                    {
                        return new Vector2(1, 0);
                    }

                case Anchor.BottomLeft:
                    {
                        return new Vector2(0, 0);
                    }

                case Anchor.BottomMiddle:
                    {
                        return new Vector2(0.5f, 0);
                    }

                case Anchor.BottomRight:
                    {
                        return new Vector2(1, 0);
                    }

                default:
                    {
                        Debug.LogError($"cannot find anchor for anchor direction {anchor}");
                        return Vector2.zero;
                    }
            }
        }

        public static Vector2 GetAnchoredPositionWithMargin(this Anchor anchor, float margin)
        {
            switch (anchor)
            {
                case Anchor.LeftTop:
                case Anchor.LeftMiddle:
                case Anchor.LeftBottom:
                    {
                        return new Vector2(-margin, 0);
                    }

                case Anchor.TopLeft:
                case Anchor.TopMiddle:
                case Anchor.TopRight:
                    {
                        return new Vector2(0, margin);
                    }

                case Anchor.RightTop:
                case Anchor.RightMiddle:
                case Anchor.RightBottom:
                    {
                        return new Vector2(margin, 0);
                    }

                case Anchor.BottomLeft:
                case Anchor.BottomMiddle:
                case Anchor.BottomRight:
                    {
                        return new Vector2(0, -margin);
                    }

                default:
                    {
                        Debug.LogError($"Cannot get position for anchor direction: {anchor}");
                        return Vector2.zero;
                    }
            }
        }
        
        public static Vector2 GetPivot(this Anchor anchor)
        {
            switch (anchor)
            {
                case Anchor.LeftTop:
                    {
                        return new Vector2(1, 0);
                    }

                case Anchor.LeftMiddle:
                    {
                        return new Vector2(1, 0.5f);
                    }

                case Anchor.LeftBottom:
                    {
                        return new Vector2(1, 1);
                    }

                case Anchor.TopLeft:
                    {
                        return new Vector2(0, 0);
                    }

                case Anchor.TopMiddle:
                    {
                        return new Vector2(0.5f, 0);
                    }

                case Anchor.TopRight:
                    {
                        return new Vector2(1, 1);
                    }

                case Anchor.RightTop:
                    {
                        return new Vector2(0, 0);
                    }

                case Anchor.RightMiddle:
                    {
                        return new Vector2(0, 0.5f);
                    }

                case Anchor.RightBottom:
                    {
                        return new Vector2(0, 1);
                    }

                case Anchor.BottomLeft:
                    {
                        return new Vector2(0, 0);
                    }

                case Anchor.BottomMiddle:
                    {
                        return new Vector2(0.5f, 1);
                    }

                case Anchor.BottomRight:
                    {
                        return new Vector2(1, 0);
                    }

                default:
                    {
                        DebugHelper.LogError(null, $"cannot determine pivot for anchor direction {anchor}");
                        return Vector2.zero;
                    }
            }
        }
    }
}