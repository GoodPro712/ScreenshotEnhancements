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
		//TODO: HERO's mod support
		//TODO: Projectile AI Freeze
		//TODO: NPC AI Freeze
		//TODO: Save and load data for invsPlayer bool

		public ScreenshotEnhancements()
		{
		}

		internal bool cheatSheet_invsPlayer = false;
		internal bool HEROsMod_invsPlayer = false;
		internal bool config_invsPlayer = false;

		internal Mod cheatSheet;
		internal Mod HEROsMod;

		public bool CheatSheetLoaded = false;
		internal bool HEROsModLoaded = false;
		internal bool useConfig = false;

		public override void Load()
		{
			cheatSheet = ModLoader.GetMod("CheatSheet");
			HEROsMod = ModLoader.GetMod("HEROsMod");
		}

		public override void PostSetupContent()
		{
			try
			{
				if (cheatSheet != null)
				{
					CheatSheetLoaded = true;
					CheatSheetButton(cheatSheet);
				}
				else if (HEROsMod != null)
				{
					HEROsModLoaded = true;
					//HEROsMod Button
				}
				else
				{
					useConfig = true;
				}
			}
			catch (Exception e)
			{
				Logger.Warn($"Screenshot Enhancements PostSetupContent Error: {e.StackTrace} {e.Message}");
			}
			cheatSheet_invsPlayer = false;
			HEROsMod_invsPlayer = false;
			config_invsPlayer = false;
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
			return cheatSheet_invsPlayer ? "Make the player visibe" : "Make the player invisible";
		}

		internal void CheatSheetButton_Pressed()
		{
			cheatSheet_invsPlayer = !cheatSheet_invsPlayer;
		}
	}

	public class SHModPlayer : ModPlayer
	{
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if (ModContent.GetInstance<ScreenshotEnhancements>().CheatSheetLoaded)
			{
				if (ModContent.GetInstance<ScreenshotEnhancements>().cheatSheet_invsPlayer)
				{
					foreach (PlayerLayer layer in layers)
					{
						layer.visible = false;
					}
				}
			}
			else if (ModContent.GetInstance<ScreenshotEnhancements>().HEROsModLoaded)
			{
				if (ModContent.GetInstance<ScreenshotEnhancements>().HEROsMod_invsPlayer)
				{
					foreach (PlayerLayer layer in layers)
					{
						layer.visible = false;
					}
				}
			}
			else if (ModContent.GetInstance<ScreenshotEnhancements>().useConfig)
			{
				if (ModContent.GetInstance<ScreenshotEnhancements>().config_invsPlayer)
				{
					foreach (PlayerLayer layer in layers)
					{
						layer.visible = false;
					}
				}
			}
		}
	}

	[Label("Clientside Config")]
	public class SHClientsideConfig : ModConfig
	{
		public override ConfigScope Mode
			=> ConfigScope.ClientSide; // per player config

		[Label("Invisible Player")]
		[Tooltip("If true, the player will be fully invisible. True by default")]
		[DefaultValue(true)]
		public bool InvisiblePlayer;
	}
}