/// <summary>
/// Interface defining the lifecycle of a perception system.
/// Implemented by components responsible for managing perception.
/// </summary>
public interface IPerception
{
    /// <summary>
    /// Activates the perception system.
    /// </summary>
    void BeginPerception();

    /// <summary>
    /// Updates perception while the system is active.
    /// Usually called once per frame or at regular intervals.
    /// </summary>
    void UpdatePerception();

    /// <summary>
    /// Deactivates the perception system.
    /// </summary>
    void EndPerception();
}