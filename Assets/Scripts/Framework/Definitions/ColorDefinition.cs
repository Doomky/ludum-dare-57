using Framework.Databases;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.Definitions
{
    public class ColorDefinition : Definition<ColorDefinition, ColorID>
    {
        [BoxGroup("Main", Order = 0)]
        [SerializeField]
        private Color _color;
        
        public Color Color => this._color;
    }
}