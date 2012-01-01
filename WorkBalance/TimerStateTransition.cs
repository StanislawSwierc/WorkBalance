namespace WorkBalance
{
    /// <summary>
    /// Valid transitions of timer state
    /// </summary>
    /// <remarks>
    /// Transition can be calculated as: 
    ///   10 * (int)PreviousState  + (int)NewState
    /// </remarks>
    public enum TimerStateTransition
    {
        ReadyToSprint = 01,
        SprintToReady = 10,
        SprintToHomeStraight = 12,
        HomeStraightToReady = 20,
        HomeStraightToBreak = 23,
        BreakToReady = 30,
        BreakToBreakOverrun = 34,
        BreakOverrunToReady = 40
    }
}
