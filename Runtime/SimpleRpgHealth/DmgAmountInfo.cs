namespace ElectricDrill.SimpleRpgHealth
{
    public class DmgAmountInfo
    {
        public DmgAmountInfo(long rawAmount, long defReducedAmount, long defBarrierReducedAmount, long netAmount) {
            RawAmount = rawAmount;
            DefReducedAmount = defReducedAmount;
            DefBarrierReducedAmount = defBarrierReducedAmount;
            NetAmount = netAmount;
        }

        /// <summary>
        /// Gets the initial damage amount.
        /// </summary>
        public long RawAmount { get; }

        /// <summary>
        /// Gets the damage amount after being reduced by the entity's defense.
        /// </summary>
        public long DefReducedAmount { get; }

        /// <summary>
        /// Gets the damage amount after being reduced by the entity's defense and barriers.
        /// </summary>
        public long DefBarrierReducedAmount { get; }

        /// <summary>
        /// Gets the actual amount of damage taken by the entity.
        /// </summary>
        public long NetAmount { get; }
    }
}