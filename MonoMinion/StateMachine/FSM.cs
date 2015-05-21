using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoMinion.StateMachine
{
    /// <summary>
    /// Finite State Machine
    /// </summary>
    /// <typeparam name="T">The type of class that owns this state machine</typeparam>
    public class FSM<T>
    {
        #region Variables & Properties
        private T _owner;
        private IState<T> _globalState;

        /// <summary>
        /// Get the current state
        /// </summary>
        public IState<T> CurrentState { get { return _currentState; } }
        private IState<T> _currentState;

        private IState<T> _previousState;
        #endregion


        #region Constructor & Update
        /// <summary>
        /// Creates a Finite State Machine
        /// </summary>
        /// <param name="owner">The machines owner</param>
        /// <param name="initialState">The initial state</param>
        /// <param name="globalState">The global state (optional)</param>
        public FSM(T owner, IState<T> initialState, IState<T> globalState = null)
        {
            _owner = owner;

            _globalState = globalState;
            _currentState = initialState;
            _previousState = null;
        }

        /// <summary>
        /// Updates the current states
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (_globalState != null) 
                _globalState.Execute(_owner, gameTime);

            if (_currentState != null) 
                _currentState.Execute(_owner, gameTime);
        }
        #endregion

        #region State Methods
        /// <summary>
        /// Changes the current state
        /// </summary>
        /// <param name="newState">The state to change to</param>
        public void ChangeState(IState<T> newState)
        {
            _previousState = _currentState;
            if (_currentState != null)
                _currentState.Exit(_owner);

            _currentState = newState;
            if (_currentState != null)
                _currentState.Enter(_owner);
        }

        /// <summary>
        /// Revert to the previous state
        /// </summary>
        public void RevertState()
        {
            if (_previousState != null)
                ChangeState(_previousState);
        }
        #endregion
    }
}
