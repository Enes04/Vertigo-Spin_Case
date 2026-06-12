using System;
using UnityEngine;

namespace CardGame.Core
{
    [Serializable]
    public class WheelSlice
    {
     
        public RewardData reward;
        
        [Range(1, 100)] 
        public int dropWeight = 10;
    }
}