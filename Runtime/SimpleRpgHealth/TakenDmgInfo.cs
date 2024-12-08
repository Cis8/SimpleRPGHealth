using ElectricDrill.SimpleRpgCore;

namespace ElectricDrill.SimpleRpgHealth {
    public struct TakenDmgInfo
    {
        public long Amount { get; }
        public DmgType Type { get; }
        public Source Source { get; }
        public EntityCore Dealer { get; }
        
        public EntityCore Target { get; }
        public bool IsCritical { get; }

        public static DmgInfoAmount Builder => TakenDmgInfoStepBuilder.Builder;

        public TakenDmgInfo(long amount, PreDmgInfo preDmgInfo, EntityCore target) {
            Amount = amount;
            Type = preDmgInfo.Type;
            Source = preDmgInfo.Source;
            Dealer = preDmgInfo.Dealer;
            Target = target;
            IsCritical = preDmgInfo.IsCritical;
        }
        
        private TakenDmgInfo(long amount, DmgType type, Source source, EntityCore dealer, EntityCore target, bool isCritical = false) {
            Amount = amount;
            Type = type;
            Source = source;
            Dealer = dealer;
            Target = target;
            IsCritical = isCritical;
        }

        public interface DmgInfoAmount
        {
            DmgInfoType WithAmount(long amount);
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
            private long amount;
            private DmgType type;
            private Source source;
            private EntityCore dealer;
            private EntityCore target;
            private bool isCritical;
            
            public static DmgInfoAmount Builder => new TakenDmgInfoStepBuilder();
            
            public DmgInfoType WithAmount(long amount)
            {
                this.amount = amount;
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
                return new TakenDmgInfo(amount, type, source, dealer, target, isCritical);
            }

        }
    }
}
