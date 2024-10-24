using Godot;

namespace TowerinSurvivor
{
    public partial class EnemyBehavior : Node2D
    {
        [Export] private int health = 10;
        [Export] private Node2D mainTower;
        [Export] private float speed = 5f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            var movementDirection = (mainTower.GlobalPosition - GlobalPosition).Normalized();
            Position += movementDirection * (float)delta * speed;
        }

        public void GetDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            var tower = mainTower as TowerManager;
            tower.RemoveTarget();
            GetParent().RemoveChild(this);
        }
    }
}
