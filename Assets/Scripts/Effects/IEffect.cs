using Player;
using System;

namespace Effects
{
    public interface IEffect
    {
        delegate void OnEffectFinishedEvent();
        public event OnEffectFinishedEvent OnFinished;

        public void Run(HPController hPController);

        public void IncreaseStepsCount();

        public void ForceComplete();
    }
}
