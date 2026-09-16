using GameFramework.Event;

namespace GameLogic
{
    public class OnActorDataChangeEventArgs : GameEventArgs
	{
		public static readonly int EventId = typeof(OnActorDataChangeEventArgs).GetHashCode();

		public override int Id => EventId;

		public ActorData ActorData { get; private set; }

		public static OnActorDataChangeEventArgs Create(ActorData actorData)
		{
			var eventArgs = new OnActorDataChangeEventArgs();
			eventArgs.ActorData = actorData;
			return eventArgs;
		}

		public override void Clear()
		{
			ActorData = null;
		}
	}
}
