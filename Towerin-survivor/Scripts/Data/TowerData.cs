using Godot;

[GlobalClass]
public partial class TowerData : Resource
{
    [Export] public int MaxHealth { get; private set; } = 1;
    [Export] public int ViewRange { get; private set; } = 1;
    [Export] public int BuildingCost { get; private set; } = 1;
    [Export] public int TickRate { get; private set; } = 1;
    [Export] public int Efficiency { get; private set; } = 1;
}
