using UnityEngine;

namespace CardGame.Core
{
    [CreateAssetMenu(fileName = "NewRewardData", menuName = "CardGame/Reward Data")]
    public class RewardData : ScriptableObject
    {
        public RewardType rewardType;
        [HideInInspector] public int amount;
        public int minAmount = 1;
        public int maxAmount = 5;
        public Sprite rewardIcon;
        
        public bool isBomb => rewardType == RewardType.Bomb; 
    }
}
