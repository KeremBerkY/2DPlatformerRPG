using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{
   [FormerlySerializedAs("setup")] public Stat_SetupSO defaultStatSetup;
   
   public Stat_ResourceGroup resources;
   public Stat_OffenseGroup offense;
   public Stat_DefenseGroup defense;
   public Stat_MajorGroup major;

   public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
   {
      float fireDamage = offense.fireDamage.GetValue();
      float iceDamage = offense.iceDamage.GetValue();
      float lightningDamage = offense.lightningDamage.GetValue();
      float bonusElementalDamage = major.intelligence.GetValue();
     
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

      float bonusFire = (element == ElementType.Fire) ? 0 : fireDamage * .5f;
      float bonusIce = (element == ElementType.Ice) ? 0 : iceDamage * .5f;
      float bonusLightning = (element == ElementType.Lightning) ? 0 : lightningDamage * .5f;

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

   public Stat GetStatByType(StatType type)
   {
      switch (type)
      {
         // Resources
         case StatType.MaxHealth:
            return resources.maxHeath;
         case StatType.HealthRegen:
            return resources.healthRegen;
         
         // Major Stats
         case StatType.Strength:
            return major.strength;
         case StatType.Agility: 
            return major.agility;
         case StatType.Intelligence:
            return major.intelligence;
         case StatType.Vitality:
            return major.vitality;
         
         // Offense
         case StatType.AttackSpeed:
            return offense.attackSpeed;
         case StatType.Damage:
            return offense.damage;
         case StatType.CritChance:
            return offense.critChance;
         case StatType.CritPower:
            return offense.critPower;
         case StatType.ArmorReduction:
            return offense.armorReduction;
         
            // Offense Elements
         case StatType.FireDamage:
            return offense.fireDamage;
         case StatType.IceDamage:
            return offense.iceDamage;
         case StatType.LightningDamage:
            return offense.lightningDamage;
         
            // Defense
         case StatType.Armor:
            return defense.armor;
         case StatType.Evasion:
            return defense.evasion;
         
            // Defense Elements
         case StatType.IceResistance:
            return defense.iceRes;
         case StatType.FireResistance:
            return defense.fireRes;
         case StatType.LightningResistance:
            return defense.lightningRes;
         
         default:
            Debug.Log($"StatType {type} not implemented yet.");
            return null;
      }
   }

   [ContextMenu("Update Default Stat Setup")]
   public void ApplyDefaultStatSetup()
   {
      if (defaultStatSetup == null)
      {
         Debug.Log("No default stat setup assigned");
         return;
      }
      
      // Resources
      resources.maxHeath.SetBaseValue(defaultStatSetup.maxHealth);
      resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);
      
      // Major Stats
      major.strength.SetBaseValue(defaultStatSetup.strength);
      major.agility.SetBaseValue(defaultStatSetup.agility);
      major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
      major.vitality.SetBaseValue(defaultStatSetup.vitality);
      
      // Offense
      offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
      offense.damage.SetBaseValue(defaultStatSetup.damage);
      offense.critChance.SetBaseValue(defaultStatSetup.critChance);
      offense.critPower.SetBaseValue(defaultStatSetup.critPower);
      offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
      
      // Offense Elemental
      offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
      offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
      offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);
      
      // Defense
      defense.armor.SetBaseValue(defaultStatSetup.armor);
      defense.evasion.SetBaseValue(defaultStatSetup.evasion);
      
      // Defense Elemental
      defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
      defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
      defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
   }
}
