namespace ElectricDrill.SimpleRpgHealth
{
    public class HealAmountInfo
    {
        public HealAmountInfo(long rawAmount, long afterModifierAmount, long netAmount) {
            RawAmount = rawAmount;
            AfterModifierAmount = afterModifierAmount;
            NetAmount = netAmount;
        }

        /// <summary>
        /// Gets the initial healing amount derived directly from PreHealInfo.
        /// </summary>
        public long RawAmount { get; }

        /// <summary>
        /// Gets the healing amount after being modified by the associated heal modifier statistic.
        /// </summary>
        public long AfterModifierAmount { get; }

        /// <summary>
        /// Gets the actual amount of health added to the entity, considering the entity's maximum health.
        /// </summary>
        public long NetAmount { get; }
    }
}