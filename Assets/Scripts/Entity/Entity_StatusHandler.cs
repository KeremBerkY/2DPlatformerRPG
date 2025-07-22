using System;
using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity _entity;
    private Entity_VFX _entityVfx;
    private Entity_Stats _entityStats;
    private Entity_Health _entityHealth;
    private ElementType currentEffect = ElementType.None;

    [Header("Electrify effect details")] 
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;
    
    private void Awake()
    {
        _entityStats = GetComponent<Entity_Stats>();
        _entityHealth = GetComponent<Entity_Health>();
        _entity = GetComponent<Entity>();
        _entityVfx = GetComponent<Entity_VFX>();
    }

    public void ApplyElectrifyEffect(float duration, float damage, float charge)
    {
        float lightningResistance = _entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge += finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
            return;
        }
        
        if (electrifyCo != null)
            StopCoroutine(electrifyCo);
            
        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        _entityVfx.StopAllVfx();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);
        _entityHealth.ReduceHealth(damage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        _entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);
        
        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = _entityStats.GetElementalResistance(ElementType.Fire);
        // float finalDuration = duration * (1 - fireResistance); // if you want you can reduce burn time.
        float finalDamage = fireDamage * (1 - fireResistance);
        
        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        _entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            _entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = _entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);
        
        StartCoroutine(ChillEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        _entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        _entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }
    
    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;
        
        return currentEffect == ElementType.None;
    }
}
