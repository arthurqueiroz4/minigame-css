using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Forn;

public enum Mode
{
    OnlyP90,
    OnlyAwp,
    OnlyDeagle,
    OnlyP250,
}

public partial class FornPlugin
{
    static string? CurrentLabel { get; set; }
    static Mode CurrentMode { get; set; }

    Dictionary<Mode, Action<List<CCSPlayerController>>> Modes { get; } = new()
    {
        {
            Mode.OnlyP90, players =>
            {
                CurrentLabel = "Only P90 Mode";
                CreateWeaponsOnlyMode([WeaponType.P90, WeaponType.Knife], players);
            }
        },
        {
            Mode.OnlyAwp, players =>
            {
                CurrentLabel = "Only Awp Mode";
                CreateWeaponsOnlyMode([WeaponType.Awp, WeaponType.Knife], players);
            }
        },
        {
            Mode.OnlyDeagle, players =>
            {
                CurrentLabel = "Only Deagle Mode";
                CreateWeaponsOnlyMode([WeaponType.Deagle, WeaponType.Knife], players);
            }
        },
        {
            Mode.OnlyP250, players =>
            {
                CurrentLabel = "Only P250 Mode";
                CreateWeaponsOnlyMode([WeaponType.P250, WeaponType.Knife], players);
            }
        }
    };

    void ChoiceMode()
    {
        var players = Utilities.GetPlayers();
        if (players.Count == 0) return;

        var modes = Enum.GetValues<Mode>();
        var choice = modes[new Random().Next(modes.Length)];
        Modes[choice].Invoke(players);
        foreach (var player in players)
        {
            player.PrintToChat($"Current Mode: {CurrentLabel}");
        }
    }
}