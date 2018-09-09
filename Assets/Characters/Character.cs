using KupoGames.Abilities;
using KupoGames.Characters.CharacterStats;
using KupoGames.Singletons;
using System.Collections.Generic;
using System;

/// <summary>
/// Defines a battle character. To be used for battle scenarios.
/// </summary>
namespace KupoGames.Characters
{
	public abstract class Character {

        #region Global standard stats
        public static int StandardAccuracy = 100;
        public static int StandardAvoidability = 5;
        #endregion

        #region Abstract Stat Modifiers
        public abstract StatModifier HealthModifier { get; }
        public abstract StatModifier AttackModifier { get; }
        public abstract StatModifier DefenseModifier { get; }
        #endregion

        #region Leveling constants
        private const int ATTACK_INCREASE = 5;
        private const int DEFENSE_INCREASE = 3;
        private const int HEALTH_INCREASE = 10;
        #endregion

        #region Private character members
        private CharacterStat _level;
		private CharacterStat _cloakLevel;
		private CharacterStat _health;
		private CharacterStat _maxHealth;
		private CharacterStat _attack;
		private CharacterStat _defense;
        private CharacterStat _accuracy;
        private CharacterStat _avoidability;
		private CharacterStat _crit;
		private CharacterStat _maxActionMeter;
		private CharacterStat _meterRegen;
		private CharacterStat _currentActionMeter;

		private List<IAbility> _abilities;
        private List<IAbility> _initialAbilities;
		private List<Elements.Type> _weaknesses;
		private List<Elements.Type> _strengths;
		private List<Elements.Type> _immunities;

        #endregion

        public enum State
        {
            Alive,
            Dead,
            Battling,
            Waiting
        }

        public State state = State.Alive;

        /// <summary>
        /// Creates a character with all parameters defined
        /// </summary>
        /// <param name="level"> The level of the character. </param>
        /// <param name="cloakLevel"> The cloak level of the character, if one exists. </param>
        /// <param name="health"> The health of the character. This is the max health. </param>
        /// <param name="attack"> The attack value of the character. </param>
        /// <param name="defense"> The defense value of the character. </param>
        /// <param name="crit"> The critical chance of the character, from 0 to 100. </param>
        /// <param name="weaknesses"> A list of elemental weaknesses that the character has. </param>
        /// <param name="strengths"> A list of elemental strengths that the character has. </param>
        /// <param name="immunities"> A list of elemental immunities that the character has. </param>
        /// <param name="abilities"> The abilities that this character possesses. </param>
        public Character(int level, int cloakLevel, int health, int attack, int defense, int accuracy, int avoidability, int crit, List<Elements.Type> weaknesses, List<Elements.Type> strengths, List<Elements.Type> immunities, Dictionary<IAbility, int> abilities) {
            SetLevel(level);
			SetCloakLevel(cloakLevel);
			SetMaxHealth(health);
			SetHealth(health);
			SetAttack(attack);
			SetDefense(defense);
            SetStatModifiers();
            SetAccuracy(accuracy);
            SetAvoidability(avoidability);
			SetCrit(crit);
			SetElementalWeaknesses(weaknesses);
			SetElementalStrengths(strengths);
			SetElementalImmunities(immunities);
			SetCurrentAbilities(abilities);
            SetInitialMeter();
		}

        /// <summary>
        /// Sets the permanent modifiers associated with a particular class/cloak
        /// </summary>
        private void SetStatModifiers()
        {
            _attack.AddModifier(AttackModifier);
            _defense.AddModifier(DefenseModifier);
            _health.AddModifier(HealthModifier);
        }

        /// <summary>
        /// (Not recommended) Use this constructor if you want to create a basic character (like a minion)
        /// </summary>
        public Character() 
            : this(
                  level: 0, 
                  cloakLevel: 0, 
                  health: 1, 
                  attack: 1, 
                  defense: 0,
                  accuracy: StandardAccuracy,
                  avoidability: StandardAvoidability,
                  crit: 0, 
                  weaknesses: new List<Elements.Type>(), 
                  strengths: new List<Elements.Type>(), 
                  immunities: new List<Elements.Type>(), 
                  abilities: new Dictionary<IAbility, int>())
		{
		}

