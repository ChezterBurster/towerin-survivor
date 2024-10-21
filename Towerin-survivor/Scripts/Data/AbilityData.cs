using Godot;

[GlobalClass]
public partial class AbilityData : Resource
{
    [Export] public PackedScene bulletScene;
    [Export] public int tickRate;
}
