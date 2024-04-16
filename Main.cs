using MelonLoader;
using BTD_Mod_Helper;
using PathsPlusPlus;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppSystem.IO;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Unity.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using Il2CppAssets.Scripts.Models.Gameplay.Mods;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using System.Linq;
using Il2CppAssets.Scripts.Unity.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using System.Runtime.ExceptionServices;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models;

[assembly: MelonInfo(typeof(FourthPath.AlchemistCombatPath), FourthPath.ModHelperData.Name, FourthPath.ModHelperData.Version, FourthPath.ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace FourthPath;

public class AlchemistCombatPath : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<AlchemistCombatPath>("FourthPath loaded!");
    }
}
public class FourthPath : PathPlusPlus
{
    public override string Tower => "Alchemist";
    public override int UpgradeCount => 5; // Increase this up to 5 as you create your Upgrades
}
public class OutvisibilityPotion : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 850;
    public override int Tier => 1;
    public override string Icon => VanillaSprites.MonkeySenseUpgradeIcon;

    public override string Description => "The Alchemist's improved formula makes his eyesight much better, he can even see camos!";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(10);

        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class ReactiveFormula : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 320;
    public override int Tier => 2;
    public override string Icon => VanillaSprites.BloonDissolverUpgradeIcon;
    //public override string Portrait => "700Sylveon_PSMD";

    public override string Description => "The Alchemist has made a new formula that grows stronger with other upgrades.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.AcidicMixtureDip)) damageModel.damage += 1;
            if (towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions)) damageModel.damage += 1;
        }

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            foreach (var behavior in weaponModel.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                if (towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions)) 
                   behavior.projectile.hasDamageModifiers = true;
                if (towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions)) 
                   behavior.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 15, false, false) { name = "DamageModifierForTagModelAcidSplash" });
            }
        }

        foreach (var overtimeModel in towerModel.GetDescendants<DamageOverTimeModel>().ToArray())
        {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerAcid)) overtimeModel.Interval -= 0.5f;
            if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerAcid)) overtimeModel.displayLifetime += 1.5f;
        }

        foreach (var cashMod in towerModel.GetDescendants<CashIncreaseModel>().ToArray())
            {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.RubberToGold)) cashMod.increase += 2;
            }

        foreach (WeaponModel weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            if (weaponModel.name.Contains("AcidSplash")) { } else
            {
                if (towerModel.appliedUpgrades.Contains(UpgradeType.BerserkerBrew)) weaponModel.Rate *= 0.90f;
                if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerStimulant)) weaponModel.Rate *= 0.95f;
                if (towerModel.appliedUpgrades.Contains(UpgradeType.FasterThrowing)) weaponModel.Rate *= 0.80f;                
            }
        }       

        if (towerModel.appliedUpgrades.Contains(UpgradeType.BerserkerBrew)) towerModel.IncreaseRange(2);
        if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerStimulant)) towerModel.IncreaseRange(3);
        if (towerModel.appliedUpgrades.Contains(UpgradeType.PermanentBrew)) towerModel.IncreaseRange(5);

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class LookMaTwoHands : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 3140; //3000+350+
    public override int Tier => 3;
    public override string Icon => VanillaSprites.DoubleShotUpgradeIcon;

    public override string Description => "The Alchemist realizes he has two hands and throws double the potions.";
    //public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var potion = towerModel.GetWeapons().First(w => w.name == "WeaponModel_Weapon").Duplicate();
        potion.name = "SecondPotion";
        potion.ejectX = 9.5f; potion.ejectY = 1.5f;
        potion.customStartCooldown = 0.5f; potion.animationOffset = 0.5f;
        towerModel.GetAttackModel().AddWeapon(weaponToAdd:potion);

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}

public class LaserEyes : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 12000; //3000+350+
    public override int Tier => 4;
    public override string Icon => VanillaSprites.LaserBlastUpgradeIcon;

    public override string Description => "The Alchemist drank something strange, and now can fire lasers from his eyes.";
    //public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        TowerModel alchemist = Game.instance.model.GetTowerFromId(TowerType.Alchemist = "TransformedMonkey");
        var laser = alchemist.GetAttackModels().First(m => m.name.Contains("TransformedAttack")).Duplicate();
        laser.name = "Laser";
        laser.GetDescendant<WeaponModel>().Rate = 0.8f;
        laser.fireWithoutTarget = false;
        laser.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        laser.range = towerModel.range;
        laser.GetDescendant<WeaponModel>().animateOnMainAttack = false;

        towerModel.AddBehavior(laser);
        
        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}

public class RbeEqualsPopsSquared : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 25000;
    public override int Tier => 5;
    public override string Icon => VanillaSprites.TheBloonSolverUpgradeIcon;

    public override string Description => "Highly reactive formula spells the end of bloons.";
    //public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            foreach (var behavior in weaponModel.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                if (towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions)) 
                   behavior.projectile.hasDamageModifiers = true;
                if (towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions)) 
                   behavior.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 49, false, false) { name = "DamageModifierForTagModelAcidSplash" });
            }
        }
        
        foreach (var weaponModel in towerModel.GetWeapons().ToArray())
        {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.AlchemistFasterThrowing)) weaponModel.Rate *= 0.40f;
            if (towerModel.appliedUpgrades.Contains(UpgradeType.LargerPotions)) weaponModel.projectile.scale *= 1.30f;
        }

        towerModel.GetAttackModels().First(m => m.name.Contains("Laser")).GetDescendant<WeaponModel>().rate *= 0.5f;
        
        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
