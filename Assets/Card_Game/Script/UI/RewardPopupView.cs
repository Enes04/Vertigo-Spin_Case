using System;
using CardGame.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CardGame.UI
{
    public class RewardPopupView : MonoBehaviour
    {
        [Header("Ana UI Referansları")]
   

        [SerializeField] private RectTransform cardRoot; // Ortada beliren kartın kendisi
        [SerializeField] private RectTransform shineEffect; // Arkada dönen parlama efekti
        [SerializeField] private RectTransform ui_middle_root;
        [Header("Kart İçeriği")] [SerializeField]
        private Image ui_image_card_icon;

        [SerializeField] private TextMeshProUGUI ui_text_card_amount;

        [Header("Uçma Efekti Ayarları")]
        
        [SerializeField] private GameObject flyingIconPrefab; // Uçacak olan küçük ikon prefab'i (İçinde sadece Image bileşeni olmalı)
        
        [SerializeField]
        private RectTransform flyTargetLeft; // İkonların uçacağı sol taraftaki hedef (Örn: Envanter butonu)

        [SerializeField]
        private int maxFlyingIcons = 10; // Ekranda aynı anda uçacak maksimum ikon sayısı (Optimizasyon için)

  
        public void ShowReward(WheelSlice winningSlice, Action onComplete)
        {
            if (winningSlice == null || winningSlice.reward == null)
            {
                onComplete?.Invoke();
                return;
            }

            // 1. UI İçeriğini Doldur
            ui_image_card_icon.sprite = winningSlice.reward.rewardIcon;
            ui_text_card_amount.text = "x" + winningSlice.reward.amount;

       
          
            cardRoot.localScale = Vector3.zero;

            // Arkadaki parlama efektini sonsuza kadar döndür
            shineEffect.DORotate(new Vector3(0, 0, -360), 6f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);

            // 3. Kartı "Pop" efektiyle (OutBack) büyüt
            cardRoot.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                // Kart ekranda 1 saniye kalsın, oyuncu ne kazandığını görsün, sonra uçma efektini başlat
                DOVirtual.DelayedCall(1f, () => { StartFlyingEffect(winningSlice, onComplete); });
            });
        }

        private void StartFlyingEffect(WheelSlice winningSlice, Action onComplete)
        {
            // Kartı geri küçülterek yok et
            cardRoot.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
            shineEffect.DOKill(); // Parlama dönmesini durdur

            // Optimizasyon: Çıkan miktar ile maksimum uçacak ikon sayısını kıyasla (Hangisi küçükse onu seç)
            int spawnCount = Mathf.Min(winningSlice.reward.amount, maxFlyingIcons);
            int completedIcons = 0;

            for (int i = 0; i < spawnCount; i++)
            {
                // İkonu üret ve merkeze koy
                GameObject flyingIcon = Instantiate(flyingIconPrefab, ui_middle_root.transform);
                flyingIcon.GetComponent<Image>().sprite = winningSlice.reward.rewardIcon;

                RectTransform rect = flyingIcon.GetComponent<RectTransform>();
                rect.position = ui_middle_root.position;
                rect.localScale = Vector3.zero;

                // Rastgele dağılma noktası bul (Kartın etrafında saçılacaklar)
                Vector2 randomOffset = Random.insideUnitCircle * 150f;
                Vector3 burstPosition = rect.localPosition + (Vector3)randomOffset;

                // DOTween Sequence (Sıralı Animasyon) oluştur
                Sequence seq = DOTween.Sequence();

                // Adım 1: Büyüyerek rastgele noktalara saçıl (Patlama hissi)
                seq.Append(rect.DOLocalMove(burstPosition, 0.3f).SetEase(Ease.OutQuad));
                seq.Join(rect.DOScale(Vector3.one * 0.7f, 0.3f).SetEase(Ease.OutBack));

                // Adım 2: Rastgele ufak bir bekleme süresi (Hepsi aynı anda uçmasın, kuyruk olsun)
                seq.AppendInterval(Random.Range(0f, 0.2f));

                // Adım 3: Sol hedefe doğru uç
                seq.Append(rect.DOMove(flyTargetLeft.position, 0.6f).SetEase(Ease.InBack));
                seq.Join(rect.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InBack)); // Giderken küçül

                // Adım 4: İşlem bitince objeyi yok et ve sayacı artır
                seq.OnComplete(() =>
                {
                    Destroy(flyingIcon);
                    completedIcons++;

                    // Tüm uçan ikonlar hedefine ulaştıysa popup'ı kapat ve oyuna devam et
                    if (completedIcons == spawnCount)
                    {
                     
                        onComplete?.Invoke();
                    }
                });
            }
        }
    }
}