using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotExtensions.scripts.utils;
using JayStation.prefabs._3d.islands.managers;

namespace JayStation.levels.managers;

public partial class ManagerProvider : Node {
	[Signal]
	public delegate void OnManagerRegisteredEventHandler(Manager manager);

	readonly List<Manager> managers = new();
	protected Log log;

	public override void _EnterTree() {
		log = Log.From(GetScript());

		// WARNING: spagetti
		GetParent<Level>().RegisterManagerProvider(this);
	}

	public override void _Ready() {
		foreach (Node node in GetChildren()) {
			if (node is not Manager manager) {
				log.Warn("Node " + node.GetPath() + " is not a manager!");
				continue;
			}

			RegisterManager(manager);
		}
	}

	public bool HasManager<T>() where T : Manager {
		return managers.Exists(manager => manager is T);
	}

	// returns manager if it exists, otherwise waits for it to be registered
	public Task<T> WaitForManager<T>() where T : Manager {
		if (HasManager<T>())
			return Task.FromResult(GetManager<T>());

		TaskCompletionSource<T> tcs = new();

		OnManagerRegistered += ManagerRegistered;
		return tcs.Task;

		void ManagerRegistered(Manager manager) {
			if (manager is not T t)
				return;

			tcs.SetResult(t);
			OnManagerRegistered -= ManagerRegistered;
		}
	}

	public T GetManager<T>() where T : Manager {
		return (T)managers.Find(manager => manager is T);
	}

	public void RegisterManager(Manager manager) {
		if (managers.Contains(manager))
			log.Error("Manager " + manager + " is already registered!");
		else {
			managers.Add(manager);
			EmitSignal(SignalName.OnManagerRegistered, manager);
		}
	}
}