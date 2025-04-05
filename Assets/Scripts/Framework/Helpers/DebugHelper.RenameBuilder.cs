using System.Collections.Generic;
using System.Text;

namespace Framework.Managers
{
    public static partial class DebugHelper
    {
        public class NameBuilder
        {
            private readonly StringBuilder _sb = new();

            private string _typeName = string.Empty;
            
            private readonly List<string> _flags = new ();

            public NameBuilder Start(object obj)
            {
                this._typeName = obj.GetType().Name;
                this._flags.Clear();
                return this;
            }

            public NameBuilder WithFlag(string flag)
            {
                this._flags.Add(flag);
                return this;
            }

            public string Build()
            {
                this._sb.Clear();

                if (!string.IsNullOrEmpty(this._typeName))
                {
                    this._sb.Append(this._typeName);
                }

                int count = this._flags.Count;
                for (int i = 0; i < count; i++)
                {
                    string flag = this._flags[i];

                    if (!string.IsNullOrEmpty(flag))
                    {
                        this._sb.Append(" | ");
                        this._sb.Append(flag);
                    }
                }

                return this._sb.ToString();
            }
        }
    }
}