using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Collections.Generic;
using System.Linq;

namespace Minigame.Minigames;

public class LifeStealMinigame : IMinigame
{
    public LifeStealMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Life Steal";

    private const float LifeMultiplier = 1.0f;
    private const int MaxHealth = 200;
    private readonly Dictionary<CCSPlayerController, float> msgTimer = new();
    private readonly Dictionary<CCSPlayerController, string> dmgMsg = new();

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
        Plugin.RegisterListener<Listeners.OnTick>(OnTick);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler<EventPlayerHurt>(OnPlayerHurt);
        Plugin.RemoveListener<Listeners.OnTick>(OnTick);
    }

    private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo info)
    {
        if (@event.Attacker is CCSPlayerController attacker && @event.Userid is CCSPlayerController victim)
        {
            if (attacker == victim) return HookResult.Continue; 
            if (attacker.TeamNum == victim.TeamNum) return HookResult.Continue; 
            var pawn = attacker.Pawn?.Value;
            if (pawn != null)
            {
                int heal = (int)(@event.DmgHealth * LifeMultiplier);
                int newHealth = pawn.Health + heal;
                if (newHealth > MaxHealth) newHealth = MaxHealth;
                pawn.Health = newHealth;
                StartPlayerMsgTimer(attacker, 1f, $"<font color='#fc4040'>+{heal} HP</font>");
                Server.NextFrame(() => Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth"));
            }
        }
        return HookResult.Continue;
    }

    private void OnTick()
    {
        foreach (var player in Utilities.GetPlayers())
        {
            if (player == null || !player.IsValid || !player.PawnIsAlive)
                continue;
            if (msgTimer.TryGetValue(player, out float timer) && timer > 0f)
            {
                timer -= Server.TickInterval;
                if (timer < 0f) timer = 0f;
                msgTimer[player] = timer;
                if (dmgMsg.TryGetValue(player, out var msg))
                {
                    player.PrintToCenterHtml(msg);
                }
            }
        }
    }

    private void StartPlayerMsgTimer(CCSPlayerController player, float duration, string message)
    {
        if (player == null || !player.IsValid)
            return;
        msgTimer[player] = duration;
        dmgMsg[player] = message;
    }
} 