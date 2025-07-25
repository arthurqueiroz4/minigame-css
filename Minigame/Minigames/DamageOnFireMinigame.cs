using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class DamageOnFireMinigame : IMinigame
{
    public DamageOnFireMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Damage On Fire";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler(WeaponFireHandler);
    }
    public void Unregister()
    {
        Plugin.DeregisterEventHandler(WeaponFireHandler);
    }

    private BasePlugin.GameEventHandler<EventWeaponFire> WeaponFireHandler =>
        (@event, info) =>
        {
            var player = @event.Userid;
            if (player == null || player.PlayerPawn.Value == null)
                return HookResult.Continue;

            var pawn = player.PlayerPawn.Value;
            var weapon = pawn.WeaponServices?.ActiveWeapon?.Value;

            if (weapon == null)
                return HookResult.Continue;

            var clipAmmo = weapon.Clip1;
            var designerName = weapon.DesignerName;
            var maxClipDict = new Dictionary<string, int>
            {
                { "weapon_glock", 20 },
                { "weapon_usp_silencer", 12 },
                { "weapon_p250", 13 },
                { "weapon_deagle", 7 },
                { "weapon_fiveseven", 20 },
                { "weapon_elite", 30 },
                { "weapon_tec9", 18 },
                { "weapon_cz75a", 12 },
                { "weapon_revolver", 8 },
                { "weapon_hkp2000", 13 },
                { "weapon_p228", 13 },
                { "weapon_mac10", 30 },
                { "weapon_mp9", 30 },
                { "weapon_mp7", 30 },
                { "weapon_mp5sd", 30 },
                { "weapon_ump45", 25 },
                { "weapon_p90", 50 },
                { "weapon_bizon", 64 },
                { "weapon_galilar", 35 },
                { "weapon_famas", 25 },
                { "weapon_m4a1", 30 },
                { "weapon_m4a1_silencer", 20 },
                { "weapon_ak47", 30 },
                { "weapon_sg556", 30 },
                { "weapon_aug", 30 },
                { "weapon_ssg08", 10 },
                { "weapon_awp", 5 },
                { "weapon_g3sg1", 20 },
                { "weapon_scar20", 20 },
                { "weapon_nova", 8 },
                { "weapon_xm1014", 7 },
                { "weapon_mag7", 5 },
                { "weapon_sawedoff", 7 },
                { "weapon_m249", 100 },
                { "weapon_negev", 150 }
            };
            int maxClip = maxClipDict.TryGetValue(designerName, out var val) ? val : 30;
            
            double percentUsed = 1.0 - ((double)clipAmmo / maxClip);
            int damage = (int)Math.Round(percentUsed * 30);
            if (damage > 0)
            {
                pawn.Health = pawn.Health - damage < 1 ? 1 : pawn.Health - damage;
                Server.NextFrame(() => Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth"));
            }

            return HookResult.Continue;
        };
}
