using System;
using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity _entity;
    private Entity_Combat _entityCombat;

    protected virtual void Awake()
    {
        _entity = GetComponentInParent<Entity>();
        _entityCombat = GetComponentInParent<Entity_Combat>();
    }

    private void CurrentStateTrigger()
    {
        _entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        _entityCombat.PerformAttack();
    }
}
