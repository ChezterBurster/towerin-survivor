using Godot;

namespace TowerinSurvivor
{
    public partial class EnemyBehavior : RigidBody2D
    {
        [Export] private int health = 10;
        [Export] private Node2D targetTower;
        [Export] private float speed = 5f;
        [Export] private int damage = 1;
        public int dineros;
        public int experience;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            BodyEntered += HandleBodyEntered;
        }

        public override void _ExitTree()
        {
            BodyEntered -= HandleBodyEntered;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            var movementDirection = (targetTower.GlobalPosition - GlobalPosition).Normalized();
            Position += movementDirection * (float)delta * speed;
        }

        public void InitializeEnemy(EnemyData enemyData, Node2D target)
        {
            health = enemyData.MaxHealth;
            speed = enemyData.Speed;
            damage = enemyData.Damage;
            dineros = enemyData.Dineros;
            experience = enemyData.Experience;
            targetTower = target;
        }

        public void GetDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }

        private void HandleBodyEntered(Node body)
        {
            if (body is not TowerManager tower) return;
            tower.ReceiveDamage(1);
            Die();
        }

        private void Die()
        {
            GameManager.Instance.EmitSignal("EnemyDied", this);
            GetParent().RemoveChild(this);
            QueueFree();
        }
    }
}
