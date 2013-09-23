using System;

namespace AR.Drone.Avionics.Objectives.IntentObtainers
{
    /// <summary>
    /// Architectual entity, that represents an idea of an intent being able
    /// to be obtained earlier than originally set by a user.
    /// For exmple, specific altitude usually obtained within 10 seconds, yet
    /// if it is reached earlier, an objective may expire at that moment,
    /// rather than waiting excess time.
    /// </summary>
    public interface EarlyObtainer : Obtainer
    {
        bool CanBeObtained { get; }
        bool Obtained { get; }
    }
}
