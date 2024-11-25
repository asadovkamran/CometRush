using System.Collections.Generic;
using UnityEngine;

public class ProcManager
{
    private List<(IAbility ability, float probability)> _abilities = new();

    public void AddAbility(IAbility ability, float probability)
    {
        _abilities.Add((ability, probability));
    }

    public void TryProc(GameObject target)
    {
        foreach (var (ability, probability) in _abilities)
        {
            if (Random.value < probability)
            {
                ability.Execute(target);
            }
        }
    }
}