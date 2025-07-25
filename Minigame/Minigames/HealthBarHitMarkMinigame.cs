using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.UserMessages;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Collections.Generic;

namespace Minigame.Minigames;

public class HealthBarHitMarkMinigame : IMinigame
{
    public HealthBarHitMarkMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "HealthBar";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
        Plugin.RegisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
        Plugin.DeregisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        return HookResult.Continue;
    }

    private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo info)
    {
        var victim = @event.Userid;
        var attacker = @event.Attacker;
        if (victim == null || !victim.IsValid || attacker == null || !attacker.IsValid) return HookResult.Continue;
        if (!victim.PawnIsAlive) return HookResult.Continue;

        var pawn = victim.PlayerPawn.Value;
        if (pawn == null) return HookResult.Continue;
        float maxHealth = pawn.MaxHealth > 0 ? pawn.MaxHealth : 100;
        float oldHealth = @event.Health + @event.DmgHealth;
        float oldRatio = oldHealth / maxHealth;
        float newRatio = (float)@event.Health / maxHealth;
        var message = UserMessage.FromPartialName("UpdateScreenHealthBar");
        message.SetInt("entidx", (int)pawn.Index);
        message.SetFloat("healthratio_old", oldRatio);
        message.SetFloat("healthratio_new", newRatio);
        message.SetInt("style", 0);
        message.Send(attacker);

        if (@event.Hitgroup == 1)
        {
            attacker.PrintToCenterHtml("<font color='#FF0000'><b>HEADSHOT!</b></font>");
            attacker.ExecuteClientCommand("play buttons\\blip1.wav");
        }
        else
        {
            attacker.ExecuteClientCommand("play buttons\\button3.wav");
        }
        return HookResult.Continue;
    }
}