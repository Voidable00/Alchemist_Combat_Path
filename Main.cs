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

[assembly: MelonInfo(typeof(FourthPath.FourthPathMod), FourthPath.ModHelperData.Name, FourthPath.ModHelperData.Version, FourthPath.ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace FourthPath;

public class FourthPathMod : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<FourthPathMod>("FourthPath loaded!");
    }
}
public class FourthPath : PathPlusPlus
{
    public override string Tower => "WizardMonkey";
    public override int UpgradeCount => 5; // Increase this up to 5 as you create your Upgrades
}
public class RangeUpgrade : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 500;
    public override int Tier => 1;
    public override string Icon => VanillaSprites.LongRangeDartsUpgradeIcon;

    public override string Description => "The Tower has more range than normal.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.IncreaseRange(15);

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class LeadPoppingPowerUpgrade : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 1000;
    public override int Tier => 2;
    public override string Icon => VanillaSprites.RedHotRangsUpgradeIcon;
    //public override string Portrait => "700Sylveon_PSMD";

    public override string Description => "The Tower's attacks can pop Lead bloons."; // and Frozen

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.immuneBloonProperties &= ~BloonProperties.Frozen;
            damageModel.immuneBloonProperties &= ~BloonProperties.Lead;
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}
public class PierceAndDamageUpgrade : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 5000; //3000+350+
    public override int Tier => 3;
    public override string Icon => VanillaSprites.CrossBowUpgradeIcon;

    public override string Description => "Increases pierce and damage of the Tower's attacks.";
    //public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 3;
        }
        foreach (var projectileModel in towerModel.GetDescendants<ProjectileModel>().ToArray())
        {
            projectileModel.pierce += 3;
        }


        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}

public class LaserShockUpgrade : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 10000; //3000+350+
    public override int Tier => 4;
    public override string Icon => VanillaSprites.LaserShockUpgradeIcon;

    public override string Description => "Adds a Laser Shock effect to the Tower's attacks";
    //public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        TowerModel dartling = Game.instance.model.GetTowerFromId(TowerType.DartlingGunner + "-300");
        AddBehaviorToBloonModel electricShock = dartling.GetDescendant<AddBehaviorToBloonModel>().Duplicate();
        electricShock.filters = null;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.AddBehavior(electricShock);
            weaponModel.projectile.collisionPasses = new[] { 0, 1 };
        }

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}

public class AttackSpeedUpgrade : UpgradePlusPlus<FourthPath>
{
    public override int Cost => 15000;
    public override int Tier => 5;
    public override string Icon => VanillaSprites.SemiAutomaticUpgradeIcon;

    public override string Description => "Triples the Tower's attack speed.";
    //public override string Portrait => "700Sylveon_PSMD";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var weaponModel in towerModel.GetWeapons().ToArray())
        {
            weaponModel.Rate /= 3;
            weaponModel.rate /= 3;
        }
        

        if (IsHighestUpgrade(towerModel))
        {
            // apply a custom display, if you want
        }
    }
}