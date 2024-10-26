using Godot;

namespace TowerinSurvivor
{
    [GlobalClass]
    public partial class EnemyData : Resource
    {
        [Export] public int MaxHealth = 10;
        [Export] public float Speed = 5f;
        [Export] public int Damage = 1;
        [Export] public int Dineros;
        [Export] public int Experience;
        [Export] public Texture2D Sprite;
    }
}
