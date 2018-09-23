using KupoGames.Characters;
using KupoGames.Singletons;

namespace KupoGames.Abilities
{
    public interface IStatusEffect {

        int Duration {get;set;}
        string Description {get;set;}
        void InflictStatus(Character onTarget);


    }
}