        /// <summary>
        ///  Takes damage, directly reducing player health
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            SetHealth(GetCurrentHealth().BaseValue - damage);
            if (GetCurrentHealth().BaseValue > 0)
            {
                // Alive
                state = State.Alive;
            }
            else
            {
                // Dead
                state = State.Dead;
            }
        }

        /// <summary>
        /// Restores health, directly increasing player health
        /// </summary>
        /// <param name="restoreAmount"></param>
        public void RestoreHealth(int restoreAmount)
        {
            SetHealth(GetCurrentHealth().BaseValue + restoreAmount);
        }

        #region Elements section

        /// <summary>
        /// Checks how the element affects the character. 
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The affinity against that element</returns>
        public Elements.Affinity CheckElement(Elements.Type element)
        {
            Elements.Affinity elementStatus = Elements.Affinity.Neutral;

            // Strengths will overrule weaknesses. Immunities overrule both
            if (_immunities.Contains(element))
            {
                elementStatus = Elements.Affinity.Immune;
            }
            else if (_strengths.Contains(element))
            {
                elementStatus = Elements.Affinity.Strong;
            }
            else if (_weaknesses.Contains(element))
            {
                elementStatus = Elements.Affinity.Weak;
            }

            return elementStatus;
        }


        private void SetElementalImmunities(List<Elements.Type> immunities)
		{
			_immunities = immunities;
		}

		public void AddElementalImmunity(Elements.Type imm)
		{
			if(!_immunities.Contains(imm))
			{
				_immunities.Add(imm);
			}
		}

		public void RemoveElementalImmunity(Elements.Type imm)
		{
			if(_immunities.Contains(imm))
			{
				_immunities.Remove(imm);
			}
		}

		private void SetElementalStrengths(List<Elements.Type> strengths)
		{
			_strengths = strengths;
		}

		public void AddElementalStrength(Elements.Type str)
		{
			if (!_strengths.Contains(str))
			{
				_strengths.Add(str);
			}
		}

		public void RemoveElementalStrength(Elements.Type str)
		{
			if (_strengths.Contains(str))
			{
				_strengths.Remove(str);
			}
		}

		private void SetElementalWeaknesses(List<Elements.Type> weaknesses)
		{
			_weaknesses = weaknesses;
		}

		public void AddElementalWeakness(Elements.Type weak)
		{
			if (!_weaknesses.Contains(weak))
			{
				_weaknesses.Add(weak);
			}
		}

		public void RemoveElementalWeakness(Elements.Type weak)
		{
			if (_weaknesses.Contains(weak))
			{
				_weaknesses.Remove(weak);
			}
		}

		#endregion

		#region Abilities

        /// <summary>
        /// Private setter for permanently learned abilities.
        /// </summary>
        /// <param name="abilities"></param>
		private void SetCurrentAbilities(Dictionary<IAbility, int> abilities)
		{
            if(_initialAbilities == null)
            {
                foreach(KeyValuePair<IAbility, int> a in abilities)
                {
                    if (a.Value <= GetCloakLevel().Value)
                    {
                        _initialAbilities.Add(a.Key);
                    }
                }
            }
			_abilities = _initialAbilities;
		}

        /// <summary>
        /// Public setter for temporary abilities, i.e. "Limit Break" form or "toad" form
        /// </summary>
        /// <param name="abilities"></param>
        public void SetTemporaryAbilities(List<IAbility> abilities)
        {
            _initialAbilities = _abilities;
            _abilities = new List<IAbility>(abilities);
        }

        /// <summary>
        /// Reverts abilities back to their state before battle
        /// </summary>
        public void UnsetTemporaryAbilities()
        {
            _abilities = _initialAbilities;
        }

