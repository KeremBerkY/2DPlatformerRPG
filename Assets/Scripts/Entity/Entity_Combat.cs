using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX _vfx;
    private Entity_Stats _stats;
    
    [Header("Target detection")] 
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status effect details")] 
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillSlowMultiplier = .2f;

    private void Awake()
    {
        _vfx = GetComponent<Entity_VFX>();
        _stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            
            if (damageable == null)
                continue;

            float elementalDamage = _stats.GetElementalDamage(out ElementType element);
            float damage = _stats.GetPhysicalDamage(out bool isCrit);
            
            bool targetGotHit = damageable.TakeDamage(damage, elementalDamage, element, transform);
            
            if (element != ElementType.None)
                ApplyStatusEffect(target.transform, element);

            if (targetGotHit)
            {
                _vfx.UpdateOnHitColor(element);
                _vfx.CreateOnHitVFX(target.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if(statusHandler == null)
            return;
        
        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
            statusHandler.ApplyChilledEffect(defaultDuration, chillSlowMultiplier);
    }
    
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
