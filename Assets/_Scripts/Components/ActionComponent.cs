
using UnityEngine;

public class ActionComponent : CharacterComponent
{
    public void PerformAction()
    {
        if (Owner == null)
            return;

        Debug.Log($"{Owner.name} performed an action!");
    }
}
