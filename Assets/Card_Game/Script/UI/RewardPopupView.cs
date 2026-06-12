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
        [Header("UI References")]
   

        [SerializeField] private RectTransform cardRoot; 
        [SerializeField] private RectTransform shineEffect;
        [SerializeField] private RectTransform ui_middle_root; 
        [SerializeField]
        private Image ui_image_card_icon;

        [SerializeField] private TextMeshProUGUI ui_text_card_amount;

       
        
        [SerializeField] private GameObject flyingIconPrefab; 
        
        [SerializeField]
        private RectTransform flyTargetLeft; 

        [SerializeField]
        private int maxFlyingIcons = 10; 

  
        public void ShowReward(WheelSlice winningSlice, Action onComplete)
        {
            if (winningSlice == null || winningSlice.reward == null)
            {
                onComplete?.Invoke();
                return;
            }

         
            ui_image_card_icon.sprite = winningSlice.reward.rewardIcon;
            ui_text_card_amount.text = "x" + winningSlice.reward.amount;

       
          
            cardRoot.localScale = Vector3.zero;

          
            shineEffect.DORotate(new Vector3(0, 0, -360), 6f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);

           
            cardRoot.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
              
                DOVirtual.DelayedCall(1f, () => { StartFlyingEffect(winningSlice, onComplete); });
            });
        }

        private void StartFlyingEffect(WheelSlice winningSlice, Action onComplete)
        {
          
            cardRoot.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
            shineEffect.DOKill();
           
            int spawnCount = Mathf.Min(winningSlice.reward.amount, maxFlyingIcons);
            int completedIcons = 0;

            for (int i = 0; i < spawnCount; i++)
            {
          
                GameObject flyingIcon = Instantiate(flyingIconPrefab, ui_middle_root.transform);
                flyingIcon.GetComponent<Image>().sprite = winningSlice.reward.rewardIcon;

                RectTransform rect = flyingIcon.GetComponent<RectTransform>();
                rect.position = ui_middle_root.position;
                rect.localScale = Vector3.zero;

               
                Vector2 randomOffset = Random.insideUnitCircle * 150f;
                Vector3 burstPosition = rect.localPosition + (Vector3)randomOffset;

               
                Sequence seq = DOTween.Sequence();

             
                seq.Append(rect.DOLocalMove(burstPosition, 0.3f).SetEase(Ease.OutQuad));
                seq.Join(rect.DOScale(Vector3.one * 0.7f, 0.3f).SetEase(Ease.OutBack));

               
                seq.AppendInterval(Random.Range(0f, 0.2f));

              
                seq.Append(rect.DOMove(flyTargetLeft.position, 0.6f).SetEase(Ease.InBack));
                seq.Join(rect.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InBack));

            
                seq.OnComplete(() =>
                {
                    Destroy(flyingIcon);
                    completedIcons++;

                 
                    if (completedIcons == spawnCount)
                    {
                     
                        onComplete?.Invoke();
                    }
                });
            }
        }
    }
}