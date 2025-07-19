using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
   private Enemy _enemy;
   private Enemy_VFX _enemyVFX;

   protected override void Awake()
   {
      base.Awake();

      _enemy = GetComponentInParent<Enemy>();
      _enemyVFX = GetComponentInParent<Enemy_VFX>();
   }

   private void EnableCounterWindow()
   {
      _enemyVFX.EnableAttackAlert(true);
      _enemy.EnableCounterWindow(true);
   }

   private void DisableCounterWindow()
   {
      _enemyVFX.EnableAttackAlert(false);
      _enemy.EnableCounterWindow(false);
   }
}
