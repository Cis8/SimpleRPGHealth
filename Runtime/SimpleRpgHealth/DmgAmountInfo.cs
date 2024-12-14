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

        public long RawAmount { get; }
        
        public long DefReducedAmount { get; }
        
        public long DefBarrierReducedAmount { get; }
        
        public long NetAmount { get; }
    }
}