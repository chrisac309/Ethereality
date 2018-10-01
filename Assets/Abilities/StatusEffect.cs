
using KupoGames.Characters;

namespace Assets.Abilities
{
    public enum StatusType
    {
        Amnesia,
        Blind,
        Burn,
        Curse,
        Embargo,
        Fear,
        Fragile,
        Halt,
        Lock,
        Perish,
        Poison,
        Sick,
        Slow,
        Stun,
        Taunt,
        Trap,
        Weak,
        Void
    }

    public class StatusEffect
    {
        private StatusType _statusType;

        public int Duration { get; set; }

        public StatusEffect(StatusType status, int duration)
        {
            _statusType = status;
            Duration = duration;
        }

        public void ApplyStatusEffect(Character onTarget)
        {
            // Create a switch case that switches a boolean value for the character such as HasFear
            // FIgure out a way to apply status effects, but boolean values are probably the way to go
            // For the most part.
            
            // If already true for a HasStatus bool, don't say it's true again
        }

        public void UnapplyStatusEffect(Character onTarget)
        {
            // might not be applicable in the context of a status effect since the character will have a list
            // of status effects that have a corresponding duration.  Every turn, the duration of the status
            // effect will lower by one, and once it reaches a value of -1, it will be removed from the list
            // and be unapplied as necessary (just switch the boolean value back)
        }
    }
}
