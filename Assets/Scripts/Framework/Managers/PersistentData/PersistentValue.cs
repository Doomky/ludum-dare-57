using UnityEngine;

namespace Framework.Managers
{
    [System.Serializable]
    public class PersistentValue
    {
        [SerializeField]
        public object Value;

        public PersistentValue(object value)
        {
            this.Value = value;
        }
    }
}
