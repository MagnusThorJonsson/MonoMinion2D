using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoMinion.StateMachine
{
    /// <summary>
    /// Used to build states for the Finite State Machine
    /// </summary>
    /// <typeparam name="T">The type that this state uses</typeparam>
    public interface IState<T>
    {
        /// <summary>
        /// Executed when a state is entered
        /// </summary>
        /// <param name="actor">The owner</param>
        void Enter(T actor);

        /// <summary>
        /// Executed on every update tick
        /// </summary>
        /// <param name="actor">The owner</param>
        /// <param name="gameTime">The current gametime object</param>
        void Execute(T actor, GameTime gameTime);

        /// <summary>
        /// Executed when a state is exited
        /// </summary>
        /// <param name="actor">The owner</param>
        void Exit(T actor);

        IState<T> Copy();
    }
}
