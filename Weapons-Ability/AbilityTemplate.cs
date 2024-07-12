using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace AncientMonkey.Weapons;

public abstract class AbilityTemplate : ModContent
{
    public float stackIndex = 0;
    public abstract string AbilityName { get; }
    public abstract string Icon { get; }
    public virtual string Description { get; }
    public virtual Sprite CustomIcon { get; }
    public virtual bool IsCamo { get; }
    public virtual bool IsLead { get; }

    public override void Register()
    {
    }

    public abstract void EditTower(Tower tower);
}