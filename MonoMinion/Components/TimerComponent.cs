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
        /// <summary>
        /// Gets the action trigger for this timer
        /// </summary>
        public Action Trigger;

        /// <summary>
        /// Gets the millisecond interval this timer uses
        /// </summary>
        public float Interval { get { return interval; } }
        protected float interval;

        /// <summary>
        /// Gets whether the timer is set on a loop
        /// </summary>
        public bool IsLoop { get { return IsLoop; } }
        protected bool isLoop;

        /// <summary>
        /// If a Timer IsActive is set to false, no updates will be made
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
        }
        protected bool isActive;

        /// <summary>
        /// Gives a number between 0 and 1 depending on how close to the elapsed time is to the interval 
        /// </summary>
        public float PercentageElapsed
        {
            get
            {
                if (Interval > 0f)
                    return elapsed / interval;

                return 0f;
            }
        }


        /// <summary>
        /// Specifies whether a Timer has finished its run
        /// </summary>
        public bool IsFinished { get { return isFinished; } }
        protected bool isFinished;


        /// <summary>
        /// Gets the number of loops this timer has run through
        /// </summary>
        public int LoopCount { get { return loopCount; } }
        protected int loopCount;

        protected float elapsed;
        #endregion


        #region Constructors
        /// <summary>
        /// Construct the timer
        /// </summary>
        /// <param name="interval">The interval in milliseconds between trigger calls</param>
        /// <param name="trigger">The method we want to trigger</param>
        /// <param name="isLoop">Specifies whether this particular trigger is a loop or not</param>
        /// <param name="autoStart">Whether to automatically start the Timer (defaults to true)</param>
        internal Timer(float interval, Action trigger, bool isLoop, bool autoStart = true)
        {
            this.interval = interval;
            this.Trigger = trigger;

            this.elapsed = 0f;
            this.isLoop = isLoop;
            this.loopCount = 0;

            isActive = autoStart;
            isFinished = false;
        }

        /// <summary>
        /// Construct the timer
        /// </summary>
        /// <param name="interval">The interval in milliseconds between trigger calls</param>
        /// <param name="isLoop">Specifies whether this particular trigger is a loop or not</param>
        /// <param name="autoStart">Whether to automatically start the Timer (defaults to true)</param>
        internal Timer(float interval, bool isLoop, bool autoStart = true)
        {
            this.interval = interval;
            this.Trigger = null;

            this.elapsed = 0f;
            this.isLoop = isLoop;
            this.loopCount = 0;

            isActive = autoStart;
            isFinished = false;
        }
        #endregion


        #region Timer Action methods
        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            isActive = true;
            isFinished = false;
            elapsed = 0f;
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="interval">The new interval to restart with</param>
        public void Start(float interval)
        {
            this.interval = interval;
            Start();
        }


        /// <summary>
        /// Pauses a timer
        /// </summary>
        public void Pause()
        {
            isActive = false;
        }
        
        /// <summary>
        /// Unpauses a timer
        /// </summary>
        /// <returns>False if the Timer is not a loop and has finished its run</returns>
        public bool Unpause()
        {
            if (isFinished)
                return false;
            else
                isActive = true;

            return true;
        }

        /// <summary>
        /// Stops a timer and resets it
        /// </summary>
        public void Stop()
        {
            isActive = false;
            isFinished = false;
            elapsed = 0f;
        }
        #endregion


        #region Main methods
        /// <summary>
        /// Updates the current timer
        /// </summary>
        /// <param name="milliseconds">Total milliseconds that have elapsed</param>
        public void Update(float milliseconds)
        {
            if (isActive)
            {
                elapsed += milliseconds;

                if (elapsed >= interval)
                {
                    // The Timer has finished its run
                    isFinished = true;

                    // Execute action if applicable
                    if (Trigger != null)
                        Trigger.Invoke();

                    if (isLoop)
                    {
                        elapsed -= interval;
                        loopCount++;
                    }
                    else
                    {
                        // TODO: Create a Timer caching system
                        isActive = false;
                        //Destroy();
                    }
                }
            }
        }

        /// <summary>
        /// Removes this timer from the manager
        /// </summary>
        public void Destroy()
        {
            TimerManager.Remove(this);
        }
        #endregion


        #region Factory methods
        /// <summary>
        /// Creates a timer
        /// </summary>
        /// <param name="interval">The interval in milliseconds between trigger calls</param>
        /// <param name="trigger">The method we want to trigger</param>
        /// <param name="isLoop">Whether this timer is a constant loop or not</param>
        /// <param name="autoStart">Whether to automatically start the Timer (defaults to true)</param>
        /// <returns>The timer that was created and added to the manager</returns>
        public static Timer Create(float interval, Action trigger, bool isLoop, bool autoStart = true)
        {
            Timer timer = new Timer(interval, trigger, isLoop, autoStart);
            TimerManager.Add(timer);

            return timer;
        }

        /// <summary>
        /// Creates a timer
        /// </summary>
        /// <param name="interval">The interval in milliseconds between trigger calls</param>
        /// <param name="isLoop">Whether this timer is a constant loop or not</param>
        /// <param name="autoStart">Whether to automatically start the Timer (defaults to true)</param>
        /// <returns>The timer that was created and added to the manager</returns>
        public static Timer Create(float interval, bool isLoop, bool autoStart = true)
        {
            Timer timer = new Timer(interval, isLoop, autoStart);
            TimerManager.Add(timer);

            return timer;
        }
        #endregion
    }


    /// <summary>
    /// Timer Manager component that holds on to all running timers
    /// </summary>
    public class TimerManager : GameComponent
    {
        protected List<Timer> toRemove;
        protected List<Timer> timers;

        #region Singleton Methods and Variables
        public static TimerManager Instance;

        /// <summary>
        /// Adds a new timer to the manager
        /// </summary>
        /// <param name="Timer">The timer to add</param>
        public static void Add(Timer Timer) { Instance.timers.Add(Timer); }

        /// <summary>
        /// Removes a timer from the manager
        /// </summary>
        /// <param name="Timer">The timer to remove</param>
        public static void Remove(Timer Timer) { Instance.toRemove.Add(Timer); }
        #endregion


        /// <summary>
        /// Creates a timer manager component and creates a singleton 
        /// </summary>
        /// <param name="game">Current game</param>
        public TimerManager(Game game)
            : base(game)
        {
            this.toRemove = new List<Timer>();
            this.timers = new List<Timer>();

            TimerManager.Instance = this;
        }

        /// <summary>
        /// Updates all timers
        /// </summary>
        /// <param name="gametime">Current game time</param>
        public override void Update(GameTime gametime)
        {
            if (toRemove.Count > 0)
            {
                for (int i = 0; i < toRemove.Count; i++)
                    timers.Remove(toRemove[i]);

                toRemove.Clear();
            }

            for (int i = 0; i < timers.Count; i++)
            {
                if (timers[i].IsActive)
                    timers[i].Update((float)gametime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Disposes of the Timer Manager component
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                toRemove.Clear();
                timers.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
