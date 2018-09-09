using KupoGames.Characters;
using UnityEngine;

namespace KupoGames.Singletons
{
    public static class BattleCalculator
    {
        public static int AttackMissed = -1;

        #region Calculation variables
        private const float _magicFactor = 100.0f / (20 * 255);

        private const float _weaknessMultiplier = 1.5f;
        private const float _strengthMultiplier = 0.5f;

        private const int _randomFactorLowerBound = 217;
        private const int _randomFactorUpperBound = _randomFactorLowerBound + 38;
        #endregion

        /*
        ROUND((A + B) / (100 + (A + B)) * (C + if(E, C,0) / 2) * C / (C + G) * D * 100 * H * F / (20 * 255))
        A = Character Level
        B = Cloak Level
        C = ATK of attacker
        D = Ability Damage Modifier
        E = True/False, determines if the ability Crit
        F = Random Number from 217 to 255
        G = DEF of defender
        H = Elemental damage modifier (Weaknesses/Strengths)
        */
        public static int CalculateDamage(Character attacker, Character defender, float damageModifier, Elements.Type elementType)
        {
            if (IsSuccessful(attacker.GetAccuracy().Value - defender.GetAvoidability().Value))
            {
                // (A + B) / (100 + (A + B))
                float levelFactor = (attacker.GetCloakLevel().Value + attacker.GetLevel().Value) / (100.0f + (attacker.GetCloakLevel().Value + attacker.GetLevel().Value));

                // (C + if(E, C,0) / 2)
                float critFactor = IsSuccessful(attacker.GetCrit().Value) ? attacker.GetAttack().Value + attacker.GetAttack().Value / 2.0f : attacker.GetAttack().Value;

                // C / (C + G) * D
                float attackFactor = (float)(attacker.GetAttack().Value) / (attacker.GetAttack().Value + defender.GetDefense().Value) * damageModifier;
                Elements.Affinity defElmStatus = defender.CheckElement(elementType);

                // H
                float elementFactor = 1.0f;
                switch (defElmStatus)
                {
                    case (Elements.Affinity.Immune):
                        // Take zero damage
                        elementFactor = 0.0f;
                        break;
                    case (Elements.Affinity.Strong):
                        // Take half damage
                        elementFactor = _strengthMultiplier;
                        break;
                    case (Elements.Affinity.Weak):
                        // Take extra damage
                        elementFactor = _weaknessMultiplier;
                        break;
                    default:
                        break;
                }

                // F
                float randomFactor = Random.Range(217, 255);

                return (int)(levelFactor * critFactor * attackFactor * elementFactor * randomFactor * _magicFactor);

            }

            // The attack missed
            return -1;
        }

        /// <summary>
        /// Given a % chance out of 100, returns whether or not an action succeeded. 
        /// </summary>
        /// <param name="chance"></param>
        /// <returns></returns>
        public static bool IsSuccessful(int chance)
        {
            int rand = (int)Random.Range(1f, 100f);
            return rand <= chance;
        }

    }
}
