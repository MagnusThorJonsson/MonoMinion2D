using System;
using Microsoft.Xna.Framework;

namespace MonoMinion.Components
{
    /// <summary>
    /// Simple Calendar component to track passage of time, counting in days, in a game.
    /// </summary>
    public class Calendar : GameComponent
    {
        #region Enums
        public enum Month
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        #endregion

        #region Variables and Properties
        private int _startYear;
        public int StartYear { get { return this._startYear; } }
        private Month _startMonth;
        public Month StartMonth { get { return this._startMonth; } }
        private int _startDay;
        public int StartDay
        {
            get { return this._startDay; }
            set
            {
                int checkDays = 31;
                // 30 day months
                if (CurrentMonth == Month.April || CurrentMonth == Month.June ||
                    CurrentMonth == Month.September || CurrentMonth == Month.November)
                    checkDays = 30;
                else if (CurrentMonth == Month.February)
                {
                    // Check if this is a leap year
                    if (((this.CurrentYear % 4) == 0 && (this.CurrentYear % 100) != 0) ||
                        (this.CurrentYear % 400) == 0)
                        checkDays = 29;
                    // Not a leap year
                    else
                        checkDays = 28;
                }

                if (value > checkDays)
                    this._startDay = checkDays;
                else if (value < 1)
                    this._startDay = 1;
                else
                    this._startDay = value;
            }
        }

        private int _currentYear;
        public int CurrentYear
        {
            get { return this._currentYear; }
            set
            {
                this._currentYear = value;
                this.OnChangedYear(null);
            }
        }
        private Month _currentMonth;
        public Month CurrentMonth
        {
            get { return this._currentMonth; }
            set
            {
                this._currentMonth = value;
                this.OnChangedMonth(null);
            }
        }
        private int _currentDay;
        public int CurrentDay
        {
            get { return this._currentDay; }
            set
            {
                int checkDays = 31;
                // 30 day months
                if (CurrentMonth == Month.April || CurrentMonth == Month.June ||
                    CurrentMonth == Month.September || CurrentMonth == Month.November)
                    checkDays = 30;
                else if (CurrentMonth == Month.February)
                {
                    // Check if this is a leap year
                    if (((this.CurrentYear % 4) == 0 && (this.CurrentYear % 100) != 0) ||
                        (this.CurrentYear % 400) == 0)
                        checkDays = 29;
                    // Not a leap year
                    else
                        checkDays = 28;
                }

                if (value > checkDays)
                    this._currentDay = checkDays;
                else if (value < 1)
                    this._currentDay = 1;
                else
                    this._currentDay = value;

                this.OnChangedDay(null);
            }
        }

        private bool _hasStarted;
        public bool HasStarted { get { return this._hasStarted; } }
        private bool _isPaused;
        public bool IsPaused { get { return this._isPaused; } }

        public int SecondsPerDay;
        private TimeSpan elapsedTime;

        public bool IsLastDay
        {
            get
            {
                int checkDays = 31;
                // 30 day months
                if (CurrentMonth == Month.April || CurrentMonth == Month.June ||
                    CurrentMonth == Month.September || CurrentMonth == Month.November)
                {
                    checkDays = 30;
                }
                else if (CurrentMonth == Month.February)
                {
                    // Check if this is a leap year
                    if (((this.CurrentYear % 4) == 0 && (this.CurrentYear % 100) != 0) ||
                        (this.CurrentYear % 400) == 0)
                        checkDays = 29;
                    // Not a leap year
                    else
                        checkDays = 28;
                }

                if (this._currentDay == checkDays)
                    return true;

                return false;
            }
        }
        public bool IsFirstDay
        {
            get
            {
                if (this._currentDay == 1)
                    return true;

                return false;
            }
        }
        public bool IsLastMonth
        {
            get
            {
                if (this._currentMonth == Month.December)
                    return true;

                return false;
            }
        }
        public bool IsFirstMonth
        {
            get
            {
                if (this._currentMonth == Month.January)
                    return true;

                return false;
            }
        }
        #endregion

        #region Event Handlers
        public event EventHandler ChangedDay;
        public event EventHandler ChangedMonth;
        public event EventHandler ChangedYear;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Calendar construction without setting start date
        /// </summary>
        /// <param name="game">Instance of the game object</param>
        public Calendar(Game game)
            : base(game)
        {
            // We default to a 60 second day
            this.SecondsPerDay = 60;

            this._startYear = this._currentYear = 0;
            this._startMonth = this._currentMonth = Month.January;
            this._startDay = this._currentDay = 1;

            this._hasStarted = false;
            this._isPaused = false;

            this.elapsedTime = TimeSpan.Zero;
        }

        /// <summary>
        /// Calendar constructor that sets the start date
        /// </summary>
        /// <param name="game">Instance of the game object</param>
        /// <param name="year">Starting Year</param>
        /// <param name="month">Starting Month</param>
        /// <param name="day">Starting Day</param>
        public Calendar(Game game, int year, Month month, int day)
            : base(game)
        {
            // We default to a 60 second day
            this.SecondsPerDay = 60;

            this._startYear = this._currentYear = year;
            this._startMonth = this._currentMonth = month;
            this._startDay = this._currentDay = day;

            this._hasStarted = false;
            this._isPaused = false;
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (this._hasStarted && !this._isPaused)
            {
                this.elapsedTime += gameTime.ElapsedGameTime;
                if (this.elapsedTime > TimeSpan.FromSeconds(this.SecondsPerDay))
                {
                    if (this.IsLastDay)
                    {
                        this.CurrentDay = 1;
                        if (this.IsLastMonth)
                        {
                            this.CurrentMonth = Month.January;
                            this.CurrentYear++;
                        }
                        else
                            this.CurrentMonth++;
                    }
                    else
                        this.CurrentDay++;

                    this.elapsedTime -= TimeSpan.FromSeconds(this.SecondsPerDay);
                }
            }

            base.Update(gameTime);
        }
        #endregion

        #region Setters
        /// <summary>
        /// Set or reset the start date
        /// </summary>
        /// <param name="year">Starting Year</param>
        /// <param name="month">Starting Month</param>
        /// <param name="day">Starting Day</param>
        public void SetStartDate(int year, Month month, int day)
        {
            this.StartDay = this.CurrentDay = day;
            this._startMonth = this._currentMonth = month;
            this._startYear = this._currentYear = year;
            this._hasStarted = false;
            this._isPaused = false;
        }
        #endregion

        #region Manipulation Functions
        /// <summary>
        /// Pause the calendar
        /// </summary>
        public void Pause()
        {
            this._isPaused = true;
        }

        /// <summary>
        /// Start recording the calendar updates again
        /// </summary>
        public void Play()
        {
            this._isPaused = false;
        }

        /// <summary>
        /// Start calendar
        /// </summary>
        public void Start()
        {
            this._hasStarted = true;
        }
        #endregion

        #region Event Functions
        /// <summary>
        /// Overridable function trigger for Day change
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangedDay(EventArgs e)
        {
            if (ChangedDay != null)
                ChangedDay(this, e);
        }

        /// <summary>
        /// Overridable function trigger for Month change
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangedMonth(EventArgs e)
        {
            if (ChangedMonth != null)
                ChangedMonth(this, e);
        }

        /// <summary>
        /// Overridable function trigger for Year change
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangedYear(EventArgs e)
        {
            if (ChangedYear != null)
                ChangedYear(this, e);
        }
        #endregion
    }
}
