using Godot;
using GameConfig.Actor;

namespace GameLogic
{
    public partial class ActorBase : CharacterBody2D
	{
		protected ActorConfig m_Config;
        
		public ActorData ActorData { get; protected set; }
        
        protected PhysicsCheck2D m_PhysicsCheck;
	}
}
