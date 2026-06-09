using UnityEngine;

namespace CardGame.Core
{
    [CreateAssetMenu(fileName = "NewRewardData", menuName = "CardGame/Reward Data")]
    public class RewardData : ScriptableObject
    {
        public RewardType rewardType;
        public int amount;
        public Sprite rewardIcon;
        
        public bool isBomb => rewardType == RewardType.Bomb; 
    }
}
