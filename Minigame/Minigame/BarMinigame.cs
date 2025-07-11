using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Forn.Minigame;

public class BarMinigame : IMinigame
{
    public BarMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Bar Minigame";
    private static BasePlugin.GameEventHandler<EventWeaponFire> WeaponFireHandler => (@event, info) =>
    {
        Server.PrintToChatAll("Mata o cara ma!");
        return HookResult.Continue;
    };

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler(WeaponFireHandler, HookMode.Pre);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler(WeaponFireHandler, HookMode.Pre);
    }
}