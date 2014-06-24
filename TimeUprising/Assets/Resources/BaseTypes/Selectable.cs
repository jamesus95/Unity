using UnityEngine;
using System.Collections;

public interface Selectable 
{
    void SetDestination(Vector3 destination);
    void UseTargetedAbility(Target target);
    
    void Select();
    void Deselect();
}
