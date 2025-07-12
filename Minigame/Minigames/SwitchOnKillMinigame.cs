using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class SwitchOnKill : IMinigame
{
    public BasePlugin Plugin { get; }
    public string Name { get; } = "Switch On Kill";

    public SwitchOnKill(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Pre);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler<EventPlayerDeath>(OnPlayerDeath, HookMode.Pre);
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var player = @event.Userid;
        
        var killedPawn = @event.Userid?.PlayerPawn;
        var killerPawn = @event.Attacker?.PlayerPawn;

        if (killedPawn == null || killerPawn == null)
        {
            return HookResult.Continue;
        }
        Plugin.AddTimer(0.1f, () => killerPawn.Value!.Teleport(killedPawn.Value!.AbsOrigin) );
       
        return HookResult.Continue;
    }
} 