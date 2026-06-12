using UnityEngine;
using UnityEngine.UI;
using CardGame.Core;
using CardGame.UI;
using DG.Tweening;

namespace CardGame.GameLogic
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI references")]
        [SerializeField] private WheelView wheelView;
        [SerializeField] private Button spinButton; 
        [SerializeField] private Button exitButton; 
        [Header("Spin Configs")]
        [SerializeField] private WheelConfig normalWheelPool;
        [SerializeField] private WheelConfig silverWheelPool;
        [SerializeField] private WheelConfig goldWheelPool;
        
        
        [Space(10)]
        [SerializeField] private RewardPopupView rewardPopupView;
        [SerializeField] private ZoneScrollerView zoneScrollerView;
        [SerializeField] private InventoryPanelView inventoryPanelView; 
        [SerializeField] private ExitSummaryView exitSummaryView; 
        [SerializeField] private BombScreenView bombScreenView; 
        
        private ZoneManager _zoneManager;
        private SpinCalculator _spinCalculator;

        private void Start()
        {
          
            _zoneManager = new ZoneManager();
            _spinCalculator = new SpinCalculator();
            _zoneManager.OnZoneChanged += HandleZoneChanged;
            
            spinButton.onClick.AddListener(OnSpinButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            PrepareWheelForCurrentZone();
            
            
        }
        private void HandleZoneChanged(int newZone, ZoneType zoneType)
        {
            if (zoneScrollerView != null)
            {
                if (newZone == 1)
                {
                    zoneScrollerView.ResetScroller();
                }
                else
                {
                    zoneScrollerView.AdvanceOneZone();
                }
            }
        }
        private void PrepareWheelForCurrentZone()
        {
            if (spinButton != null) spinButton.interactable = false;
            if (exitButton != null) exitButton.interactable = false;
            WheelConfig currentConfig = normalWheelPool;

            ZoneType currentZoneType = _zoneManager.GetCurrentZoneType();
            
            if (currentZoneType == ZoneType.Safe) 
            {
                currentConfig = silverWheelPool;
            }
            else if (currentZoneType == ZoneType.Super) 
            {
                currentConfig = goldWheelPool;
            }

            wheelView.SetupWheelVisuals(currentConfig);
            DOVirtual.DelayedCall(0.6f, () => 
            {
                if (spinButton != null) spinButton.interactable = true;
                if (exitButton != null) exitButton.interactable = true;
            });
        }

        private void OnSpinButtonClicked()
        {
            spinButton.interactable = false;
            exitButton.interactable = false;
            var spinResult = _spinCalculator.CalculateSpinResult(wheelView.CurrentActiveSlices);


            wheelView.SpinToSlice(spinResult.index, () => 
            {
                OnSpinCompleted(spinResult.slice);
            });
        }

        private void OnSpinCompleted(WheelSlice winningSlice)
        {
        
            if (winningSlice.reward.isBomb)
            {
                bombScreenView.ShowBombScreen(() =>
                {
                    _zoneManager.HandleBombHit(); 
                    inventoryPanelView.ClearInventory(); 
                    PrepareWheelForCurrentZone(); 
            }, () =>
                {
                    _zoneManager.AdvanceZone(); 
                    PrepareWheelForCurrentZone();
            });
               
            }
            else
            {
                _zoneManager.AdvanceZone();
                
                rewardPopupView.ShowReward(winningSlice, () => 
                {
                    inventoryPanelView.AddReward(winningSlice.reward);
                    PrepareWheelForCurrentZone();
                  });
            }

         
        }
        private void OnExitButtonClicked()
        {
            var collectedItems = inventoryPanelView.GetCollectedItems();
            if (collectedItems.Count == 0)
            {
                ResetGameAfterExit();
                return;
            }
            exitSummaryView.ShowSummary(collectedItems, () => 
            {
                ResetGameAfterExit();
            });
          
        }
        private void ResetGameAfterExit()
        {
            _zoneManager.RestartGame();
            inventoryPanelView.ClearInventory(); 
            PrepareWheelForCurrentZone(); 
        }
        private void OnDestroy()
        {
            if (spinButton != null)
            {
                spinButton.onClick.RemoveListener(OnSpinButtonClicked);
            }
            if (exitButton != null) exitButton.onClick.RemoveListener(OnExitButtonClicked);
            if (_zoneManager != null)
            {
                _zoneManager.OnZoneChanged -= HandleZoneChanged;
            }
        }
    }
}