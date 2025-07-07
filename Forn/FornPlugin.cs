using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace Forn.CSSharp;

using CounterStrikeSharp.API.Core;

public partial class FornPlugin : BasePlugin
{
    public override string ModuleName => "FornPlugin";

    public override string ModuleVersion => "0.0.1";

    // bool DebugMode = false;


    public override void Load(bool hotReload)
    {
        WriteColor("FornPlugins is [*Loaded*]", ConsoleColor.Green);
        RegisterListener<Listeners.OnTick>(() =>
        {
            var players = Utilities.GetPlayers();
            foreach (var player in players)
            {
                if (GameRules.FreezePeriod && CurrentLabel != null)
                {
                    player.PrintToCenterHtml(
                        $"<font color='gray'>----</font> <font class='fontSize-l' color='green'>Mix dos Crias - FORN</font><font color='gray'>----</font><br>" +
                        $"<font color='gray'>Now playing</font> <font class='fontSize-m' color='green'>[{CurrentLabel}]</font>"
                    );
                }
            }
        });
    }

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        WriteColor("FornPlugin - [*OnRoundStart*] event triggered.", ConsoleColor.Yellow);
        OnlyP90Mode();
        return HookResult.Continue;
    }
}