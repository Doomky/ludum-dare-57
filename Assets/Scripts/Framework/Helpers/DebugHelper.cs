using Framework.Extensions;
using UnityEngine;

namespace Framework.Managers
{
    public static partial class DebugHelper
    {

        private static Color GameColor = Color.green;
        
        private static Color ManagerColor = (Color.green / 2) + Color.white / 2;

        private static readonly NameBuilder renameBuilder = new();

        public static bool Assert(object obj, bool condition, string message)
        {
            message = FormatMessage(obj, message);

            Debug.Assert(condition, message);

            return condition;
        }

        public static void Log(this object obj, string message)
        {
            Debug.Log(FormatMessage(obj, message));
        }

        public static void LogError(this object obj, string message)
        {
            Debug.LogError(FormatMessage(obj, message));
        }

        public static void LogWarning(this object obj, string message)
        {
            Debug.LogWarning(FormatMessage(obj, message));
        }

        public static NameBuilder StartName(this object obj)
        {
            return renameBuilder.Start(obj);
        }

        private static string FormatMessage(this object obj, string message)
        {
            string typeTag;
            switch (obj)
            {
                case Game game:
                    {
                        typeTag = $"[{game.GetType().Name}]".Colorize(GameColor);
                        break;
                    }

                case GameState gameState:
                    {
                        typeTag = $"[{gameState.GetType().Name}]".Colorize(GameColor);
                        break;
                    }

                case Manager manager:
                    {
                        typeTag = $"[{manager.GetType().Name}]".Colorize(ManagerColor);
                        break;
                    }

                case null:
                    {
                        typeTag = "[null]".Colorize(Color.red);
                        break;
                    }

                default:
                    {
                        typeTag = $"[{obj.GetType().Name}]";
                        break;
                    }
            }

            return $"{typeTag} {message}";
        }
    }
}