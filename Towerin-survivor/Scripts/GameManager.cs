using System.Collections.Generic;
using Godot;

namespace TowerinSurvivor
{
    public partial class GameManager : Node
    {
        [Export] private TowerData[] towersData;
        [Export] private Camera2D camera;
        private readonly Dictionary<Node2D, PackedScene> towerDictionary = new();
        private readonly Dictionary<TowerData, Node2D> towerOptions = new();
        private Node2D towerSelected;
        private TowerData selectedData;

        // Called when the node enters the scene tree for the first time.

        public override void _Ready()
        {
            InitializeTowerDictionaries();
            SetSelection(towersData[0]);
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
    }
}
