using System.Collections;
using System.Collections.Generic;
using ElectricDrill.SimpleRpgCore;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth {
    public class PreDmgInfo
    {
        public long Amount { get; }
        public DmgType Type { get; }
        public Source Source { get; }
        public EntityCore Dealer { get; }
        public bool IsCritical { get; }

        public static DmgInfoAmount Builder => new PreDmgInfoStepBuilder();

        private PreDmgInfo(long amount, DmgType type, Source source, EntityCore dealer, bool isCritical = false) {
            Amount = amount;
            Type = type;
            Source = source;
            Dealer = dealer;
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
            PreDmgInfoStepBuilder WithDealer(EntityCore dealer);
        }
        
        public sealed class PreDmgInfoStepBuilder : DmgInfoAmount, DmgInfoType, DmgInfoSource, DmgInfoDealer
        {
            private long amount;
            private DmgType type;
            private Source source;
            private EntityCore dealer;
            private bool isCritical;
            
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

            public PreDmgInfoStepBuilder WithDealer(EntityCore dealer)
            {
                this.dealer = dealer;
                return this;
            }

            public PreDmgInfoStepBuilder WithIsCritical(bool isCritical)
            {
                this.isCritical = isCritical;
                return this;
            }

            public PreDmgInfo Build()
            {
                return new PreDmgInfo(amount, type, source, dealer, isCritical);
            }
        }
    }
}
