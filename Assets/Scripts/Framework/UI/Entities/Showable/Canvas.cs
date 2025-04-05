using Framework.Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public abstract class Canvas : Entity, ICanvas
    {
        [FoldoutGroup("Main"), HideInPlayMode, SerializeField]
        private List<PrefabReference<WindowGroup>> _windowGroupPrefabs;

        [FoldoutGroup("Main"), ShowInInspector, HideInEditorMode]
        private List<WindowGroup> _windowGroups = new();

        [FoldoutGroup("Animations")]
        [SerializeField]
        private ShowableAnimationStateMachine _stateMachine = new();

        public IReadOnlyList<WindowGroup> WindowGroups => this._windowGroups;

        public abstract void Load();

        public abstract void Unload();

        public void LoadWindowGroups()
        {
            int windowGroupsCount = this._windowGroupPrefabs.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                PrefabReference<WindowGroup> windowGroupPrefab = this._windowGroupPrefabs[i];
                WindowGroup windowGroup = windowGroupPrefab.InstantiateAtLocalPosition(windowGroupPrefab.Prefab.transform.localPosition, this.transform);

                this._windowGroups.Add(windowGroup);
            }

            for (int i = 0; i < windowGroupsCount; i++)
            {
                this._windowGroups[i].Load();
            }

            this._stateMachine.EnterState += this.AnimationStateMachine_EnterState;
            this._stateMachine.ExitState += this.AnimationStateMachine_ExitState;
        }

        public void UnloadWindowGroups()
        {
            this._stateMachine.ExitState -= this.AnimationStateMachine_ExitState;
            this._stateMachine.EnterState -= this.AnimationStateMachine_EnterState;

            int windowGroupsCount = this._windowGroups.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                this._windowGroups[i].Unload();
            }
        }

        public void UpdateVisibility()
        {
            int windowGroupsCount = this._windowGroups.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                this._windowGroups[i].UpdateVisibility();
            }
        }

        public T GetWindow<T>()
        {
            int windowGroupsCount = this._windowGroups.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                T window = this._windowGroups[i].GetWindow<T>();
                if (window != null)
                {
                    return window;
                }
            }

            return default;
        }

        private void AnimationStateMachine_EnterState(StateMachine.EnumStateMachine<ShowableAnimationStateMachine.State, ShowableAnimationStateMachine.Action> action, ShowableAnimationStateMachine.State state)
        {
            switch (state)
            {
                case ShowableAnimationStateMachine.State.Hidden:
                    {
                        this.transform.gameObject.SetActive(false);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void AnimationStateMachine_ExitState(StateMachine.EnumStateMachine<ShowableAnimationStateMachine.State, ShowableAnimationStateMachine.Action> action, ShowableAnimationStateMachine.State state)
        {
            switch (state)
            {
                case ShowableAnimationStateMachine.State.Hidden:
                    {
                        this.transform.gameObject.SetActive(true);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}