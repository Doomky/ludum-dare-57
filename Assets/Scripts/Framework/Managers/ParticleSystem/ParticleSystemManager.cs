using Framework.Core;
using Framework.Particles;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{
    public abstract partial class ParticleSystemManager : Manager
    {
        [ShowInInspector, HideInEditorMode]
        private SuperPool<BurstPS> _bpsSuperPool = new();

        [ShowInInspector, HideInEditorMode]
        private SuperPool<OrphanPS> _opsSuperPool = new();

        [ShowInInspector, HideInEditorMode]
        private SuperPool<ContinuousPS> _cpsSuperPool = new();

        [ShowInInspector, HideInEditorMode]
        private List<BurstPS> _bpsInstances = new();

        [ShowInInspector, HideInEditorMode]
        private List<OrphanPS> _opsInstances = new();

        [ShowInInspector, HideInEditorMode]
        private List<ContinuousPS> _cpsInstances = new();

        public void Spawn(PrefabReference<ContinuousPS> prefabReference, Transform parent, ref ContinuousPS continuousPS)
        {
            if (prefabReference == null || !prefabReference.HasPrefab())
            {
                return;
            }

            this._cpsSuperPool.Allocate(prefabReference, transform.position, parent, ref continuousPS);

            this._cpsInstances.Add(continuousPS);
        }

        public void Spawn(PrefabReference<OrphanPS> prefabReference, Transform parent, ref OrphanPS orphanPS)
        {
            if (prefabReference == null || !prefabReference.HasPrefab())
            {
                return;
            }

            this._opsSuperPool.Allocate(prefabReference, transform.position, transform, ref orphanPS);

            orphanPS.Bind(this);
            orphanPS.BindParent(parent);
            orphanPS.transform.localRotation = Quaternion.identity;
            orphanPS.ParticleSystemStopped += this.OnOrphanPSStopped;
        }

        public void Spawn(PrefabReference<BurstPS> prefabReference, Vector2 position)
        {
            if (prefabReference == null || !prefabReference.HasPrefab())
            {
                return;
            }

            BurstPS burstPS = null;

            this._bpsSuperPool.Allocate(prefabReference, position, Quaternion.identity, this.transform, ref burstPS);

            burstPS.ParticleSystemStopped += this.OnBurstPSStopped;

            this._bpsInstances.Add(burstPS);
        }

        public void Spawn(PrefabReference<BurstPS> prefabReference, Vector2 position, Quaternion rotation)
        {
            if (prefabReference == null || !prefabReference.HasPrefab())
            {
                return;
            }

            BurstPS burstPS = null;

            this._bpsSuperPool.Allocate(prefabReference, position, rotation, this.transform, ref burstPS);

            burstPS.ParticleSystemStopped += this.OnBurstPSStopped;

            this._bpsInstances.Add(burstPS);
        }

        public void Spawn(PrefabReference<BurstPS> prefabReference, Vector2 position, ref BurstPS burstPS)
        {
            if (prefabReference == null)
            {
                return;
            }

            this._bpsSuperPool.Allocate(prefabReference, position, Quaternion.identity, this.transform, ref burstPS);

            burstPS.ParticleSystemStopped += this.OnBurstPSStopped;

            this._bpsInstances.Add(burstPS);
        }

        public void Spawn(PrefabReference<BurstPS> prefabReference, Vector2 position, Quaternion rotation, ref BurstPS burstPS)
        {
            if (prefabReference == null || !prefabReference.HasPrefab())
            {
                return;
            }

            this._bpsSuperPool.Allocate(prefabReference, position, rotation, this.transform, ref burstPS);

            burstPS.ParticleSystemStopped += this.OnBurstPSStopped;

            this._bpsInstances.Add(burstPS);
        }

        public void Despawn(ref ContinuousPS continuousPS)
        {
            this._cpsInstances.Remove(continuousPS);

            this._cpsSuperPool.Release(ref continuousPS, this.transform);
        }

        public void Despawn(ref BurstPS burstPS)
        {
            this._bpsInstances.Remove(burstPS);

            burstPS.ParticleSystemStopped -= this.OnBurstPSStopped;

            this._bpsSuperPool.Release(ref burstPS, this.transform);
        }

        public void Despawn(OrphanPS orphanPS)
        {
            this._opsInstances.Remove(orphanPS);

            orphanPS.ParticleSystemStopped -= this.OnOrphanPSStopped;
            orphanPS.Unbind();

            this._opsSuperPool.Release(ref orphanPS, this.transform);
        }

        private void OnBurstPSStopped(BurstPS ps)
        {
            this.Despawn(ref ps);
        }

        private void OnOrphanPSStopped(OrphanPS ps)
        {
            this.Despawn(ps);
        }
    }
}
