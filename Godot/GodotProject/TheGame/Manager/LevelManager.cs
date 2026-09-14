using Godot;
using System;
using System.Linq;
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
    
    private Wizard m_Wizard;

	public async void StartLevel(string level)
	{
		m_CurrentSceneConfig = m_SceneConfig.DataList.FirstOrDefault(x => x.Level == level);
		if (m_CurrentSceneConfig == null)
		{
			Log.Error($"LevelManager StartLevel failed, level {level} not found in TbSceneConfig");
			return;
		}
        
        m_Wizard = await GF.Entity.ShowEntityAsync<Wizard>(EntityId.Wizard);

		GF.Scene.LoadScene(m_CurrentSceneConfig.AssetPath);
	}
}
