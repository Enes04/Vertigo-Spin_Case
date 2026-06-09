// Scripts/Core/SpinCalculator.cs
using System;
using UnityEngine; // Debug.LogError için eklendi

namespace CardGame.Core
{
    public class SpinCalculator
    {
        private System.Random _random;

        public SpinCalculator()
        {
            _random = new System.Random(); 
        }

      
        public (int index, WheelSlice slice) CalculateSpinResult(WheelSlice[] activeSlices)
        {
            if (activeSlices == null || activeSlices.Length == 0)
            {
                Debug.LogError("Hesaplama yapılamadı: Aktif dilim listesi boş!");
                return (0, null);
            }

            int totalWeight = 0;
            // Sadece ekrandaki o 8 dilimin ağırlıklarını topluyoruz
            foreach (var slice in activeSlices)
            {
                if (slice != null) totalWeight += slice.dropWeight;
            }

            int randomValue = _random.Next(0, totalWeight);
            int currentWeight = 0;

            // Hangi dilimin kazandığını bul ve hem indeksini hem de kendisini gönder
            for (int i = 0; i < activeSlices.Length; i++)
            {
                if (activeSlices[i] == null) continue;

                currentWeight += activeSlices[i].dropWeight;
                if (randomValue < currentWeight)
                {
                    return (i, activeSlices[i]);
                }
            }

            // Fallback (Beklenmeyen bir hata olursa her zaman ilk dilimi ver)
            return (0, activeSlices[0]);
        }
    }
}