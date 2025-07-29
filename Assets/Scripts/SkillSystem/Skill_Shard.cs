using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard _currentShard;
    
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")] 
    [SerializeField] private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")] 
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;

    protected override void Awake()
    {
        base.Awake();

        currentCharges = maxCharges;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_Multicast))
            HandleShardMulticast();
    }

    private void HandleShardMulticast()
    {
        if (currentCharges <= 0)
            return;
        
        CreateShard();
        _currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
            StartCoroutine(ShardRechargeCo());
    }

    private IEnumerator ShardRechargeCo()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isRecharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        _currentShard.MoveTowardsClosestTarget(shardSpeed);
        
        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        _currentShard = shard.GetComponent<SkillObject_Shard>();
        _currentShard.SetupShard(detonateTime);
    }
}
