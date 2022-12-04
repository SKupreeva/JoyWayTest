using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    // class controls character's ui changes, effect's ui changes

    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private Image _healthFill;
        [SerializeField] private TextMeshProUGUI _healthPoints;
        [SerializeField] private RectTransform _protectionContainer;
        [SerializeField] private TextMeshProUGUI _protectionPoints;
        [SerializeField] private Image _poisonIcon;

        private int _maxHpValue = 0;

        public void Setup(int maxHpValue)
        {
            _maxHpValue = maxHpValue;

            _healthFill.gameObject.SetActive(true);
            _protectionContainer.gameObject.SetActive(false);
            _poisonIcon.gameObject.SetActive(false);

            ChangeHPValue(maxHpValue, 0);
        }

        public void ChangeHPValue(int healthPoints, int protectionHP)
        {
            _healthFill.fillAmount = (float)healthPoints / _maxHpValue;
            _healthPoints.text = healthPoints.ToString();
            _protectionPoints.text = protectionHP.ToString();
        }

        public void AddProtection(int protectionValue)
        {
            _protectionContainer.gameObject.SetActive(true);
            _protectionPoints.text = protectionValue.ToString();
        }

        public void RemoveProtection()
        {
            _protectionContainer.gameObject.SetActive(false);
        }

        public void AddPoison()
        {
            _poisonIcon.gameObject.SetActive(true);
        }

        public void RemovePoison()
        {
            _poisonIcon.gameObject.SetActive(false);
        }
    }
}
