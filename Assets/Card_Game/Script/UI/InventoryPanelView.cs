// Scripts/UI/InventoryPanelView.cs
using System.Collections.Generic;
using UnityEngine;
using CardGame.Core;

namespace CardGame.UI
{
    public class InventoryPanelView : MonoBehaviour
    {
        [Header("Referanslar")]
        [Tooltip("İçinde Vertical Layout Group olan sol panelimiz")]
        [SerializeField] private RectTransform container; 
        [SerializeField] private InventoryItemView itemPrefab;

        // Aynı eşyadan var mı diye kontrol etmek için Sözlük kullanıyoruz
        // Key: Ödül Tipi (Gold, Cash vb.), Value: Ekrandaki o satırın kendisi
        private Dictionary<RewardData, InventoryItemView> _inventoryItems = new Dictionary<RewardData, InventoryItemView>();

        public void AddReward(RewardData reward)
        {
            if (reward == null || reward.isBomb) return;

            // Eğer bu ödülden (örneğin Gold) daha önce listeye eklediysek:
            if (_inventoryItems.ContainsKey(reward))
            {
                // Sadece miktarını artır
                _inventoryItems[reward].AddAmount(reward.amount);
            }
            else
            {
                // İlk defa kazanıyorsak, yeni bir satır (Prefab) üret
                InventoryItemView newItem = Instantiate(itemPrefab, container);
                newItem.Setup(reward.rewardIcon, reward.amount);
                
                // Sözlüğe ekle ki bir sonrakinde bulabilelim
                _inventoryItems.Add(reward, newItem);
            }
        }

        public List<(Sprite icon, int amount)> GetCollectedItems()
        {
            List<(Sprite icon, int amount)> collected = new List<(Sprite icon, int amount)>();
            foreach (var item in _inventoryItems.Values)
            {
                if (item != null)
                {
                    collected.Add((item.ItemIcon, item.CurrentAmount));
                }
            }
            return collected;
        }
        public void ClearInventory()
        {
            foreach (var item in _inventoryItems.Values)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            _inventoryItems.Clear();
        }
    }
}