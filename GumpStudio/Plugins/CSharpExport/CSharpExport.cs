using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Ultima;

using GumpStudio.Elements;

namespace GumpStudio.Plugins
{
	public class CSharpExport : BasePlugin
	{
		#region Gump Template for RunUO and ServUO

		private static readonly string Template = @"
#region References
using System;

using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;
#endregion

namespace Server.Gumps
{
	public class ~gump_type~ : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register(""~gump_type~"", AccessLevel.Administrator, e => DisplayTo(e.Mobile));
		}

		public static ~gump_type~ DisplayTo(Mobile user)
		{
			if (user == null || user.Deleted || !user.Player || user.NetState == null)
				return null;

			user.CloseGump(typeof(~gump_type~));

			var gump = new ~gump_type~(user);

			user.SendGump(gump);

			return gump;
		}

		public Mobile User { get; }

		private ~gump_type~(Mobile user) 
			: base(~gump_location~)
		{
			User = user;

			Dragable = true;
			Closable = true;
			Resizable = false;
			Disposable = false;

			AddPage(0);
			~gump_layout~
		}
		~gump_controls~
		public override void OnResponse(NetState sender, RelayInfo info)
		{
		}

		public override void OnServerClose(NetState owner)
		{
		}
	}
}
";

		#endregion

		private readonly Settings _Config = new Settings();

		public override BaseConfig Config => _Config;

		public override PluginInfo Info { get; } = new PluginInfo("C# Exporter", "1.1", "Vorspire", "admin@vita-nex.com", "Exports a C# file compatible with emulators targeting .NET");

		private ToolStripMenuItem _MenuFileExport;
		private ToolStripMenuItem _MenuFileImport;

			protected override void OnLoaded()
			{
				base.OnLoaded();

				Designer.MenuFileExport.Enabled = true;

				if (_MenuFileExport == null)
				{
					_MenuFileExport = new ToolStripMenuItem(".NET C#");
					_MenuFileExport.DropDownItems.AddRange(new ToolStripItem[]
					{
						new ToolStripMenuItem("All Elements", null, ExportFileClick),
						new ToolStripMenuItem("Selected Elements", null, ExportSelectionClick)
					});
				}

				Designer.MenuFileExport.DropDownItems.Add(_MenuFileExport);

				Designer.MenuFileImport.Enabled = true;

				if (_MenuFileImport == null)
				{
					_MenuFileImport = new ToolStripMenuItem(".NET C#", null, ImportFileClick);
				}

				Designer.MenuFileImport.DropDownItems.Add(_MenuFileImport);
			}

			protected override void OnUnloaded()
			{
				base.OnUnloaded();

				Designer.MenuFileExport.DropDownItems.Remove(_MenuFileExport);

				if (Designer.MenuFileExport.DropDownItems.Count == 0)
				{
					Designer.MenuFileExport.Enabled = false;
				}

				Designer.MenuFileImport.DropDownItems.Remove(_MenuFileImport);

				if (Designer.MenuFileImport.DropDownItems.Count == 0)
				{
					Designer.MenuFileImport.Enabled = false;
				}
			}

		private void ImportFileClick(object sender, EventArgs e)
		{
			using var dialog = new OpenFileDialog
			{
				Filter = "C# Files (*.cs)|*.cs|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
				Title = "Import C# Gump File"
			};

			if (dialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			try
			{
				var content = File.ReadAllText(dialog.FileName);
				var elements = ParseCSharpGump(content);

				if (elements.Count == 0)
				{
					MessageBox.Show("No supported gump elements found in the file.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				foreach (var element in elements)
				{
					Designer.AddElement(element);
				}

				MessageBox.Show($"Successfully imported {elements.Count} element(s).", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to import file:\n{ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private static List<BaseElement> ParseCSharpGump(string content)
		{
			var elements = new List<BaseElement>();

			// Match Add*(...) calls, stripping optional trailing comments
			var regex = new Regex(@"Add(\w+)\s*\(([^)]*)\)", RegexOptions.Compiled);

			foreach (Match match in regex.Matches(content))
			{
				var method = match.Groups[1].Value;
				var rawArgs = match.Groups[2].Value;

				try
				{
					var element = CreateElement(method, rawArgs);

					if (element != null)
					{
						elements.Add(element);
					}
				}
				catch
				{
					// Skip lines that don't parse cleanly
				}
			}

			return elements;
		}

		private static string[] SplitArgs(string rawArgs)
		{
			// Split on commas but respect quoted strings
			var args = new List<string>();
			var current = new StringBuilder();
			var inQuote = false;
			var escape = false;

			foreach (var ch in rawArgs)
			{
				if (escape)
				{
					current.Append(ch);
					escape = false;
					continue;
				}

				if (ch == '\\')
				{
					escape = true;
					continue;
				}

				if (ch == '"')
				{
					inQuote = !inQuote;
					continue;
				}

				if (ch == ',' && !inQuote)
				{
					args.Add(current.ToString().Trim());
					current.Clear();
					continue;
				}

				current.Append(ch);
			}

			if (current.Length > 0)
			{
				args.Add(current.ToString().Trim());
			}

			return args.ToArray();
		}

		private static int ParseInt(string value)
		{
			// Handle expressions like "(int)Buttons.Something" or "(int)Inputs.Something" by returning 0
			value = value.Trim();

			if (value.StartsWith("(int)"))
			{
				return 0;
			}

			return Int32.TryParse(value, out var result) ? result : 0;
		}

		private static bool ParseBool(string value)
		{
			return Boolean.TryParse(value.Trim(), out var result) && result;
		}

		private static BaseElement CreateElement(string method, string rawArgs)
		{
			var args = SplitArgs(rawArgs);

			switch (method)
			{
				case "Background" when args.Length >= 5:
				{
					var el = new BackgroundElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Width = ParseInt(args[2]),
						Height = ParseInt(args[3]),
						GumpID = ParseInt(args[4])
					};
					return el;
				}

				case "AlphaRegion" when args.Length >= 4:
				{
					var el = new AlphaElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Width = ParseInt(args[2]),
						Height = ParseInt(args[3])
					};
					return el;
				}

				case "Image" when args.Length >= 3:
				{
					var el = new ImageElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						GumpID = ParseInt(args[2])
					};
					return el;
				}

				case "ImageTiled" when args.Length >= 5:
				{
					var el = new TiledElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Width = ParseInt(args[2]),
						Height = ParseInt(args[3]),
						GumpID = ParseInt(args[4])
					};
					return el;
				}

				case "Label" when args.Length >= 4:
				{
					var el = new LabelElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Hue = new Hue(ParseInt(args[2])),
						Text = args[3]
					};
					return el;
				}

				case "Html" when args.Length >= 7:
				{
					var el = new HTMLElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Width = ParseInt(args[2]),
						Height = ParseInt(args[3]),
						HTML = args[4],
						ShowBackground = ParseBool(args[5]),
						ShowScrollbar = ParseBool(args[6])
					};
					return el;
				}

				case "HtmlLocalized" when args.Length >= 7:
				{
					var el = new HTMLElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Width = ParseInt(args[2]),
						Height = ParseInt(args[3]),
						CliLocID = ParseInt(args[4]),
						TextType = HTMLElementType.Localized,
						ShowBackground = ParseBool(args[5]),
						ShowScrollbar = ParseBool(args[6])
					};
					return el;
				}

				case "Button" when args.Length >= 7:
				{
					var isPage = args[5].Contains("Page");
					var el = new ButtonElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						NormalID = ParseInt(args[2]),
						PressedID = ParseInt(args[3]),
						ButtonType = isPage ? ButtonTypeEnum.Page : ButtonTypeEnum.Reply,
						Param = isPage ? ParseInt(args[6]) : 0
					};
					return el;
				}

				case "Check" when args.Length >= 6:
				{
					var el = new CheckboxElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						UnCheckedID = ParseInt(args[2]),
						CheckedID = ParseInt(args[3]),
						Checked = ParseBool(args[4])
					};
					return el;
				}

				case "Radio" when args.Length >= 6:
				{
					var el = new RadioElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						UnCheckedID = ParseInt(args[2]),
						CheckedID = ParseInt(args[3]),
						Checked = ParseBool(args[4])
					};
					return el;
				}

				case "TextEntry" when args.Length >= 7:
				{
					var el = new TextEntryElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						Width = ParseInt(args[2]),
						Height = ParseInt(args[3]),
						Hue = new Hue(ParseInt(args[4])),
						InitialText = args[6]
					};

					if (args.Length >= 8)
					{
						el.MaxLength = ParseInt(args[7]);
					}

					return el;
				}

				case "Item" when args.Length >= 3:
				{
					var el = new ItemElement
					{
						X = ParseInt(args[0]),
						Y = ParseInt(args[1]),
						ItemID = ParseInt(args[2])
					};
					return el;
				}

				case "Page":
					// Pages are structural, not elements - skip
					return null;

				default:
					return null;
			}
		}

		private void ExportFileClick(object sender, EventArgs e)
		{
			ExportFile(false);
		}

		private void ExportSelectionClick(object sender, EventArgs e)
		{
			ExportFile(true);
		}

		private void ExportFile(bool selected)
		{
			var fullPath = $"{Path.GetTempFileName()}.txt";

			var indent = new StringBuilder();

			var layoutBegin = Template.IndexOf("~gump_layout~");

			while (--layoutBegin >= 0)
			{
				if (Template[layoutBegin] == '\r' || Template[layoutBegin] == '\n')
				{
					break;
				}

				if (!Char.IsWhiteSpace(Template, layoutBegin))
				{
					break;
				}

				indent.Insert(0, Template[layoutBegin]);
			}

			var tabs = indent.ToString();

			var template = new StringBuilder(Template);

			template = template.Replace("~gump_type~", "CustomGump");

			var stacks = new Dictionary<GroupElement, ICSharpExportable[]>();

			if (selected)
			{
				var elements = Designer.ElementStack.SelectedElements.OfType<ICSharpExportable>().ToArray();

				if (elements.Length > 0)
				{
					stacks[Designer.ElementStack] = elements;
				}
			}
			else
			{
				foreach (var stack in Designer.Stacks)
				{
					var elements = stack.AllElements.OfType<ICSharpExportable>().ToArray();

					if (elements.Length > 0)
					{
						stacks[stack] = elements;
					}
				}
			}

			var location = Point.Empty;

			if (_Config.RelativeOffsets)
			{
				location.X = Int32.MaxValue;
				location.Y = Int32.MaxValue;

				foreach (var element in stacks.Values.SelectMany(o => o.OfType<BaseElement>()))
				{
					location.X = Math.Min(location.X, element.X);
					location.Y = Math.Min(location.Y, element.Y);
				}

				if (location.X == Int32.MaxValue)
				{
					location.X = 0;
				}

				if (location.Y == Int32.MaxValue)
				{
					location.Y = 0;
				}
			}

			template = template.Replace("~gump_location~", $"{location.X}, {location.Y}");

			var layout = new StringBuilder();

			var page = -1;

			foreach (var entry in stacks)
			{
				if (++page >= 1)
				{
					layout.AppendLine($"{tabs}AddPage({page});");
				}

				foreach (var exportable in entry.Value)
				{
					if (exportable is BaseElement element)
					{
						if (_Config.RelativeOffsets)
						{
							element.X -= location.X;
							element.Y -= location.Y;
						}

						var csharp = exportable.ToCSharpString();

						if (_Config.NoComments)
						{
							var index = csharp.IndexOf("//");

							if (index >= 0)
							{
								csharp = csharp.Substring(0, index);
							}
						}

						layout.AppendLine($"{tabs}{csharp}");

						if (_Config.RelativeOffsets)
						{
							element.X += location.X;
							element.Y += location.Y;
						}
					}
				}
			}

			template = template.Replace("~gump_layout~", layout.ToString().Trim());

			layout.Clear();
			indent.Clear();

			layoutBegin = Template.IndexOf("~gump_controls~");

			while (--layoutBegin >= 0)
			{
				if (Template[layoutBegin] == '\r' || Template[layoutBegin] == '\n')
				{
					break;
				}

				if (!Char.IsWhiteSpace(Template, layoutBegin))
				{
					break;
				}

				indent.Insert(0, Template[layoutBegin]);
			}

			tabs = indent.ToString();

			var buttonCount = 0;

			foreach (var button in stacks.Values.SelectMany(o => o.OfType<ButtonElement>().Where(b => b.ButtonType == ButtonTypeEnum.Reply)))
			{
				if (++buttonCount == 1)
				{
					layout.AppendLine();
					layout.AppendLine($"{tabs}public enum Buttons");
					layout.AppendLine($"{tabs}{{");
				}

				layout.AppendLine($"{tabs}\t{button.Name.Replace(" ", String.Empty)} = {buttonCount},");
			}

			if (buttonCount > 0)
			{
				layout.AppendLine($"{tabs}}}");
			}

			var checkCount = 0;

			foreach (var check in stacks.Values.SelectMany(o => o.OfType<CheckboxElement>()))
			{
				if (++checkCount == 1)
				{
					layout.AppendLine();
					layout.AppendLine($"{tabs}public enum Switches");
					layout.AppendLine($"{tabs}{{");
				}

				layout.AppendLine($"{tabs}\t{check.Name.Replace(" ", String.Empty)} = {checkCount},");
			}

			if (checkCount > 0)
			{
				layout.AppendLine($"{tabs}}}");
			}

			var inputCount = 0;

			foreach (var input in stacks.Values.SelectMany(o => o.OfType<TextEntryElement>()))
			{
				if (++inputCount == 1)
				{
					layout.AppendLine();
					layout.AppendLine($"{tabs}public enum Inputs");
					layout.AppendLine($"{tabs}{{");
				}

				layout.AppendLine($"{tabs}\t{input.Name.Replace(" ", String.Empty)} = {inputCount},");
			}

			if (inputCount > 0)
			{
				layout.AppendLine($"{tabs}}}");
			}

			if (layout.Length > 0)
			{
				template = template.Replace("~gump_controls~", $"{Environment.NewLine}{tabs}{layout.ToString().Trim()}{Environment.NewLine}");
			}
			else
			{
				template = template.Replace("~gump_controls~", String.Empty);
			}

			try
			{
				File.WriteAllText(fullPath, template.ToString().Trim());

				Process.Start(new ProcessStartInfo(fullPath)
				{
					UseShellExecute = true
				});
			}
			catch { }
		}

		[Serializable]
		public class Settings : BaseConfig
		{
			public override string Name => "C# Exporter";

			public bool RelativeOffsets { get; set; } = false;

			public bool NoComments { get; set; } = true;
		}
	}
}
