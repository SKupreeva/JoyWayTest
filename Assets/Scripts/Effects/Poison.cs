using Player;

namespace Effects
{
    public class Poison : Effect
    {
        private int _poisonPower;
        private int _stepsNumber;
        private int _stepsCount;
        private HPController _currentHPController;

        public override EffectType Type => EffectType.Poison;
        public override event IEffect.OnEffectFinishedEvent OnFinished;

        public Poison(int effectPower, int stepsNumber) : base(effectPower, stepsNumber)
        {
            _poisonPower = effectPower;
            _stepsNumber = stepsNumber;
        }

        public override void Run(HPController hpController)
        {
            _currentHPController = hpController;
            _currentHPController.ApplyDamage(_poisonPower);
            _stepsCount = 1;
        }

        public override void IncreaseStepsCount()
        {
            if (_stepsCount >= _stepsNumber)
            {
                OnFinished?.Invoke();
                return;
            }
            _stepsCount++;
            _currentHPController.ApplyDamage(_poisonPower);
        }
    }
}
