namespace ElectricDrill.SimpleRpgHealth
{
    public interface IDamageable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="preDmg"></param>
        public void TakeDamage(PreDmgInfo preDmg);
    }
}