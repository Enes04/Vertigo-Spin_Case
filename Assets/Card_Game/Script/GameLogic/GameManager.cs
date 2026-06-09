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

        // Arka plan yöneticilerimiz
        private ZoneManager _zoneManager;
        private SpinCalculator _spinCalculator;

        private void Start()
        {
            // Yöneticileri "new" anahtar kelimesiyle ayağa kaldırıyoruz (Memory'de yaratıyoruz)
            _zoneManager = new ZoneManager();
            _spinCalculator = new SpinCalculator();

            // DOKÜMAN KURALI: Unity Event (Inspector) yerine kodu dinliyoruz!
            spinButton.onClick.AddListener(OnSpinButtonClicked);

            // Oyuna başlarken ilk çarkı hazırla
            PrepareWheelForCurrentZone();
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
                Debug.Log("💥 BOMBAYA BASILDI! (Burada Revive/Pes Et ekranı açılacak)");
                _zoneManager.HandleBombHit();
            }
            else
            {
                Debug.Log($"🎉 KAZANDIN: {winningSlice.reward.amount} {winningSlice.reward.rewardType}");
                
                // Bölgeyi 1 ilerlet
                _zoneManager.AdvanceZone();
                
                // Bir sonraki tur için çarkın içindeki 8'liyi taze ve yeni kurallara göre hazırla
                PrepareWheelForCurrentZone();
            }

            // Animasyon ve işlemler bitti, butonu yeni tur için aktif et
            spinButton.interactable = true;
        }

        private void OnDestroy()
        {
            // Obje silinirken memory leak (bellek sızıntısı) olmaması için dinleyiciyi kaldırıyoruz
            if (spinButton != null)
            {
                spinButton.onClick.RemoveListener(OnSpinButtonClicked);
            }
        }
    }
}