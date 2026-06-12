using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class ExitSummaryView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject popupRoot; 
        [SerializeField] private Transform container; 
        [SerializeField] private InventoryItemView summaryCardPrefab; 
        [SerializeField] private Button collectButton;
         [SerializeField] private Button returnButtonn;
        private Action _onCollectCallback;

        private void Start()
        {
            popupRoot.SetActive(false); 
            collectButton.onClick.AddListener(OnCollectClicked);
            returnButtonn.onClick.AddListener(OnReturnClicked);
        }

        public void ShowSummary(List<(Sprite icon, int amount)> items, Action onCollect)
        {
            _onCollectCallback = onCollect;

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in items)
            {
                InventoryItemView newCard = Instantiate(summaryCardPrefab, container);
                newCard.Setup(item.icon, item.amount);
            }

            popupRoot.SetActive(true);
        }

        private void OnCollectClicked()
        {
            popupRoot.SetActive(false);
            _onCollectCallback?.Invoke(); 
        }

        private void OnReturnClicked()
        {
            popupRoot.SetActive(false);
        }
    }
}