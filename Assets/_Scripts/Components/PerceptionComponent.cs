using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour, IPerception
{
    [Header("Perception Settings")]
    [SerializeReference] private List<Stimulus> stimulis = new();

    // Tracks which stimuli currently detect each actor.
    private Dictionary<GameObject, HashSet<Stimulus>> perceivedActors = new();

    private bool isPerceptionActive;

    // Returns all currently perceived actors.
    public IEnumerable<GameObject> GetPerceivedActors() => perceivedActors.Keys;

    public void BeginPerception()
    {
        isPerceptionActive = true;
    }

    public void EndPerception()
    {
        isPerceptionActive = false;
        perceivedActors.Clear();
    }

    // Register an actor detected by a specific stimulus.
    public void RegisterActor(GameObject actor, Stimulus stimulus)
    {
        if (!isPerceptionActive || actor == null || stimulus == null)
            return;

        if (!perceivedActors.ContainsKey(actor))
            perceivedActors.Add(actor, new HashSet<Stimulus>());

        perceivedActors[actor].Add(stimulus);
    }

    // Remove an actor from a specific stimulus.
    public void RemoveActor(GameObject actor, Stimulus stimulus)
    {
        if (actor == null || stimulus == null)
            return;

        if (!perceivedActors.TryGetValue(actor, out var sources))
            return;

        sources.Remove(stimulus);

        // Only forget the actor when no stimuli detect it.
        if (sources.Count == 0)
            perceivedActors.Remove(actor);
    }

    public void UpdatePerception()
    {
        if (!isPerceptionActive)
            return;

        foreach (Stimulus stimulus in stimulis)
            stimulus?.OnPerception(gameObject);
    }

    public void Perceive(Stimulus stimulus)
    {
        if (!isPerceptionActive || stimulus == null)
            return;

        if (stimulis.Contains(stimulus))
            stimulus.OnPerception(gameObject);
    }
}