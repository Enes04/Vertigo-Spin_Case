// Scripts/GameLogic/GameManager.cs
using UnityEngine;
using UnityEngine.UI;
using CardGame.Core;
using CardGame.UI;

namespace CardGame.GameLogic
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private WheelView wheelView;
        [SerializeField] private Button spinButton; // Çevirme butonumuz

        [Header("Çark Veri Havuzları (Configs)")]
        [SerializeField] private WheelConfig normalWheelPool;
        [SerializeField] private WheelConfig silverWheelPool;
        [SerializeField] private WheelConfig goldWheelPool;
        [SerializeField] private RewardPopupView rewardPopupView;
        [SerializeField] private ZoneScrollerView zoneScrollerView;
        [SerializeField] private InventoryPanelView inventoryPanelView; // YENİ EKLENDİ
        // Arka plan yöneticilerimiz
        private ZoneManager _zoneManager;
        private SpinCalculator _spinCalculator;

        private void Start()
        {
          
            _zoneManager = new ZoneManager();
            _spinCalculator = new SpinCalculator();
            _zoneManager.OnZoneChanged += HandleZoneChanged;
            
            spinButton.onClick.AddListener(OnSpinButtonClicked);

            PrepareWheelForCurrentZone();
            
            
        }
        private void HandleZoneChanged(int newZone, ZoneType zoneType)
        {
           
            if (zoneScrollerView != null)
            {
                zoneScrollerView.AdvanceOneZone();
            }
        }
        private void PrepareWheelForCurrentZone()
        {
            WheelConfig currentConfig = normalWheelPool;

            // ZoneManager'a soruyoruz: Şu an kaçıncı bölgedeyiz ve tipi ne?
            ZoneType currentZoneType = _zoneManager.GetCurrentZoneType();
            
            if (currentZoneType == ZoneType.Safe) 
            {
                currentConfig = silverWheelPool;
            }
            else if (currentZoneType == ZoneType.Super) 
            {
                currentConfig = goldWheelPool;
            }

            // Seçilen havuzdan rastgele 8'li üret ve UI'a diz
            wheelView.SetupWheelVisuals(currentConfig);
        }

        private void OnSpinButtonClicked()
        {
            // 1. Oyuncu peş peşe basamasın diye butonu hemen kilitle
            spinButton.interactable = false;

            // 2. Çarkın üzerindeki aktif 8 dilimi hesaplayıcıya gönderip kazananı seç
            var spinResult = _spinCalculator.CalculateSpinResult(wheelView.CurrentActiveSlices);

            Debug.Log($"Çark dönüyor... Hedef İndeks: {spinResult.index}");

            // 3. UI'a "Şu indekse dön" komutu ver ve animasyon bittiğinde OnSpinCompleted'i çağır
            wheelView.SpinToSlice(spinResult.index, () => 
            {
                OnSpinCompleted(spinResult.slice);
            });
        }

        private void OnSpinCompleted(WheelSlice winningSlice)
        {
        
            // Ödül Bomba mı kontrol et
            if (winningSlice.reward.isBomb)
            {
                _zoneManager.HandleBombHit();
                inventoryPanelView.ClearInventory();
            }
            else
            {
                _zoneManager.AdvanceZone();
                
                rewardPopupView.ShowReward(winningSlice, () => 
                {
                    inventoryPanelView.AddReward(winningSlice.reward);
                    PrepareWheelForCurrentZone();
                    spinButton.interactable = true;
                });
            }

            // Animasyon ve işlemler bitti, butonu yeni tur için aktif et
         
        }

        private void OnDestroy()
        {
            // Obje silinirken memory leak (bellek sızıntısı) olmaması için dinleyiciyi kaldırıyoruz
            if (spinButton != null)
            {
                spinButton.onClick.RemoveListener(OnSpinButtonClicked);
            }
            
            if (_zoneManager != null)
            {
                _zoneManager.OnZoneChanged -= HandleZoneChanged;
            }
        }
    }
}