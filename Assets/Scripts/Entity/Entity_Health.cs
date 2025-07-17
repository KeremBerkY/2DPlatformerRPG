using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity_VFX _entityVFX;
    private Entity _entity;
    
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("On Damage KnockBack")] 
    [SerializeField] private float knockBackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockBack = new Vector2(1.5f, 2.5f);
    [Space]
    [Range(0,1)]
    [SerializeField] private float heavyDamageThreshold = .3f; // percentage of maxHp you should loose to get heavy knockback
    [SerializeField] private float heavyKnockBackDuration = .5f;
    [SerializeField] private Vector2 onHeavyDamageKnockBack = new Vector2(7, 7);
    
    protected void Awake()
    {
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<Entity_VFX>();
    }

    private void Start()
    {
        currentHp = maxHp;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        float duration = CalculateDuration(damage);
        Vector2 knockBack = CalculateKnockBack(damage, damageDealer);
        
        _entityVFX?.PlayOnDamageVfx();
        _entity?.ReceiveKnockBack(knockBack, duration);
        ReduceDamage(damage);
    }

    protected void ReduceDamage(float damage)
    {
        // maxHp -= damage;
        currentHp -= damage;

        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        _entity.EntityDeath(); 
    }

    private Vector2 CalculateKnockBack(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        
        Vector2 knockBack = IsHeavyDamage(damage) ? onHeavyDamageKnockBack : onDamageKnockBack;
        
        knockBack.x = knockBack.x * direction;

        return knockBack;
    }

    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockBackDuration : knockBackDuration;
    }

    private bool IsHeavyDamage(float damage) => damage / maxHp > heavyDamageThreshold;
}
