using Player;
using System;

namespace Effects
{
    [Serializable]
    public enum EffectType
    {
        Attack,
        Heal,
        Poison,
        Protection
    }

    public abstract class Effect : IEffect
    {
        public virtual EffectType Type => EffectType.Attack;

        public Effect(int effectPower, int stepsCount) { }

        public virtual event IEffect.OnEffectFinishedEvent OnFinished;

        public virtual void Run(HPController hPController) { OnFinished?.Invoke(); }

        public virtual void IncreaseStepsCount() { }

        public virtual void ForceComplete() { }
    }
}
