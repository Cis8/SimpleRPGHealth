using ElectricDrill.SimpleRpgCore;

namespace ElectricDrill.SimpleRpgHealth {
    public struct ReceivedHealInfo
    {
        public HealAmountInfo HealAmount { get; }
        public Source Source { get; }
        public EntityCore Healer { get; }
        public EntityCore Target { get; }
        public bool IsCritical { get; }

        public static HealInfoAmount Builder => ReceivedHealInfoStepBuilder.Builder;

        public ReceivedHealInfo(HealAmountInfo healAmount, PreHealInfo preHealInfo, EntityCore target) {
            HealAmount = healAmount;
            Source = preHealInfo.Source;
            Healer = preHealInfo.Healer;
            Target = target;
            IsCritical = preHealInfo.IsCritical;
        }

        private ReceivedHealInfo(HealAmountInfo healAmount, Source source, EntityCore healer, EntityCore target, bool isCritical = false) {
            HealAmount = healAmount;
            Source = source;
            Healer = healer;
            Target = target;
            IsCritical = isCritical;
        }

        public interface HealInfoAmount
        {
            HealInfoSource WithAmount(HealAmountInfo healAmount);
        }

        public interface HealInfoSource
        {
            HealInfoHealer WithSource(Source source);
        }

        public interface HealInfoHealer
        {
            HealInfoTarget WithHealer(EntityCore healer);
        }

        public interface HealInfoTarget
        {
            ReceivedHealInfoStepBuilder WithTarget(EntityCore target);
        }

        public sealed class ReceivedHealInfoStepBuilder : HealInfoAmount, HealInfoSource, HealInfoHealer, HealInfoTarget
        {
            private HealAmountInfo healAmount;
            private Source source;
            private EntityCore healer;
            private EntityCore target;
            private bool isCritical;

            public static HealInfoAmount Builder => new ReceivedHealInfoStepBuilder();

            public HealInfoSource WithAmount(HealAmountInfo healAmount)
            {
                this.healAmount = healAmount;
                return this;
            }

            public HealInfoHealer WithSource(Source source)
            {
                this.source = source;
                return this;
            }

            public HealInfoTarget WithHealer(EntityCore healer)
            {
                this.healer = healer;
                return this;
            }

            public ReceivedHealInfoStepBuilder WithTarget(EntityCore target)
            {
                this.target = target;
                return this;
            }

            public ReceivedHealInfoStepBuilder WithIsCritical(bool isCritical)
            {
                this.isCritical = isCritical;
                return this;
            }

            public ReceivedHealInfo Build()
            {
                return new ReceivedHealInfo(healAmount, source, healer, target, isCritical);
            }
        }
    }
}