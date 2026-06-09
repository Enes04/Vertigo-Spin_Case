using UnityEngine;
using CardGame.Core;
using TMPro;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class ZoneItemView : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private TextMeshProUGUI ui_text_zone_value;
        [SerializeField] private Image ui_image_zone_background;
        
        [Header("Zone Colors")]
        [SerializeField] private Color normalColor = new Color(0,0,0,0);
        [SerializeField] private Color safeColor = Color.green;
        [SerializeField] private Color superColor = Color.yellow;
        
        private RectTransform _rectTransform;
        public RectTransform RectTransform 
        {
            get 
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        public void Setup(int zoneNumber, ZoneType zoneType)
        {
            ui_text_zone_value.text = zoneNumber.ToString();
            
            switch (zoneType)
            {
                case ZoneType.Safe:
                    ui_image_zone_background.color = safeColor;
                    break;
                case ZoneType.Super:
                    ui_image_zone_background.color = superColor;
                    break;
                default:
                    ui_image_zone_background.color = normalColor;
                    break;
            }
        }
    }
}
