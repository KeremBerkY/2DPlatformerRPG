using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider _healthBar;
    private Entity_VFX _entityVFX;
    private Entity _entity;
    private Entity_Stats _stats;
    
    // [SerializeField] protected float maxHp = 100;
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
        _stats = GetComponent<Entity_Stats>();
        _healthBar = GetComponentInChildren<Slider>();

        currentHp = _stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        float duration = CalculateDuration(damage);
        Vector2 knockBack = CalculateKnockBack(damage, damageDealer);
        
        _entityVFX?.PlayOnDamageVfx();
        _entity?.ReceiveKnockBack(knockBack, duration);
        ReduceDamage(damage);
        
        return true;
    }

    private bool AttackEvaded() => Random.Range(0, 100) < _stats.GetEvasion();

    protected void ReduceDamage(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        _entity.EntityDeath(); 
    }

    private void UpdateHealthBar() => _healthBar.value = currentHp / _stats.GetMaxHealth();

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

    private bool IsHeavyDamage(float damage) => damage / _stats.GetMaxHealth() > heavyDamageThreshold;
}
