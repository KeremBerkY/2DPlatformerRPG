using UnityEngine;
using UnityEngine.Serialization;

public class Entity_Stats : MonoBehaviour
{
   public Stat maxHealth;
   public Stat_MajorGroup major;
   public Stat_OffenseGroup offense;
   public Stat_DefenseGroup defense;

   public float GetPhysicalDamage(out bool isCrit)
   {
      float baseDamage = offense.damage.GetValue();
      float bonusDamage = major.strength.GetValue();
      float totalBaseDamage = baseDamage + bonusDamage;

      float baseCritChance = offense.critChance.GetValue();
      float bonusCritChance = major.agility.GetValue() * 0.3f; // Bonus crit chance from Agility: +0.3% per AGI
      float critChance = baseCritChance + bonusCritChance;

      
      float baseCritPower = offense.critPower.GetValue();
      float bonusCritPower = major.strength.GetValue() * .5f; // Bonus crit chance from Strength: +0.5% per STR
      float critPower = (baseCritPower + bonusCritPower) / 100; // Total crit power as multiplier ( e.g 150/100 = 1.5f -> multiplier)

      isCrit = Random.Range(0, 100) < critChance;
      float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;
      
      return finalDamage;
   }
   
   public float GetMaxHealth()
   {
      float baseMaxHealth = maxHealth.GetValue();
      float bonusMaxHealth = major.vitality.GetValue() * 5;
      float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

      return finalMaxHealth;
   }

   public float GetEvasion()
   {
      float baseEvasion = defense.evasion.GetValue();
      float bonusEvasion = major.agility.GetValue() * .5f;

      float totalEvasion = baseEvasion + bonusEvasion;
      float evasionCap = 80f; // Max evasion will be capped 80%

      float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

      return finalEvasion;
   }
}
