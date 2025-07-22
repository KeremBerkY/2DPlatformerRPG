using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider _healthBar;
    private Entity _entity;
    private Entity_VFX _entityVFX;
    private Entity_Stats _entityStats;
    
    // [SerializeField] protected float maxHp = 100;
    [FormerlySerializedAs("currentHp")] [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("Health Regen")] 
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;

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
        _entityStats = GetComponent<Entity_Stats>();
        _healthBar = GetComponentInChildren<Slider>();

        currentHealth = _entityStats.GetMaxHealth();
        UpdateHealthBar();
        
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats ? attackerStats.GetArmorReduction() : 0;

        float mitigation = _entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = _entityStats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);
        
        TakeKnockBack(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);
        
        return true;
    }

    private bool AttackEvaded() => Random.Range(0, 100) < _entityStats.GetEvasion();

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;

        float regenAmount = _entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    
    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = _entityStats.resources.maxHeath.GetValue();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        _entityVFX?.PlayOnDamageVfx();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        _entity.EntityDeath(); 
    }

    private void UpdateHealthBar() => _healthBar.value = currentHealth / _entityStats.GetMaxHealth();

    private Vector2 CalculateKnockBack(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        
        Vector2 knockBack = IsHeavyDamage(damage) ? onHeavyDamageKnockBack : onDamageKnockBack;
        
        knockBack.x = knockBack.x * direction;

        return knockBack;
    }
    
    private void TakeKnockBack(Transform damageDealer, float finalDamage)
    {
        Vector2 knockBack = CalculateKnockBack(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);
        
        _entity?.ReceiveKnockBack(knockBack, duration);
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockBackDuration : knockBackDuration;
    private bool IsHeavyDamage(float damage) => damage / _entityStats.GetMaxHealth() > heavyDamageThreshold;
}
