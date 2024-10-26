using System;
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
        private int resetTime = 10;
        private float timer = 0f;

        public override void _EnterTree()
        {
            GlobalPosition = startingPosition;
            BodyEntered += HandleBodyEntered;
        }

        public override void _ExitTree()
        {
            BodyEntered -= HandleBodyEntered;
        }

        private void HandleBodyEntered(Node body)
        {
            if (GetContactCount() == 0) return;
            var enemy = body as EnemyBehavior;
            enemy.GetDamage(damage);
            ResetBullet();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            timer += (float)delta;
            if (timer >= resetTime)
                ResetBullet();
            GlobalPosition += targetDirection * impulse * (float)delta;
        }

        private void ResetBullet()
        {
            towerManager.PushBulletToOwnPool(this, abilityData);
            towerManager.CallDeferred("remove_child", this);
            timer = 0f;
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
