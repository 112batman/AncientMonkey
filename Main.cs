using System;
using System.Linq;
using AncientMonkey;
using AncientMonkey.Challenge;
using AncientMonkey.Weapons;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.AbilitiesMenu;
using Il2CppNinjaKiwi.Common;
using Il2CppSystem.IO;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using Random = Il2CppSystem.Random;

[assembly:
    MelonInfo(typeof(AncientMonkey.AncientMonkey), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AncientMonkey;

public class AncientMonkey : BloonsTD6Mod
{
    internal static readonly ModSettingBool SandboxMode = new(false)
    {
        requiresRestart = true,
        icon = VanillaSprites.SandboxBtn,
        description = "Enable Sandbox Mode"
    };


    public static AncientMonkey mod;
    public int abilitySlot = 1;

    public ChallengeTemplate? activeChallenge = new None();
    public float attackSpeedBoostSandbox = 1;
    public float baseNewAbilityCost = 1750;
    public float baseNewWeaponCost = 250;
    public float baseStrongerWeaponCost = 1100;
    public int damageBoostSandbox;
    public float epicChance = 100;
    public float epicStrongChance = 100;
    public float exoticChance = 100;
    public float exoticStrongChance = 100;
    public int ExtraAbilitySlotCost = 160000;
    public int ExtraAbilitySlotLevel;
    public int ExtraAbilitySlotLevelMax = 1;
    public float ExtraLuckCost = 250;
    public int ExtraLuckLevel;
    public int ExtraLuckMax = 20;
    public int extraWeaponSlotCost = 80000;
    public int extraWeaponSlotLevel;
    public int extraWeaponSlotLevelMax = 1;
    public float godlyChance = 100;
    public float godlyStrongerChance = 100;
    public float legendaryChance = 100;
    public float legendaryStrongChance = 100;
    public float level;
    public WeaponTemplate.Rarity maxNewWeaponRarity = WeaponTemplate.Rarity.Exotic;
    public WeaponTemplate.Rarity maxStrongWeaponRarity = WeaponTemplate.Rarity.Exotic;
    public bool mib;
    public WeaponTemplate.Rarity minNewWeaponRarity = WeaponTemplate.Rarity.Common;
    public WeaponTemplate.Rarity minStrongWeaponRarity = WeaponTemplate.Rarity.Common;
    public float moneyBoostSandbox = 1;
    public float newAbilityCost = 6500;
    public float newWeaponCost = 250;
    public int newWeaponSlot = 3;
    public float omegaChance = 100;
    public float omegaStrongerChance = 100;
    public bool panelOpen;
    public int pierceBoostSandbox;
    public float rangeBoostSandbox = 1;
    public float rareChance = 85;
    public float rareStrongChance = 85;
    public bool selectingWeaponOpen;
    public float strongerWeaponCost = 1100;
    public int strongExtraWeaponSlotCost = 100000;
    public int strongExtraWeaponSlotLevel;
    public int strongExtraWeaponSlotLevelMax = 1;
    public int strongWeaponSlot = 3;
    public float Upgrade2Cost = 1000000;
    public float UpgradeCost = 125000;
    public bool upgradeOpen;
    public int XP;
    public int XPMax;

    public ChallengeTemplate selectedChallenge
    {
        get => activeChallenge;
        set => activeChallenge = value;
    }

    public override void OnApplicationStart()
    {
        mod = this;

        foreach (var weapon in ModContent.GetContent<WeaponTemplate>().FindAll(template =>
                 {
                     if (!Settings.moddedTowers)
                     {
                         if (template.WeaponName == new SeekingDart().WeaponName) return false;
                         if (template.WeaponName == new Incendiary().WeaponName) return false;
                         if (template.WeaponName == new Ripper().WeaponName) return false;
                         if (template.WeaponName == new Epic.Rifleman().WeaponName) return false;
                         if (template.WeaponName == new Epic.PlasmaBomb().WeaponName) return false;
                         if (template.WeaponName == new Epic.FireCracker().WeaponName) return false;
                         if (template.WeaponName == new Epic.IceMagic().WeaponName) return false;
                         if (template.WeaponName == new Epic.AciGunner().WeaponName) return false;
                         if (template.WeaponName == new Epic.FlakGun().WeaponName) return false;
                         if (template.WeaponName == new Epic.AssaultSub().WeaponName) return false;
                         if (template.WeaponName == new Epic.Dreadnought().WeaponName) return false;
                         if (template.WeaponName == new Epic.Dogfighter().WeaponName) return false;
                         if (template.WeaponName == new Epic.Railgun().WeaponName) return false;
                         if (template.WeaponName == new Epic.CarpetBomb().WeaponName) return false;
                         if (template.WeaponName == new Epic.Volley().WeaponName) return false;
                         if (template.WeaponName == new Epic.SpectralClaws().WeaponName) return false;
                         if (template.WeaponName == new Epic.PotionLauncher().WeaponName) return false;
                         if (template.WeaponName == new Epic.DeepDruid().WeaponName) return false;
                         if (template.WeaponName == new Epic.HealthierBananas().WeaponName) return false;
                         if (template.WeaponName == new Epic.ShieldGenerator().WeaponName) return false;
                         if (template.WeaponName == new Epic.PrototypeSentries().WeaponName) return false;
                         if (template.WeaponName == new SeekingDart().WeaponName) return false;
                         if (template.WeaponName == new Exotic.ExplosionKing().WeaponName) return false;
                         if (template.WeaponName == new Exotic.PolarVortex().WeaponName) return false;
                         if (template.WeaponName == new Exotic.KingCobra().WeaponName) return false;
                         if (template.WeaponName == new Exotic.FlyingDutchman().WeaponName) return false;
                         if (template.WeaponName == new Exotic.Incinerator().WeaponName) return false;
                         if (template.WeaponName == new Exotic.MasterWaves().WeaponName) return false;
                         if (template.WeaponName == new Exotic.Lockdown().WeaponName) return false;
                         if (template.WeaponName == new Exotic.PrimalLegend().WeaponName) return false;
                         if (template.WeaponName == new ArcaneGuardian().WeaponName) return false;
                         if (template.WeaponName == new Legendary.PlasmaTank().WeaponName) return false;
                         if (template.WeaponName == new Legendary.BladeGod().WeaponName) return false;
                         if (template.WeaponName == new Legendary.SuperNova().WeaponName) return false;
                         if (template.WeaponName == new Legendary.Fireworks().WeaponName) return false;
                         if (template.WeaponName == new Legendary.Bloonzooka().WeaponName) return false;
                         if (template.WeaponName == new Legendary.NauticDestroyer().WeaponName) return false;
                         if (template.WeaponName == new Legendary.SidewinderAce().WeaponName) return false;
                         if (template.WeaponName == new Legendary.RollingThunder().WeaponName) return false;
                         if (template.WeaponName == new Legendary.SorcererSupreme().WeaponName) return false;
                         if (template.WeaponName == new Legendary.DemonBlade().WeaponName) return false;
                         if (template.WeaponName == new Legendary.SuperAcid().WeaponName) return false;
                         if (template.WeaponName == new Legendary.OmegaBeacon().WeaponName) return false;
                         if (template.WeaponName == new Legendary.Overcharge().WeaponName) return false;
                         if (template.WeaponName == new Chakrams().WeaponName) return false;
                         if (template.WeaponName == new Frostbite().WeaponName) return false;
                         if (template.WeaponName == new CrackShotDarts().WeaponName) return false;
                         if (template.WeaponName == new HighGradeDarts().WeaponName) return false;
                         if (template.WeaponName == new SaiToss().WeaponName) return false;
                         if (template.WeaponName == new SeaHeart().WeaponName) return false;
                         if (template.WeaponName == new BananaStock().WeaponName) return false;
                         if (template.WeaponName == new BurstFire().WeaponName) return false;
                     }

                     return true;
                 }).OrderByDescending(c => c.mod == mod))
        {
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Common)
            {
                Common.CommonWpn.Add(weapon.WeaponName);
                if (weapon.CustomIcon)
                    Common.CommonCustomImg.Add(weapon.CustomIcon);
                else
                    Common.CommonCustomImg.Add(new Sprite());
                Common.CommonImg.Add(weapon.Icon);
            }

            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Rare)
            {
                Rare.RareWpn.Add(weapon.WeaponName);
                Rare.RareImg.Add(weapon.Icon);
                if (weapon.CustomIcon)
                    Rare.RareCustomImg.Add(weapon.CustomIcon);
                else
                    Rare.RareCustomImg.Add(new Sprite());
            }

            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Epic)
            {
                Epic.EpicWpn.Add(weapon.WeaponName);
                Epic.EpicImg.Add(weapon.Icon);
                if (weapon.CustomIcon)
                    Epic.EpicCustomImg.Add(weapon.CustomIcon);
                else
                    Epic.EpicCustomImg.Add(new Sprite());
            }

            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Legendary)
            {
                Legendary.LegendaryWpn.Add(weapon.WeaponName);
                Legendary.LegendaryImg.Add(weapon.Icon);
                if (weapon.CustomIcon)
                    Legendary.LegendaryCustomImg.Add(weapon.CustomIcon);
                else
                    Legendary.LegendaryCustomImg.Add(new Sprite());
            }

            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Exotic)
            {
                Exotic.ExoticWpn.Add(weapon.WeaponName);
                Exotic.ExoticImg.Add(weapon.Icon);
                if (weapon.CustomIcon)
                    Exotic.ExoticCustomImg.Add(weapon.CustomIcon);
                else
                    Exotic.ExoticCustomImg.Add(new Sprite());
            }

            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Godly)
            {
                Godly.GodlyWpn.Add(weapon.WeaponName);
                Godly.GodlyImg.Add(weapon.Icon);
                if (weapon.CustomIcon)
                    Godly.GodlyCustomImg.Add(weapon.CustomIcon);
                else
                    Godly.GodlyCustomImg.Add(new Sprite());
            }

            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Omega)
            {
                Omega.OmegaWpn.Add(weapon.WeaponName);
                Omega.OmegaImg.Add(weapon.Icon);
                if (weapon.CustomIcon)
                    Omega.OmegaCustomImg.Add(weapon.CustomIcon);
                else
                    Omega.OmegaCustomImg.Add(new Sprite());
            }
        }

        foreach (var ability in ModContent.GetContent<AbilityTemplate>().FindAll(template =>
                 {
                     if (!Settings.moddedAbilities)
                         if (template.AbilityName == new BananaStockExchange().AbilityName)
                             return false;
                     return true;
                 }).OrderByDescending(c => c.mod == mod))
        {
            AbilityClass.AbilityName.Add(ability.AbilityName);
            AbilityClass.AbilityImg.Add(ability.Icon);
            if (ability.CustomIcon)
                AbilityClass.AbilityCustomImg.Add(ability.CustomIcon);
            else
                AbilityClass.AbilityCustomImg.Add(new Sprite());
        }
    }

    public void Reset()
    {
        newWeaponCost = 325 * mod.selectedChallenge.NewWeaponCostMult;
        rareChance = 90;
        epicChance = 100;
        rareStrongChance = 90;
        epicStrongChance = 100;
        legendaryStrongChance = 100;
        legendaryChance = 100;
        exoticChance = 100;
        exoticStrongChance = 100;
        omegaChance = 100;
        omegaStrongerChance = 100;
        baseNewWeaponCost = 250 * mod.selectedChallenge.NewWeaponCostMult;
        strongerWeaponCost = 500 * mod.selectedChallenge.StrongerWeaponCostMult;
        baseStrongerWeaponCost = 500 * mod.selectedChallenge.StrongerWeaponCostMult;
        newAbilityCost = 7500 * mod.selectedChallenge.AbilityWeaponCostMult;
        baseNewAbilityCost = 1000 * mod.selectedChallenge.AbilityWeaponCostMult;
        upgradeOpen = false;
        selectingWeaponOpen = false;
        mib = false;
        panelOpen = false;
        level = 0;
        newWeaponSlot = 3;
        strongWeaponSlot = 3;
        abilitySlot = 1;
        extraWeaponSlotLevel = 0;
        extraWeaponSlotCost = 50000;
        strongExtraWeaponSlotLevel = 0;
        strongExtraWeaponSlotCost = 65000;
        ExtraAbilitySlotLevel = 0;
        ExtraAbilitySlotCost = 100000;
        ExtraLuckCost = 700 * mod.selectedChallenge.LuckCostMult;
        ExtraLuckLevel = 0;
        minNewWeaponRarity = mod.selectedChallenge.MinNURarity;
        maxNewWeaponRarity = mod.selectedChallenge.MaxNURarity;
        minStrongWeaponRarity = mod.selectedChallenge.MinNURarity;
        maxStrongWeaponRarity = mod.selectedChallenge.MaxNURarity;
        UpgradeCost = 125000 * mod.selectedChallenge.UpgradeCostMult;
        Upgrade2Cost = 1000000 * mod.selectedChallenge.UpgradeCostMult;
        XP = 0;
        XPMax = 0;
    }

    public override void OnGameModelLoaded(GameModel model)
    {
        Reset();
    }

    public override void OnTowerSold(Tower tower, float amount)
    {
        if (tower.towerModel.name.Contains("AncientMonkey-AncientMonkey")) Reset();
    }

    public override void OnRestart()
    {
        MenuUi.instance.CloseMenu();
    }

    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.towerModel.name.Contains("AncientMonkey-AncientMonkey"))
        {
            var game = InGame.instance;
            var rect = game.uiRect;
            minNewWeaponRarity = WeaponTemplate.Rarity.Common;
            maxNewWeaponRarity = WeaponTemplate.Rarity.Common;
            newWeaponSlot = 5;
            MenuUi.NewWeaponPanel(rect, tower, false);
            Reset();
        }
    }

    public override void OnNewGameModel(GameModel result)
    {
        foreach (var tower in result.towerSet.ToList())
            if (tower.name.Contains("AncientMonkey-AncientMonkey"))
                tower.GetShopTowerDetails().towerCount = 1;
    }

    public override void OnTowerSelected(Tower tower)
    {
        if (tower.towerModel.name.Contains("AncientMonkey-AncientMonkey"))
        {
            var game = InGame.instance;
            var rect = game.uiRect;
            MenuUi.CreateUpgradeMenu(rect, tower);
            upgradeOpen = true;
        }
    }

    public override void OnTowerDeselected(Tower tower)
    {
        if (tower.towerModel.name.Contains("AncientMonkey-AncientMonkey"))
        {
            var game = InGame.instance;
            var rect = game.uiRect;
            upgradeOpen = false;
            if (MenuUi.instance) MenuUi.instance.CloseMenu();
        }
    }

    [RegisterTypeInIl2Cpp(false)]
    public class MenuUi : MonoBehaviour
    {
        public static MenuUi instance;

        public ModHelperInputField input;

        public string[] Monkeys =
        {
            "DartMonkey",
            "BoomerangMonkey",
            "BombShooter",
            "TackShooter",
            "IceMonkey",
            "GlueGunner",
            "SniperMonkey",
            "MonkeySub",
            "MonkeyBuccaneer",
            "MonkeyAce",
            "HeliPilot",
            "MortarMonkey",
            "DartlingGunner",
            "WizardMonkey",
            "SuperMonkey",
            "NinjaMonkey",
            "Alchemist",
            "Druid",
            "BananaFarm",
            "MonkeyVillage",
            "SpikeFactory",
            "EngineerMonkey"
        };

        public string Monkey = "DartMonkey";
        public int upgradeTier;
        public int upgradePath;

        public void CloseMenu()
        {
            if (gameObject) Destroy(gameObject);
        }

        public void NewWeapon(Tower tower)
        {
            var game = InGame.instance;
            if (SandboxMode)
            {
                var rect = game.uiRect;
                NewWeaponPanel(rect, tower, false);
                instance.CloseMenu();
                return;
            }

            if (game.GetCash() >= mod.newWeaponCost)
            {
                game.AddCash(-mod.newWeaponCost);
                var rect = game.uiRect;
                mod.newWeaponCost += mod.baseNewWeaponCost;
                tower.worth += mod.newWeaponCost - mod.baseNewWeaponCost;
                mod.baseNewWeaponCost *= 1.06f;
                NewWeaponPanel(rect, tower, false);
                instance.CloseMenu();
            }
        }

        public void WeaponSelected(string Weapon, Tower tower, bool levelup)
        {
            mod.panelOpen = false;
            var game = InGame.instance;
            var rect = game.uiRect;
            if (!SandboxMode) Destroy(gameObject);

            foreach (var weapon in ModContent.GetContent<WeaponTemplate>().OrderByDescending(c => c.mod == mod))
                if (weapon.WeaponName == Weapon)
                {
                    weapon.stackIndex += 1;
                    weapon.EditTower(tower);
                }

            mod.XP += 1;
            if (levelup) mod.XP = 0;
            if (mod.XP >= mod.XPMax && mod.level >= 2)
            {
                mod.XPMax += 2;
                NewWeaponPanel(rect, tower, true);

                return;
            }

            mod.rareChance -= 5f + mod.ExtraLuckLevel * 0.45f;
            mod.epicChance -= 1.45f + mod.ExtraLuckLevel * 0.1f;
            if (mod.epicChance <= 88) mod.legendaryChance -= 0.85f + mod.ExtraLuckLevel * 0.06f;
            if (mod.legendaryChance <= 91) mod.exoticChance -= 0.50f + mod.ExtraLuckLevel * 0.04f;
            if (mod.level >= 1)
                if (mod.exoticChance <= 94)
                    mod.godlyChance -= 0.3f + mod.ExtraLuckLevel * 0.025f;
            if (mod.level >= 2)
                if (mod.godlyChance <= 95)
                    mod.omegaChance -= 0.15f + mod.ExtraLuckLevel * 0.01f;

            if (mod.upgradeOpen && !SandboxMode) CreateUpgradeMenu(rect, tower);

            mod.selectingWeaponOpen = false;
            if (mod.mib)
            {
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                {
                    if (weaponModel.projectile.HasBehavior<DamageModel>())
                        weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    if (weaponModel.projectile.HasBehavior<CreateProjectileOnContactModel>())
                        weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel()
                            .immuneBloonProperties = BloonProperties.None;
                    if (weaponModel.projectile.HasBehavior<CreateProjectileOnExhaustFractionModel>())
                        weaponModel.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile
                            .GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                tower.UpdateRootModel(towerModel);
            }
        }

        public static ModHelperPanel CreateWeapon(WeaponTemplate weapon, Tower tower)
        {
            var sprite = VanillaSprites.GreyInsertPanel;
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Rare) sprite = VanillaSprites.BlueInsertPanel;
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Epic) sprite = VanillaSprites.MainBgPanelParagon;
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Legendary) sprite = VanillaSprites.MainBGPanelYellow;
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Exotic) sprite = VanillaSprites.MainBgPanelWhiteSmall;
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Godly) sprite = VanillaSprites.MainBGPanelSilver;
            if (weapon.WeaponRarity == WeaponTemplate.Rarity.Omega) sprite = VanillaSprites.MainBgPanelHematite;
            var panel = ModHelperPanel.Create(new Info("WeaponContent" + weapon.WeaponName, 0, 0, 2250, 150), sprite);
            var upgradeUi = panel.AddComponent<MenuUi>();
            var wpnName = panel.AddText(new Info("wpnName", -600, 0, 1000, 150), weapon.WeaponName, 80,
                TextAlignmentOptions.MidlineLeft);
            var rarity = panel.AddText(new Info("rarity", 275, 0, 600, 150), weapon.WeaponRarity.ToString(), 80,
                TextAlignmentOptions.MidlineLeft);
            var image = panel.AddImage(new Info("image", -100, 0, 140, 140), weapon.Icon);
            var selectWpnBtn = panel.AddButton(new Info("selectWpnBtn", 900, 0, 400, 120), VanillaSprites.GreenBtnLong,
                new Action(() => { upgradeUi.WeaponSelected(weapon.WeaponName, tower, false); }));
            if (weapon.IsCamo)
            {
                var camoImg = panel.AddImage(new Info("camoImg", 460, 0, 120, 120), VanillaSprites.CamoBloonIcon);
            }

            if (weapon.IsLead)
            {
                var leadImg = panel.AddImage(new Info("leadImg", 580, 0, 120, 120), VanillaSprites.LeadBloonIcon);
            }

            var selectWpn = selectWpnBtn.AddText(new Info("selectWpn", 0, 0, 700, 160), "Select", 60);
            return panel;
        }

        public static void SandBoxWeaponPanel(RectTransform rect, Tower tower)
        {
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new Vector2()),
                VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();
            var scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 0, 0, 2500, 1850),
                RectTransform.Axis.Vertical, VanillaSprites.MainBGPanelBlue, 15, 50);
            var exit = panel.AddButton(new Info("exit", 1200, 900, 135, 135), VanillaSprites.RedBtn, new Action(() =>
            {
                tower.SetSelectionBlocked(false);
                panel.DeleteObject();
                if (mod.upgradeOpen) CreateUpgradeMenu(rect, tower);
            }));
            var x = exit.AddText(new Info("x", 0, 0, 700, 160), "X", 80);

            for (var i = 1; i < 100; i++)
                foreach (var weapon in ModContent.GetContent<WeaponTemplate>())
                    if (weapon.SandboxIndex == i)
                        scrollPanel.AddScrollContent(CreateWeapon(weapon, tower));
        }

        public static void NewWeaponPanel(RectTransform rect, Tower tower, bool Levelup)
        {
            mod.panelOpen = true;
            if (instance) instance.CloseMenu();

            if (SandboxMode)
            {
                SandBoxWeaponPanel(rect, tower);
                return;
            }

            var weaponPanelWidth = 833.33f;
            var weaponPanelX = 412.5f;
            float weaponPanelY = 900;
            float wpnContentX = 25 - (mod.newWeaponSlot - 1) * 425;
            var panelWidth = mod.newWeaponSlot * weaponPanelWidth;
            var imag = VanillaSprites.BrownInsertPanel;
            if (mod.level == 1) imag = VanillaSprites.BlueInsertPanel;
            if (mod.level == 2) imag = VanillaSprites.MainBgPanelParagon;
            var panel = rect.gameObject.AddModHelperPanel(
                new Info("Panel_", 2200, 1500, panelWidth, 1850, new Vector2()), imag);

            var upgradeUi = panel.AddComponent<MenuUi>();
            var selectWpn = panel.AddText(new Info("selectWpn", 0, 800, 2500, 180), "Select New Weapon", 100);
            if (!instance)
            {
                selectWpn.Text.text = "Choose A starter Weapon";
                selectWpn.Text.color = new Color(0, 1, 0);
            }

            if (Levelup)
            {
                selectWpn.Text.text = "Choose A level up Weapon";
                selectWpn.Text.color = new Color(0.46f, 0, 0.78f);
            }

            var rnd = new Random();
            // TODO Remove + 3
            for (var i = 0; i < mod.newWeaponSlot + 3; i++)
            {
                var WpnRarityNum = rnd.Next(1, 100);
                var WpnRarity = "Common";
                var RarityNumber = 1;
                var MinNum = 1;
                var MaxNum = 1;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Common) MinNum = 1;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Rare) MinNum = 2;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Epic) MinNum = 3;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Legendary) MinNum = 4;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Exotic) MinNum = 5;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Godly) MinNum = 6;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Omega) MinNum = 7;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Common) MaxNum = 1;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Rare) MaxNum = 2;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Epic) MaxNum = 3;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Legendary) MaxNum = 4;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Exotic) MaxNum = 5;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Godly) MaxNum = 6;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Omega) MaxNum = 7;
                if (WpnRarityNum >= mod.rareChance) RarityNumber = 2;
                if (WpnRarityNum >= mod.epicChance) RarityNumber = 3;
                if (WpnRarityNum >= mod.legendaryChance) RarityNumber = 4;
                if (WpnRarityNum >= mod.exoticChance) RarityNumber = 5;
                if (WpnRarityNum >= mod.godlyChance) RarityNumber = 6;
                if (WpnRarityNum >= mod.omegaChance) RarityNumber = 7;
                if (RarityNumber < MinNum) RarityNumber = MinNum;
                if (RarityNumber > MaxNum) RarityNumber = MaxNum;
                if (RarityNumber == 2) WpnRarity = "Rare";
                if (RarityNumber == 3) WpnRarity = "Epic";
                if (RarityNumber == 4) WpnRarity = "Legendary";
                if (RarityNumber == 5) WpnRarity = "Exotic";
                if (RarityNumber == 6) WpnRarity = "Godly";
                if (RarityNumber == 7) WpnRarity = "Omega";

                var sprite = VanillaSprites.GreyInsertPanel;
                var numWpn = rnd.Next(0, Common.CommonWpn.Count);
                var weapon = Common.CommonWpn[numWpn];
                var img = Common.CommonImg[numWpn];
                var csprite = Common.CommonCustomImg[numWpn];
                if (WpnRarity == "Rare")
                {
                    numWpn = rnd.Next(0, Rare.RareWpn.Count);
                    sprite = VanillaSprites.BlueInsertPanel;
                    weapon = Rare.RareWpn[numWpn];
                    img = Rare.RareImg[numWpn];
                    csprite = Rare.RareCustomImg[numWpn];
                }

                if (WpnRarity == "Epic")
                {
                    numWpn = rnd.Next(0, Epic.EpicWpn.Count);
                    sprite = VanillaSprites.MainBgPanelParagon;
                    weapon = Epic.EpicWpn[numWpn];
                    img = Epic.EpicImg[numWpn];
                    csprite = Epic.EpicCustomImg[numWpn];
                }

                if (WpnRarity == "Legendary")
                {
                    numWpn = rnd.Next(0, Legendary.LegendaryWpn.Count);
                    sprite = VanillaSprites.MainBGPanelYellow;
                    weapon = Legendary.LegendaryWpn[numWpn];
                    img = Legendary.LegendaryImg[numWpn];
                    csprite = Legendary.LegendaryCustomImg[numWpn];
                }

                if (WpnRarity == "Exotic")
                {
                    numWpn = rnd.Next(0, Exotic.ExoticWpn.Count);
                    sprite = VanillaSprites.MainBgPanelWhiteSmall;
                    weapon = Exotic.ExoticWpn[numWpn];
                    img = Exotic.ExoticImg[numWpn];
                    csprite = Exotic.ExoticCustomImg[numWpn];
                }

                if (WpnRarity == "Godly")
                {
                    numWpn = rnd.Next(0, Godly.GodlyWpn.Count);
                    sprite = VanillaSprites.MainBGPanelSilver;
                    weapon = Godly.GodlyWpn[numWpn];
                    img = Godly.GodlyImg[numWpn];
                    csprite = Godly.GodlyCustomImg[numWpn];
                }

                if (WpnRarity == "Omega")
                {
                    numWpn = rnd.Next(0, Omega.OmegaWpn.Count);
                    sprite = VanillaSprites.MainBgPanelHematite;
                    weapon = Omega.OmegaWpn[numWpn];
                    img = Omega.OmegaImg[numWpn];
                    csprite = Omega.OmegaCustomImg[numWpn];
                }

                // TODO Start remove
                numWpn = rnd.Next(0, Godly.GodlyWpn.Count);
                sprite = VanillaSprites.MainBGPanelSilver;
                weapon = Godly.GodlyWpn[numWpn];
                img = Godly.GodlyImg[numWpn];
                csprite = Godly.GodlyCustomImg[numWpn];
                // End remove

                var wpnPanel =
                    panel.AddPanel(new Info("wpnPanel", weaponPanelX, weaponPanelY, 650, 1450, new Vector2()), sprite);
                var rarityText = panel.AddText(new Info("rarityText", wpnContentX, 600, 800, 180), WpnRarity, 100);
                var weaponText = panel.AddText(new Info("weaponText", wpnContentX, 500, 800, 180), weapon, 75);

                var selectWpnBtn = panel.AddButton(new Info("selectWpnBtn", wpnContentX, -600, 500, 160),
                    VanillaSprites.GreenBtnLong, new Action(() => upgradeUi.WeaponSelected(weapon, tower, Levelup)));
                var selectWpnTxt = selectWpnBtn.AddText(new Info("selectWpnTxt", 0, 0, 700, 160), "Select", 70);
                foreach (var weaponContent in ModContent.GetContent<WeaponTemplate>()
                             .OrderByDescending(c => c.mod == mod))
                    if (weaponContent.WeaponName == weapon)
                    {
                        var descText = panel.AddText(new Info("descText", wpnContentX, 400, 800, 180),
                            weaponContent.Description, 55);
                        var StackIndex = panel.AddText(new Info("StackIndex", wpnContentX - 275, 650, 100, 100),
                            $"{weaponContent.stackIndex}", 80);
                        if (weaponContent.CustomIcon)
                        {
                            var image = panel.AddImage(new Info("image", wpnContentX, 0, 400, 400), csprite);
                        }
                        else
                        {
                            var image = panel.AddImage(new Info("image", wpnContentX, 0, 400, 400), img);
                        }

                        if (weaponContent.IsCamo && !weaponContent.IsLead)
                        {
                            var camoImg = panel.AddImage(new Info("camoImg", wpnContentX + 275, 650, 100, 100),
                                VanillaSprites.CamoBloonIcon);
                        }

                        if (weaponContent.IsLead && !weaponContent.IsCamo)
                        {
                            var leadImg = panel.AddImage(new Info("leadImg", wpnContentX + 275, 650, 100, 100),
                                VanillaSprites.LeadBloonIcon);
                        }

                        if (weaponContent.IsLead && weaponContent.IsCamo)
                        {
                            var camoImg = panel.AddImage(new Info("camoImg", wpnContentX + 275, 650, 100, 100),
                                VanillaSprites.CamoBloonIcon);
                            var leadImg = panel.AddImage(new Info("leadImg", wpnContentX + 275, 560, 100, 100),
                                VanillaSprites.LeadBloonIcon);
                        }
                    }

                weaponPanelX += weaponPanelWidth;
                wpnContentX += weaponPanelWidth;
            }
        }

        public static void SandBoxStrongWeaponPanel(RectTransform rect, Tower tower)
        {
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1250, 1500, new Vector2()),
                VanillaSprites.MainBGPanelBlue);
            var upgradeUi = panel.AddComponent<MenuUi>();
            panel.transform.DestroyAllChildren();
            if (mod.damageBoostSandbox < 0) mod.damageBoostSandbox = 0;
            if (mod.pierceBoostSandbox < 0) mod.pierceBoostSandbox = 0;
            if (mod.rangeBoostSandbox <= 0.9) mod.rangeBoostSandbox = 1f;
            if (mod.attackSpeedBoostSandbox <= 0) mod.attackSpeedBoostSandbox = 0.05f;
            if (mod.attackSpeedBoostSandbox >= 1.05f) mod.attackSpeedBoostSandbox = 1f;
            if (mod.moneyBoostSandbox <= 0.9) mod.moneyBoostSandbox = 1f;

            mod.rangeBoostSandbox = Mathf.Round(mod.rangeBoostSandbox * 10) / 10;
            mod.attackSpeedBoostSandbox = Mathf.Round(mod.attackSpeedBoostSandbox * 100) / 100;
            mod.moneyBoostSandbox = Mathf.Round(mod.moneyBoostSandbox * 10) / 10;

            panel.AddText(new Info("text", 300, 350, 1200, 150), "Damage Boost: " + mod.damageBoostSandbox, 75,
                TextAlignmentOptions.TopLeft);
            panel.AddButton(new Info("button1", -400, 430, 100, 100), VanillaSprites.AddMoreBtn, new Action(() =>
            {
                mod.damageBoostSandbox += 1;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));
            panel.AddButton(new Info("button2", -400, 350, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() =>
            {
                mod.damageBoostSandbox -= 1;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));

            panel.AddText(new Info("text2", 300, 150, 1200, 150), "Pierce Boost: " + mod.pierceBoostSandbox, 75,
                TextAlignmentOptions.TopLeft);
            panel.AddButton(new Info("button3", -400, 230, 100, 100), VanillaSprites.AddMoreBtn, new Action(() =>
            {
                mod.pierceBoostSandbox += 1;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));
            panel.AddButton(new Info("button4", -400, 150, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() =>
            {
                mod.pierceBoostSandbox -= 1;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));

            panel.AddText(new Info("text2", 300, -50, 1200, 150), "Range Boost: " + mod.rangeBoostSandbox, 75,
                TextAlignmentOptions.TopLeft);
            panel.AddButton(new Info("button3", -400, 30, 100, 100), VanillaSprites.AddMoreBtn, new Action(() =>
            {
                mod.rangeBoostSandbox += 0.1f;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));
            panel.AddButton(new Info("button4", -400, -50, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() =>
            {
                mod.rangeBoostSandbox -= 0.1f;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));

            panel.AddText(new Info("text2", 300, -250, 1200, 150), "Attack Speed Boost: " + mod.attackSpeedBoostSandbox,
                75, TextAlignmentOptions.TopLeft);
            panel.AddButton(new Info("button3", -400, -170, 100, 100), VanillaSprites.AddMoreBtn, new Action(() =>
            {
                mod.attackSpeedBoostSandbox -= 0.05f;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));
            panel.AddButton(new Info("button4", -400, -250, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() =>
            {
                mod.attackSpeedBoostSandbox += 0.05f;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));

            panel.AddText(new Info("text2", 300, -450, 1200, 150), "Money Boost: " + mod.moneyBoostSandbox, 75,
                TextAlignmentOptions.TopLeft);
            panel.AddButton(new Info("button3", -400, -370, 100, 100), VanillaSprites.AddMoreBtn, new Action(() =>
            {
                mod.moneyBoostSandbox += 0.1f;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));
            panel.AddButton(new Info("button4", -400, -450, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() =>
            {
                mod.moneyBoostSandbox -= 0.1f;
                panel.DeleteObject();
                ;
                SandBoxStrongWeaponPanel(rect, tower);
            }));

            var selectWpnBtn = panel.AddButton(new Info("selectWpnBtn", 0, -600, 400, 120), VanillaSprites.GreenBtnLong,
                new Action(() =>
                {
                    upgradeUi.StrongWpnSelected(mod.damageBoostSandbox, mod.pierceBoostSandbox,
                        mod.attackSpeedBoostSandbox, mod.rangeBoostSandbox, mod.moneyBoostSandbox, tower);
                    panel.DeleteObject();
                }));
            var selectWpn = selectWpnBtn.AddText(new Info("selectWpn", 0, 0, 700, 160), "Edit", 70);
        }

        public static void StrongWeaponPanel(RectTransform rect, Tower tower)
        {
            mod.panelOpen = true;
            if (SandboxMode)
            {
                SandBoxStrongWeaponPanel(rect, tower);
                return;
            }

            var strongWeaponPanelWidth = 833.33f;
            var strongWeaponPanelX = 412.5f;
            float strongWeaponPanelY = 900;
            float strongWpnContentX = 25 - (mod.strongWeaponSlot - 1) * 425;
            var panelWidth = mod.strongWeaponSlot * strongWeaponPanelWidth;
            var img = VanillaSprites.BrownInsertPanel;
            if (mod.level == 1) img = VanillaSprites.BlueInsertPanel;
            if (mod.level == 2) img = VanillaSprites.MainBgPanelParagon;
            var panel = rect.gameObject.AddModHelperPanel(
                new Info("Panel_", 2200, 1500, panelWidth, 1850, new Vector2()), img);
            var selectStrongWpn = panel.AddText(new Info("selectStrongWpn", 0, 800, 2500, 180),
                "Select Stronger Weapon Card", 100);
            var rnd = new Random();

            var upgradeUi = panel.AddComponent<MenuUi>();
            for (var i = 0; i < mod.strongWeaponSlot; i++)
            {
                var StrongWpnRarityNum = rnd.Next(1, 100);
                var StrongWpnRarity = "Common";
                var RarityNumber = 1;
                var MinNum = 1;
                var MaxNum = 1;
                var damageBoost = rnd.Next(0, 2);
                var pierceBoost = rnd.Next(0, 2);
                // TODO 0 -- 7
                float rangeBoost = rnd.Next(40, 50);
                float attackSpeedBoost = rnd.Next(95, 101);
                float moneyBoost = rnd.Next(0, 7);
                var sprite = VanillaSprites.GreyInsertPanel;


                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Common) MinNum = 1;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Rare) MinNum = 2;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Epic) MinNum = 3;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Legendary) MinNum = 4;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Exotic) MinNum = 5;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Godly) MinNum = 6;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Omega) MinNum = 7;
                if (mod.minNewWeaponRarity == WeaponTemplate.Rarity.Common) MaxNum = 1;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Rare) MaxNum = 2;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Epic) MaxNum = 3;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Legendary) MaxNum = 4;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Exotic) MaxNum = 5;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Godly) MaxNum = 6;
                if (mod.maxNewWeaponRarity == WeaponTemplate.Rarity.Omega) MaxNum = 7;
                if (StrongWpnRarityNum >= mod.rareStrongChance) RarityNumber = 2;
                if (StrongWpnRarityNum >= mod.epicStrongChance) RarityNumber = 3;
                if (StrongWpnRarityNum >= mod.legendaryStrongChance) RarityNumber = 4;
                if (StrongWpnRarityNum >= mod.exoticStrongChance) RarityNumber = 5;
                if (StrongWpnRarityNum >= mod.godlyStrongerChance) RarityNumber = 6;
                if (StrongWpnRarityNum >= mod.omegaStrongerChance) RarityNumber = 7;
                if (RarityNumber < MinNum) RarityNumber = MinNum;
                if (RarityNumber > MaxNum) RarityNumber = MaxNum;
                if (RarityNumber == 2) StrongWpnRarity = "Rare";
                if (RarityNumber == 3) StrongWpnRarity = "Epic";
                if (RarityNumber == 4) StrongWpnRarity = "Legendary";
                if (RarityNumber == 5) StrongWpnRarity = "Exotic";
                if (RarityNumber == 6) StrongWpnRarity = "Godly";
                if (RarityNumber == 7) StrongWpnRarity = "Omega";
                if (StrongWpnRarity == "Rare")
                {
                    damageBoost = rnd.Next(0, 3);
                    pierceBoost = rnd.Next(0, 3);
                    rangeBoost = rnd.Next(0, 10);
                    attackSpeedBoost = rnd.Next(91, 101);
                    moneyBoost = rnd.Next(0, 10);
                    sprite = VanillaSprites.BlueInsertPanel;
                }

                if (StrongWpnRarity == "Epic")
                {
                    damageBoost = rnd.Next(1, 5);
                    pierceBoost = rnd.Next(1, 5);
                    rangeBoost = rnd.Next(0, 13);
                    attackSpeedBoost = rnd.Next(88, 101);
                    moneyBoost = rnd.Next(0, 13);
                    sprite = VanillaSprites.MainBgPanelParagon;
                }

                if (StrongWpnRarity == "Legendary")
                {
                    damageBoost = rnd.Next(2, 7);
                    pierceBoost = rnd.Next(2, 7);
                    rangeBoost = rnd.Next(0, 15);
                    attackSpeedBoost = rnd.Next(85, 101);
                    moneyBoost = rnd.Next(0, 15);
                    sprite = VanillaSprites.MainBGPanelYellow;
                }

                if (StrongWpnRarity == "Exotic")
                {
                    damageBoost = rnd.Next(3, 9);
                    pierceBoost = rnd.Next(3, 9);
                    rangeBoost = rnd.Next(0, 18);
                    attackSpeedBoost = rnd.Next(83, 101);
                    moneyBoost = rnd.Next(0, 18);
                    sprite = VanillaSprites.MainBgPanelWhiteSmall;
                }

                if (StrongWpnRarity == "Godly")
                {
                    damageBoost = rnd.Next(5, 15);
                    pierceBoost = rnd.Next(5, 15);
                    rangeBoost = rnd.Next(0, 23);
                    attackSpeedBoost = rnd.Next(75, 101);
                    moneyBoost = rnd.Next(0, 23);
                    sprite = VanillaSprites.MainBGPanelSilver;
                }

                if (StrongWpnRarity == "Omega")
                {
                    damageBoost = rnd.Next(8, 22);
                    pierceBoost = rnd.Next(8, 22);
                    rangeBoost = rnd.Next(0, 28);
                    attackSpeedBoost = rnd.Next(71, 101);
                    moneyBoost = rnd.Next(0, 28);
                    sprite = VanillaSprites.MainBgPanelHematite;
                }

                attackSpeedBoost = attackSpeedBoost / 100;
                rangeBoost = 1 + rangeBoost / 100;
                moneyBoost = 1 + moneyBoost / 100;
                var strongWpnPanel =
                    panel.AddPanel(
                        new Info("strongWpnPanel", strongWeaponPanelX, strongWeaponPanelY, 650, 1450, new Vector2()),
                        sprite);

                var rarityText = panel.AddText(new Info("rarityText", strongWpnContentX, 600, 800, 180),
                    StrongWpnRarity, 100);
                var cardText = panel.AddText(new Info("cardText", strongWpnContentX, 500, 800, 180),
                    "Stronger Weapon Card", 50);
                var dmgBoostText = panel.AddText(new Info("dmgBoostText", strongWpnContentX, 200, 800, 180),
                    "Damage Boost :" + damageBoost, 50);
                var prcBoostText = panel.AddText(new Info("prcBoostText", strongWpnContentX, 100, 800, 180),
                    "Pierce Boost :" + pierceBoost, 50);
                var rngBoostText = panel.AddText(new Info("rngBoostText", strongWpnContentX, 0, 800, 180),
                    "Range Boost : X" + rangeBoost, 50);
                var atkspdBoostText = panel.AddText(new Info("atkspdBoostText", strongWpnContentX, -100, 800, 180),
                    "Attack Speed Boost : X" + attackSpeedBoost, 50);
                var mnyBoostText = panel.AddText(new Info("mnyBoostText", strongWpnContentX, -200, 800, 180),
                    "Money Boost : X" + moneyBoost, 50);
                var selectWpnBtn = panel.AddButton(new Info("selectWpnBtn", strongWpnContentX, -600, 500, 160),
                    VanillaSprites.GreenBtnLong,
                    new Action(() => upgradeUi.StrongWpnSelected(damageBoost, pierceBoost, attackSpeedBoost, rangeBoost,
                        moneyBoost, tower)));
                var selectWpn = selectWpnBtn.AddText(new Info("selectWpn", 0, 0, 700, 160), "Select", 70);
                strongWeaponPanelX += strongWeaponPanelWidth;
                strongWpnContentX += strongWeaponPanelWidth;
            }
        }

        public void StrongWeapon(Tower tower)
        {
            var game = InGame.instance;
            if (SandboxMode)
            {
                var rect = game.uiRect;
                StrongWeaponPanel(rect, tower);
                instance.CloseMenu();

                return;
            }

            if (game.GetCash() >= mod.strongerWeaponCost)
            {
                game.AddCash(-mod.strongerWeaponCost);
                var rect = game.uiRect;
                StrongWeaponPanel(rect, tower);
                mod.strongerWeaponCost += mod.baseStrongerWeaponCost;
                tower.worth += mod.strongerWeaponCost - mod.baseStrongerWeaponCost;
                mod.baseStrongerWeaponCost *= 1.06f;


                instance.CloseMenu();
            }
        }

        public void StrongWpnSelected(int Dmg, int Pierce, float AtkSpd, float Range, float Money, Tower tower)
        {
            mod.panelOpen = false;
            var game = InGame.instance;
            Destroy(gameObject);
            var rect = game.uiRect;
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            if (mod.upgradeOpen) CreateUpgradeMenu(rect, tower);
            towerModel.range *= Range;
            if (towerModel.HasBehavior<TowerCreateTowerModel>())
                foreach (var towercreate in towerModel.GetBehaviors<TowerCreateTowerModel>().ToArray())
                    if (towercreate.towerModel.HasBehavior<AirUnitModel>())
                    {
                        foreach (var attackModel in towercreate.towerModel.GetBehavior<AirUnitModel>()
                                     .GetBehaviors<AttackAirUnitModel>().ToArray())
                            try
                            {
                                if (towerModel.GetBehavior<TowerCreateTowerModel>().towerModel
                                    .HasBehavior<AirUnitModel>())
                                {
                                    if (attackModel.weapons[0].projectile.HasBehavior<DamageModel>())
                                    {
                                        attackModel.weapons[0].rate *= AtkSpd;
                                        attackModel.weapons[0].projectile.pierce += Pierce;
                                        attackModel.weapons[0].projectile.GetDamageModel().damage += Dmg;
                                    }

                                    if (attackModel.weapons[0].projectile.HasBehavior<TravelStraitModel>())
                                        attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan +=
                                            Range / 90;
                                    if (attackModel.weapons[0].projectile.HasBehavior<CashModel>())
                                    {
                                        attackModel.weapons[0].projectile.GetBehavior<CashModel>().minimum *= Money;
                                        attackModel.weapons[0].projectile.GetBehavior<CashModel>().maximum *= Money;
                                    }
                                }
                            }
                            catch
                            {
                            }

                        foreach (var attackModel in towercreate.towerModel.GetBehavior<AirUnitModel>()
                                     .GetBehaviors<AttackModel>().ToArray())
                            try
                            {
                                if (attackModel.weapons[0].projectile.HasBehavior<DamageModel>())
                                {
                                    attackModel.weapons[0].rate *= AtkSpd;
                                    attackModel.weapons[0].projectile.pierce += Pierce;
                                    attackModel.weapons[0].projectile.GetDamageModel().damage += Dmg;
                                }

                                if (attackModel.weapons[0].projectile.HasBehavior<TravelStraitModel>())
                                    attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan +=
                                        Range / 90;
                                if (attackModel.weapons[0].projectile.HasBehavior<CashModel>())
                                {
                                    attackModel.weapons[0].projectile.GetBehavior<CashModel>().minimum *= Money;
                                    attackModel.weapons[0].projectile.GetBehavior<CashModel>().maximum *= Money;
                                }
                            }
                            catch
                            {
                            }
                    }

            foreach (var attackModel in towerModel.GetDescendants<AttackModel>().ToArray())
                try
                {
                    if (!attackModel.weapons[0].projectile.HasBehavior<SlowModel>())
                    {
                        attackModel.weapons[0].rate *= AtkSpd;
                        if (attackModel.weapons[0].projectile.HasBehavior<DamageModel>())
                        {
                            attackModel.weapons[0].projectile.pierce += Pierce;
                            attackModel.weapons[0].projectile.GetDamageModel().damage += Dmg;
                        }

                        if (attackModel.weapons[0].projectile.HasBehavior<TravelStraitModel>())
                            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan += Range / 60;
                        if (attackModel.weapons[0].projectile.HasBehavior<CashModel>())
                        {
                            attackModel.weapons[0].projectile.GetBehavior<CashModel>().minimum *= Money;
                            attackModel.weapons[0].projectile.GetBehavior<CashModel>().maximum *= Money;
                        }

                        if (attackModel.weapons[0].projectile.HasBehavior<CreateProjectileOnContactModel>())
                        {
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile
                                .pierce += Pierce;
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile
                                .GetDamageModel().damage += Dmg;
                        }

                        if (attackModel.weapons[0].projectile.HasBehavior<CreateProjectileOnExhaustFractionModel>())
                        {
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>()
                                .projectile.pierce += Pierce;
                            attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>()
                                .projectile.GetDamageModel().damage += Dmg;
                        }
                    }
                }
                catch
                {
                }

            foreach (var attackModel in towerModel.GetDescendants<AttackModel>().ToArray())
                try
                {
                    attackModel.range *= Range;
                }
                catch
                {
                }

            mod.rareStrongChance -= 6.7f + mod.ExtraLuckLevel * 0.55f;
            mod.epicStrongChance -= 2f + mod.ExtraLuckLevel * 0.15f;
            if (mod.epicStrongChance <= 88) mod.legendaryStrongChance -= 1.15f + mod.ExtraLuckLevel * 0.09f;
            if (mod.legendaryStrongChance <= 91) mod.exoticStrongChance -= 0.75f + mod.ExtraLuckLevel * 0.05f;
            if (mod.exoticStrongChance <= 93) mod.godlyStrongerChance -= 0.55f + mod.ExtraLuckLevel * 0.03f;
            if (mod.godlyChance <= 94) mod.omegaStrongerChance -= 0.30f + mod.ExtraLuckLevel * 0.015f;
            tower.UpdateRootModel(towerModel);
        }

        public void NewAbility(Tower tower)
        {
            var game = InGame.instance;
            if (SandboxMode)
            {
                var rect = game.uiRect;
                NewAbilityPanel(rect, tower);
                instance.CloseMenu();
                return;
            }

            if (game.GetCash() >= mod.newAbilityCost)
            {
                game.AddCash(-mod.newAbilityCost);
                var rect = game.uiRect;
                mod.newAbilityCost += mod.baseNewAbilityCost;
                tower.worth += mod.newAbilityCost - mod.baseNewAbilityCost;
                NewAbilityPanel(rect, tower);
                instance.CloseMenu();
            }
        }

        public static ModHelperPanel CreateAbility(AbilityTemplate ability, Tower tower)
        {
            var sprite = VanillaSprites.GreyInsertPanel;
            var panel = ModHelperPanel.Create(new Info("WeaponContent" + ability.AbilityName, 0, 0, 2250, 150), sprite);
            var upgradeUi = panel.AddComponent<MenuUi>();
            var abilityName = panel.AddText(new Info("abilityName", -600, 0, 1000, 150), ability.AbilityName, 80,
                TextAlignmentOptions.MidlineLeft);
            var image = panel.AddImage(new Info("image", -100, 0, 140, 140), ability.Icon);
            var selectWpnBtn = panel.AddButton(new Info("selectWpnBtn", 900, 0, 400, 120), VanillaSprites.GreenBtnLong,
                new Action(() => { upgradeUi.AbilitySelected(ability.AbilityName, tower, "none"); }));
            var selectWpn = selectWpnBtn.AddText(new Info("selectWpn", 0, 0, 700, 160), "Select", 60);
            return panel;
        }

        public static void SandBoxAbilityPanel(RectTransform rect, Tower tower)
        {
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new Vector2()),
                VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();

            var scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 0, 0, 2500, 1850),
                RectTransform.Axis.Vertical, VanillaSprites.MainBGPanelBlue, 15, 50);
            var exit = panel.AddButton(new Info("exit", 1200, 900, 135, 135), VanillaSprites.RedBtn, new Action(() =>
            {
                tower.SetSelectionBlocked(false);
                panel.DeleteObject();
            }));
            var x = exit.AddText(new Info("x", 0, 0, 700, 160), "X", 80);
            foreach (var ability in ModContent.GetContent<AbilityTemplate>())
                scrollPanel.AddScrollContent(CreateAbility(ability, tower));
        }

        public static void NewAbilityPanel(RectTransform rect, Tower tower)
        {
            mod.panelOpen = true;
            if (SandboxMode)
            {
                SandBoxAbilityPanel(rect, tower);
                return;
            }

            var abilityPanelWidth = 833.33f;
            var abilityPanelX = 412.5f;
            float abilityPanelY = 900;
            float abilityContentX = 25 + (mod.abilitySlot - 1) * -425;
            var panelWidth = mod.abilitySlot * abilityPanelWidth;
            var sprite = VanillaSprites.BrownInsertPanel;
            if (mod.level == 1) sprite = VanillaSprites.BlueInsertPanel;
            if (mod.level == 2) sprite = VanillaSprites.MainBgPanelParagon;
            var panel = rect.gameObject.AddModHelperPanel(
                new Info("Panel_", 2200, 1500, panelWidth, 1850, new Vector2()), sprite);
            var selectAb = panel.AddText(new Info("selectAb", 0, 800, 2500, 180), "Select New Ability", 100);
            var upgradeUi = panel.AddComponent<MenuUi>();

            var rnd = new Random();
            for (var i = 0; i < mod.abilitySlot; i++)
            {
                var num = rnd.Next(0, AbilityClass.AbilityName.Count);
                var abSelected = AbilityClass.AbilityName[num];
                var imgSelected = AbilityClass.AbilityImg[num];
                var csprite = AbilityClass.AbilityCustomImg[num];
                var abilityPanel =
                    panel.AddPanel(new Info("abilityPanel", abilityPanelX, abilityPanelY, 650, 1450, new Vector2()),
                        VanillaSprites.GreyInsertPanel);
                var abilityText =
                    panel.AddText(new Info("abilityText", abilityContentX, 600, 800, 180), "Ability", 100);
                var abilityText2 = panel.AddText(new Info("abilityText2", abilityContentX, 500, 800, 180), abSelected,
                    75);
                var selectAbilityBtn = panel.AddButton(new Info("selectAbilityBtn", abilityContentX, -600, 500, 160),
                    VanillaSprites.GreenBtnLong,
                    new Action(() => upgradeUi.AbilitySelected(abSelected, tower, "Common")));
                var selectAbility1 = selectAbilityBtn.AddText(new Info("selectAbility1", 0, 0, 700, 160), "Select", 70);


                foreach (var ability in ModContent.GetContent<AbilityTemplate>().OrderByDescending(c => c.mod == mod))
                    if (ability.AbilityName == abSelected)
                    {
                        var descText = panel.AddText(new Info("descText", abilityContentX, 400, 800, 180),
                            ability.Description, 55);
                        var StackIndex = panel.AddText(new Info("StackIndex", abilityContentX - 275, 650, 100, 100),
                            $"{ability.stackIndex}", 80);
                        if (ability.CustomIcon)
                        {
                            var image = panel.AddImage(new Info("image", abilityContentX, 0, 400, 400), csprite);
                        }
                        else
                        {
                            var image = panel.AddImage(new Info("image", abilityContentX, 0, 400, 400), imgSelected);
                        }
                    }

                abilityPanelX += abilityPanelWidth;
                abilityContentX += abilityPanelWidth;
            }
        }

        public void AbilitySelected(string Ability, Tower tower, string rarity)
        {
            mod.panelOpen = false;
            var game = InGame.instance;
            if (!SandboxMode) Destroy(gameObject);
            var rect = game.uiRect;
            if (mod.upgradeOpen && !SandboxMode) CreateUpgradeMenu(rect, tower);

            if (Ability == "MIB")
            {
                mod.mib = true;
                var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                {
                    if (weaponModel.projectile.HasBehavior<DamageModel>())
                        weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    if (weaponModel.projectile.HasBehavior<CreateProjectileOnContactModel>())
                        weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel()
                            .immuneBloonProperties = BloonProperties.None;
                    if (weaponModel.projectile.HasBehavior<CreateProjectileOnExhaustFractionModel>())
                        weaponModel.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile
                            .GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }

                tower.UpdateRootModel(towerModel);
            }

            var mutated = false;
            foreach (var ability in ModContent.GetContent<AbilityTemplate>().OrderByDescending(c => c.mod == mod))
                if (ability.AbilityName == Ability)
                {
                    ability.EditTower(tower);
                    mutated = true;
                    ability.stackIndex += 1;
                }

            if (mutated) AbilityMenu.instance.AbilitiesChanged();
        }

        public void Upgrade1Panel(Tower tower)
        {
            instance.CloseMenu();
            var game = InGame.instance;
            var rect = game.uiRect;
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new Vector2()),
                VanillaSprites.BrownInsertPanel);
            var upgradeUi = panel.AddComponent<MenuUi>();
            var upgradeText = panel.AddText(new Info("upgradeText", 0, 800, 2500, 180),
                "Upgrade To Advanced Ancient Monkey", 100);
            var text1 = panel.AddText(new Info("text1", 0, 500, 2500, 180), "-Unlock a new rarity", 75);
            var text2 = panel.AddText(new Info("text2", 0, 400, 2500, 180), "-Increase New Weapon Slot by 1", 75);
            var text3 = panel.AddText(new Info("text3", 0, 300, 2500, 180), "-Increase Stronger Weapon Slot by 1", 75);
            var text4 = panel.AddText(new Info("text4", 0, 200, 2500, 180), "-Increase New Ability Slot by 1", 75);
            var text5 = panel.AddText(new Info("text5", 0, 100, 2500, 180), "-Greatly Increased Luck", 75);
            var text6 = panel.AddText(new Info("text6", 0, 0, 2500, 180), "-Removed Common Rarity", 75);
            var text7 = panel.AddText(new Info("text7", 0, -100, 2500, 180),
                "-Stronger Weapon and New Weapon Cost Increased", 75);
            var text8 = panel.AddText(new Info("text8", 0, -200, 2500, 180), "-New Ability Cost Decreased", 75);
            var text9 = panel.AddText(new Info("text9", 0, -300, 2500, 180), "-Keep Everything", 75);
            var upgrade1 = panel.AddButton(new Info("upgrade1", 350, -800, 500, 160), VanillaSprites.GreenBtnLong,
                new Action(() => upgradeUi.Upgrade1(tower)));
            var upgrade1Buy = upgrade1.AddText(new Info("upgrade1Buy", 0, 0, 700, 160),
                "Upgrade ($" + mod.UpgradeCost / 1000 + "K)", 70);
            var cancel = panel.AddButton(new Info("cancel", -350, -800, 500, 160), VanillaSprites.RedBtnLong,
                new Action(() => upgradeUi.Cancel(tower)));
            var cancelText = cancel.AddText(new Info("cancelText", 0, 0, 700, 160), "Cancel", 70);
        }

        public void Upgrade2Panel(Tower tower)
        {
            instance.CloseMenu();
            var game = InGame.instance;
            var rect = game.uiRect;
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new Vector2()),
                VanillaSprites.BrownInsertPanel);
            var upgradeUi = panel.AddComponent<MenuUi>();
            var upgradeText = panel.AddText(new Info("upgradeText", 0, 800, 2500, 180),
                "Upgrade To Super Ancient Monkey", 100);
            var text1 = panel.AddText(new Info("text1", 0, 500, 2500, 180), "-Unlock a new rarity", 75);
            var text2 = panel.AddText(new Info("text2", 0, 400, 2500, 180), "-Greatly Increased Luck", 75);
            var text3 = panel.AddText(new Info("text3", 0, 300, 2500, 180), "-Removed Rare Rarity", 75);
            var text4 = panel.AddText(new Info("text4", 0, 200, 2500, 180),
                "-Stronger Weapon and New Weapon Cost Increased", 75);
            var text5 = panel.AddText(new Info("text5", 0, 100, 2500, 180), "-New Ability Cost Decreased", 75);
            var text6 = panel.AddText(new Info("text5", 0, 0, 2500, 180), "-New XP System", 75);
            var text7 = panel.AddText(new Info("text7", 0, -100, 2500, 180), "-Keep Everything", 75);
            var upgrade1 = panel.AddButton(new Info("upgrade1", 350, -800, 500, 160), VanillaSprites.GreenBtnLong,
                new Action(() => upgradeUi.Upgrade2(tower)));
            var upgrade1Buy = upgrade1.AddText(new Info("upgrade1Buy", 0, 0, 700, 160),
                "Upgrade ($" + mod.Upgrade2Cost / 1000000 + "M)", 70);
            var cancel = panel.AddButton(new Info("cancel", -350, -800, 500, 160), VanillaSprites.RedBtnLong,
                new Action(() => upgradeUi.Cancel(tower)));
            var cancelText = cancel.AddText(new Info("cancelText", 0, 0, 700, 160), "Cancel", 70);
        }

        public void ChangeSkin(Tower tower)
        {
            var rnd = new Il2CppNinjaKiwi.Common.Random();
            var monkey = Monkeys[rnd.Next(1, Monkeys.Length)];
            var upgradeTier = rnd.Next(1, 6);
            var upgradePath = rnd.Next(1, 4);
            instance.Monkey = monkey;
            instance.upgradePath = upgradePath;
            instance.upgradeTier = upgradeTier;

            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();
            if (upgradePath == 1)
                towerModel.display = Game.instance.model.GetTowerFromId(monkey + "-" + upgradeTier + "00").display;
            if (upgradePath == 2)
                towerModel.display = Game.instance.model.GetTowerFromId(monkey + "-0" + upgradeTier + "0").display;
            if (upgradePath == 3)
                towerModel.display = Game.instance.model.GetTowerFromId(monkey + "-00" + upgradeTier).display;
            tower.UpdateRootModel(towerModel);
        }

        public void ExtraPanel(Tower tower)
        {
            mod.panelOpen = true;

            var game = InGame.instance;
            var rect = game.uiRect;
            var sprite = VanillaSprites.BrownInsertPanel;

            if (mod.level == 1) sprite = VanillaSprites.BlueInsertPanel;
            if (mod.level == 2) sprite = VanillaSprites.MainBgPanelParagon;
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new Vector2()),
                sprite);
            var upgradeUi = panel.AddComponent<MenuUi>();
            var upgradeText = panel.AddText(new Info("upgradeText", 0, 800, 2500, 180), "Extra Upgrades Panel", 100);

            var pipY = 400;
            var pipX = -650;
            for (var i = 0; i < mod.extraWeaponSlotLevel; i++)
            {
                var pipUpgraded =
                    panel.AddImage(new Info("pipUpgraded", pipX, pipY, 80, 160), VanillaSprites.MkOnGreen);
                pipX += 87;
            }

            for (var i = 0; i < mod.extraWeaponSlotLevelMax - mod.extraWeaponSlotLevel; i++)
            {
                var pipNotUpgraded = panel.AddImage(new Info("pipNotUpgraded", pipX, pipY, 80, 160),
                    VanillaSprites.MkOffRed);
                pipX += 87;
            }

            if (mod.extraWeaponSlotLevel == mod.extraWeaponSlotLevelMax)
            {
                var upgradeExtraSlotLevel = panel.AddButton(new Info("upgradeExtraSlotLevel", -980, 400, 525, 145),
                    VanillaSprites.GreenBtnLong, null);
                var upgradeExtraSlotLevelText =
                    upgradeExtraSlotLevel.AddText(new Info("upgradeExtraSlotLevelText", 0, 0, 550, 150), "Max", 50);
            }
            else
            {
                var upgradeExtraSlotLevel = panel.AddButton(new Info("upgradeExtraSlotLevel", -980, 400, 525, 145),
                    VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        if (game.GetCash() >= mod.extraWeaponSlotCost)
                        {
                            game.AddCash(mod.extraWeaponSlotCost *= -1);
                            mod.extraWeaponSlotLevel += 1;
                            panel.DeleteObject();
                            ExtraPanel(tower);
                            mod.newWeaponSlot++;
                        }
                    }));
                var upgradeExtraSlotLevelText = upgradeExtraSlotLevel.AddText(
                    new Info("upgradeExtraSlotLevelText", 0, 0, 550, 150),
                    "Extra New Weapon Slot : $" + mod.extraWeaponSlotCost, 45);
            }

            pipY = 250;
            pipX = -650;
            for (var i = 0; i < mod.strongExtraWeaponSlotLevel; i++)
            {
                var pipUpgraded =
                    panel.AddImage(new Info("pipUpgraded", pipX, pipY, 80, 160), VanillaSprites.MkOnGreen);
                pipX += 87;
            }

            for (var i = 0; i < mod.extraWeaponSlotLevelMax - mod.strongExtraWeaponSlotLevel; i++)
            {
                var pipNotUpgraded = panel.AddImage(new Info("pipNotUpgraded", pipX, pipY, 80, 160),
                    VanillaSprites.MkOffRed);
                pipX += 87;
            }

            if (mod.strongExtraWeaponSlotLevel == mod.extraWeaponSlotLevelMax)
            {
                var upgradeExtraSlotLevel = panel.AddButton(new Info("upgradeExtraSlotLevel", -980, 250, 525, 145),
                    VanillaSprites.GreenBtnLong, null);
                var upgradeExtraSlotLevelText =
                    upgradeExtraSlotLevel.AddText(new Info("upgradeExtraSlotLevelText", 0, 0, 550, 150), "Max", 60);
            }
            else
            {
                var upgradeExtraSlotLevel = panel.AddButton(new Info("upgradeExtraSlotLevel", -980, 250, 525, 145),
                    VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        if (game.GetCash() >= mod.strongExtraWeaponSlotCost)
                        {
                            game.AddCash(mod.strongExtraWeaponSlotCost *= -1);
                            mod.strongExtraWeaponSlotLevel += 1;
                            panel.DeleteObject();
                            ExtraPanel(tower);
                            mod.strongWeaponSlot++;
                        }
                    }));
                var upgradeExtraSlotLevelText = upgradeExtraSlotLevel.AddText(
                    new Info("upgradeExtraSlotLevelText", 0, 0, 550, 150),
                    "Extra Stronger Weapon Slot : $" + mod.strongExtraWeaponSlotCost, 45);
            }

            pipY = 100;
            pipX = -650;
            for (var i = 0; i < mod.ExtraAbilitySlotLevel; i++)
            {
                var pipUpgraded =
                    panel.AddImage(new Info("pipUpgraded", pipX, pipY, 80, 160), VanillaSprites.MkOnGreen);
                pipX += 87;
            }

            for (var i = 0; i < mod.ExtraAbilitySlotLevelMax - mod.ExtraAbilitySlotLevel; i++)
            {
                var pipNotUpgraded = panel.AddImage(new Info("pipNotUpgraded", pipX, pipY, 80, 160),
                    VanillaSprites.MkOffRed);
                pipX += 87;
            }

            if (mod.ExtraAbilitySlotLevel == mod.ExtraAbilitySlotLevelMax)
            {
                var upgradeExtraSlotLevel = panel.AddButton(new Info("upgradeExtraSlotLevel", -980, 100, 525, 145),
                    VanillaSprites.GreenBtnLong, null);
                var upgradeExtraSlotLevelText =
                    upgradeExtraSlotLevel.AddText(new Info("upgradeExtraSlotLevelText", 0, 0, 550, 150), "Max", 60);
            }
            else
            {
                var upgradeExtraSlotLevel = panel.AddButton(new Info("upgradeExtraSlotLevel", -980, 100, 525, 145),
                    VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        if (game.GetCash() >= mod.ExtraAbilitySlotCost)
                        {
                            game.AddCash(mod.ExtraAbilitySlotCost *= -1);
                            mod.ExtraAbilitySlotLevel += 1;
                            panel.DeleteObject();
                            ExtraPanel(tower);
                            mod.abilitySlot++;
                        }
                    }));
                var upgradeExtraSlotLevelText = upgradeExtraSlotLevel.AddText(
                    new Info("upgradeExtraSlotLevelText", 0, 0, 550, 150),
                    "Extra New Ability Slot : $" + mod.ExtraAbilitySlotCost, 50);
            }

            pipY = -50;
            pipX = -650;
            for (var i = 0; i < mod.ExtraLuckLevel; i++)
            {
                var pipUpgraded =
                    panel.AddImage(new Info("pipUpgraded", pipX, pipY, 80, 160), VanillaSprites.MkOnGreen);
                pipX += 87;
            }

            for (var i = 0; i < mod.ExtraLuckMax - mod.ExtraLuckLevel; i++)
            {
                var pipNotUpgraded = panel.AddImage(new Info("pipNotUpgraded", pipX, pipY, 80, 160),
                    VanillaSprites.MkOffRed);
                pipX += 87;
            }

            if (mod.ExtraLuckLevel == mod.ExtraLuckMax)
            {
                var upgradeExtraLevel = panel.AddButton(new Info("upgradeExtraLevel", -980, -50, 525, 145),
                    VanillaSprites.GreenBtnLong, null);
                var upgradeExtraLevelText =
                    upgradeExtraLevel.AddText(new Info("upgradeExtraLevelText", 0, 0, 550, 150), "Max", 60);
            }
            else
            {
                var upgradeExtraLevel = panel.AddButton(new Info("upgradeExtraLevel", -980, -50, 525, 145),
                    VanillaSprites.GreenBtnLong, new Action(() =>
                    {
                        if (game.GetCash() >= mod.ExtraLuckCost)
                        {
                            game.AddCash(mod.ExtraLuckCost * -1);
                            mod.ExtraLuckLevel += 1;
                            mod.ExtraLuckCost *= 1.35f;
                            ;
                            panel.DeleteObject();
                            ExtraPanel(tower);
                        }
                    }));
                var upgradeExtraLevelText = upgradeExtraLevel.AddText(new Info("upgradeExtraLevelText", 0, 0, 550, 150),
                    "Extra Luck : $" + Mathf.Round(mod.ExtraLuckCost), 50);
            }


            var monkeySkinChange = panel.AddButton(new Info("monkeySkinChange", -980, -200, 525, 145),
                VanillaSprites.GreenBtnLong, new Action(() =>
                {
                    ChangeSkin(tower);
                    panel.DeleteObject();
                    ExtraPanel(tower);
                }));
            var monkeySkinChangeText =
                monkeySkinChange.AddText(new Info("monkeySkinChangeText", 0, 0, 550, 150), "Change Skin", 60);
            if (upgradePath == 1)
            {
                var monkeySkinChangeMonkeyText =
                    panel.AddText(new Info("monkeySkinChangeMonkeyText", 50, -200, 1000, 150),
                        Monkey + " " + upgradeTier + "00", 60);
            }

            if (upgradePath == 2)
            {
                var monkeySkinChangeMonkeyText =
                    panel.AddText(new Info("monkeySkinChangeMonkeyText", 50, -200, 1000, 150),
                        Monkey + " 0" + upgradeTier + "0", 60);
            }

            if (upgradePath == 3)
            {
                var monkeySkinChangeMonkeyText =
                    panel.AddText(new Info("monkeySkinChangeMonkeyText", 50, -200, 1000, 150),
                        Monkey + " 00" + upgradeTier, 60);
            }


            var cancel = panel.AddButton(new Info("cancel", 0, -800, 500, 160), VanillaSprites.RedBtnLong,
                new Action(() => upgradeUi.Cancel(tower)));
            var cancelText = cancel.AddText(new Info("cancelText", 0, 0, 700, 160), "Cancel", 70);
        }

        public void Upgrade1(Tower tower)
        {
            var game = InGame.instance;
            if (game.GetCash() >= mod.UpgradeCost)
            {
                game.AddCash(-mod.UpgradeCost);

                var rect = game.uiRect;

                mod.newWeaponCost = 450 * mod.selectedChallenge.NewWeaponCostMult;
                mod.baseNewWeaponCost = 350 * mod.selectedChallenge.NewWeaponCostMult;
                mod.rareChance = 0;
                mod.epicChance = 75;
                mod.legendaryChance = 98;
                mod.exoticChance = 100;
                mod.godlyChance = 100;
                mod.rareStrongChance = 0;
                mod.epicStrongChance = 70;
                mod.legendaryStrongChance = 95;
                mod.exoticStrongChance = 100;
                mod.godlyStrongerChance = 100;
                mod.strongerWeaponCost = 950 * mod.selectedChallenge.StrongerWeaponCostMult;
                mod.baseStrongerWeaponCost = 750 * mod.selectedChallenge.StrongerWeaponCostMult;
                mod.newAbilityCost = 6000 * mod.selectedChallenge.AbilityWeaponCostMult;

                mod.baseNewAbilityCost = 750 * mod.selectedChallenge.AbilityWeaponCostMult;
                mod.level += 1;
                mod.newWeaponSlot += 1;
                mod.strongWeaponSlot += 1;
                mod.abilitySlot += 1;
                mod.minNewWeaponRarity = mod.selectedChallenge.MinURarity;
                mod.maxNewWeaponRarity = mod.selectedChallenge.MaxURarity;
                mod.minStrongWeaponRarity = mod.selectedChallenge.MinURarity;
                mod.maxStrongWeaponRarity = mod.selectedChallenge.MaxURarity;
                mod.panelOpen = false;
                if (mod.upgradeOpen) CreateUpgradeMenu(rect, tower);

                Destroy(gameObject);
            }
        }

        public void Upgrade2(Tower tower)
        {
            var game = InGame.instance;
            if (game.GetCash() >= mod.Upgrade2Cost)
            {
                game.AddCash(-mod.Upgrade2Cost);

                var rect = game.uiRect;

                mod.newWeaponCost = 2350 * mod.selectedChallenge.NewWeaponCostMult;
                mod.baseNewWeaponCost = 1750 * mod.selectedChallenge.NewWeaponCostMult;
                mod.rareChance = 0;
                mod.epicChance = 0;
                mod.legendaryChance = 80;
                mod.exoticChance = 99;
                mod.godlyChance = 100;
                mod.rareStrongChance = 0;
                mod.epicStrongChance = 0;
                mod.legendaryStrongChance = 75;
                mod.exoticStrongChance = 98;
                mod.godlyStrongerChance = 100;
                mod.strongerWeaponCost = 3500 * mod.selectedChallenge.StrongerWeaponCostMult;
                mod.baseStrongerWeaponCost = 2250 * mod.selectedChallenge.StrongerWeaponCostMult;
                mod.newAbilityCost = 5500 * mod.selectedChallenge.AbilityWeaponCostMult;

                mod.baseNewAbilityCost = 500 * mod.selectedChallenge.AbilityWeaponCostMult;
                mod.level += 1;
                mod.minNewWeaponRarity = mod.selectedChallenge.MinUURarity;
                mod.maxNewWeaponRarity = mod.selectedChallenge.MaxUURarity;
                mod.minStrongWeaponRarity = mod.selectedChallenge.MinUURarity;
                mod.maxStrongWeaponRarity = mod.selectedChallenge.MaxUURarity;
                mod.XP = 0;
                mod.XPMax = 2;

                mod.panelOpen = false;
                if (mod.upgradeOpen) CreateUpgradeMenu(rect, tower);

                Destroy(gameObject);
            }
        }

        public void Cancel(Tower tower)
        {
            var game = InGame.instance;
            var rect = game.uiRect;
            Destroy(gameObject);
            mod.panelOpen = false;
            if (mod.upgradeOpen) CreateUpgradeMenu(rect, tower);
        }

        public static void CreateUpgradeMenu(RectTransform rect, Tower tower)
        {
            if (mod.panelOpen) return;
            var sprite = VanillaSprites.BrownInsertPanel;
            if (mod.level == 1) sprite = VanillaSprites.BlueInsertPanel;
            if (mod.level == 2) sprite = VanillaSprites.MainBgPanelParagon;
            var panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 200, 3333, 450, new Vector2()),
                sprite);
            var upgradeUi = panel.AddComponent<MenuUi>();
            instance = upgradeUi;

            if (SandboxMode)
            {
                var newWpnCost = panel.AddText(new Info("newWpnCost", -416, 140, 1000, 180), "New Weapon: Free", 70);
            }
            else
            {
                var newWpnCost = panel.AddText(new Info("newWpnCost", -416, 140, 1000, 180),
                    "New Weapon: $" + Mathf.Round(mod.newWeaponCost), 70);
            }

            if (mod.selectingWeaponOpen == false)
            {
                var newWpnBtn = panel.AddButton(new Info("newWpnBtn", -416, 20, 500, 160), VanillaSprites.GreenBtnLong,
                    new Action(() => upgradeUi.NewWeapon(tower)));
                var newWpnBuy = newWpnBtn.AddText(new Info("newWpnBuy", 0, 0, 700, 160), "Buy", 70);
            }

            var newWpnDesc = panel.AddText(new Info("newWpnDesc", -416, -100, 860, 180), "Give an extra weapon", 42);

            if (SandboxMode)
            {
                var strongWpnCost = panel.AddText(new Info("strongWpnCost", -1216, 140, 1000, 180),
                    "Stronger Weapon: Free", 70);
            }
            else
            {
                var strongWpnCost = panel.AddText(new Info("strongWpnCost", -1216, 140, 1000, 180),
                    "Stronger Weapon: $" + Mathf.Round(mod.strongerWeaponCost), 70);
            }

            if (mod.selectingWeaponOpen == false)
            {
                var strongWpnBtn = panel.AddButton(new Info("strongWpnBtn", -1216, 20, 500, 160),
                    VanillaSprites.GreenBtnLong, new Action(() => upgradeUi.StrongWeapon(tower)));
                var strongWpnBuy = strongWpnBtn.AddText(new Info("strongWpnBuy", 0, 0, 700, 160), "Buy", 70);
            }

            var strongWpnDesc = panel.AddText(new Info("strongWpnDesc", -1216, -100, 860, 180),
                "Make your current weapon stronger", 42);
            if (SandboxMode)
            {
                var abilityCost = panel.AddText(new Info("abilityCost", 384, 140, 1000, 180), "New Ability: Free", 70);
            }
            else
            {
                var abilityCost = panel.AddText(new Info("abilityCost", 384, 140, 1000, 180),
                    "New Ability: $" + Mathf.Round(mod.newAbilityCost), 70);
            }

            var extraText = panel.AddText(new Info("extraText", 1217, 140, 1000, 180), "Extra Panel", 70);
            var extraBtn = panel.AddButton(new Info("extraBtn", 1217, 20, 500, 160), VanillaSprites.GreenBtnLong,
                new Action(() =>
                {
                    upgradeUi.ExtraPanel(tower);
                    instance.CloseMenu();
                }));
            var extraOpen = extraBtn.AddText(new Info("extraOpen", 0, 0, 700, 160), "Open", 70);

            if (mod.selectingWeaponOpen == false)
            {
                var abilityBtn = panel.AddButton(new Info("abilityBtn", 384, 20, 500, 160), VanillaSprites.GreenBtnLong,
                    new Action(() => upgradeUi.NewAbility(tower)));
                var abilityBuy = abilityBtn.AddText(new Info("abilityBuy", 0, 0, 700, 160), "Buy", 70);
            }

            var abilityDesc = panel.AddText(new Info("abilityDesc", 384, -100, 860, 180), "Give an ability", 42);

            if (mod.level == 0)
            {
                var upgrade1 = panel.AddButton(new Info("upgrade1", 0, 300, 500, 160), VanillaSprites.GreenBtnLong,
                    new Action(() => upgradeUi.Upgrade1Panel(tower)));
                var upgrade1Buy = upgrade1.AddText(new Info("upgrade1Buy", 0, 0, 700, 160), "Upgrade", 70);
            }

            if (mod.level == 1)
            {
                var upgrade1 = panel.AddButton(new Info("upgrade2", 0, 300, 500, 160), VanillaSprites.GreenBtnLong,
                    new Action(() => upgradeUi.Upgrade2Panel(tower)));
                var upgrade1Buy = upgrade1.AddText(new Info("upgrade2Buy", 0, 0, 700, 160), "Upgrade", 70);
            }

            if (mod.level >= 2)
            {
                var percent = mod.XP * 100 / mod.XPMax;
                var size = 2970 * percent / 100;
                var xppanel = panel.AddPanel(new Info("Panel", 0, 325, 3000, 180), VanillaSprites.BrownInsertPanel);
                var xpbar = panel.AddPanel(new Info("Panel", size - size / 2 - 2970 / 2, 325, size, 150),
                    VanillaSprites.MainBgPanelParagon);
                var upgrade1Buy = panel.AddText(new Info("text", 0, 325, 3000, 180),
                    mod.XPMax - mod.XP + " Until Free Weapon", 70);
            }
        }
    }
}