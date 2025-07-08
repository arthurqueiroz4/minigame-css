using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace Forn;

//TODO - Add more modes: 500HP, Velocity 2.0x/0.5x, Jump, Only HS, Random gun, DECOY 1 DE HP
public partial class FornPlugin : BasePlugin
{
    public override string ModuleName => "FornPlugin";

    public override string ModuleVersion => "0.0.1";

    public override void Load(bool hotReload)
    {
        WriteColor("FornPlugins is [*Loaded*]", ConsoleColor.Green);
        RegisterListener<Listeners.OnTick>(() =>
        {
            var players = Utilities.GetPlayers();
            foreach (var player in players)
                if (GameRules.FreezePeriod && CurrentLabel != null)
                    player.PrintToCenterHtml(
                        $"<font color='gray'>----</font> <font class='fontSize-l' color='green'>Mix dos Crias - FORN</font><font color='gray'>----</font><br>" +
                        $"<font color='gray'>Now playing</font> <font class='fontSize-m' color='green'>[{CurrentLabel}]</font>"
                    );
        });
    }

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        WriteColor("FornPlugin - [*OnRoundStart*] event triggered.", ConsoleColor.Yellow);
        TurnOnMode();
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        WriteColor("FornPlugin - [*OnRoundEnd*] event triggered.", ConsoleColor.Yellow);
        TurnOffMode();
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !IsPlayerAlive(player)) return HookResult.Continue;
        ConstructorWeaponModes[CurrentMode].Invoke([player]);
        return HookResult.Continue;
    }
}