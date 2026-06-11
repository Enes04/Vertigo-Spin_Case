// Scripts/UI/InventoryItemView.cs
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
        // Setup fonksiyonunun üstüne bir yere ekleyebilirsin
        public Sprite ItemIcon => ui_image_icon.sprite;
        public int CurrentAmount => _currentAmount;
        
        
        private int _currentAmount = 0;

        // İlk yaratıldığında çalışacak kurulum
        public void Setup(Sprite icon, int initialAmount)
        {
            ui_image_icon.sprite = icon;
            _currentAmount = initialAmount;
            UpdateUI();
        }

        // Aynı eşyadan tekrar kazanıldığında sadece miktarı artıracak
        public void AddAmount(int amountToAdd)
        {
            _currentAmount += amountToAdd;
            UpdateUI();
            
            // Sayı arttığında tatlı bir büyüme/küçülme efekti verelim (Juice!)
            transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        }

        private void UpdateUI()
        {
            ui_text_amount.text = "x" + _currentAmount.ToString();
        }
    }
}