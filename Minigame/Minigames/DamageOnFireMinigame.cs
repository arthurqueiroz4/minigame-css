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

    //TODO: 
    //Capturar o evento de tiro;
    //Calcular a quantidade de balas que a arma possui e calcular o dano baseado na quantidade de balas;
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

            // var clipAmmo = weapon.Clip1;
            // var reserveAmmo = weapon.ReserveAmmo[0];
            // int maxClip = reserveAmmo + clipAmmo;
            // int bulletsUsed = maxClip - clipAmmo;
            //
            // double percentUsed = (double)bulletsUsed / reserveAmmo;
            // int damage = (int)Math.Round(percentUsed * 30);
            // pawn.Health = pawn.Health - damage < 0 ? 1 : pawn.Health - damage;
            //
            pawn.Health -= 5;

            Server.PrintToChatAll($"Health {pawn.Health} ");
            return HookResult.Continue;
        };
}
