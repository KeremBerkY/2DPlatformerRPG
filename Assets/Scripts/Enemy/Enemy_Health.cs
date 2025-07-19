using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy _enemy => GetComponent<Enemy>();
    
    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damage, damageDealer);

        if (wasHit == false)
            return false;
        
        if (damageDealer.GetComponent<Player>() != null)
            _enemy.TryEnterBattleState(damageDealer);

        return true;
    }
}
