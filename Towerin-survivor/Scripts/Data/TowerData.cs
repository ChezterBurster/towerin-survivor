using Godot;

namespace TowerinSurvivor
{
    [GlobalClass]
    public partial class TowerData : Resource
    {
        [Export] public PackedScene TowerPrefab;
        [Export] public PackedScene TowerPrevisual;
        [Export] public int MaxHealth { get; private set; } = 1;
        [Export] public int ViewRange { get; private set; } = 1;
        [Export] public int BuildingCost { get; private set; } = 1;
        [Export] public int TickRate { get; private set; } = 1;
        [Export] public int Efficiency { get; private set; } = 1;
    }
}
