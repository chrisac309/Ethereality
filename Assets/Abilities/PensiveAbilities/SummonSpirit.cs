
using KupoGames.Characters;
using KupoGames.Characters.Cloaks;
using KupoGames.Meter;
using KupoGames.Singletons;

namespace KupoGames.Abilities.PensiveAbilities
{
    /// <summary>
    /// Summon Spirit (Medium)  [2 CHAIN] - Summons a spirit from the available spirit pool.
    /// </summary>
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
            Name = "Summon Spirit";
            Description = "Summons a spirit from the available spirit pool.";
            Chainable = true;
            ElementType = Elements.Type.Neutral;
            Cost = 2;
            DamageModifier = 0.0f;
            Priority = Priorities.Speed.Medium;
            Passive = false;
            Ultimate = false;
        }

        public bool Activate(Character caster, Character[] enemyTargets, Character[] allyTargets)
        {
            // Select a spirit to summon
            // Spirit = CharacterUI.ShowSummonSpiritOptions
            PensiveMeter.Spirits s = PensiveMeter.Spirits.None;
            Pensiveness pen = caster as Pensiveness;
            pen.PenMeter.AddSpirit(s);
            return false;
        }
    }
}
