using Godot;

public partial class BulletControlller : Node
{
	private TowerManager towerManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InitializeBullet(TowerManager manager)
	{
		towerManager = manager;
	}

	public void SubscribeToTowerManager() => towerManager.TowerTick += OnTowerTick;
	public void UnsubscribeToTowerManager() => towerManager.TowerTick -= OnTowerTick;

	private void OnTowerTick(Vector2 target)
	{
		towerManager.AddChild(this);
		UnsubscribeToTowerManager();
		ShootAt(target);
	}

	private void ShootAt(Vector2 target)
	{

	}

}
