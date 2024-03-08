using System.Numerics;
using System.Runtime.Serialization;

namespace ArchEditor.Models;

[DataContract(IsReference = true)]
internal class Transform
{
	[DataMember] public Vector3 Position { get; set; }
	[DataMember] public Vector3 Rotation { get; set; }
	[DataMember] public Vector3 Scale { get; set; }
}
