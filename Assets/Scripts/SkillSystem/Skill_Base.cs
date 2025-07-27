using System;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")] 
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public void SetSkillUpgrade(SkillUpgradeType upgrade)
    {
        upgradeType = upgrade;
    }

    public bool CanUseSkill()
    {
        if (OnCooldown())
        { 
            Debug.Log("On Cooldown");
            return false;
        }
        
        // TODO: mana check, unlock check...

        return true;
    }

    private bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;
}
