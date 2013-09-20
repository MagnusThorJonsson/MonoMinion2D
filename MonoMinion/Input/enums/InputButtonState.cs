using System;

namespace MonoMinion.Input.enums
{
    /// <summary>
    /// Contains the available gamepad button states
    /// </summary>
    [Flags]
    public enum InputButtonState
    {
        Clicked,
        Held,
        Released,
        None
    }
}
