using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX _vfx;
    private Entity_Stats _stats;
    
    public float damage = 10;
    
    [Header("Target detection")] 
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

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
            
            float damage = _stats.GetPhysicalDamage(out bool isCrit);
            bool targetGotHit = damageable.TakeDamage(damage, transform);
            
            if (targetGotHit)
                _vfx.CreateOnHitVFX(target.transform, isCrit);
        }
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
