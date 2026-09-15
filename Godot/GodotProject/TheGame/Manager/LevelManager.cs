using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using GameConfig.Scene;
using GameLogic;
using GodotGameFramework;
using GodotGameFrameworkCore.SingletonSystem;
using GameConfig.Entity;
using GodotGameFramework.Entity;

public partial class LevelManager : SingletonNode<LevelManager>
{
	private TbSceneConfig m_SceneConfig => ConfigSystem.Instance.Tables.TbSceneConfig;

	private SceneConfig m_CurrentSceneConfig;
    
    public Wizard Wizard { get; private set; }

	public async Task StartLevel(string level)
	{
		m_CurrentSceneConfig = m_SceneConfig.DataList.FirstOrDefault(x => x.Level == level);
		if (m_CurrentSceneConfig == null)
		{
			Log.Error($"LevelManager StartLevel failed, level {level} not found in TbSceneConfig");
			return;
		}
        
        Wizard = await GF.Entity.ShowEntityAsync<Wizard>(EntityId.Wizard);

		GF.Scene.LoadScene(m_CurrentSceneConfig.AssetPath);
	}
}
