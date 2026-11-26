using System;
using UnityEngine;

/// <summary>
/// Base class for all perception stimuli.
/// Serializable so each perceiver owns its own stimulus instances.
/// </summary>
[Serializable]
public abstract class Stimulus : MonoBehaviour
{
    /// <summary>
    /// Executes the perception logic.
    /// </summary>
    public abstract void OnPerception(GameObject perceiver);
}