using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.Managers
{
    [GUIColor(0.5f, 1, 0.5f, 1)]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public class PersistentField<T> : IPersistentField<T>
    {
        [HideInEditorMode]
        [ShowInInspector]
        private T _value;

        [HideLabel]
        [HideInPlayMode]
        [SerializeField]
        private T _defaultValue;

        public T Value 
        {
            get
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    return this._defaultValue;
                }
                else
                {
                    return this._value;
                }
            }
            set
            {
                try
                {
                    if (Application.isEditor && !Application.isPlaying)
                    {
                        this._defaultValue = value;
                    }
                    else
                    {
                        this._value = value;
                    }
                }
                catch (System.Exception)
                {
                    DebugHelper.LogError(this, $"Failed to set value of type {value.GetType()} to {typeof(T).Name}");
                }
            }
        }

        object IPersistentField.Value
        { 
            get => this.Value; 
            set => this.Value = (T)value; 
        }

        public PersistentField(T defaultValue)
        {
            this._defaultValue = defaultValue;
            this._value = defaultValue;
        }

        public static implicit operator T(PersistentField<T> persitentDataField)
        {
            return persitentDataField._value;
        }

        public void Init()
        {
            this._value = this._defaultValue;
        }

        public void Set(T t)
        {
            this._value = t;
        }
    }
}
