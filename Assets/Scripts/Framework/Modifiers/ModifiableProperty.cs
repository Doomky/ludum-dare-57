using Framework.Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Modifiers
{
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    [GUIColor("#00FFCC")]
    public abstract class ModifiableProperty<T>
    {
        [VerticalGroup("Main")]

        [HideLabel]
        [PropertyRange(nameof(Min), nameof(Max))]
#if UNITY_EDITOR
        [BoxGroup("Main/Details", showLabel: false, Order = 1, VisibleIf = nameof(_editor_showDetails))]
#endif
        [HorizontalGroup("Main/Details/FirstRow")]
        [PropertyOrder(1)]
        [SerializeField]
        protected T _baseValue = default;

        [HideLabel]
        [HorizontalGroup("Main/Details/FirstRow")]
        [PropertyOrder(0)]
        [SerializeField]
        protected T _min = default;

        [HideLabel]
        [HorizontalGroup("Main/Details/FirstRow")]
        [PropertyOrder(2)]
        [SerializeField]
        protected T _max = default;

        [ReadOnly]
        [HideLabel]
        [HorizontalGroup("Main/Default")]
        [HideInEditorMode]
        [PropertyOrder(0)]
        [ShowInInspector]
        protected T _currentValue = default;

        [HorizontalGroup("Main/Details/Modifiers")]
        [HideLabel]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [HideInEditorMode]
        [ShowInInspector]
        protected List<IPropertyModifier> _modifiers = null;

        private bool _isDirty = true;

#if UNITY_EDITOR
        private bool _editor_showDetails = false;
#endif

        public T BaseValue
        {
            get => this._baseValue;
            set
            {
                if (!this._baseValue.Equals(value))
                {
                    this._baseValue = value;
                    this._isDirty = true;
                    this.ValueChanged?.Invoke(this.Get());
                }
            }
        }

        public virtual T Min
        {
            get => this._min;
            set
            {
                if (!this._min.Equals(value))
                {
                    this._min = value;
                    this._isDirty = true;
                    this.ValueChanged?.Invoke(this.Get());
                }
            }
        }

        public T Max
        {
            get => this._max;
            set
            {
                if (!this._max.Equals(value))
                {
                    this._max = value;
                    this._isDirty = true;
                    this.ValueChanged?.Invoke(this.Get());
                }
            }
        }

        public IReadOnlyList<IPropertyModifier> Modifiers => this._modifiers;

        public event Action<T> ValueChanged;

        public ModifiableProperty()
        {
        }

        public ModifiableProperty(T baseValue, T min, T max)
        {
            this._baseValue = baseValue;
            this._min = min;
            this._max = max;
        }

        public T Get()
        {
            if (this._isDirty)
            {
                this.Recompute();
                this._isDirty = false;
            }

            return this._currentValue;
        }

        public bool HasModifier(IPropertyModifier propertyModifier)
        {
            return this._modifiers != null && this._modifiers.Contains(propertyModifier);
        }

        public int AddModifier(IPropertyModifier modifier)
        {
            this._modifiers ??= new();

            modifier.ValueChanged += this.Modifier_OnValueChanged;
            
            this._modifiers.Add(modifier);
            this._isDirty = true;
            this.ValueChanged?.Invoke(this.Get());

            return this._modifiers.Count - 1;
        }

        public void RemoveModifier(IPropertyModifier modifier)
        {
            modifier.ValueChanged -= this.Modifier_OnValueChanged;

            this._modifiers.Remove(modifier);
            this._isDirty = true;
            this.ValueChanged?.Invoke(this.Get());
        }

        [HideInEditorMode]
        [ShowInInspector]
        [HorizontalGroup("Main/Details/Actions")]
        [Button]
        protected abstract void Recompute();

        private void Modifier_OnValueChanged(IPropertyModifier propertyModifier)
        {
            this._isDirty = true;
            this.ValueChanged?.Invoke(this.Get());
        }

#if UNITY_EDITOR
        [OnInspectorGUI]
        [HorizontalGroup("Main/Default")]
        [PropertyOrder(99)]
        private void DetailsButton()
        {
            this._editor_showDetails = GUILayoutHelpers.Toggle(this._editor_showDetails);
        }
#endif
    }
}
