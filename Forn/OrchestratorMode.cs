using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Forn;

public enum Mode
{
    OnlyP90,
    OnlyAwp,
    OnlyDeagle,
    OnlyP250,
    SpeedVelocity,
    SlowVelocity,
    HighJump,
    IncreasedHp
}

public partial class FornPlugin
{
    private static string? CurrentLabel { get; set; }
    private static Mode CurrentMode { get; set; }
    private Dictionary<Mode, Action<List<CCSPlayerController>>> AllConstructorModes = new();
    private Dictionary<Mode, Action<List<CCSPlayerController>>> DestructorModes { get; } = new()
    {
        {
            Mode.HighJump, _ => { TurnOffHighJumpMode(); }
        },
        {
            Mode.IncreasedHp, TurnOffIncreasedHpMode
        },
        {
            Mode.SpeedVelocity, TurnOffVelocityMode
        },
        {
            Mode.SlowVelocity, TurnOffVelocityMode
        }
    };

    private void TurnOnMode()
    {
        var players = Utilities.GetPlayers();
        if (players.Count == 0) return;

        var modes = Enum.GetValues<Mode>();
        var choice = modes[new Random().Next(modes.Length)];
        CurrentMode = choice;

        AllConstructorModes
            .Concat(ConstructorWeaponModes)
            .Concat(ConstructorStateModes)
            .ToDictionary();
        
        AllConstructorModes[choice].Invoke(players);
        Server.PrintToChatAll($"Current Mode: {CurrentLabel}");
    }

    private void TurnOffMode()
    {
        var players = Utilities.GetPlayers();
        if (players.Count == 0) return;
        DestructorModes[CurrentMode].Invoke(players);
    }
}