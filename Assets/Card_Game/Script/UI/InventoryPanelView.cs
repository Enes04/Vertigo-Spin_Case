using System.Collections.Generic;
using UnityEngine;
using CardGame.Core;

namespace CardGame.UI
{
    public class InventoryPanelView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform container; 
        [SerializeField] private InventoryItemView itemPrefab;

     
        private Dictionary<string, InventoryItemView> _inventoryItems = new Dictionary<string, InventoryItemView>();
        public void AddReward(RewardData reward)
        {
            if (reward == null || reward.isBomb) return;

            if (_inventoryItems.ContainsKey(reward.name))
            {
                _inventoryItems[reward.name].AddAmount(reward.amount);
            }
            else
            {
                InventoryItemView newItem = Instantiate(itemPrefab, container);
                newItem.Setup(reward.rewardIcon, reward.amount);
                
                _inventoryItems.Add(reward.name, newItem);
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