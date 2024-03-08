using System.Diagnostics;
using System.Runtime.Serialization;

namespace ArchEditor.Models;

[DataContract]
internal class Component
{
	[DataMember] public GameObject ParentGameObject { get; private set; }

	public Component(GameObject parentGameObject)
	{
		Debug.Assert(parentGameObject is not null);
		ParentGameObject = parentGameObject;
	}
}
