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
             
                return (0, null);
            }

            int totalWeight = 0;
           
            foreach (var slice in activeSlices)
            {
                if (slice != null) totalWeight += slice.dropWeight;
            }

            int randomValue = _random.Next(0, totalWeight);
            int currentWeight = 0;


            for (int i = 0; i < activeSlices.Length; i++)
            {
                if (activeSlices[i] == null) continue;

                currentWeight += activeSlices[i].dropWeight;
                if (randomValue < currentWeight)
                {
                    return (i, activeSlices[i]);
                }
            }

         
            return (0, activeSlices[0]);
        }
    }
}