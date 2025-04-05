using Framework.Managers;
using UnityEngine;

namespace Framework.Particles
{
    public class OrphanPS : ParticleSystem<OrphanPS>
    {
        private ParticleSystemManager _particleSystemManager;

        public void Bind(ParticleSystemManager particleSystemManager)
        {
            this._particleSystemManager = particleSystemManager;
        }

        public void Unbind()
        {
            this._particleSystemManager = null;
        }

        public void BindParent(Transform parent)
        {
            this.transform.parent = parent;
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
        }

        public void UnbindParent()
        {
            this.transform.parent = this._particleSystemManager.transform;
            this._ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
