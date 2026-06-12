using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace CardGame.UI
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private Image ui_image_icon;
        [SerializeField] private TextMeshProUGUI ui_text_amount;
        public Sprite ItemIcon => ui_image_icon.sprite;
        public int CurrentAmount => _currentAmount;
        
        
        private int _currentAmount = 0;

        public void Setup(Sprite icon, int initialAmount)
        {
            ui_image_icon.sprite = icon;
            _currentAmount = initialAmount;
            UpdateUI();
        }

        public void AddAmount(int amountToAdd)
        {
            _currentAmount += amountToAdd;
            UpdateUI();
            
            transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        }

        private void UpdateUI()
        {
            ui_text_amount.text = "x" + _currentAmount.ToString();
        }
    }
}