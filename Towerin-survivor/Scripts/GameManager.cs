using System.Collections.Generic;
using Godot;

namespace TowerinSurvivor
{
    public partial class GameManager : Node
    {
        //Public Variables
        public List<Node2D> Towers = new();
        //Private Variables
        [Export] private TowerData[] towersData;
        [Export] private Camera2D camera;
        private GameManager() { }
        public static GameManager Instance { get; private set; }
        private readonly Dictionary<Node2D, PackedScene> towerDictionary = new();
        private readonly Dictionary<TowerData, Node2D> towerOptions = new();
        private Node2D towerSelected;
        private TowerData selectedData;

        //Currency and Stats
        [Export] private int dineros;
        [Export] private int experience;
        [Export] private int life;

        //Custom Signals
        [Signal] public delegate void TowerDestroyedEventHandler();
        [Signal] public delegate void EnemyDiedEventHandler(Node2D enemy);

        private void Singleton()
        {
            if (Instance == null)
                Instance = this;
            else
                Dispose();
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Singleton();
            InitializeTowerDictionaries();
            SetSelection(towersData[0]);
            TowerDestroyed += HandleTowerDestroyed;
            EnemyDied += HandleEnemyDied;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (towerSelected == null) return;
            towerSelected.GlobalPosition = camera.GetGlobalMousePosition();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("BuildTower"))
            {
                BuildTower();
            }
        }

        private void InitializeTowerDictionaries()
        {
            for (var i = 0; i < towersData.Length; i++)
            {
                var towerOption = towersData[i].TowerPrevisual.Instantiate() as Node2D;
                towerOptions.Add(towersData[i], towerOption);
                towerDictionary.Add(towerOption, towersData[i].TowerPrefab);
            }
        }

        private void BuildTower()
        {
            if (towerDictionary.TryGetValue(towerSelected, out var newTower))
            {
                var towerNode = newTower.Instantiate() as Node2D;
                towerNode.GlobalPosition = towerSelected.GlobalPosition;
                (towerNode.GetNode("TowerManager") as TowerManager).SetTowerData(selectedData);
                AddChild(towerNode);
                Towers.Add(towerNode);
            }
        }

        public void SetSelection(TowerData towerData)
        {
            if (towerOptions.TryGetValue(towerData, out var Option))
            {
                towerSelected = Option;
                AddChild(towerSelected);
                selectedData = towerData;
            }
        }

        private void HandleTowerDestroyed()
        {
            life--;
        }

        private void HandleEnemyDied(Node2D enemy)
        {
            var deadEnemy = enemy as EnemyBehavior;
            dineros += deadEnemy.dineros;
            experience += deadEnemy.experience;
        }

        public Node2D GetClosestTower(Vector2 position)
        {
            var closestposition = new Vector2(1000, 1000);
            Node2D closestNode = Towers[0];
            foreach (var tower in Towers)
            {
                var reference = position - tower.GlobalPosition;
                if (reference < closestposition)
                {
                    closestposition = tower.GlobalPosition;
                    closestNode = tower;
                }
            }
            return closestNode;
        }
    }
}
