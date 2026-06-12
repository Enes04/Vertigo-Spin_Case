using System;
using UnityEngine;
using DG.Tweening;
using CardGame.Core;
using UnityEngine.UI;


namespace CardGame.UI
{
    public class WheelView : MonoBehaviour
    {
        [Header("Wheel Settings")] [SerializeField]
        private RectTransform ui_image_wheel_base;

        [SerializeField]
        private Image ui_image_spin_indicator;
        [SerializeField] private int totalSlices = 8;
        [SerializeField] private float spinDuration = 3f;
        [SerializeField] private int extraSpins = 5;
        [SerializeField] private WheelSliceView[] sliceViews;
        public WheelSlice[] CurrentActiveSlices { get; private set; }


        private bool _isSpinning = false;

        public void SetupWheelVisuals(WheelConfig config)
        {
            _isSpinning = true;

            ui_image_wheel_base.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                if (config == null)
                {
                    _isSpinning = false;
                    return;
                }

                ui_image_wheel_base.eulerAngles = Vector3.zero;

                if (config.wheelBackgroundSprite != null)
                {
                    ui_image_wheel_base.GetComponent<UnityEngine.UI.Image>().sprite = config.wheelBackgroundSprite;
                    ui_image_spin_indicator.sprite = config.spinIndicator;
                }
                    

                CurrentActiveSlices = config.GenerateRandom8Slices();

                if (CurrentActiveSlices.Length != sliceViews.Length)
                {
                   
                    _isSpinning = false;
                    return;
                }

                for (int i = 0; i < sliceViews.Length; i++)
                    sliceViews[i].Setup(CurrentActiveSlices[i]);

                ui_image_wheel_base.transform.DOScale(Vector3.one, 0.1f)
                    .OnComplete(() => _isSpinning = false); 
            });
        }

        public void SpinToSlice(int targetSliceIndex, Action onComplete)
        {
            if (_isSpinning) return;
            _isSpinning = true;

            float sliceAngle = 360f / totalSlices;
            float targetFinalAngle = targetSliceIndex * sliceAngle;

            float currentAngle = ui_image_wheel_base.eulerAngles.z;
            if (currentAngle > 180f) currentAngle -= 360f;

            float delta = targetFinalAngle - currentAngle;
            delta -= 360f * extraSpins;

            ui_image_wheel_base.DORotate(
                    new Vector3(0, 0, ui_image_wheel_base.eulerAngles.z + delta),
                    spinDuration,
                    RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuart)
                .OnComplete(() =>
                {
                    _isSpinning = false;
                    float z = ui_image_wheel_base.eulerAngles.z % 360f;
                    ui_image_wheel_base.eulerAngles = new Vector3(0, 0, z);


                    onComplete?.Invoke();
                });
        }
    }
}