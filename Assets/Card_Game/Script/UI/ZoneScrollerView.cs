using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CardGame.Core;

namespace CardGame.UI
{
    public class ZoneScrollerView : MonoBehaviour
    {
        [Header("Prefabs & Parents")] [SerializeField]
        private ZoneItemView zoneItemPrefab;

        [SerializeField] private RectTransform container;

        [Header("Settings")] [SerializeField] private int visibleItemCount = 8;
        [SerializeField] private int itemsToKeepOnLeft = 3;
        [SerializeField] private float itemWidth = 150f;
        [SerializeField] private float spacing = 20f;
        [SerializeField] private float animationDuration = 0.5f;

        private List<ZoneItemView> _activeItems = new List<ZoneItemView>();
        private int _currentHighestZone = 1;
        private float _stepDistance;


        private bool _isAnimating = false;

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            _stepDistance = itemWidth + spacing;
            _currentHighestZone = 1;


            int totalPoolSize = visibleItemCount + itemsToKeepOnLeft;

            for (int i = 0; i < totalPoolSize; i++)
            {
                ZoneItemView newItem = Instantiate(zoneItemPrefab, container);
                float startX = i * _stepDistance;
                newItem.RectTransform.anchoredPosition = new Vector2(startX, 0);

                ZoneType type = GetZoneType(_currentHighestZone);
                newItem.Setup(_currentHighestZone, type);

                _activeItems.Add(newItem);
                _currentHighestZone++;
            }
        }

        public void ResetScroller()
        {
            // 1. Devam eden animasyonlar varsa durdur ve spam kilidini aç
            _isAnimating = false;

            // 2. Havuzdaki mevcut kutuları (ve onların animasyonlarını) tamamen yok et
            foreach (var item in _activeItems)
            {
                if (item != null)
                {
                    item.RectTransform.DOKill(); // DOTween hareketini anında kes
                    Destroy(item.gameObject);
                }
            }

            _activeItems.Clear();

            // 3. Havuzu 1. bölgeden itibaren yepyeni kutularla baştan diz
            InitializePool();
        }

        public void AdvanceOneZone()
        {
            if (_isAnimating) return;
            _isAnimating = true;
            Debug.Log("AdvanceZone");
            int completedTweens = 0;

            float despawnThreshold = -(_stepDistance * itemsToKeepOnLeft);

            for (int i = 0; i < _activeItems.Count; i++)
            {
                ZoneItemView item = _activeItems[i];
                float newX = item.RectTransform.anchoredPosition.x - _stepDistance;

                item.RectTransform.DOAnchorPosX(newX, animationDuration).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    if (newX < despawnThreshold)
                    {
                        RecycleItem(item);
                    }


                    completedTweens++;


                    if (completedTweens == _activeItems.Count)
                    {
                        _isAnimating = false;
                    }
                });
            }
        }

        private void RecycleItem(ZoneItemView item)
        {
            float rightmostX = float.MinValue;
            foreach (var activeItem in _activeItems)
            {
                if (activeItem.RectTransform.anchoredPosition.x > rightmostX)
                {
                    rightmostX = activeItem.RectTransform.anchoredPosition.x;
                }
            }

            item.RectTransform.anchoredPosition = new Vector2(rightmostX + _stepDistance, 0);

            ZoneType type = GetZoneType(_currentHighestZone);
            item.Setup(_currentHighestZone, type);
            _currentHighestZone++;
        }

        private ZoneType GetZoneType(int zoneNumber)
        {
            if (zoneNumber % 30 == 0) return ZoneType.Super;
            if (zoneNumber % 5 == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }
    }
}