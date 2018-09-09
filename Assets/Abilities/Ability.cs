using KupoGames.Characters;
using KupoGames.Singletons;

namespace KupoGames.Abilities
{
    public interface IAbility
    {
        /// <summary>
        /// The name of the ability
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The description for the ability
        /// </summary>
		string Description { get; set; }

        /// <summary>
        /// Is this ability chainable?
        /// </summary>
		bool Chainable { get; set; }

		Elements.Type ElementType { get; set; }

		int Cost { get; set; }

		float DamageModifier { get; set; }

		Priorities.Speed Priority { get; set; }

        /// <summary>
        /// Indicates whether this ability is Passive.  Used for passives and ultimates.
        /// </summary>
		bool Passive { get; set; }

        /// <summary>
        /// Is this ability an Ultimate ability?
        /// </summary>
		bool Ultimate { get; set; }

        /// <summary>
        /// Activates the ability of the caster.
        /// </summary>
        /// <param name="caster"> The character casting the ability. </param>
        /// <param name="enemyTargets"> The enemy targets selected by the character. </param>
        /// <param name="allyTargets"> The allied targets selected by the character. </param>
        /// <returns> Whether the activation succeeded. </returns>
		bool Activate(Character caster, Character[] enemyTargets, Character[] allyTargets);

    }
}