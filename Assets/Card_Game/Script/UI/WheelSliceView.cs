// Scripts/UI/WheelSliceView.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardGame.Core;

namespace CardGame.UI
{
    public class WheelSliceView : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private Image ui_image_reward_icon;
        [SerializeField] private TextMeshProUGUI ui_text_reward_value;

        // Core'dan gelen veriyi UI'a işleyen fonksiyon
        public void Setup(WheelSlice sliceData)
        {
            if (sliceData == null || sliceData.reward == null) return;

            // İkonu ayarla
            ui_image_reward_icon.sprite = sliceData.reward.rewardIcon;
            
            // Eğer ödül bomba ise miktar yazısını boş bırak (veya özel bir stil uygula)
            if (sliceData.reward.isBomb)
            {
                ui_text_reward_value.text = "";
            }
            else
            {
                // PDF'teki görsele uygun olarak sayıların başına "x" koyuyoruz
                ui_text_reward_value.text = "x" + sliceData.reward.amount.ToString();
            }
        }
    }
}