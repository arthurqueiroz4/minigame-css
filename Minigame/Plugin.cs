using System.Numerics;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CS2TraceRay.Class;
using CS2TraceRay.Enum;
using CS2TraceRay.Struct;
using Vector = CounterStrikeSharp.API.Modules.Utils.Vector;
using Minigame.Minigames;

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