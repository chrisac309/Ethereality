using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KupoGames.Characters.CharacterStats
{
	[Serializable]
	public class CharacterStat
	{
		public int BaseValue;

		protected bool isDirty = true;
		protected int lastBaseValue;

		protected int _value;
		public virtual int Value {
			get {
				if(isDirty || lastBaseValue != BaseValue) {
					lastBaseValue = BaseValue;
					_value = CalculateFinalValue();
					isDirty = false;
				}
				return _value;
			}
		}

		protected readonly List<StatModifier> statModifiers;
		public readonly ReadOnlyCollection<StatModifier> StatModifiers;

		CharacterStat()
		{
			statModifiers = new List<StatModifier>();
			StatModifiers = statModifiers.AsReadOnly();
		}

		public CharacterStat(int baseValue) : this()
		{
			BaseValue = baseValue;
		}

        /// <summary>
        /// Adds a new stat modifier to the current stat.
        /// </summary>
        /// <param name="mod"></param>
		public virtual void AddModifier(StatModifier mod)
		{
			isDirty = true;
			statModifiers.Add(mod);
			statModifiers.Sort(CompareModifierDuration);
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (statModifiers.Remove(mod))
			{
				isDirty = true;
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(StatModSource source)
		{
			bool didRemove = false;

			for (int i = statModifiers.Count - 1; i >= 0; i--)
			{
				if (statModifiers[i].Source == source)
				{
					isDirty = true;
					didRemove = true;
					statModifiers.RemoveAt(i);
				}
			}
			return didRemove;
		}

		protected virtual int CompareModifierDuration(StatModifier a, StatModifier b)
		{
			if (a.Duration < b.Duration)
				return -1;
			else if (a.Duration > b.Duration)
				return 1;
			return 0; //if (a.Duration == b.Duration)
		}
		
		protected virtual int CalculateFinalValue()
		{
			float finalValue = BaseValue;
			float sumPercentAdd = 0;

			for (int i = 0; i < statModifiers.Count; i++)
			{
				StatModifier mod = statModifiers[i];

				if (mod.Type == StatModType.Flat)
				{
					finalValue += mod.Value;
				}
				else if (mod.Type == StatModType.PercentAdd)
				{
					sumPercentAdd += mod.Value;

					if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
					{
						finalValue *= 1 + sumPercentAdd;
						sumPercentAdd = 0;
					}
				}
				else if (mod.Type == StatModType.PercentMult)
				{
					finalValue *= 1 + mod.Value;
				}
			}

            // Values should not go below zero
            if (finalValue < 0)
            {
                finalValue = 0;
            }

			// Workaround for float calculation errors, like displaying 12.00002 instead of 12
			return (int)Math.Round(finalValue, 4);
		}
	}
}
