using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotExtensions.scripts.utils;

namespace JayStation.scripts;

public partial class ServiceProvider : Node {
	[Signal]
	public delegate void OnServiceRegisteredEventHandler(Service service);

	readonly List<Service> services = new();
	protected Log log;

	public override void _EnterTree() {
		log = Log.From(GetScript());
	}

	public override void _Ready() {
		foreach (Node node in GetChildren()) {
			if (node is not Service service) {
				log.Error("Node " + node.GetPath() + " is not a service!");
				continue;
			}

			RegisterService(service);
		}
	}

	public bool HasService<T>() where T : Service {
		return services.Exists(service => service is T);
	}

	// returns service if it exists, otherwise waits for it to be registered
	public Task<T> WaitForService<T>() where T : Service {
		if (HasService<T>())
			return Task.FromResult(GetService<T>());

		TaskCompletionSource<T> tcs = new();

		OnServiceRegistered += ServiceRegistered;
		return tcs.Task;

		void ServiceRegistered(Service service) {
			if (service is not T t)
				return;

			tcs.SetResult(t);
			OnServiceRegistered -= ServiceRegistered;
		}
	}

	public T GetService<T>() where T : Service {
		return (T)services.Find(service => service is T);
	}

	public void RegisterService(Service service) {
		if (services.Contains(service))
			log.Error("Service " + service + " is already registered!");
		else {
			services.Add(service);
			EmitSignal(SignalName.OnServiceRegistered, service);
		}
	}
}