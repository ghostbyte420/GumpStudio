using System;
using System.Windows.Forms;

using GumpStudio.Elements;
using GumpStudio.Forms;

namespace GumpStudio.Plugins
{
	public abstract class BasePlugin
	{
		public abstract PluginInfo Info { get; }

		public abstract BaseConfig Config { get; }

		public virtual string Name => Info?.Name;

		public bool IsLoaded { get; private set; }

		public DesignerForm Designer { get; private set; }

		public ToolStripMenuItem Menu { get; } = new ToolStripMenuItem();

			private ToolStripMenuItem _SettingsMenu;

			public BasePlugin()
			{
				Menu.Text = Name;
			}

			public void Load(DesignerForm designer)
			{
				if (IsLoaded)
				{
					return;
				}

				Designer = designer;

				Config?.Load();

				if (!OnLoad())
				{
					return;
				}

				if (Config != null)
				{
					if (_SettingsMenu == null)
					{
						_SettingsMenu = new ToolStripMenuItem("Settings", null, OnEditConfig);
					}

					Menu.DropDownItems.Add(_SettingsMenu);

					Config.ValueChanged += OnUpdateConfig;
				}

				Designer.MenuPlugins.DropDownItems.Add(Menu);

				IsLoaded = true;

				OnLoaded();
			}

			public void Unload()
			{
				if (!IsLoaded)
				{
					return;
				}

				Config?.Save();

				if (!OnUnload())
				{
					return;
				}

				if (Config != null)
				{
					Config.Close(false);

					Menu.DropDownItems.Remove(_SettingsMenu);

					Config.ValueChanged -= OnUpdateConfig;
				}

				Designer.MenuPlugins.DropDownItems.Remove(Menu);

			IsLoaded = false;

			OnUnloaded();
		}

		protected virtual bool OnLoad()
		{
			return true;
		}

		protected virtual bool OnUnload()
		{
			return true;
		}

		protected virtual void OnLoaded()
		{
		}

		protected virtual void OnUnloaded()
		{
		}

		protected virtual void OnEditConfig(object sender, EventArgs e)
		{
			Config?.Edit();
		}

		protected virtual void OnUpdateConfig(object sender, PropertyValueChangedEventArgs e)
		{
			Designer.CanvasImage.Refresh();
		}

		public virtual void MouseMovement(MouseMovementEventArgs e)
		{ }

		public virtual void AddContextMenus(BaseElement source, ref ToolStripMenuItem groupMenu, ref ToolStripMenuItem positionMenu, ref ToolStripMenuItem orderMenu, ref ToolStripMenuItem miscMenu)
		{ }
	}
}