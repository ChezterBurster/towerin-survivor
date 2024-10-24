using Godot;
using System.Collections.Generic;

namespace TowerinSurvivor
{
    public partial class TowerManager : Area2D
    {
        [Export] private AbilityData startingAbility;
        private TowerData towerData;
        private Vector2 targetPos;
        private Node2D Target;
        private readonly List<AbilityData> abilities = new();
        private readonly Dictionary<AbilityData, Stack<Node>> bulletPool = new();
        public int tickCounter = 0;
        private float tickTimer = 0;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            abilities.Add(startingAbility);
            InitializeBulletPool();
        }

        public void SetTowerData(TowerData towerData)
        {
            this.towerData = towerData;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (Target == null)
            {
                var bodies = GetOverlappingBodies();
                if (bodies.Count > 0)
                    Target = bodies[0];
            }
            else
            {
                if (IsTimeToTick((float)delta))
                    Tick();
            }
        }

        //Crea una pool de balas para cada abilidad que tenga la torre
        private void InitializeBulletPool()
        {
            //Instantiate a pool of bullets in a list
            foreach (AbilityData ability in abilities)
            {
                bulletPool[ability] = new Stack<Node>();
                for (int i = 0; i < towerData.Efficiency; i++)
                {
                    AddBulletToPool(ability);
                }
            }
        }

        private bool IsTimeToTick(float delta)
        {
            tickTimer += delta;
            var tickRate = 1 / towerData.TickRate;
            if (tickTimer >= tickRate)
            {
                tickTimer = 0;
                return true;
            }
            return false;
        }

        private void Tick()
        {
            targetPos = Target.GlobalPosition;
            tickCounter++;
            foreach (AbilityData ability in abilities)
            {
                if (tickCounter % ability.tickRate != 0)
                    continue;
                else
                    PopBullets(ability, targetPos);
            }
        }

        private void PopBullets(AbilityData ability, Vector2 targetPos)
        {
            for (var i = 0; i < towerData.Efficiency; i++)
            {
                BulletControlller bulletScript;
                if (!bulletPool.TryGetValue(ability, out Stack<Node> bulletStack))
                    return;
                if (bulletStack.TryPop(out Node bullet))
                {
                    bulletScript = bullet as BulletControlller;
                }
                else
                {
                    AddBulletToPool(ability);
                    bullet = bulletStack.Pop();
                    bulletScript = bullet as BulletControlller;
                }
                bulletScript.OnTowerTick(targetPos);
            }
        }

        private void AddBulletToPool(AbilityData ability)
        {
            if (!bulletPool.TryGetValue(ability, out Stack<Node> bulletStack))
                return;
            var bullet = ability.bulletScene.Instantiate();
            bulletStack.Push(bullet);
            AddChild(bullet);
            var bulletScript = bullet as BulletControlller;
            bulletScript.InitializeBullet(this, towerData, ability);
        }

        public void PushBulletToOwnPool(Node bullet, AbilityData ability)
        {
            bulletPool.TryGetValue(ability, out Stack<Node> bulletStack);
            bulletStack.Push(bullet);
        }

        public void RemoveTarget()
        {
            Target = null;
        }
    }
}
