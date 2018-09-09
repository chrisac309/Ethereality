
using KupoGames.Meter;
using KupoGames.Abilities;
using KupoGames.Singletons;
using System.Collections.Generic;
using KupoGames.Characters.CharacterStats;
using KupoGames.Abilities.PensiveAbilities;

namespace KupoGames.Characters.Cloaks
{
    public class Pensiveness : Character {

        public PensiveMeter PenMeter;

        private static Dictionary<IAbility, int> _abilities = new Dictionary<IAbility, int>()
        {

        };

        /// <summary>
        /// Calls the base constructor for Character, as well as initializing the meter.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="cloakLevel"></param>
        /// <param name="health"></param>
        /// <param name="attack"></param>
        /// <param name="defense"></param>
        /// <param name="crit"></param>
        /// <param name="weaknesses"></param>
        /// <param name="strengths"></param>
        /// <param name="immunities"></param>
        /// <param name="abilities"></param>
        public Pensiveness(
            int level, 
            int cloakLevel, 
            int health, 
            int attack, 
            int defense, 
            int accuracy,
            int avoidability,
            int crit, 
            List<Elements.Type> weaknesses, 
            List<Elements.Type> strengths, 
            List<Elements.Type> immunities) 
            : base(
                  level, 
                  cloakLevel,
                  health,
                  attack,
                  defense, 
                  StandardAccuracy, 
                  StandardAvoidIAbility, 
                  crit,
                  weaknesses,
                  strengths,
                  immunities,
                  abilities)
        {
            PenMeter = new PensiveMeter();
        }

        public override StatModifier HealthModifier
        {
            get
            {
                return new StatModifier(1.0f, StatModType.PercentMult);
            }
        }

        public override StatModifier AttackModifier
        {
            get
            {
                return new StatModifier(1.2f, StatModType.PercentMult);
            }
        }

        public override StatModifier DefenseModifier
        {
            get
            {
                return new StatModifier(0.8f, StatModType.PercentMult);
            }
        }
    }
}