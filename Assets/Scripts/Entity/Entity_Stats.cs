using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{
   public Stat_ResourceGroup resources;
   public Stat_MajorGroup major;
   public Stat_OffenseGroup offense;
   public Stat_DefenseGroup defense;

   public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
   {
      float fireDamage = offense.fireDamage.GetValue();
      float iceDamage = offense.iceDamage.GetValue();
      float lightningDamage = offense.lightningDamage.GetValue();

      float bonusElementalDamage = major.intelligence.GetValue(); // Bonus elemental damage from Intelligence +1 per S

      float highestDamage = fireDamage;
      element = ElementType.Fire;

      if (iceDamage > highestDamage)
      {
         highestDamage = iceDamage;
         element = ElementType.Ice;
      }

      if (lightningDamage > highestDamage)
      {
         highestDamage = lightningDamage;
         element = ElementType.Lightning;
      }

      if (highestDamage <= 0)
      {
         element = ElementType.None;
         return 0;
      }

      float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * .5f;
      float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * .5f;
      float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * .5f;

      float weakerElementsDamage = bonusFire + bonusIce + bonusLightning;
      float finalDamage = highestDamage + weakerElementsDamage + bonusElementalDamage;

      return finalDamage * scaleFactor;
   }

   public float GetElementalResistance(ElementType element)
   {
      float baseResistance = 0;
      float bonusResistance = major.intelligence.GetValue() * .5f; // Bonus resistance from intelligence: +0.5% per INT

      switch (element)
      {
         case ElementType.Fire:
            baseResistance = defense.fireRes.GetValue();
            break;
         case ElementType.Ice:
            baseResistance = defense.iceRes.GetValue();
            break;
         case ElementType.Lightning:
            baseResistance = defense.lightningRes.GetValue();
            break;
      }

      float resistance = baseResistance + bonusResistance;
      float resistanceCap = 75f; // Resistance will be capped at 75%
      float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

      return finalResistance;
   }
   
   public float GetPhysicalDamage(out bool isCrit,  float scaleFactor = 1)
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
      
      return finalDamage * scaleFactor;
   }

   public float GetArmorMitigation(float armorReduction)
   {
      float baseArmor = defense.armor.GetValue();
      float bonusArmor = major.vitality.GetValue(); // Bonus armor from Vitality: +1 per VIT
      float totalArmor = baseArmor + bonusArmor;

      float reductionMultiplier = Math.Clamp(1 - armorReduction, 0, 1); ;
      float effectiveArmor = totalArmor * reductionMultiplier;

      float mitigation = effectiveArmor / (effectiveArmor + 100);
      float mitigationCap = .85f; // Max mitigation will be capped at 85%

      float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

      return finalMitigation;
   }

   public float GetArmorReduction()
   {
      // Total armor reduction as multiplier ( e.g 30 / 100 = 0.3f -> multiplier)
      float finalReduction = offense.armorReduction.GetValue() / 100;

      return finalReduction;
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
   
   public float GetMaxHealth()
   {
      float baseMaxHealth = resources.maxHeath.GetValue();
      float bonusMaxHealth = major.vitality.GetValue() * 5;
      float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

      return finalMaxHealth;
   }
}
