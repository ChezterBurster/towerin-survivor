using Godot;

namespace TowerinSurvivor
{
    public partial class BulletControlller : RigidBody2D
    {
        [Export] private float impulse = -500f;
        private Vector2 targetDirection;
        private Vector2 startingPosition;
        private TowerManager towerManager;
        private AbilityData abilityData;
        private int damage;

        public override void _EnterTree()
        {
            GlobalPosition = startingPosition;
            BodyEntered += OnBodyEntered;
        }

        public override void _ExitTree()
        {

            BodyEntered -= OnBodyEntered;
        }

        private void OnBodyEntered(Node body)
        {
            if (GetContactCount() == 0) return;
            var enemy = body as EnemyBehavior;
            enemy.GetDamage(damage);
            towerManager.PushBulletToOwnPool(this, abilityData);
            towerManager.CallDeferred("remove_child", this);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (!IsInsideTree()) return;
            GlobalPosition += targetDirection * impulse * (float)delta;
        }

        public void InitializeBullet(TowerManager manager, TowerData towerData, AbilityData abilityData)
        {
            towerManager = manager;
            this.abilityData = abilityData;
            startingPosition = towerManager.GlobalPosition;
            damage = towerData.Efficiency;
            towerManager.RemoveChild(this);
        }

        public void OnTowerTick(Vector2 target)
        {
            targetDirection = (startingPosition - target).Normalized();
            towerManager.AddChild(this);
        }
    }
}
