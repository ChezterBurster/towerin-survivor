using Godot;

namespace TowerinSurvivor
{
    [GlobalClass]
    public partial class WaveData : Resource
    {
        [Export] public PackedScene enemyPrefab;
        [Export] public EnemyData normalEnemy;
        [Export] public EnemyData eliteEnemy;
        [Export] public int TimeBetweenWaves = 10;
        [Export] public int EnemiesPerWave = 1;
        [Export] public int EliteIncrement = 1;
        [Export] public int EliteInterval = 3;
    }
}
