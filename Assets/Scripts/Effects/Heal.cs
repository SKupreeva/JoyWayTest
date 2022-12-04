using Player;

namespace Effects
{
    public class Heal : Effect
    {
        private int _healPower;
        private int _stepsNumber;
        private int _stepsCount;
        private HPController _currentHPController;

        public override EffectType Type => EffectType.Heal;
        public override event IEffect.OnEffectFinishedEvent OnFinished;

        public Heal(int effectPower, int stepsNumber) : base(effectPower, stepsNumber)
        {
            _healPower = effectPower;
            _stepsNumber = stepsNumber;
        }

        public override void Run(HPController hpController)
        {
            _currentHPController = hpController;
            _currentHPController.ApplyHeal(_healPower);
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
            _currentHPController.ApplyHeal(_healPower);
        }
    }
}
