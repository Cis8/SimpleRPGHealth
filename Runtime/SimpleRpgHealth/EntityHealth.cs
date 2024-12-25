using System;
using System.Collections;
using System.Collections.Generic;
using ElectricDrill.SimpleRpgCore;
using ElectricDrill.SimpleRpgCore.Events;
using ElectricDrill.SimpleRpgCore.Scaling;
using ElectricDrill.SimpleRpgCore.Stats;
using ElectricDrill.SimpleRpgCore.Utils;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace ElectricDrill.SimpleRpgHealth
{
    [RequireComponent(typeof(EntityCore))]
    public class EntityHealth : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField, HideInInspector] private bool healthCanBeNegative = false;
        [SerializeField, HideInInspector] private LongVar deathThreshold;
        // if true, the max hp will be taken from the class and assigned to _maxHp, overriding any value set in the inspector
        // if false, the value set in the inspector will be used
        [SerializeField, HideInInspector] private bool useClassMaxHp = false;
        [SerializeField] internal LongRef baseMaxHp;
        internal long _flatMaxHpModifier = 0;
        internal Percentage _percentageMaxHpModifier = 0;
        [SerializeField, HideInInspector] private AttributesScalingComponent healthAttributesScaling;
        private long _previousMaxHp = 0; // used to check if max hp has changed
        [SerializeField, HideInInspector] internal LongRef totalMaxHp;
        [SerializeField, HideInInspector] internal LongRef hp;
        [SerializeField, HideInInspector] internal bool restoreHpOnLevelUp = true;
        [SerializeField, HideInInspector] internal LongRef barrier;
        [SerializeField, HideInInspector] private Stat healAmountModifierStat;
        [SerializeField, HideInInspector] private LifestealConfig lifestealConfig;
        [SerializeField] private OnDeathStrategy onDeathStrategy;

        private EntityCore _core;
        internal EntityStats _stats;
        private EntityClass _entityClass;
        
        // Events
        [SerializeField, HideInInspector] private PreDmgGameEvent preDmgInfoEvent;
        [SerializeField, HideInInspector] private TakenDmgGameEvent takenDmgInfoEvent;
        [SerializeField, HideInInspector] private EntityMaxHealthChangedGameEvent maxHealthChangedEvent;
        [SerializeField, HideInInspector] private EntityGainedHealthGameEvent gainedHealthEvent;
        [SerializeField, HideInInspector] private EntityLostHealthGameEvent lostHealthEvent;
        [SerializeField, HideInInspector] private EntityDiedGameEvent entityDiedEvent;
        [SerializeField, HideInInspector] private PreHealGameEvent preHealEvent;
        [SerializeField, HideInInspector] private EntityHealedGameEvent entityHealedEvent;

        public long MaxHp => totalMaxHp;
        public long Hp => hp;
        public long Barrier => barrier;
        
        public OnDeathStrategy OnDeathStrategy { get => onDeathStrategy; set => onDeathStrategy = value; }
        public bool HealthCanBeNegative { get => healthCanBeNegative; set => healthCanBeNegative = value; }

        private void Awake() {
            ValidateConstraints();
            _stats = GetComponent<EntityStats>();
            Assert.IsTrue(healAmountModifierStat == null || _stats.StatSet.Contains(healAmountModifierStat), $"StatSet of {gameObject.name} doesn't contain the stat {healAmountModifierStat}");
            _entityClass = GetComponent<EntityClass>();
            _core = GetComponent<EntityCore>();
            SetupBaseMaxHp();
            SetHpToMax();
            Assert.IsFalse(MaxHp <= 0, $"Max HP of an Entity must be greater than 0. {name}'s Max HP was {MaxHp}");
        }

        private void Start() {
        }

        private void Update() {
        }
        
        public void SetupBaseMaxHp() {
            if (useClassMaxHp) {
                Assert.IsNotNull(_entityClass, $"Class of {gameObject.name} is missing");
                baseMaxHp.Value = _entityClass.Class.GetMaxHpAt(_core.Level);
            }
            SetupMaxHp();
        }
        
        private void SetupMaxHp() {
            totalMaxHp.Value = baseMaxHp + _flatMaxHpModifier;
            if (healthAttributesScaling != null)
                totalMaxHp.Value += healthAttributesScaling.CalculateValue(_core);
            totalMaxHp.Value += (long)(totalMaxHp.Value * (double)_percentageMaxHpModifier);
            
            // check if max hp has changed
            if (_previousMaxHp != totalMaxHp) {
                maxHealthChangedEvent?.Raise(this, totalMaxHp, _previousMaxHp);
                _previousMaxHp = totalMaxHp;
            }
        }
        
        public void AddMaxHpFlatModifier(long amount) {
            _flatMaxHpModifier += amount;
            SetupMaxHp();
        }
        
        public void AddMaxHpPercentageModifier(Percentage amount) {
            _percentageMaxHpModifier += amount;
            SetupMaxHp();
        }

        public virtual void TakeDamage(PreDmgInfo preDmg) {
            Assert.IsTrue(preDmg.Amount >= 0, "Damage amount must be greater than or equal to 0");

            preDmgInfoEvent?.Raise(preDmg, _core);

            // get stats from the entity
            var defensiveStat = preDmg.Type.ReducedBy != null ? _stats.Get(preDmg.Type.ReducedBy) : 0;
            var piercingStat = preDmg.Type.DefensiveStatPiercedBy != null ? preDmg.Dealer.Stats.Get(preDmg.Type.DefensiveStatPiercedBy) : 0;

            // calculate the reduced defensive stat and the reduced amount of damage
            var dmgToBeTaken = CalculateReducedDmg(preDmg.Amount, piercingStat, preDmg.Type.DefReductionFn,
                defensiveStat, preDmg.Type.DmgReductionFn);
            
            var barrierReducedDmgToBeTaken = dmgToBeTaken;
            if (!preDmg.Type.IgnoresBarrier)
            {
                // subtract the dmg to be taken to the entity's barrier (if it has any)
                if (barrier > 0) {
                    barrierReducedDmgToBeTaken = Math.Max(0, dmgToBeTaken - barrier);
                    RemoveBarrier(barrierReducedDmgToBeTaken);
                }
            }

            // subtract the barrier-reduced dmg to be taken to the entity's health
            var healthLost = RemoveHealth(barrierReducedDmgToBeTaken);
            
            // create DmgAmountInfo with all parameters
            var dmgAmountInfo = new DmgAmountInfo(
                preDmg.Amount,
                dmgToBeTaken,
                barrierReducedDmgToBeTaken,
                healthLost
            );
            
            var takenDmgInfo = new TakenDmgInfo(dmgAmountInfo, preDmg, _core);
            takenDmgInfoEvent?.Raise(takenDmgInfo);
            if (IsDead()) {
                entityDiedEvent.Raise(this, takenDmgInfo);
                onDeathStrategy.Die(this);
            }
        }

        public void AddBarrier(long amount) {
            Assert.IsTrue(amount >= 0, $"Barrier amount to be added must be greater than or equal to 0, was {amount}");
            barrier.Value += amount;
        }
        
        private void RemoveBarrier(long amount) {
            Assert.IsTrue(amount >= 0, $"Barrier amount to be removed must be greater than or equal to 0, was {amount}");
            barrier.Value = Math.Max(0, barrier - amount);
        }
        
        public void SetHpToMax() {
            AddHealth(MaxHp);
        }
        
        /// <summary>
        /// Adds <paramref name="amount"/> health to the current health. If <paramref name="amount"/> would cause the
        /// current health to trespass the max health, current health is set to max health instead.
        /// </summary>
        /// <param name="amount">Health amount to be added</param>
        /// <returns>Actual amount of health gained</returns>
        private long AddHealth(long amount) {
            Assert.IsTrue(amount >= 0, $"Health amount to be added must be greater than or equal to 0, was {amount}");
            long previousHp = hp;
            hp.Value = Math.Min(hp + amount, MaxHp);
            long gainedHealth = hp - previousHp;
            gainedHealthEvent?.Raise(this, gainedHealth);
            return gainedHealth;
        }

        /// <summary>
        /// Subtracts <paramref name="amount"/> health to the current health. If <paramref name="amount"/> would cause
        /// the current health to go below zero and HealthCanBeNegative is false, it is set to 0 instead.
        /// </summary>
        /// <returns>Actual amount of health lost</returns>
        /// <param name="amount">Health amount to be removed</param>
        private long RemoveHealth(long amount) {
            Assert.IsTrue(amount >= 0, $"Health amount to be removed must be greater than or equal to 0, was {amount}");
            long previousHp = hp;
            long newHp = hp - amount;
            if (newHp < 0 && !healthCanBeNegative) {
                newHp = 0;
            }
            hp.Value = newHp;
            var lostHp = previousHp - hp;
            lostHealthEvent?.Raise(this, lostHp);
            return lostHp;
        }
        
        public void Heal(PreHealInfo info) {
            Assert.IsTrue(info.Amount >= 0, $"Heal amount must be greater than or equal to 0, was {info.Amount}");
            preHealEvent.Raise(info, _core);
            
            var healedAmount = info.Amount;
            if (healAmountModifierStat != null) {
                Percentage healModifier = _stats.Get(healAmountModifierStat);
                healedAmount = (long) (healedAmount * healModifier);   
            }
            
            var gainedHealth = AddHealth(healedAmount);
            var healAmountInfo = new HealAmountInfo(info.Amount, healedAmount, gainedHealth);
            var receivedHealInfo = new ReceivedHealInfo(healAmountInfo, info, _core);
            entityHealedEvent.Raise(receivedHealInfo);
        }

        private void CheckLifesteal(TakenDmgInfo dmgInfo) {
            if (dmgInfo.Dealer != _core) return;
            if (!lifestealConfig) return;
            if (!lifestealConfig.LifestealMappings.ContainsKey(dmgInfo.Type)) return;
            var lifestealStat = lifestealConfig.LifestealMappings[dmgInfo.Type].LifestealStat;
            if (!lifestealStat) return;
            if (!_stats.StatSet.Contains(lifestealStat)) return;
            
            Percentage lifestealStatValue = _stats.Get(lifestealStat);
            long dmgToLifesteal;
            
            switch (lifestealConfig.LifestealMappings[dmgInfo.Type].DmgStateSelector) {
                case DmgStateSelector.Raw:
                    dmgToLifesteal = dmgInfo.DmgAmountInfo.RawAmount;
                    break;
                case DmgStateSelector.DefReduced:
                    dmgToLifesteal = dmgInfo.DmgAmountInfo.DefReducedAmount;
                    break;
                case DmgStateSelector.DefBarrierReduced:
                    dmgToLifesteal = dmgInfo.DmgAmountInfo.DefBarrierReducedAmount;
                    break;
                case DmgStateSelector.Net:
                    dmgToLifesteal = dmgInfo.DmgAmountInfo.NetAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var lifestealAmount = (long) (dmgToLifesteal * lifestealStatValue);
            
            Heal(PreHealInfo.Builder
                .WithAmount(lifestealAmount)
                .WithSource(lifestealConfig.LifestealMappings[dmgInfo.Type].LifestealSource)
                .WithHealer(_core)
                .Build());
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <returns>true if current health is <= death threshold, false otherwise</returns>
        public bool IsDead() {
            return hp <= deathThreshold;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>true if current health is > death threshold, false otherwise</returns>
        public bool IsAlive() {
            return !IsDead();
        }
        
        private void OnLevelUp(int level) {
            SetupBaseMaxHp();
            if (restoreHpOnLevelUp)
                SetHpToMax();
        }
        
        internal static long CalculateReducedDmg(long amount, long piercingStatValue, [CanBeNull] DefReductionFn defReductionFn, long defensiveStatValue, [CanBeNull] DmgReductionFn dmgReductionFn) {
            static double CalculateReducedDef(long piercingStatValue, long defensiveStatValue, [CanBeNull] DefReductionFn defReductionFn) {
                return defReductionFn ? defReductionFn.ReducedDef(piercingStatValue, defensiveStatValue) : defensiveStatValue;
            }
        
            static long CalculateReducedDmgFromFn(long amount, double defensiveStatValue, [CanBeNull] DmgReductionFn dmgReductionFn) {
                return dmgReductionFn ? dmgReductionFn.ReducedDmg(amount, defensiveStatValue) : amount;
            }
            
            var reducedDefensiveStat = CalculateReducedDef(piercingStatValue, defensiveStatValue, defReductionFn);
            return CalculateReducedDmgFromFn(amount, reducedDefensiveStat, dmgReductionFn);
        }
        
        // UTILS
        
        private void ValidateConstraints() {
            Assert.IsNotNull(preDmgInfoEvent, $"PreDmgGameEvent is missing for {gameObject.name}");
            Assert.IsNotNull(takenDmgInfoEvent, $"TakenDmgGameEvent is missing for {gameObject.name}");
            Assert.IsFalse(hp < 0, $"HP of an Entity must be greater than or equal to 0. {name}'s HP was {Hp}");
            if (healthCanBeNegative)
                Assert.IsNotNull(deathThreshold, $"Death Threshold is missing for {gameObject.name}");
            Assert.IsNotNull(gainedHealthEvent, $"GainedHealthGameEvent is missing for {gameObject.name}");
            Assert.IsNotNull(lostHealthEvent, $"LostHealthGameEvent is missing for {gameObject.name}");
            Assert.IsNotNull(maxHealthChangedEvent, $"MaxHealthChangedGameEvent is missing for {gameObject.name}");
            Assert.IsNotNull(onDeathStrategy, $"OnDeathStrategy is missing for {gameObject.name}");
            Assert.IsNotNull(entityDiedEvent, $"DiedGameEvent is missing for {gameObject.name}");
            Assert.IsNotNull(preHealEvent, $"PreHealGameEvent is missing for {gameObject.name}");
            Assert.IsNotNull(entityHealedEvent, $"EntityHealedGameEvent is missing for {gameObject.name}");
        }
        
        private void OnEnable() {
            _core = GetComponent<EntityCore>();
            _core.Level.OnLevelUp += OnLevelUp;
            takenDmgInfoEvent.OnEventRaised += CheckLifesteal;
#if UNITY_EDITOR
            Selection.selectionChanged += OnSelectionChanged;
#endif
        }


        private void OnDisable() {
            _core.Level.OnLevelUp -= OnLevelUp;
            takenDmgInfoEvent.OnEventRaised -= CheckLifesteal;
#if UNITY_EDITOR
            Selection.selectionChanged -= OnSelectionChanged;
#endif
        }

        private void OnValidate() {
        }
        
#if UNITY_EDITOR
        private void OnSelectionChanged() {
            if (Selection.activeGameObject == gameObject) {
                OnValidate();
            }
        }
#endif
    }
}