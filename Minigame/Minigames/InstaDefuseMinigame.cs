using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;

namespace Minigame.Minigames;

public class InstaDefuseMinigame : IMinigame
{
    public InstaDefuseMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Insta Defuse";

    private float _bombPlantedTime = float.NaN;

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler<EventBombBegindefuse>(OnBombBeginDefuse);
        Plugin.RegisterEventHandler<EventBombPlanted>(OnBombPlanted);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler<EventBombBegindefuse>(OnBombBeginDefuse);
        Plugin.DeregisterEventHandler<EventBombPlanted>(OnBombPlanted);
    }

    private static CPlantedC4? FindPlantedBomb()
    {
        var plantedBombList = Utilities.FindAllEntitiesByDesignerName<CPlantedC4>("planted_c4").ToList();
        if (plantedBombList.Any())
        {
            return plantedBombList.FirstOrDefault();
        }
        Console.WriteLine($"No planted bomb entities have been found!");
        return null;
    }

    private HookResult OnBombPlanted(EventBombPlanted @event, GameEventInfo info)
    {
        _bombPlantedTime = Server.CurrentTime;
        return HookResult.Continue;
    }

    private static bool TeamHasAlivePlayers(CsTeam team)
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            if (!player.IsValid) continue;
            if (player.Team != team) continue;
            if (!player.PawnIsAlive) continue;
            return true;
        }
        return false;
    }

    private HookResult OnBombBeginDefuse(EventBombBegindefuse @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player != null && player.IsValid && player.PawnIsAlive)
        {
            AttemptInstadefuse(player);
        }
        return HookResult.Continue;
    }

    private void AttemptInstadefuse(CCSPlayerController defuser)
    {
        var plantedBomb = FindPlantedBomb();
        if (plantedBomb == null)
        {
            Console.WriteLine($"Planted bomb is null!");
            return;
        }
        if (plantedBomb.CannotBeDefused)
        {
            return;
        }
        var bombTimeUntilDetonation = plantedBomb.TimerLength - (Server.CurrentTime - _bombPlantedTime);
        var defuseLength = plantedBomb.DefuseLength;
        if (defuseLength != 5 && defuseLength != 10)
        {
            defuseLength = defuser.PawnHasDefuser ? 5.0f : 10.0f;
        }
        var timeLeftAfterDefuse = bombTimeUntilDetonation - defuseLength;
        var bombCanBeDefusedInTime = timeLeftAfterDefuse >= 0.0f;
        if (bombCanBeDefusedInTime)
        {
            Server.NextFrame(() =>
            {
                var bomb = FindPlantedBomb();
                if (bomb == null)
                {
                    Console.WriteLine($"Planted bomb is null!");
                    return;
                }
                bomb.DefuseCountDown = 0;
                Server.PrintToChatAll($"\x04{defuser.PlayerName} Insta defused.");
            });
        }
    }
} 