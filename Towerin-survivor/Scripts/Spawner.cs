using Godot;

namespace TowerinSurvivor
{
    public partial class Spawner : Node2D
    {
        [Export] private WaveData waveData;

        private int waveCount = 0;

        public override void _Ready()
        {
            var timer = GetNode("WaveTimer") as Timer;
            timer.WaitTime = waveData.TimeBetweenWaves;
            timer.Start();
        }

        public override void _Process(double delta)
        {
        }

        private void OnWaveTimerTimeout()
        {
            HandleWave();
        }

        private async void HandleWave()
        {
            waveCount++;
            var closestTower = GameManager.Instance.GetClosestTower(GlobalPosition);
            var enemyTimer = GetNode("EnemyTimer") as Timer;
            enemyTimer.Start();
            for (int i = 0; i < waveCount * waveData.EnemiesPerWave; i++)
            {
                SpawnEnemy(waveData.normalEnemy, closestTower);
                await ToSignal(enemyTimer, "timeout");
            }
            if (waveCount % waveData.EliteInterval == 0)
            {
                var eliteCount = waveCount / waveData.EliteInterval * waveData.EliteIncrement;
                for (int ii = 0; ii < eliteCount; ii++)
                {
                    SpawnEnemy(waveData.eliteEnemy, closestTower);
                    await ToSignal(enemyTimer, "timeout");
                }
            }
            enemyTimer.Stop();
        }

        public void SpawnEnemy(EnemyData enemyData, Node2D target)
        {
            var enemy = waveData.enemyPrefab.Instantiate() as EnemyBehavior;
            var sprite2d = enemy.GetNode("Sprite2D") as Sprite2D;
            sprite2d.Texture = enemyData.Sprite;
            enemy.GlobalPosition = GlobalPosition;
            enemy.InitializeEnemy(enemyData, target);
            GetParent().AddChild(enemy);
        }


    }
}
