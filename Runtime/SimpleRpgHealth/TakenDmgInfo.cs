using ElectricDrill.SimpleRpgCore;

namespace ElectricDrill.SimpleRpgHealth {
    public struct TakenDmgInfo
    {
        public DmgAmountInfo DmgAmountInfo { get; }
        public DmgType Type { get; }
        public Source Source { get; }
        public EntityCore Dealer { get; }
        
        public EntityCore Target { get; }
        public bool IsCritical { get; }

        public static DmgInfoAmount Builder => TakenDmgInfoStepBuilder.Builder;

        public TakenDmgInfo(DmgAmountInfo dmgAmountInfo, PreDmgInfo preDmgInfo, EntityCore target) {
            DmgAmountInfo = dmgAmountInfo;
            Type = preDmgInfo.Type;
            Source = preDmgInfo.Source;
            Dealer = preDmgInfo.Dealer;
            Target = target;
            IsCritical = preDmgInfo.IsCritical;
        }
        
        private TakenDmgInfo(DmgAmountInfo dmgAmountInfo, DmgType type, Source source, EntityCore dealer, EntityCore target, bool isCritical = false) {
            DmgAmountInfo = dmgAmountInfo;
            Type = type;
            Source = source;
            Dealer = dealer;
            Target = target;
            IsCritical = isCritical;
        }

        public interface DmgInfoAmount
        {
            DmgInfoType WithDmgAmountInfo(DmgAmountInfo dmgAmountInfo);
        }
        
         public interface DmgInfoType
        {
            DmgInfoSource WithType(DmgType type);
        }
        
         public interface DmgInfoSource
        {
            DmgInfoDealer WithSource(Source source);
        }
        
         public interface DmgInfoDealer
        {
            DmgInfoTarget WithDealer(EntityCore dealer);
        }
         
        public interface DmgInfoTarget
        {
            TakenDmgInfoStepBuilder WithTarget(EntityCore target);
        }

        public sealed class TakenDmgInfoStepBuilder : DmgInfoAmount, DmgInfoType, DmgInfoSource, DmgInfoDealer, DmgInfoTarget
        {
            private DmgAmountInfo dmgAmountInfo;
            private DmgType type;
            private Source source;
            private EntityCore dealer;
            private EntityCore target;
            private bool isCritical;
            
            public static DmgInfoAmount Builder => new TakenDmgInfoStepBuilder();
            
            public DmgInfoType WithDmgAmountInfo(DmgAmountInfo dmgAmountInfo)
            {
                this.dmgAmountInfo = dmgAmountInfo;
                return this;
            }

            public DmgInfoSource WithType(DmgType type)
            {
                this.type = type;
                return this;
            }

            public DmgInfoDealer WithSource(Source source)
            {
                this.source = source;
                return this;
            }

            public DmgInfoTarget WithDealer(EntityCore dealer)
            {
                this.dealer = dealer;
                return this;
            }
            
            public TakenDmgInfoStepBuilder WithTarget(EntityCore target)
            {
                this.target = target;
                return this;
            }

            public TakenDmgInfoStepBuilder WithIsCritical(bool isCritical)
            {
                this.isCritical = isCritical;
                return this;
            }

            public TakenDmgInfo Build()
            {
                return new TakenDmgInfo(dmgAmountInfo, type, source, dealer, target, isCritical);
            }

        }
    }
}