        /// <summary>
        /// Used upon acquiring a new, PERMANENT IAbility
        /// </summary>
        /// <param name="additional"></param>
        public void AddIAbility(IAbility additional)
        {
            _abilities.Add(additional);
        }

		/// <summary>
		/// Casts the selected IAbility. Returns whether or not the IAbility succeeded.
		/// </summary>
		/// <param name="cast"> The IAbility to cast. </param>
		/// <param name="enemyTargets"> The enemies that this IAbility will target. </param>
		/// <param name="allyTargets"> The allies that this IAbility will target. </param>
		/// <returns></returns>
		public bool CastIAbility(IAbility cast, Character[] enemyTargets, Character[] allyTargets)
		{
			// First, pay the cost.
			if(PayIAbilityCost(cast.Cost))
			{
				// Then, activate the IAbility if the cost can be paid.
				return cast.Activate(this, enemyTargets, allyTargets);
			}
			return false;
		}

        #endregion

        #region Level

        /// <summary>
        /// Gets the level of the Character
        /// </summary>
        /// <returns></returns>
        public CharacterStat GetLevel() {
			return _level;
		}

        /// <summary>
        /// Sets the level of the Character
        /// </summary>
        private void SetLevel(int level) {
            if (_level != null)
            {
                _level.BaseValue = level;
            }
            else
            {
                _level = new CharacterStat(level);
            }
		}

        /// <summary>
        /// Character level up
        /// </summary>        
        public void LevelUp()
        {
            SetLevel(GetLevel().BaseValue + 1);

            // Increase all stats
            IncreaseMaxHealth(GetMaxHealth().BaseValue + HEALTH_INCREASE);
            IncreaseAttack(GetAttack().BaseValue + ATTACK_INCREASE);
            IncreaseDefense(GetDefense().BaseValue + DEFENSE_INCREASE);
        }

        /// <summary>
        /// Gets the cloak level of the character
        /// </summary>
        /// <returns></returns>        
        public CharacterStat GetCloakLevel()
        {
            return _cloakLevel;
        }

        /// <summary>
        /// Sets the cloak level of the character
        /// </summary>
        /// <param name="cloakLevel"></param>        
        private void SetCloakLevel(int cloakLevel)
		{
            if (_cloakLevel != null)
            {
                _cloakLevel.BaseValue = cloakLevel;
            } else
            {
                _cloakLevel = new CharacterStat(cloakLevel);
            }
		}


        /// <summary>
        /// Level up the cloak
        /// </summary>        
        public void LevelCloakUp()
        {
            SetCloakLevel(GetCloakLevel().BaseValue + 1);
            
            // Potential additional checks for things like new, learned abilities
        }
        #endregion

        #region Health

        /// <summary>
        /// Gets the max health value of the Character
        /// </summary>
        /// <returns></returns>
        public CharacterStat GetMaxHealth()
		{
			return _maxHealth;
		}

        /// <summary>
        /// Sets the max health value of the Character
        /// </summary>
        private void SetMaxHealth(int health)
		{
            if( _maxHealth != null )
            {
                _maxHealth.BaseValue = health;
            }
            else
            {
                _maxHealth = new CharacterStat(health);
            }
        }

        /// <summary>
        /// Increases max health. Trickles down to current health. Used for leveling up.
        /// </summary>
        /// <param name="amount"></param>
        private void IncreaseMaxHealth(int amount)
        {
            _maxHealth.BaseValue += amount;
            _health.BaseValue += amount;
        }

        /// <summary>
        /// Gets the current health value of the Character.
        /// </summary>
        /// <returns></returns>        
        public CharacterStat GetCurrentHealth()
        {
            return _health;
        }

        /// <summary>
        /// Sets the health value of the Character
        /// </summary>
        /// <param name="health"></param>        
        private void SetHealth(int health)
		{
            if( _health != null )
            {
                _health.BaseValue = health;
            } else
            {
                _health = new CharacterStat(health);
            }
		}

        #endregion

        #region Attack

