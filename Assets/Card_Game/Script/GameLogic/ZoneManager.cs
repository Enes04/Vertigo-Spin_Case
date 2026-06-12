using System;
using CardGame.Core;

namespace CardGame.GameLogic
{
    public class ZoneManager
    {
        public int CurrentZone { get; private set; } = 1;

      
        public event Action<int, ZoneType> OnZoneChanged;
        public event Action OnBombHit;

        
        public void AdvanceZone()
        {
            CurrentZone++;
            ZoneType type = GetCurrentZoneType();
            
            OnZoneChanged?.Invoke(CurrentZone, type);
        }

     
        public void HandleBombHit()
        {
            CurrentZone = 1;
            
            OnBombHit?.Invoke();
        }
        public void RestartGame()
        {
            CurrentZone = 1;
    
            OnZoneChanged?.Invoke(CurrentZone, GetCurrentZoneType());
        }
   
        public ZoneType GetCurrentZoneType()
        {
            if (CurrentZone % 30 == 0) return ZoneType.Super;
            if (CurrentZone % 5 == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }
    }
}