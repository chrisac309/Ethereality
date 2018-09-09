
using KupoGames.Characters;
using KupoGames.Characters.Cloaks;
using KupoGames.Meter;
using KupoGames.Singletons;

namespace KupoGames.Abilities.PensiveAbilities
{
    /// <summary>
    /// Spiricast (Higher) [1 CHAIN] - Destroy the least recently summoned spirit. 
    /// Inflict 200% of that spirit’s element as damage to an enemy.
    /// </summary>
    /// <param name=""></param>
	public class Spiricast : IAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Chainable { get; set; }
        public Elements.Type ElementType { get; set; }
        public int Cost { get; set; }
        public float DamageModifier { get; set; }
        public Priorities.Speed Priority { get; set; }
        public bool Passive { get; set; }
        public bool Ultimate { get; set; }

        public Spiricast() {
            Name = "Spiricast";
            Description = "Destroy the oldest spirit to inflict that spirit's element to an enemy.";
            Chainable = true;
            ElementType = Elements.Type.Neutral;
            Cost = 1;
            DamageModifier = 2.0f;
            Priority = Priorities.Speed.Higher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="enemyTargets"> Contains the single selection for the enemy.</param>
        /// <param name="allyTargets"></param>
        /// <returns></returns>
		public bool Activate(Character caster, Character[] enemyTargets, Character[] allyTargets)
        {
            Pensiveness penChar = caster as Pensiveness;

            if (penChar.PenMeter.CanRemoveAnyFromMeter())
            {
                PensiveMeter.Spirits spirit = penChar.PenMeter.RemoveOldestFromMeter();
                int damage = BattleCalculator.CalculateDamage(caster, enemyTargets[0], 2.0f, (Elements.Type) spirit);

                if (damage != BattleCalculator.AttackMissed)
                {
                    enemyTargets[0].TakeDamage(damage);
                }
            }

            return false;
        }

    }
}
