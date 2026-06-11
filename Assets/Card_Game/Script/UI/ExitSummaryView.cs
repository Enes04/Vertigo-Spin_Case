// Scripts/UI/ExitSummaryView.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class ExitSummaryView : MonoBehaviour
    {
        [Header("Referanslar")]
        [SerializeField] private GameObject popupRoot; // Ekranı karartan ana obje
        [SerializeField] private Transform container; // Kartların dizileceği yer (Grid Layout Group)
        [SerializeField] private InventoryItemView summaryCardPrefab; // Daha önce sol panel için yaptığımız prefab'ı kullanabiliriz!
        [SerializeField] private Button collectButton; // Hepsini Topla butonu

        private Action _onCollectCallback;

        private void Start()
        {
            popupRoot.SetActive(false); // Başlangıçta gizli
            collectButton.onClick.AddListener(OnCollectClicked);
        }

        public void ShowSummary(List<(Sprite icon, int amount)> items, Action onCollect)
        {
            _onCollectCallback = onCollect;

            // Önce içerideki eski kartları temizle
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            // Envanterden gelen verilerle ekrana kartları diz
            foreach (var item in items)
            {
                InventoryItemView newCard = Instantiate(summaryCardPrefab, container);
                newCard.Setup(item.icon, item.amount);
            }

            // Paneli görünür yap
            popupRoot.SetActive(true);
        }

        private void OnCollectClicked()
        {
            popupRoot.SetActive(false);
            _onCollectCallback?.Invoke(); // GameManager'a "Tamam, oyuncu paraları aldı oyunu sıfırla" mesajı gönder
        }
    }
}