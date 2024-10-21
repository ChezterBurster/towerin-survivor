using Godot;
using System.Collections.Generic;

public partial class TowerManager : Node
{
	[Export] private TowerData towerData;
	[Export] private AbilityData startingAbility;
	[Export] public Vector2 targetPos;
	private readonly List<AbilityData> abilities = new List<AbilityData>();
	private readonly Dictionary<AbilityData, Stack<Node>> bulletPool = new Dictionary<AbilityData, Stack<Node>>();
	private float tickTimer = 0;
	public int tickCounter = 0;
	[Signal] public delegate void TowerTickEventHandler(Vector2 target);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		abilities.Add(startingAbility);
		InitializeBulletPool();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsTimeToTick((float)delta)) Tick();
	}

	private void InitializeBulletPool()
	{
		//Instantiate a pool of bullets in a list
		foreach (AbilityData ability in abilities)
		{
			Stack<Node> bulletStack = new Stack<Node>();
			bulletPool[ability] = bulletStack;
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
		tickCounter++;
		foreach (AbilityData ability in abilities)
		{
			if (tickCounter % ability.tickRate != 0)
				continue;
			else
				PopBullets(ability);
		}
		EmitSignal(SignalName.TowerTick, targetPos);
	}

	private void PopBullets(AbilityData ability)
	{
		for (int i = 0; i < towerData.Efficiency; i++)
		{
			if (!bulletPool.TryGetValue(ability, out Stack<Node> bulletStack))
				return;
			if (bulletStack.TryPop(out Node bullet))
			{
				var bulletScript = bullet as BulletControlller;
				bulletScript.SubscribeToTowerManager();
			}
			else
			{
				AddBulletToPool(ability);
				bullet = bulletStack.Pop();
				var bulletScript = bullet as BulletControlller;
				bulletScript.SubscribeToTowerManager();
			}
		}
	}

	private void AddBulletToPool(AbilityData ability)
	{
		if (!bulletPool.TryGetValue(ability, out Stack<Node> bulletStack))
			return;
		var bullet = ability.bulletScene.Instantiate();
		bulletStack.Push(bullet);
		var bulletScript = bullet as BulletControlller;
		bulletScript.InitializeBullet(this);
	}

}
