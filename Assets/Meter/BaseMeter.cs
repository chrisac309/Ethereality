using KupoGames.Characters.CharacterStats;
using System.Collections.Generic;

namespace KupoGames.Meter
{
    /// <summary>
    /// Defines a base meter that works as an action meter.
    /// Can be extended to define specialty meters.
    /// </summary>
    public class BaseMeter<T>
    { 

        private CharacterStat _maxMeter;
        private Queue<T> _meter;

        public BaseMeter(int maxMeter = 5)
        {
            _maxMeter.BaseValue = 5;
            _meter = new Queue<T>();
        }

        public Queue<T> Meter
        {
            get
            {
                if (_meter == null)
                {
                    _meter = new Queue<T>();
                }
                return _meter;
            }
            set
            {
                _meter = value;
                while(_meter.Count > _maxMeter.Value)
                {
                    _meter.Dequeue();
                }
            }
        }

        public CharacterStat MaxMeter
        {
            get
            {
                return _maxMeter;
            }

            set
            {
                _maxMeter = value;
            }
        }

        public bool CanAddAnyToMeter()
        {
            return CanAddToMeter(1);
        }

        public bool CanAddToMeter(int amountToAdd)
        {
            return (_meter.Count + amountToAdd <= _maxMeter.Value);
        }

        /// <summary>
        /// Adds to the meter. Returns true if it increased.
        /// </summary>
        /// <param name="segment"></param>
        public virtual bool AddToMeter(T segment)
        {
            if(CanAddAnyToMeter())
            {
                _meter.Enqueue(segment);
                return true;
            }
            return false;
        }

        public bool CanRemoveAnyFromMeter()
        {
            return CanRemoveFromMeter(1);
        }

        public bool CanRemoveFromMeter(int amountToRemove)
        {
            return _meter.Count > amountToRemove;
        }

        /// <summary>
        /// Removes the object targetObj. Returns that object.
        /// </summary>
        /// <param name="targetIndex"></param>
        /// <returns></returns>
        public T RemoveFromMeter(T targetObj)
        {
            T removed = default(T);
            bool rem = false;

            int initialSize = _meter.Count;
            for(int i = 0; i < initialSize; i++)
            {
                T currRem = _meter.Dequeue();
                if (!rem && currRem.Equals(targetObj))
                {
                    removed = currRem;
                    rem = true;
                }
                else
                {
                    _meter.Enqueue(currRem);
                }

            }

            return removed;
        }

        /// <summary>
        /// Removes the object at the target index (0 - 4). Returns that object.
        /// </summary>
        /// <param name="targetIndex"></param>
        /// <returns></returns>
        public virtual T RemoveFromMeter(int targetIndex)
        {
            T removed = default(T);

            int initialSize = _meter.Count;
            for (int i = 0; i < initialSize; i++)
            {
                T currRem = _meter.Dequeue();
                if (i == targetIndex) {
                    removed = currRem;
                }
                else
                {
                    _meter.Enqueue(currRem);
                }

            }

            return removed;
        }

        public virtual T RemoveOldestFromMeter()
        {
            if(CanRemoveAnyFromMeter())
            {
                return _meter.Dequeue();
            } else
            {
                return default(T);
            }
        }

    }

}
