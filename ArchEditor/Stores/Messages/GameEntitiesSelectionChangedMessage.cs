using ArchEditor.Models;

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArchEditor.Stores.Messages;

internal class GameObjectsSelectionChangedMessage : ValueChangedMessage<GameObject>
{
  public GameObjectsSelectionChangedMessage(GameObject value) : base(value)
  {
  }
}
