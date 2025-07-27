using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;
    public SkillType skillType;
    public SkillUpgradeType upgradeType;
    
    [Header("Skill Description")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
    
    // TODO: Skill type that you should unlock
}
