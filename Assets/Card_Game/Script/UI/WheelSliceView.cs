using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardGame.Core;

namespace CardGame.UI
{
    public class WheelSliceView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image ui_image_reward_icon;
        [SerializeField] private TextMeshProUGUI ui_text_reward_value;

       
        public void Setup(WheelSlice sliceData)
        {
            if (sliceData == null || sliceData.reward == null) return;

            ui_image_reward_icon.sprite = sliceData.reward.rewardIcon;
            
            if (sliceData.reward.isBomb)
            {
                ui_text_reward_value.text = "";
            }
            else
            {
                ui_text_reward_value.text = "x" + sliceData.reward.amount.ToString();
            }
        }
    }
}