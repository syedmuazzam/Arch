using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace ArchEditor.Models;

// Inheriting from INotifyPropertyChanged instead of ObservableObject because of the serialization
[DataContract(IsReference = true)]
internal class GameObject : INotifyPropertyChanged
{
	[DataMember(Name = nameof(Name))]
	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}

	}
	public Scene ParentScene { get; set; }
	[DataMember] public Transform Transform { get; set; }
	[DataMember(Name = nameof(Components))] private readonly ObservableCollection<Component> _components = new();
	public ReadOnlyObservableCollection<Component> Components { get; private set; }

	public GameObject(string name, Scene scene)
	{
		Debug.Assert(scene is not null);
		_name = name;
		ParentScene = scene;
		Transform = new Transform();
		Components = new ReadOnlyObservableCollection<Component>(_components);
	}

	[OnDeserialized]
	// ReSharper disable once UnusedParameter.Local
	private void OnDeserialized(StreamingContext context)
	{
		Components = new ReadOnlyObservableCollection<Component>(_components);
	}

	public void AddComponent()
	{
		_components.Add(new Component(this));
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
