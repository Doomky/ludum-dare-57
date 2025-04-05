using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Framework.Particles
{
    public abstract class ParticleSystem<TPS> : MonoBehaviour where TPS : ParticleSystem<TPS>
    {
        [SerializeField, Required]
        protected ParticleSystem _ps;

        public ParticleSystem PS => this._ps;

        public event Action<TPS> ParticleSystemStopped;

        private void OnParticleSystemStopped()
        {
            this.ParticleSystemStopped?.Invoke((TPS)this);
        }
    }
}
