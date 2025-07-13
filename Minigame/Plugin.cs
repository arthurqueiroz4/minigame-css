using CounterStrikeSharp.API.Core;

namespace Minigame;

//TODO - Add more modes: 500HP, Velocity 2.0x/0.5x, Jump, Only HS, Random gun, DECOY 1 DE HP
public partial class Plugin : BasePlugin
{
    public override string ModuleName => "Cria's Minigame";

    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        Orchestrator.Setup(this);
    }
}
