namespace Slapground.Core
{
    /// <summary>
    /// How an object's mass is free to move under impact, independent of Mass/
    /// Fragility/Elasticity. Decides which Unity joint setup a Level 1 object
    /// factory should build (not this library's job - this enum is the data,
    /// the Unity-side factory that turns it into Rigidbody/HingeJoint/joint-chain
    /// is unbuilt and belongs in the Unity project, not here).
    /// </summary>
    public enum Articulation
    {
        /// <summary>Single rigid body, no constraints - the alarm clock.</summary>
        Rigid,

        /// <summary>Constrained to rotate about one axis - a printer lid/tray.</summary>
        Hinged,

        /// <summary>A loose end whips faster than its body for the same input energy - the corded phone.</summary>
        Flail,
    }
}
