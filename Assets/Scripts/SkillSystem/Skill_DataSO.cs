using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Description")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
    
    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;
    
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType UpgradeType;
    public float cooldown;
    [FormerlySerializedAs("damageScale")] public DamageScaleData damageScaleData;
}
