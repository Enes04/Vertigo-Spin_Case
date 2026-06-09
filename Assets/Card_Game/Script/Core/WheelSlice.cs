// Scripts/Core/WheelSlice.cs
using System;
using UnityEngine;

namespace CardGame.Core
{
    [Serializable]
    public class WheelSlice
    {
        [Tooltip("Bu dilime denk gelindiğinde verilecek ödül veya bomba verisi")]
        public RewardData reward;
        
        [Tooltip("Bu dilimin çıkma ihtimali (Ağırlık/Weight)")]
        [Range(1, 100)] 
        public int dropWeight = 10;
    }
}