        /// <summary>
        /// Gets the attack value of the Character
        /// </summary>
        /// <returns></returns>        
        public CharacterStat GetAttack(){
			return _attack;
		}

        /// <summary>
        /// Sets the attack value of the Character
        /// </summary>
        /// <param name="attack"></param>        
        private void SetAttack(int attack) {
			_attack.BaseValue = attack;
		}

        /// <summary>
        /// Increases the attack value by a flat amount (Used for leveling up)
        /// </summary>
        /// <param name="flatAmount"></param>        
        private void IncreaseAttack(int flatAmount){
			_attack.BaseValue += flatAmount;
		}

        #endregion

        #region Defense

        /// <summary>
        /// Gets the defense value of the Character
        /// </summary>
        /// <returns></returns>        
        public CharacterStat GetDefense()
		{
			return _defense;
		}

        /// <summary>
        /// Sets the defense value of the Character
        /// </summary>
        /// <param name="defense"></param>
        private void SetDefense(int defense)
		{
			_defense.BaseValue = defense;
		}

        /// <summary>
        /// Increases the defense value by a flat amount (Used for leveling up)
        /// </summary>
        /// <param name="flatAmount"></param>
        private void IncreaseDefense(int flatAmount)
		{
			_defense.BaseValue += flatAmount;
		}

        #endregion

        #region Accuracy

        public void SetAccuracy(int accuracy)
        {
            _accuracy.BaseValue = accuracy;
        }

        public CharacterStat GetAccuracy()
        {
            return _accuracy;
        }

        #endregion

        #region Avoidability

        public void SetAvoidability(int accuracy)
        {
            _avoidability.BaseValue = accuracy;
        }

        public CharacterStat GetAvoidability()
        {
            return _avoidability;
        }

        #endregion

        #region Crit

        /// <summary>
        /// Gets the crit value of the Character, from 0 to 100 as a CharacterStat
        /// </summary>
        /// <returns></returns>        
        public CharacterStat GetCrit()
		{
            return _crit;
		}

        /// <summary>
        /// Sets the crit value of the Character
        /// </summary>
        /// <param name="crit"></param>        
        private void SetCrit(int crit)
		{
			_crit.BaseValue = crit;
		}

        /// <summary>
        /// Increases the crit value by a flat amount
        /// </summary>
        /// <param name="flatAmount"></param>        
        private void IncreaseCrit(int flatAmount)
		{
            _crit.BaseValue += flatAmount;
		}

		#endregion

		#region ActionMeter

        private void SetInitialMeter()
        {
            SetMaxActionMeter(5);
            SetCurrentActionMeter(1);
            SetRegenRate(2);
        }

		public int GetMaxActionMeter()
		{
			return _maxActionMeter.BaseValue;
		}

		public void SetMaxActionMeter(int newMax)
		{
			_maxActionMeter.BaseValue = newMax;
		}

		public int GetCurrentActionMeter()
		{
			return _currentActionMeter.BaseValue;
		}

		public bool SetCurrentActionMeter(int newCurrent)
		{
			bool success = true;
			if (newCurrent >= _maxActionMeter.BaseValue) {
				_currentActionMeter = _maxActionMeter;
			}
			else if (newCurrent <= 0) {
				_currentActionMeter.BaseValue = 0;
			}
			else {
				_currentActionMeter.BaseValue = newCurrent;
			}
			return success;
		}

		public void SetRegenRate(int newRate)
		{
			_meterRegen.BaseValue = newRate;
		}

		public int GetRegenRate()
		{
			return _meterRegen.BaseValue;
		}

		public void RegenerateMeter()
		{
			SetCurrentActionMeter(GetCurrentActionMeter() + GetRegenRate());
		}

		private bool PayIAbilityCost(int costToCast)
		{
			bool paid = false;
			if(GetCurrentActionMeter() >= costToCast)
			{
				paid = SetCurrentActionMeter(GetCurrentActionMeter() - costToCast);
			}

			return paid;
		}

		#endregion

	}
}
