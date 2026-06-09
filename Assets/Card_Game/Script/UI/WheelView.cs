using System;
using UnityEngine;
using DG.Tweening;
using CardGame.Core;


namespace CardGame.UI
{
    public class WheelView : MonoBehaviour
    {
        [Header("Wheel Settings")] [SerializeField]
        private RectTransform ui_image_wheel_base; 

        [SerializeField] private int totalSlices = 8;
        [SerializeField] private float spinDuration = 3f; 
        [SerializeField] private int extraSpins = 5; 
        [SerializeField] private WheelSliceView[] sliceViews;
        public WheelSlice[] CurrentActiveSlices { get; private set; }
        
        
        private bool _isSpinning = false;

        public void SetupWheelVisuals(WheelConfig config)
        {
            if (config == null) return;

            // YENİ EKLENEN SATIR: Çarkın arka plan resmini Config'den gelen resimle değiştir
            if (config.wheelBackgroundSprite != null)
            {
                ui_image_wheel_base.GetComponent<UnityEngine.UI.Image>().sprite = config.wheelBackgroundSprite;
            }

            // 1. Config havuzundan rastgele 8 adet dilim üret
            CurrentActiveSlices = config.GenerateRandom8Slices();

            // 2. Güvenlik kontrolü: Üretilen sayı ile UI elemanı sayısı uyuyor mu?
            if (CurrentActiveSlices.Length != sliceViews.Length)
            {
                Debug.LogError("Uyarı: Üretilen dilim sayısı ile sahnedeki dilim UI sayısı eşleşmiyor!");
                return;
            }

            // 3. Üretilen taze 8'li menüyü sahnedeki UI dilimlerine yerleştir
            for (int i = 0; i < sliceViews.Length; i++)
            {
                sliceViews[i].Setup(CurrentActiveSlices[i]);
            }
        }
        
        public void SpinToSlice(int targetSliceIndex, Action onComplete)
        {
            if (_isSpinning) return;
            _isSpinning = true;

          
            float sliceAngle = 360f / totalSlices;

        
            float targetAngle = -(targetSliceIndex * sliceAngle);

          
            float totalRotation = targetAngle - (360f * extraSpins);

       
            ui_image_wheel_base.DORotate(new Vector3(0, 0, totalRotation), spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuart)
                .OnComplete(() =>
                {
                    _isSpinning = false;

                
                    Vector3 currentEuler = ui_image_wheel_base.eulerAngles;
                    ui_image_wheel_base.eulerAngles = new Vector3(0, 0, currentEuler.z % 360f);

                  
                    onComplete?.Invoke();
                });
        }
    }
}