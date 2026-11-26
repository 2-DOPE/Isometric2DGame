using Unity.VisualScripting;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// Executes first On Entry.
    /// </summary>
    public void OnEnterState()
    {
        ;
    }

    /// <summary>
    /// Runs every frame same as traditional update
    /// </summary>
    public void UpdateState() {  ; }

    /// <summary>
    /// Executes last before leaving state.
    /// </summary>
    public void OnExitState()
    {
        ;
    }
}