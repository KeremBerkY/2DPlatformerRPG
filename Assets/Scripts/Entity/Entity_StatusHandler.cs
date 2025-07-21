using System;
using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity _entity;
    private Entity_VFX _entityVFX;
    private ElementType currentEffect = ElementType.None;
    
    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<Entity_VFX>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        _entity.SlowDownEntity(duration, slowMultiplier);
        StartCoroutine(ChilledEffectCo(duration));
    }

    private IEnumerator ChilledEffectCo(float duration)
    {
        currentEffect = ElementType.Ice;
        _entityVFX.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }
    
    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
