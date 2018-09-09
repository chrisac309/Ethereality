namespace KupoGames.Character
{
	public enum StatModType
	{
		Flat = 100,
		PercentAdd = 200,
		PercentMult = 300,
	}

	public enum StatModSource {
		AllyBuff,
		AllyDebuff,
		EnemyBuff,
		EnemyDebuff,
		SelfBuff,
		SelfDebuff,
	}

	// Will need to differentiate between Stats in-battle and overall stats
	public class StatModifier
	{
		public readonly float Value;
		public readonly StatModType Type;
		public readonly int CurrentLife;
		public readonly int Duration;
		public readonly StatModSource Source;

		private static int INDEFINITE_BUFF = -1;

		public StatModifier(float value, StatModType type, int duration, StatModSource source)
		{
			Value = value;
			Type = type;
			CurrentLife = 0;
			Duration = duration;
			Source = source;
		}

		public StatModifier(float value, StatModType type) : this(value, type, INDEFINITE_BUFF, StatModSource.SelfBuff) { }

		public StatModifier(float value, StatModType type, int duration) : this(value, type, INDEFINITE_BUFF, StatModSource.SelfBuff) { }

		public StatModifier(float value, StatModType type, StatModSource source) : this(value, type, INDEFINITE_BUFF, source) { }
	}
}
