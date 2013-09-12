using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoMinion.Components
{
    /// <summary>
    /// Timer object that is handled by the TimerManager component
    /// </summary>
    public class Timer
    {
        #region Variables and Properties
        public Action Trigger;
        public float Interval;

        protected bool isLoop;
        protected int loopCount;
        protected float Elapsed;
        /// <summary>
        /// Gives a number between 0 and 1 depending on how close to the elapsed time is to the interval 
        /// </summary>
        public float PercentageElapsed
        {
            get
            {
                if (Interval > 0f)
                    return Elapsed / Interval;

                return 0f;
            }
        }

        /// <summary>
        /// Gets the number of loops this timer has run through
        /// </summary>
        public int Loops { get { return this.loopCount; } }
        #endregion

        /// <summary>
        /// Construct the timer
        /// </summary>
        /// <param name="interval">The interval in seconds between trigger calls</param>
        /// <param name="trigger">The method we want to trigger</param>
        /// <param name="isLoop">Specifies whether this particular trigger is a loop or not</param>
        public Timer(float interval, Action trigger, bool isLoop)
        {
            this.Interval = interval;
            this.Trigger = trigger;

            this.Elapsed = 0f;
            this.isLoop = isLoop;
            this.loopCount = 0;
        }

        /// <summary>
        /// Updates the current timer
        /// </summary>
        /// <param name="seconds">Total seconds that have elapsed</param>
        public void Update(float seconds)
        {
            Elapsed += seconds;
            if (Elapsed >= Interval)
            {
                Trigger.Invoke();
                if (isLoop)
                {
                    Elapsed -= Interval;
                    loopCount++;
                }
                else
                    Destroy();
            }
        }

        /// <summary>
        /// Removes this timer from the manager
        /// </summary>
        public void Destroy()
        {
            TimerManager.Remove(this);
        }


        /// <summary>
        /// Creates a timer
        /// </summary>
        /// <param name="Interval">The interval in seconds between trigger calls</param>
        /// <param name="Trigger">The method we want to trigger</param>
        /// <returns>The timer that was created and added to the manager</returns>
        public static Timer Create(float Interval, Action Trigger, bool isLoop)
        {
            Timer timer = new Timer(Interval, Trigger, isLoop);
            TimerManager.Add(timer);

            return timer;
        }
    }


    /// <summary>
    /// Timer Manager component that holds on to all running timers
    /// </summary>
    public class TimerManager : GameComponent
    {
        protected List<Timer> ToRemove;
        protected List<Timer> Timers;

        #region Singleton Methods and Variables
        public static TimerManager Instance;

        /// <summary>
        /// Adds a new timer to the manager
        /// </summary>
        /// <param name="Timer">The timer to add</param>
        public static void Add(Timer Timer) { Instance.Timers.Add(Timer); }

        /// <summary>
        /// Removes a timer from the manager
        /// </summary>
        /// <param name="Timer">The timer to remove</param>
        public static void Remove(Timer Timer) { Instance.ToRemove.Add(Timer); }
        #endregion


        /// <summary>
        /// Creates a timer manager component and creates a singleton 
        /// </summary>
        /// <param name="game">Current game</param>
        public TimerManager(Game game)
            : base(game)
        {
            this.ToRemove = new List<Timer>();
            this.Timers = new List<Timer>();

            TimerManager.Instance = this;
        }

        /// <summary>
        /// Updates all timers
        /// </summary>
        /// <param name="gametime">Current game time</param>
        public override void Update(GameTime gametime)
        {
            for (int i = 0; i < this.ToRemove.Count; i++)
                Timers.Remove(this.ToRemove[i]);

            for (int i = 0; i < this.Timers.Count; i++)
                this.Timers[i].Update((float)gametime.ElapsedGameTime.TotalSeconds);

            ToRemove.Clear();
        }
    }
}
