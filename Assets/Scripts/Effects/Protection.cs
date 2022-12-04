using Player;

namespace Effects
{
    public class Protection : Effect
    {
        private int _protectionPower;
        private int _stepsNumber;
        private int _stepsCount;
        private HPController _currentHPController;

        public override EffectType Type => EffectType.Protection;
        public override event IEffect.OnEffectFinishedEvent OnFinished;

        public Protection(int effectPower, int stepsNumber) : base(effectPower, stepsNumber)
        {
            _protectionPower = effectPower;
            _stepsNumber = stepsNumber;
        }

        public override void Run(HPController hpController)
        {
            _currentHPController = hpController;
            _currentHPController.ApplyProtection(_protectionPower);
            _stepsCount = 1;
        }

        public override void IncreaseStepsCount()
        {
            if (_stepsCount >= _stepsNumber)
            {
                _currentHPController.RemoveProtection();
                OnFinished?.Invoke();
                return;
            }
            _stepsCount++;
        }

        public override void ForceComplete()
        {
            _stepsCount = _stepsNumber;
            _currentHPController.RemoveProtection();
            OnFinished?.Invoke();
        }
    }
}
