using Godot;
using GameConfig.Actor;
using GodotGameFramework;

namespace GameLogic
{
	public class ActorData
	{
		public int ActorId { get; set; }

		public float MaxHp
		{
			get;
			set
			{
				field = value;
				GF.Event.Fire(this, OnActorDataChangeEventArgs.Create(this));
			}
		}

		public float CurHp
		{
			get;
			set
			{
				field = Mathf.Clamp(value, 0, MaxHp);
				if (value <= 0)
				{
					// todo: 触发死亡事件
				}

				GF.Event.Fire(this, OnActorDataChangeEventArgs.Create(this));
			}
		}

		public float MaxAttack
		{
			get;
			set
			{
				field = value;
				GF.Event.Fire(this, OnActorDataChangeEventArgs.Create(this));
			}
		}

		public float CurAttack
		{
			get;
			set
			{
				field = value;
				GF.Event.Fire(this, OnActorDataChangeEventArgs.Create(this));
			}
		}

		public float MaxAttackRange
		{
			get;
			set
			{
				field = value;
				GF.Event.Fire(this, OnActorDataChangeEventArgs.Create(this));
			}
		}

		public float CurAttackRange
		{
			get;
			set
			{
				field = value;
				GF.Event.Fire(this, OnActorDataChangeEventArgs.Create(this));
			}
		}

		public ActorData(int id, ActorConfig config)
		{
			ActorId = id;
			CurHp = MaxHp = config.Hp;
			MaxAttack = CurAttack = config.Attack;
			MaxAttackRange = CurAttackRange = config.AttackRange;
		}
	}
}
