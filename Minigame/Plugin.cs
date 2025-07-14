using CounterStrikeSharp.API.Core;
using Minigame;
using Minigame.Minigames;

namespace Forn;

public partial class Plugin : BasePlugin
{
    public override string ModuleName => "Cria's Minigame";

    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        Orchestrator.Setup(this, new OnlyGrenadeHEMinigame(this));
    }
}
