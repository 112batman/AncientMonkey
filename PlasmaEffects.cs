using System.Collections.Generic;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace PlasmaEffects;

public class LightningDisplay : ModDisplay
{
    public Dictionary<string, Color> psColor = new()
    {
        { "Glow", new Color(0.7f, 0.1f, 1f, 0.409f) },
        { "Lightning", new Color(0.7f, 0.1f, 1f) },
        { "Pulse", new Color(0.5f, 0.1f, 1f, 0.518f) },
        { "PulseBig", new Color(0.5f, 0.1f, 1f, 0.518f) }
    };

    public override string BaseDisplay => Game.instance.model.GetTowerFromId(TowerType.SuperMonkey + "-050")
        .GetDescendant<CreateEffectOnAbilityModel>().effectModel.assetId.GUID;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
#if DEBUG
        node.PrintInfo();
#endif

        foreach (var ps in node.GetComponentsInChildren<ParticleSystem>())
        {
            ps.startSize *= 0.1f;
            if (psColor.ContainsKey(ps.gameObject.name)) ps.startColor = psColor[ps.gameObject.name];
        }
    }
}

public class WpnPlasmaDisplay : ModDisplay
{
    public Dictionary<string, Color> psColor = new()
    {
        //{ "Glow", new Color(0.7f, 0.1f, 1f, 0.409f) },
        //{ "Lightning", new Color(0.7f, 0.1f, 1f) },
        //{ "Pulse", new Color(0.5f, 0.1f, 1f, 0.518f) },
        //{ "PulseBig", new Color(0.5f, 0.1f, 1f, 0.518f) }
    };

    public override string BaseDisplay => Game.instance.model.GetTowerFromId(TowerType.SuperMonkey + "-040")
        .GetDescendant<CreateEffectOnAbilityModel>().effectModel.assetId.GUID;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
#if DEBUG
        node.PrintInfo();
#endif

        foreach (var ps in node.GetComponentsInChildren<ParticleSystem>())
        {
            ps.startSize *= 0.75f;
            if (psColor.ContainsKey(ps.gameObject.name)) ps.startColor = psColor[ps.gameObject.name];
        }
    }
}

public class WpnNovaDisplay : ModDisplay
{
    public Dictionary<string, Color> psColor = new()
    {
        //{ "Glow", new Color(0.7f, 0.1f, 1f, 0.409f) },
        //{ "Lightning", new Color(0.7f, 0.1f, 1f) },
        //{ "Pulse", new Color(0.5f, 0.1f, 1f, 0.518f) },
        //{ "PulseBig", new Color(0.5f, 0.1f, 1f, 0.518f) }
    };

    public override string BaseDisplay => Game.instance.model.GetTowerFromId(TowerType.SuperMonkey + "-050")
        .GetDescendant<CreateEffectOnAbilityModel>().effectModel.assetId.GUID;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
#if DEBUG
        node.PrintInfo();
#endif

        foreach (var ps in node.GetComponentsInChildren<ParticleSystem>())
        {
            ps.startSize *= 0.75f;
            if (psColor.ContainsKey(ps.gameObject.name)) ps.startColor = psColor[ps.gameObject.name];
        }
    }
}