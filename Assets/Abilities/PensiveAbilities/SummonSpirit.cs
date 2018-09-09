
using KupoGames.Characters;
using KupoGames.Meter;
using KupoGames.Singletons;

namespace KupoGames.Abilities.PensiveAbilities
{
    class SummonSpirit : IAbility
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

        public SummonSpirit()
        {
            Name = "";
        }

        public bool Activate(Character caster, Character[] enemyTargets, Character[] allyTargets)
        {
            throw new System.NotImplementedException();
        }
    }
}
