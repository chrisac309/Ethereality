using KupoGames.Singletons;
using System.Collections.Generic;

namespace KupoGames.Meter
{

    /// <summary>
    /// Defines the meter for the cloak that consists of up to five spirits
    /// </summary>
    public class PensiveMeter : BaseMeter<PensiveMeter.Spirits>
    {

        public enum Spirits
        {
            Red = Elements.Type.Fire,
            Green = Elements.Type.Plant,
            Blue = Elements.Type.Water,
            Yellow = Elements.Type.Electric,
            Brown = Elements.Type.Wood,
            None = Elements.Type.Neutral
        }

        public PensiveMeter() : base()
        {

        }

        public void AddSpirit(Spirits s)
        {
            Meter.Enqueue(s);

        }


        public Spirits RemoveThreeSimilarSpirits()
        {
            Spirits three = Spirits.None;
            int[] counts = new int[5];

            // Determines which spirit occurs 3 times
            foreach (Spirits s in Meter)
            {
                if (three == Spirits.None)
                {
                    switch (s)
                    {
                        case Spirits.Red:
                            counts[0]++;
                            if (counts[0] == 3)
                            {
                                three = Spirits.Red;
                            }
                            break;
                        case Spirits.Green:
                            counts[1]++;
                            if (counts[1] == 3)
                            {
                                three = Spirits.Green;
                            }
                            break;
                        case Spirits.Blue:
                            counts[2]++;
                            if (counts[2] == 3)
                            {
                                three = Spirits.Blue;
                            }
                            break;
                        case Spirits.Brown:
                            counts[3]++;
                            if (counts[3] == 3)
                            {
                                three = Spirits.Brown;
                            }
                            break;
                        case Spirits.Yellow:
                            counts[4]++;
                            if (counts[4] == 3)
                            {
                                three = Spirits.Yellow;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            // Removes the triple spirit, if one exists
            if (three != Spirits.None)
            {
                for (int i = 0; i < 2; i++)
                {
                    RemoveFromMeter(three);
                }
            }

            // Returns the Spirit that was removed thrice
            return three;
        }

        /// <summary>
        /// Removes three different spirits, prioritizing the oldest spirits in the meter
        /// </summary>
        /// <returns></returns>
        public bool RemoveThreeDifferentSpirits()
        {
            bool success = false;
            Spirits[] copy = Meter.ToArray();
            int i_found = 0;
            int j_found = 0;
            int k_found = 0;

            // Find three different spirits
            for (int i = 0; i < copy.Length - 2 && !success; i++)
            {
                for (int j = 1; j < copy.Length - 1 && !success; j++)
                {
                    for (int k = 2; k < copy.Length && !success; k++)
                    {
                        if(copy[i] != copy[j] && copy[j] != copy[k])
                        {
                            i_found = i;
                            j_found = j;
                            k_found = k;
                            success = true;
                        }
                    }
                }
            }

            // Remove three different spirits
            for (int i = 0; i < copy.Length; i++)
            {
                Spirits s = Meter.Dequeue();
                if(!( i == i_found || i == j_found || i == k_found ))
                {
                    Meter.Enqueue(s);
                }
                
            }           

            return success;
        }

        public bool RemoveFiveDifferentSpirits()
        {
            Spirits[] allTypes = { Spirits.Red, Spirits.Green, Spirits.Blue, Spirits.Brown, Spirits.Yellow };
            int currentSpirit = 0;
            while (CanRemoveAnyFromMeter())
            {
                currentSpirit++;
                RemoveFromMeter(allTypes[currentSpirit]);
            }

            return (currentSpirit == 4);
        }

    }


   
}
