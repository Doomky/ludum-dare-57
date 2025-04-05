using UnityEngine;

namespace Framework.DataStructures
{
    /// <summary>
    /// Used to read a path segment by segment.
    /// </summary>
    public class PathReader
    {
        public enum LoopModeEnum
        {
            None,
            Loop,
            PingPong
        }
    }

    public class PathReader<TNumeric> : PathReader
    {
        readonly Path<TNumeric> _path;

        [SerializeField] protected int _index;

        [SerializeField] protected LoopModeEnum _loopMode;

        protected int _increment = 1;

        public Path<TNumeric> Path
        {
            get => _path;
        }

        public LoopModeEnum LoopMode
        {
            get => this._loopMode;
            set => this._loopMode = value;
        }

        public int Index => this._index;

        public PathReader(Path<TNumeric> path)
        {
            this._path = path;
        }

        public bool IsOnEdge()
        {
            return this._index == 0 || this._index == this._path.Positions.Count - 1;
        }

        public bool IsOver()
        {
            switch (this._loopMode)
            {
                case LoopModeEnum.None:
                    {
                        return this._index == this._path.Positions.Count - 1;
                    }

                case LoopModeEnum.Loop:
                case LoopModeEnum.PingPong:
                    {
                        return false;
                    }
                    
                default:
                    {
                        Debug.LogError($"{this._loopMode} is not handled");
                        return false;
                    }
            }
        }

        public TNumeric GetCurrentPosition()
        {
            return this._path.Positions[this._index];
        }

        public bool MoveToNext()
        {
            int lastIndex = this._path.Positions.Count - 1;

            switch (this._loopMode)
            {
                case LoopModeEnum.None:
                    {
                        bool isLastIndex = this._index == lastIndex;

                        if (!isLastIndex)
                        {
                            this._increment = 1;
                            this._index += this._increment;
                        }
                        
                        return !isLastIndex;
                    }
                    
                case LoopModeEnum.Loop:
                    {
                        if (this._index < lastIndex)
                        {
                            this._increment = 1;
                            this._index += this._increment;
                        }
                        else
                        {
                            this._index = 0;
                        }

                        return true;
                    }

                case LoopModeEnum.PingPong:
                    {
                        if (this._index == lastIndex || this._index == 0)
                        {
                            this._increment = this._index == lastIndex ? -1 : 1;
                            this._index += this._increment;
                        }
                        else
                        {
                            this._index += this._increment;
                        }

                        return true;
                    }

                default:
                    {
                        Debug.LogError($"{this._loopMode} is not handled");
                        return false;
                    }
            }
        }
    }
}
