using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace AncientMonkey;

public abstract class WeaponTemplate : ModContent
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Exotic,
        Godly,
        Omega
    }

    public float stackIndex = 0;
    public abstract int SandboxIndex { get; }
    public abstract Rarity WeaponRarity { get; }
    public abstract string WeaponName { get; }
    public abstract string Icon { get; }
    public virtual bool IsCamo { get; }
    public virtual bool IsLead { get; }
    public virtual string Description { get; }
    public virtual Sprite CustomIcon { get; }

    public override void Register()
    {
    }

    public abstract void EditTower(Tower tower);
}