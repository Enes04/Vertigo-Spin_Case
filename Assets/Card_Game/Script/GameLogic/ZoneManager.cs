// Scripts/GameLogic/ZoneManager.cs
using System;
using CardGame.Core;

namespace CardGame.GameLogic
{
    public class ZoneManager
    {
        public int CurrentZone { get; private set; } = 1;

        // UI modülünün dinleyeceği olaylar
        public event Action<int, ZoneType> OnZoneChanged;
        public event Action OnBombHit;

        // Bölgeyi bir ileri taşır
        public void AdvanceZone()
        {
            CurrentZone++;
            ZoneType type = GetCurrentZoneType();
            
            OnZoneChanged?.Invoke(CurrentZone, type);
        }

        // Oyuncu bombaya bastığında bölgeleri sıfırlar
        public void HandleBombHit()
        {
            CurrentZone = 1;
            
            OnBombHit?.Invoke();
        }

        // Mevcut bölgenin tipini hesaplayan kural (Dokümandaki 5 ve 30 kuralı)
        public ZoneType GetCurrentZoneType()
        {
            if (CurrentZone % 30 == 0) return ZoneType.Super;
            if (CurrentZone % 5 == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }
    }
}