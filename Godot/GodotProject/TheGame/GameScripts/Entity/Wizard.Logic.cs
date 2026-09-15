using GameFramework.UI;
using Godot;
using GodotGameFramework.UI;
using System;
using GameFramework.Entity;
using System.Linq;
using GameConfig.Entity;
using GameConfig.Actor;
using GameFramework.Event;
using GameFramework.Fsm;
using GodotGameFramework;

namespace GameLogic
{
    public class ActorData
	{
        public bool IsPlayer { get; set; }
        
		public float MaxHp { get; set; }

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

		public float MaxAttack { get; set; }
		public float CurAttack { get; set; }
		public float MaxAttackRange { get; set; }
		public float CurAttackRange { get; set; }

		public ActorData(ActorConfig config, bool isPlayer)
		{
			IsPlayer = isPlayer;
			CurHp = MaxHp = config.Hp;
			MaxAttack = CurAttack = config.Attack;
			MaxAttackRange = CurAttackRange = config.AttackRange;
		}
	}
    
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
    
	/// <summary>
	/// 界面逻辑（此文件仅在首次生成时创建，之后不会被覆盖）。
	/// </summary>
	public partial class Wizard
	{
        public class IdleState : FsmState<Wizard>
		{
			protected internal override void OnInit(IFsm<Wizard> fsm)
			{
				base.OnInit(fsm);
			}

			protected internal override void OnEnter(IFsm<Wizard> fsm)
			{
				base.OnEnter(fsm);
                fsm.Owner.Anim.Play("Idle");
			}

			protected internal override void OnUpdate(IFsm<Wizard> fsm, float elapseSeconds, float realElapseSeconds)
			{
				base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (fsm.Owner.IsMoving)
				{
					ChangeState<MoveState>(fsm);
				}
			}
		}

		public class MoveState : FsmState<Wizard>
		{
			protected internal override void OnInit(IFsm<Wizard> fsm)
			{
				base.OnInit(fsm);
			}
            
            protected internal override void OnEnter(IFsm<Wizard> fsm)
            {
	            base.OnEnter(fsm);
	            fsm.Owner.Anim.Play("Move");
            }

			protected internal override void OnUpdate(IFsm<Wizard> fsm, float elapseSeconds, float realElapseSeconds)
			{
				base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (!fsm.Owner.IsMoving)
				{
					ChangeState<IdleState>(fsm);
				}
			}
		}

		private ActorConfig m_Config;
        
        private Fsm<Wizard> m_Fsm;

        public bool IsMoving { get; set; }

        public AnimationPlayer Anim => m_AnimationPlayer;

        public ActorData ActorData { get; private set; }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance, object userData)
		{
			#region 框架逻辑
			Id = entityId;
			EntityAssetName = entityAssetName;
			Name = GameFramework.Utility.Text.Format("Entity_{0}_{1}", entityId, entityAssetName);
			EntityGroup = entityGroup;
			#endregion
			if(isNewInstance)
			{
				#region 界面逻辑
				m_Config = ConfigSystem.Instance.Tables.TbActorConfig.DataList.FirstOrDefault(x => x.EntityId == EntityId.Wizard);
                m_Fsm = (Fsm<Wizard>)GF.Fsm.CreateFsm<Wizard>(this, new IdleState(), new MoveState());

                ActorData = new ActorData(m_Config, true);

                #endregion
			}
		}

		/// <summary>
		/// 实体回收。
		/// Entity 节点不销毁，等待对象池复用或池释放。
		/// </summary>
		public void OnRecycle()
		{
			Id = 0;
			EntityAssetName = null;
			Name = "Entity (Recycled)";
			Visible = false;
		}

		/// <summary>
		/// 实体显示。
		/// </summary>
		public void OnShow(object userData)
		{
			Visible = true;
            m_Fsm.Start<IdleState>();
		}

		/// <summary>
		/// 实体隐藏。
		/// </summary>
		public void OnHide(bool isShutdown, object userData)
		{
			Visible = false;
		}

		/// <summary>
		/// 实体附加子实体。
		/// </summary>
		public void OnAttached(IEntity childEntity, object userData)
		{

		}

		/// <summary>
		/// 实体解除子实体。
		/// </summary>
		public void OnDetached(IEntity childEntity, object userData)
		{

		}

		/// <summary>
		/// 实体被附加到父实体。
		/// </summary>
		public void OnAttachTo(IEntity parentEntity, object userData)
		{

		}

		/// <summary>
		/// 实体从父实体解除。
		/// </summary>
		public void OnDetachFrom(IEntity parentEntity, object userData)
		{

		}

		/// <summary>
		/// 实体轮询。
		/// 每帧调用，用于处理实体逻辑。
		/// </summary>
		public void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
            KeyBoardMove();

			// 模拟测试扣血
            if (Input.IsActionJustPressed("ui_accept"))
            {
                ActorData.CurHp -= 10;
            }
		}

		private void KeyBoardMove()
		{
			var inputVec = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            Velocity = inputVec * m_Config.MoveSpeed;
            MoveAndSlide();
            
            IsMoving = inputVec != Vector2.Zero;
            if (IsMoving)
			{
				m_Sprite2D.FlipH = Velocity.X < 0;
			}
		}
	}
}
