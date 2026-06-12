using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Core
{
    [CreateAssetMenu(fileName = "NewWheelPool", menuName = "CardGame/Wheel Pool Config")]
    public class WheelConfig : ScriptableObject
    {
        [Header("Wheel Visuals")] public Sprite wheelBackgroundSprite;
        public Sprite spinIndicator;

        public bool requiresOneBomb = true;

        [Header("Reward Pool")] public List<WheelSlice> rewardPool = new List<WheelSlice>();

        public WheelSlice[] GenerateRandom8Slices()
        {
            WheelSlice[] selectedSlices = new WheelSlice[8];
            if (rewardPool.Count == 0) return selectedSlices;


            List<WheelSlice> bombSlices = new List<WheelSlice>();
            List<WheelSlice> regularSlices = new List<WheelSlice>();

            foreach (var slice in rewardPool)
            {
                if (slice.reward != null && slice.reward.isBomb)
                {
                    bombSlices.Add(slice);
                }
                else
                {
                    regularSlices.Add(slice);
                }
            }

            int bombIndex = -1;


            if (requiresOneBomb && bombSlices.Count > 0)
            {
                WheelSlice selectedBomb = GetRandomItemFromList(bombSlices);
                bombIndex = Random.Range(0, 8);
                selectedSlices[bombIndex] = CreateRuntimeSlice(selectedBomb);
            }


            for (int i = 0; i < 8; i++)
            {
                if (i == bombIndex) continue;

                WheelSlice selectedNormal = GetRandomItemFromList(regularSlices);
                selectedSlices[i] = CreateRuntimeSlice(selectedNormal);
            }

            return selectedSlices;
        }

        private WheelSlice CreateRuntimeSlice(WheelSlice original)
        {
            WheelSlice cloneSlice = new WheelSlice();
            cloneSlice.dropWeight = original.dropWeight;


            if (original.reward != null)
            {
                cloneSlice.reward = Instantiate(original.reward);
                cloneSlice.reward.name = original.reward.name;
                cloneSlice.reward.amount = Random.Range(original.reward.minAmount, original.reward.maxAmount + 1);
            }

            return cloneSlice;
        }

        private WheelSlice GetRandomItemFromList(List<WheelSlice> list)
        {
            if (list.Count == 0) return null;

            int totalWeight = 0;
            foreach (var slice in list) totalWeight += slice.dropWeight;

            int randomValue = Random.Range(0, totalWeight);
            int currentWeight = 0;

            foreach (var slice in list)
            {
                currentWeight += slice.dropWeight;
                if (randomValue < currentWeight)
                {
                    return slice;
                }
            }

            return list[0];
        }
    }
}