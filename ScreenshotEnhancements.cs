using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ScreenshotEnhancements
{
	public class ScreenshotEnhancements : Mod
	{
		//TODO: NPC AI Freeze
		//TODO: Save and load data for invsPlayer bool

		public ScreenshotEnhancements()
		{
		}

		internal bool invsPlayer = false;

		internal Mod cheatSheet;

		public override void Load()
		{
			cheatSheet = ModLoader.GetMod("CheatSheet");
		}

		public override void PostSetupContent()
		{
			try
			{
				if (cheatSheet != null)
				{
					//cheatSheet.Call("AddButton_Test", GetTexture("Item_297"), invsPlayer = !invsPlayer, invsPlayer ? "Make the player visibe" : "Make the player invisible");
					CheatSheetButton(cheatSheet);
				}
			}
			catch (Exception e)
			{
				Logger.Warn($"Screenshot Enhancements PostSetupContent Error: {e.StackTrace} {e.Message}");
			}
			invsPlayer = false;
		}

		internal void CheatSheetButton(Mod cheatSheet)
		{
			if (!Main.dedServ)
			{
				CheatSheet.CheatSheetInterface.RegisterButton(cheatSheet, Main.itemTexture[297], CheatSheetButton_Pressed, CheatSheetButton_Tooltip);
			}
		}

		internal string CheatSheetButton_Tooltip()
		{
			return invsPlayer ? "Make the player visibe" : "Make the player invisible";
		}

		internal void CheatSheetButton_Pressed()
		{
			invsPlayer = !invsPlayer;
		}
	}

	public class SHModPlayer : ModPlayer
	{
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			bool config = false;
			if (ModContent.GetInstance<ScreenshotEnhancements>().cheatSheet == null)
			{
				config = true;
			}
			if (ModContent.GetInstance<ScreenshotEnhancements>().invsPlayer || config)
			{
				foreach (PlayerLayer layer in layers)
				{
					layer.visible = false;
				}
			}
		}
	}

	[Label("Clientside Config")]
	public class SHClientsideConfig : ModConfig
	{
		public override ConfigScope Mode
			=> ConfigScope.ClientSide; // per player config


		[Header("Player")]

		[Label("Invisible Player")]
		[Tooltip("If true, the player will be fully invisible. True by default")]
		[DefaultValue(true)]
		public bool InvisiblePlayer;
	}
}