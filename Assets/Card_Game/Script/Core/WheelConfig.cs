// Scripts/Core/WheelConfig.cs
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Core
{
    [CreateAssetMenu(fileName = "NewWheelPool", menuName = "CardGame/Wheel Pool Config")]
    public class WheelConfig : ScriptableObject
    {
        [Header("Reward Pool (20-30 item ekleyebilirsin)")]
        [Tooltip("Çarkta çıkma ihtimali olan TÜM ödüllerin havuzu")]
        public List<WheelSlice> rewardPool = new List<WheelSlice>();

        // Havuzdan ağırlıklara (şansa) göre rastgele 8 adet dilim seçer
        public WheelSlice[] GenerateRandom8Slices()
        {
            WheelSlice[] selectedSlices = new WheelSlice[8];
            
            // Eğer havuzda yeterli eleman yoksa hata vermemesi için güvenlik kontrolü
            if (rewardPool.Count == 0) return selectedSlices;

            int totalPoolWeight = 0;
            foreach (var slice in rewardPool)
            {
                totalPoolWeight += slice.dropWeight;
            }

            for (int i = 0; i < 8; i++)
            {
                int randomValue = Random.Range(0, totalPoolWeight);
                int currentWeight = 0;

                // Ağırlıklı rastgele seçim (Weighted Random)
                foreach (var slice in rewardPool)
                {
                    currentWeight += slice.dropWeight;
                    if (randomValue < currentWeight)
                    {
                        selectedSlices[i] = slice;
                        break; // Bu dilimi bulduk, bir sonraki 8'li slota geç
                    }
                }
            }

            return selectedSlices;
        }
    }
}