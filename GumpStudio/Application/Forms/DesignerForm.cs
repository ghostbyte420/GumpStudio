using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using GumpStudio.Elements;
using GumpStudio.Plugins;

using Ultima;

namespace GumpStudio.Forms
{
	[System.ComponentModel.DesignerCategory("")]
	public sealed partial class DesignerForm : Form
	{
		private Panel _Container;
		private Panel _CanvasContainer;
		private Label _ToolboxTitle;

		private Panel _PropertiesContainer;
		private ComboBox _PropertiesSelector;
		private PropertyGrid _PropertiesGrid;

		private TextBox _CanvasFocus;
		private PictureBox _CanvasImage;

		public TextBox CanvasFocus => _CanvasFocus;
		public PictureBox CanvasImage => _CanvasImage;

		private Panel _Panel1;
		private Panel _Panel2;
		private Panel _PanelCanvasScroller;
		private Panel _PanelToolbox;

		private OpenFileDialog _OpenDialog;
		private SaveFileDialog _SaveDialog;

		private Splitter _Splitter1;
		private Splitter _Splitter2;
		private StatusStrip _StatusBar;
		private ToolStripStatusLabel _StatusLabel;
		private ToolStripStatusLabel _StatusLabelUpdated;
		private TabPage _TabPage;
		private TabControl _TabPages;
		private TabControl _TabToolbox;
		private TabPage _PageCustom;
		private TabPage _PageStandard;
		private TreeView _Gumplings;

		#region Main Menu

		private MenuStrip _Menu;

		private ToolStripMenuItem _MenuFile;
		private ToolStripMenuItem _MenuFileNew;
		private ToolStripSeparator _MenuFileSep1;
		private ToolStripMenuItem _MenuFileOpen;
		private ToolStripMenuItem _MenuFileSave;
		private ToolStripMenuItem _MenuFileImport;
		private ToolStripMenuItem _MenuFileExport;
		private ToolStripSeparator _MenuFileSep2;
		private ToolStripMenuItem _MenuFileExit;

		private ToolStripMenuItem _MenuEdit;
		private ToolStripMenuItem _MenuEditUndo;
		private ToolStripMenuItem _MenuEditRedo;
		private ToolStripSeparator _MenuEditSep1;
		private ToolStripMenuItem _MenuEditCut;
		private ToolStripMenuItem _MenuEditCopy;
		private ToolStripMenuItem _MenuEditPaste;
		private ToolStripMenuItem _MenuEditDelete;
		private ToolStripSeparator _MenuEditSep2;
		private ToolStripMenuItem _MenuEditSelectAll;

		private ToolStripMenuItem _MenuMisc;
		private ToolStripMenuItem _MenuMiscImportGumpling;
		private ToolStripMenuItem _MenuMiscLoadGumpling;
		private ToolStripMenuItem _MenuMiscDataPath;
		private ToolStripMenuItem _MenuMiscExportGumpling;

		private ToolStripMenuItem _MenuPage;
		private ToolStripMenuItem _MenuPageAdd;
		private ToolStripMenuItem _MenuPageClear;
		private ToolStripMenuItem _MenuPageDelete;
		private ToolStripMenuItem _MenuPageInsert;
		private ToolStripSeparator _MenuPageSep1;
		private ToolStripMenuItem _MenuPageShow0;

		private ToolStripMenuItem _MenuPlugins;
		private ToolStripMenuItem _MenuPluginsManager;

		private ToolStripMenuItem _MenuHelp;
		private ToolStripMenuItem _MenuHelpAbout;

		public ToolStripMenuItem MenuFile => _MenuFile;
		public ToolStripMenuItem MenuFileNew => _MenuFileNew;
		public ToolStripMenuItem MenuFileOpen => _MenuFileOpen;
		public ToolStripMenuItem MenuFileSave => _MenuFileSave;
		public ToolStripMenuItem MenuFileImport => _MenuFileImport;
		public ToolStripMenuItem MenuFileExport => _MenuFileExport;


		public ToolStripMenuItem MenuFileExit => _MenuFileExit;

		public ToolStripMenuItem MenuEdit => _MenuEdit;
		public ToolStripMenuItem MenuEditUndo => _MenuEditUndo;
		public ToolStripMenuItem MenuEditRedo => _MenuEditRedo;
		public ToolStripMenuItem MenuEditCut => _MenuEditCut;
		public ToolStripMenuItem MenuEditCopy => _MenuEditCopy;
		public ToolStripMenuItem MenuEditPaste => _MenuEditPaste;
		public ToolStripMenuItem MenuEditDelete => _MenuEditDelete;
		public ToolStripMenuItem MenuEditSelectAll => _MenuEditSelectAll;

		public ToolStripMenuItem MenuMisc => _MenuMisc;
		public ToolStripMenuItem MenuMiscImportGumpling => _MenuMiscImportGumpling;
		public ToolStripMenuItem MenuMiscLoadGumpling => _MenuMiscLoadGumpling;
		public ToolStripMenuItem MenuMiscExportGumpling => _MenuMiscExportGumpling;
		public ToolStripMenuItem MenuMiscDataPath => _MenuMiscDataPath;

		public ToolStripMenuItem MenuPage => _MenuPage;
		public ToolStripMenuItem MenuPageAdd => _MenuPageAdd;
		public ToolStripMenuItem MenuPageClear => _MenuPageClear;
		public ToolStripMenuItem MenuPageDelete => _MenuPageDelete;
		public ToolStripMenuItem MenuPageInsert => _MenuPageInsert;
		public ToolStripMenuItem MenuPageShow0 => _MenuPageShow0;

		public ToolStripMenuItem MenuPlugins => _MenuPlugins;
		public ToolStripMenuItem MenuPluginsManager => _MenuPluginsManager;

		public ToolStripMenuItem MenuHelp => _MenuHelp;
		public ToolStripMenuItem MenuHelpAbout => _MenuHelpAbout;

		#endregion Main Menu

		#region Gumpling Menu

		private ContextMenuStrip _GumplingMenu;

		private ToolStripMenuItem _GumplingMenuAddFolder;
		private ToolStripMenuItem _GumplingMenuAddGumpling;
		private ToolStripMenuItem _GumplingMenuDelete;
		private ToolStripSeparator _GumplingMenuSep1;
		private ToolStripMenuItem _GumplingMenuMove;
		private ToolStripMenuItem _GumplingMenuRename;

		public ContextMenuStrip GumplingMenu => _GumplingMenu;

		public ToolStripMenuItem GumplingMenuAddFolder => _GumplingMenuAddFolder;
		public ToolStripMenuItem GumplingMenuAddGumpling => _GumplingMenuAddGumpling;
		public ToolStripMenuItem GumplingMenuDelete => _GumplingMenuDelete;
		public ToolStripMenuItem GumplingMenuMove => _GumplingMenuMove;
		public ToolStripMenuItem GumplingMenuRename => _GumplingMenuRename;

		#endregion Gumpling Menu

		private void InitializeComponent()
		{
			_Resources = new ComponentResourceManager(typeof(DesignerForm));

			_Components = new Container();

			_Panel1 = new Panel();
			_Panel2 = new Panel();
			_PropertiesContainer = new Panel();
			_CanvasContainer = new Panel();

			_PanelCanvasScroller = new Panel();
			_Container = new Panel();

			_TabToolbox = new TabControl();
			_TabPages = new TabControl();

			_PageStandard = new TabPage();
			_PageCustom = new TabPage();

			_TabPage = new TabPage();

			_PanelToolbox = new Panel();

			_Gumplings = new TreeView();
			_ToolboxTitle = new Label();
			_StatusBar = new StatusStrip();
			_StatusLabel = new ToolStripStatusLabel();

			_StatusLabelUpdated = new ToolStripStatusLabel();

			_PropertiesSelector = new ComboBox();
			_PropertiesGrid = new PropertyGrid();

			_Splitter1 = new Splitter();
			_Splitter2 = new Splitter();

			_OpenDialog = new OpenFileDialog();
			_SaveDialog = new SaveFileDialog();

			_CanvasFocus = new TextBox();
			_CanvasImage = new PictureBox();

			#region Main Menu

			_Menu = new MenuStrip();

			_MenuFile = new ToolStripMenuItem();
			_MenuFileNew = new ToolStripMenuItem();
			_MenuFileSep1 = new ToolStripSeparator();
			_MenuFileOpen = new ToolStripMenuItem();
			_MenuFileSave = new ToolStripMenuItem();
			_MenuFileImport = new ToolStripMenuItem();
			_MenuFileExport = new ToolStripMenuItem();
			_MenuFileSep2 = new ToolStripSeparator();
			_MenuFileExit = new ToolStripMenuItem();

			_MenuEdit = new ToolStripMenuItem();
			_MenuEditUndo = new ToolStripMenuItem();
			_MenuEditRedo = new ToolStripMenuItem();
			_MenuEditSep1 = new ToolStripSeparator();
			_MenuEditCut = new ToolStripMenuItem();
			_MenuEditCopy = new ToolStripMenuItem();
			_MenuEditPaste = new ToolStripMenuItem();
			_MenuEditDelete = new ToolStripMenuItem();
			_MenuEditSep2 = new ToolStripSeparator();
			_MenuEditSelectAll = new ToolStripMenuItem();





			_MenuMisc = new ToolStripMenuItem();
			_MenuMiscImportGumpling = new ToolStripMenuItem();
			_MenuMiscLoadGumpling = new ToolStripMenuItem();
			_MenuMiscExportGumpling = new ToolStripMenuItem();
			_MenuMiscDataPath = new ToolStripMenuItem();





			_MenuPage = new ToolStripMenuItem();
			_MenuPageAdd = new ToolStripMenuItem();
			_MenuPageClear = new ToolStripMenuItem();
			_MenuPageDelete = new ToolStripMenuItem();
			_MenuPageInsert = new ToolStripMenuItem();
			_MenuPageSep1 = new ToolStripSeparator();
			_MenuPageShow0 = new ToolStripMenuItem();

			_MenuPlugins = new ToolStripMenuItem();
			_MenuPluginsManager = new ToolStripMenuItem();

			_MenuHelp = new ToolStripMenuItem();
			_MenuHelpAbout = new ToolStripMenuItem();

			#endregion Main Menu

			#region Gumpling Menu

			_GumplingMenu = new ContextMenuStrip();

			_GumplingMenuAddFolder = new ToolStripMenuItem();
			_GumplingMenuAddGumpling = new ToolStripMenuItem();
			_GumplingMenuDelete = new ToolStripMenuItem();
			_GumplingMenuSep1 = new ToolStripSeparator();
			_GumplingMenuMove = new ToolStripMenuItem();
			_GumplingMenuRename = new ToolStripMenuItem();

			#endregion Gumpling Menu

			((ISupportInitialize)CanvasImage).BeginInit();

			_Panel1.SuspendLayout();
			_Panel2.SuspendLayout();
			_PropertiesContainer.SuspendLayout();
			_CanvasContainer.SuspendLayout();

			_PanelCanvasScroller.SuspendLayout();
			_Container.SuspendLayout();

			_TabToolbox.SuspendLayout();
			_TabPages.SuspendLayout();

			_PageStandard.SuspendLayout();
			_PageCustom.SuspendLayout();

			SuspendLayout();

			_Container.BorderStyle = BorderStyle.Fixed3D;
			_Container.Controls.Add(_CanvasContainer);
			_Container.Controls.Add(_ToolboxTitle);
			_Container.Dock = DockStyle.Left;
			_Container.Location = new Point(0, 0);
			_Container.Name = nameof(_Container);
			_Container.Size = new Size(128, 685);
			_Container.TabIndex = 0;

			_CanvasContainer.Controls.Add(_TabToolbox);
			_CanvasContainer.Dock = DockStyle.Fill;
			_CanvasContainer.Location = new Point(0, 16);
			_CanvasContainer.Name = nameof(_CanvasContainer);
			_CanvasContainer.Size = new Size(124, 665);
			_CanvasContainer.TabIndex = 1;

			_TabToolbox.Controls.Add(_PageStandard);
			_TabToolbox.Controls.Add(_PageCustom);
			_TabToolbox.Dock = DockStyle.Fill;
			_TabToolbox.Location = new Point(0, 0);
			_TabToolbox.Multiline = true;
			_TabToolbox.Name = nameof(_TabToolbox);
			_TabToolbox.SelectedIndex = 0;
			_TabToolbox.Size = new Size(124, 665);
			_TabToolbox.TabIndex = 1;

			_PageStandard.Controls.Add(_PanelToolbox);
			_PageStandard.Location = new Point(4, 22);
			_PageStandard.Name = nameof(_PageStandard);
			_PageStandard.Size = new Size(116, 639);
			_PageStandard.TabIndex = 0;
			_PageStandard.Text = "Standard";

			_PanelToolbox.AutoScroll = true;
			_PanelToolbox.Dock = DockStyle.Fill;
			_PanelToolbox.Location = new Point(0, 0);
			_PanelToolbox.Name = nameof(_PanelToolbox);
			_PanelToolbox.Size = new Size(116, 639);
			_PanelToolbox.TabIndex = 1;

			_PageCustom.Controls.Add(_Gumplings);
			_PageCustom.Location = new Point(4, 22);
			_PageCustom.Name = nameof(_PageCustom);
			_PageCustom.Size = new Size(116, 639);
			_PageCustom.TabIndex = 1;
			_PageCustom.Text = "Gumplings";

			_Gumplings.Dock = DockStyle.Fill;
			_Gumplings.Location = new Point(0, 0);
			_Gumplings.Name = nameof(_Gumplings);
			_Gumplings.Size = new Size(116, 639);
			_Gumplings.TabIndex = 1;
			_Gumplings.DoubleClick += Gumplings_DoubleClick;
			_Gumplings.MouseUp += Gumplings_MouseUp;

			_ToolboxTitle.BackColor = SystemColors.ControlDark;
			_ToolboxTitle.BorderStyle = BorderStyle.Fixed3D;
			_ToolboxTitle.Dock = DockStyle.Top;
			_ToolboxTitle.Location = new Point(0, 0);
			_ToolboxTitle.Name = nameof(_ToolboxTitle);
			_ToolboxTitle.Size = new Size(124, 16);
			_ToolboxTitle.TabIndex = 0;
			_ToolboxTitle.Text = "Toolbox";
			_ToolboxTitle.TextAlign = ContentAlignment.MiddleCenter;

			_StatusBar.Location = new Point(0, 685);
			_StatusBar.Name = nameof(_StatusBar);
			_StatusBar.Size = new Size(1350, 23);
			_StatusBar.TabIndex = 0;
			_StatusBar.Items.Add(_StatusLabel);

			_StatusLabel.Spring = true;
			_StatusLabel.TextAlign = ContentAlignment.MiddleLeft;
			_StatusLabelUpdated.Font = new Font(_StatusLabelUpdated.Font.FontFamily, 11f, FontStyle.Regular);

			var buildDate = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);

			_StatusLabelUpdated.Text = $"Last Updated: {buildDate:MM/dd/yyyy}";
			_StatusLabelUpdated.ForeColor = Color.SlateGray;
			_StatusLabelUpdated.Alignment = ToolStripItemAlignment.Right;

			_StatusBar.Items.Add(_StatusLabelUpdated);

			_Splitter1.BorderStyle = BorderStyle.Fixed3D;
			_Splitter1.Location = new Point(128, 0);
			_Splitter1.MinSize = 80;
			_Splitter1.Name = nameof(_Splitter1);
			_Splitter1.Size = new Size(3, 685);
			_Splitter1.TabIndex = 1;
			_Splitter1.TabStop = false;

			_Panel1.Controls.Add(_Panel2);
			_Panel1.Dock = DockStyle.Fill;
			_Panel1.Location = new Point(131, 0);
			_Panel1.Name = nameof(_Panel1);
			_Panel1.Size = new Size(1219, 685);
			_Panel1.TabIndex = 2;

			_Panel2.Controls.Add(_PanelCanvasScroller);
			_Panel2.Controls.Add(_TabPages);
			_Panel2.Controls.Add(_Splitter2);
			_Panel2.Controls.Add(_PropertiesContainer);
			_Panel2.Dock = DockStyle.Fill;
			_Panel2.Location = new Point(0, 0);
			_Panel2.Name = nameof(_Panel2);
			_Panel2.Size = new Size(1219, 685);
			_Panel2.TabIndex = 0;

			_PanelCanvasScroller.AutoScroll = true;
			_PanelCanvasScroller.AutoScrollMargin = new Size(1, 1);
			_PanelCanvasScroller.AutoScrollMinSize = new Size(1, 1);
			_PanelCanvasScroller.BackColor = Color.Silver;
			_PanelCanvasScroller.BorderStyle = BorderStyle.Fixed3D;
			_PanelCanvasScroller.Controls.Add(CanvasImage);
			_PanelCanvasScroller.Dock = DockStyle.Fill;
			_PanelCanvasScroller.Location = new Point(0, 24);
			_PanelCanvasScroller.Name = nameof(_PanelCanvasScroller);
			_PanelCanvasScroller.Size = new Size(949, 661);
			_PanelCanvasScroller.TabIndex = 2;
			_PanelCanvasScroller.MouseLeave += CanvasScroller_MouseLeave;

			_CanvasImage.BackColor = Color.Black;
			_CanvasImage.Location = new Point(0, 0);
			_CanvasImage.Name = nameof(CanvasImage);
			_CanvasImage.Size = new Size(1600, 1200);
			_CanvasImage.TabIndex = 0;
			_CanvasImage.TabStop = false;
			_CanvasImage.Paint += ImageCanvas_Paint;
			_CanvasImage.MouseDown += ImageCanvas_MouseDown;
			_CanvasImage.MouseMove += ImageCanvas_MouseMove;
			_CanvasImage.MouseUp += ImageCanvas_MouseUp;

			_TabPages.Controls.Add(_TabPage);
			_TabPages.Dock = DockStyle.Top;
			_TabPages.HotTrack = true;
			_TabPages.Location = new Point(0, 0);
			_TabPages.Name = nameof(_TabPages);
			_TabPages.SelectedIndex = 0;
			_TabPages.Size = new Size(949, 24);
			_TabPages.TabIndex = 3;
			_TabPages.SelectedIndexChanged += TabPages_SelectedIndexChanged;

			_TabPage.Location = new Point(4, 22);
			_TabPage.Name = nameof(_TabPage);
			_TabPage.Size = new Size(941, 0);
			_TabPage.TabIndex = 0;
			_TabPage.Text = "0";

			_Splitter2.Dock = DockStyle.Right;
			_Splitter2.Location = new Point(949, 0);
			_Splitter2.Name = nameof(_Splitter2);
			_Splitter2.Size = new Size(22, 685);
			_Splitter2.TabIndex = 1;
			_Splitter2.TabStop = false;

			_PropertiesContainer.Controls.Add(_PropertiesSelector);
			_PropertiesContainer.Controls.Add(_PropertiesGrid);
			_PropertiesContainer.Controls.Add(CanvasFocus);
			_PropertiesContainer.Dock = DockStyle.Right;
			_PropertiesContainer.Location = new Point(971, 0);
			_PropertiesContainer.Name = nameof(_PropertiesContainer);
			_PropertiesContainer.Size = new Size(248, 685);
			_PropertiesContainer.TabIndex = 0;

			_PropertiesSelector.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_PropertiesSelector.DropDownStyle = ComboBoxStyle.DropDownList;
			_PropertiesSelector.Location = new Point(0, 8);
			_PropertiesSelector.Name = nameof(_PropertiesSelector);
			_PropertiesSelector.Size = new Size(240, 21);
			_PropertiesSelector.TabIndex = 1;
			_PropertiesSelector.SelectedIndexChanged += ComboElements_SelectedIndexChanged;
			_PropertiesSelector.Click += ComboElements_Click;

			_PropertiesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_PropertiesGrid.Cursor = Cursors.HSplit;
			_PropertiesGrid.LineColor = SystemColors.ScrollBar;
			_PropertiesGrid.Location = new Point(0, 32);
			_PropertiesGrid.Name = nameof(_PropertiesGrid);
			_PropertiesGrid.Size = new Size(240, 651);
			_PropertiesGrid.TabIndex = 0;
			_PropertiesGrid.PropertyValueChanged += ElementProperties_PropertyValueChanged;

			_CanvasFocus.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			_CanvasFocus.Location = new Point(16, 635);
			_CanvasFocus.Name = nameof(_CanvasFocus);
			_CanvasFocus.Size = new Size(100, 20);
			_CanvasFocus.TabIndex = 1;
			_CanvasFocus.Text = "";

			#region Main Menu

			_Menu.Items.AddRange(new ToolStripItem[]
			{
					_MenuFile,
					_MenuEdit,
					_MenuMisc,
					_MenuPage,
					_MenuPlugins,
					_MenuHelp
			});

			_MenuFile.DropDownItems.AddRange(new ToolStripItem[]
			{
					_MenuFileNew,
					_MenuFileSep1,
					_MenuFileOpen,
					_MenuFileSave,
					_MenuFileImport,
					_MenuFileExport,
					_MenuFileSep2,
					_MenuFileExit
			});
			_MenuFile.Text = "File";

			_MenuFileNew.Text = "New";
			_MenuFileNew.Click += MenuFileNew_Click;

			_MenuFileOpen.Text = "Open";
			_MenuFileOpen.Click += MenuFileOpen_Click;

			_MenuFileSave.Text = "Save";
			_MenuFileSave.Click += MenuFileSave_Click;

			_MenuFileImport.Enabled = false;
			_MenuFileImport.Text = "Import";

			_MenuFileExport.Enabled = false;
			_MenuFileExport.Text = "Export";

			_MenuFileExit.Text = "Exit";
			_MenuFileExit.Click += MenuFileExit_Click;

			_MenuEdit.DropDownItems.AddRange(new ToolStripItem[]
			{
					_MenuEditUndo,
					_MenuEditRedo,
					_MenuEditSep1,
					_MenuEditCut,
					_MenuEditCopy,
					_MenuEditPaste,
					_MenuEditDelete,
					_MenuEditSep2,
					_MenuEditSelectAll
			});
			_MenuEdit.Text = "Edit";

			_MenuEditUndo.Enabled = false;
			_MenuEditUndo.ShortcutKeys = Keys.Control | Keys.Z;
			_MenuEditUndo.Text = "Undo";
			_MenuEditUndo.Click += MenuEditUndo_Click;

			_MenuEditRedo.Enabled = false;
			_MenuEditRedo.ShortcutKeys = Keys.Control | Keys.Y;
			_MenuEditRedo.Text = "Redo";
			_MenuEditRedo.Click += MenuEditRedo_Click;

			_MenuEditCut.ShortcutKeys = Keys.Control | Keys.X;
			_MenuEditCut.Text = "Cu&t";
			_MenuEditCut.Click += MenuCut_Click;

			_MenuEditCopy.ShortcutKeys = Keys.Control | Keys.C;
			_MenuEditCopy.Text = "&Copy";
			_MenuEditCopy.Click += MenuCopy_Click;

			_MenuEditPaste.ShortcutKeys = Keys.Control | Keys.V;
			_MenuEditPaste.Text = "&Paste";
			_MenuEditPaste.Click += MenuPaste_Click;

			_MenuEditDelete.Text = "Delete";
			_MenuEditDelete.Click += MenuDelete_Click;

			_MenuEditSelectAll.ShortcutKeys = Keys.Control | Keys.A;
			_MenuEditSelectAll.Text = "Select &All";
			_MenuEditSelectAll.Click += MenuSelectAll_Click;






			_MenuMisc.DropDownItems.AddRange(new ToolStripItem[]
			{
					_MenuMiscLoadGumpling,
					_MenuMiscImportGumpling,
					_MenuMiscExportGumpling,
					_MenuMiscDataPath
			});
			_MenuMisc.Text = "Misc";

			_MenuMiscLoadGumpling.Text = "Place Gumpling on Canvas";
			_MenuMiscLoadGumpling.Click += MenuMiscLoadGumpling_Click;

			_MenuMiscImportGumpling.Text = "Import Gumpling to Toolbox";
			_MenuMiscImportGumpling.Click += MenuImportGumpling_Click;

			_MenuMiscExportGumpling.Text = "Export Selection as Gumpling";
			_MenuMiscExportGumpling.Click += MenuMiscExportGumpling_Click;

			_MenuMiscDataPath.Text = "Data Path";
			_MenuMiscDataPath.Click += MenuDataFile_Click;





			_MenuPage.DropDownItems.AddRange(new ToolStripItem[]
			{
					_MenuPageAdd,
					_MenuPageInsert,
					_MenuPageDelete,
					_MenuPageClear,
					_MenuPageSep1,
					_MenuPageShow0
			});
			_MenuPage.Text = "Page";

			_MenuPageAdd.Text = "Add Page";
			_MenuPageAdd.Click += MenuAddPage_Click;

			_MenuPageInsert.Text = "Insert Page";
			_MenuPageInsert.Click += MenuPageInsert_Click;

			_MenuPageDelete.Text = "Delete Page";
			_MenuPageDelete.Click += MenuPageDelete_Click;

			_MenuPageClear.Text = "Clear Page";
			_MenuPageClear.Click += MenuPageClear_Click;

			_MenuPageShow0.Checked = true;
			_MenuPageShow0.Text = "Always Show Page 0";
			_MenuPageShow0.Click += MenuShowPage0_Click;

			_MenuPlugins.DropDownItems.AddRange(new ToolStripItem[]
			{
					_MenuPluginsManager
			});
			_MenuPlugins.Text = "Plug-Ins";

			_MenuPluginsManager.Text = "Manager";
			_MenuPluginsManager.Click += MenuPluginManager_Click;

			_MenuHelp.DropDownItems.AddRange(new ToolStripItem[]
			{
					_MenuHelpAbout
			});
			_MenuHelp.Text = "Help";

			_MenuHelpAbout.Text = "About...";
			_MenuHelpAbout.Click += MenuHelpAbout_Click;

			#endregion Main Menu

			#region Gumpling Menu

			_GumplingMenu.Items.AddRange(new ToolStripItem[]
			{
					_GumplingMenuRename,
					_GumplingMenuMove,
					_GumplingMenuDelete,
					_GumplingMenuSep1,
					_GumplingMenuAddGumpling,
					_GumplingMenuAddFolder
			});

			_GumplingMenuRename.Text = "Rename";

			_GumplingMenuMove.Text = "Move";

			_GumplingMenuDelete.Text = "Delete";

			_GumplingMenuAddGumpling.Text = "Add Gumpling";

			_GumplingMenuAddFolder.Text = "Add Folder";

			#endregion Gumpling Menu

			AutoScaleBaseSize = new Size(5, 13);
			ClientSize = new Size(1350, 708);

			Controls.Add(_Panel1);
			Controls.Add(_Splitter1);
			Controls.Add(_Container);
			Controls.Add(_StatusBar);

			Controls.Add(_Menu);
			MainMenuStrip = _Menu;

			KeyPreview = true;

			//Icon = (Icon)_Resources.GetObject("$this.Icon");
			Icon = new Icon(Path.Combine(Application.StartupPath, "Resources", "Icon.ico"));

			Name = "DesignerForm";
			Text = "Gump Studio (-Unsaved Gump-)";

			Load += DesignerForm_Load;
			KeyDown += DesignerForm_KeyDown;
			KeyUp += DesignerForm_KeyUp;
			FormClosing += DesignerForm_FormClosing;

			((ISupportInitialize)CanvasImage).EndInit();

			_Panel1.ResumeLayout(false);
			_Panel2.ResumeLayout(false);
			_PropertiesContainer.ResumeLayout(false);
			_CanvasContainer.ResumeLayout(false);

			_PanelCanvasScroller.ResumeLayout(false);
			_Container.ResumeLayout(false);

			_TabToolbox.ResumeLayout(false);
			_TabPages.ResumeLayout(false);

			_PageStandard.ResumeLayout(false);
			_PageCustom.ResumeLayout(false);

			ResumeLayout(false);
		}
	}

	public sealed partial class DesignerForm
	{
		public static string AppPath => Application.StartupPath;

		private IContainer _Components;

		private ComponentResourceManager _Resources;

		private Point _LastPosition;
		private Point _Anchor;
		private Size _AnchorOffset;
		private Rectangle _SelectionRect;

		private LinearGradientBrush _SelectionBG;
		private Pen _SelectionFG;

		private bool _ElementChanged;
		private bool _ShowPage0 = true;
		private bool _ShowSelectionRect;

		private int _MoveCount;
		private int _CurrentUndoPoint = -1;

		private string _FileName = String.Empty;
		private string _AboutElementAppend = String.Empty;

		private MoveType _MoveMode;

		private Bitmap _Canvas;

		private readonly ContextMenuStrip _ContextMenu = new ContextMenuStrip();

		private readonly ArrayList _UndoPoints = new ArrayList();
		private readonly ArrayList _RegisteredTypes = new ArrayList();

		private readonly HashSet<BasePlugin> _AvailablePlugins = new HashSet<BasePlugin>();
		private readonly HashSet<BasePlugin> _LoadedPlugins = new HashSet<BasePlugin>();

		public HashSet<PluginInfo> PluginsInfo { get; } = new HashSet<PluginInfo>();

		public HashSet<BaseElement> ToolboxElements { get; } = new HashSet<BaseElement>();

		public List<GroupElement> Stacks { get; } = new List<GroupElement>();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public decimal ArrowKeyDelta { get; set; } = 1;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int MaxUndoPoints { get; set; } = 50;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PluginClearsCanvas { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SuppressUndoPoints { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ShouldClearActiveElement { get; set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeFolder UncategorizedFolder { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeFolder GumplingsFolder { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeFolder GumplingTree { get; set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public BaseElement ActiveElement { get; set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GroupElement ElementStack { get; set; } = new GroupElement(null, null, "CanvasStack", true);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GumpProperties GumpProperties { get; set; } = new GumpProperties();

		public IEnumerable<BaseElement> AllElements => Stacks.OfType<GroupElement>().SelectMany(s => s.AllElements);

		public event HookKeyDownEventHandler HookKeyDown;
		public event HookPostRenderEventHandler HookPostRender;
		public event HookPreRenderEventHandler HookPreRender;

		public DesignerForm()
		{
			Closed += DesignerForm_Closed;
			Closing += DesignerForm_Closing;

			InitializeComponent();

			GlobalObjects.DesignerForm = this;
		}

		public void NormalizeNames()
		{
			var elements = AllElements.ToArray();

			var nums = String.Join(String.Empty, Enumerable.Range(0, 9)).ToCharArray();

			foreach (var o in elements)
			{
				o.Name = Regex.Replace(o.Name, "Copy Of", String.Empty, RegexOptions.IgnoreCase).TrimEnd(nums).Trim();
			}

			var buttons = 0;
			var switches = 0;

			foreach (var g in elements.GroupBy(o => o.GetType()))
			{
				var index = 0;

				foreach (var group in g.ToLookup(o => o.Name))
				{
					var name = group.Key;
					var count = group.Count();

					if (count == 0)
					{
						continue;
					}

					foreach (var o in group)
					{
						if (o is TextEntryElement te)
						{
							te.ID = index;
						}

						if (o is ButtonElement)
						{
							o.Name = $"{name}{++buttons}";
						}
						else if (o is CheckboxElement)
						{
							o.Name = $"{name}{++switches}";
						}
						else
						{
							o.Name = $"{name}{++index}";
						}
					}
				}
			}
		}

		public void AddElement(BaseElement element)
		{
			ElementStack.AddElement(element);

			element.Selected = true;

			SetActiveElement(element, true);

			CanvasImage.Invalidate();

			CreateUndoPoint(element.Name + " added");
		}

		public int AddPage()
		{
			var index = Stacks.Count;

			Stacks.Add(new GroupElement(null, null, "CanvasStack", true));

			_TabPages.TabPages.Add(new TabPage(index.ToString()));

			_TabPages.SelectedIndex = index;

			ChangeActiveStack(index);

			return index;
		}

		public void BuildGumplingTree()
		{
			_Gumplings.Nodes.Clear();

			BuildGumplingTree(GumplingTree, null);
		}

		public void BuildGumplingTree(TreeFolder Item, TreeNode Node)
		{
			foreach (TreeItem item in Item.GetChildren())
			{
				var treeNode = new TreeNode
				{
					Text = item.Text,
					Tag = item
				};

				if (Node == null)
				{
					_Gumplings.Nodes.Add(treeNode);
				}
				else
				{
					Node.Nodes.Add(treeNode);
				}

				if (item is TreeFolder folder)
				{
					BuildGumplingTree(folder, treeNode);
				}
			}
		}

		private void BuildToolbox()
		{
			ToolboxElements.Clear();

			_PanelToolbox.Controls.Clear();

			var y = 0;

			foreach (Type type in _RegisteredTypes)
			{
				try
				{
					var element = (BaseElement)Activator.CreateInstance(type);

					ToolboxElements.Add(element);

					var button = new Button
					{
						Text = element.Type,
						Location = new Point(0, y),
						FlatStyle = FlatStyle.System,
						Width = _PanelToolbox.Width,
						Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
						Tag = type
					};

					y += button.Height - 1;

					_PanelToolbox.Controls.Add(button);

					button.Click += CreateElementFromToolbox;

					if (element.DispayInAbout())
					{
						_AboutElementAppend += $"{Environment.NewLine}{element.Type}: {element.GetAboutText()}{Environment.NewLine}";
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
				}
			}

			BaseElement.ResetID();

			GumplingTree = new TreeFolder("Root");
			GumplingsFolder = new TreeFolder("My Gumplings");
			UncategorizedFolder = new TreeFolder("Uncategorized");

			GumplingTree.AddItem(GumplingsFolder);
			GumplingTree.AddItem(UncategorizedFolder);

			BuildGumplingTree();
		}

		private void ComboElements_Click(object sender, EventArgs e)
		{
			foreach (var element in ElementStack.Elements)
			{
				element.Selected = false;
			}

			ActiveElement = null;
		}

		private void ComboElements_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetActiveElement(_PropertiesSelector.SelectedItem as BaseElement, false);

			CanvasImage.Invalidate();
		}

		private void ChangeActiveElementEventHandler(BaseElement e, bool DeselectOthers)
		{
			SetActiveElement(e, DeselectOthers);

			CanvasImage.Invalidate();
		}

		public void ChangeActiveStack(int stackID)
		{
			if (stackID >= Stacks.Count)
			{
				return;
			}

			SetActiveElement(null, true);

			if (ElementStack != null)
			{
				ElementStack.UpdateParent -= ChangeActiveElementEventHandler;
				ElementStack.Repaint -= RefreshView;
			}

			ElementStack = Stacks[stackID];

			ElementStack.UpdateParent += ChangeActiveElementEventHandler;
			ElementStack.Repaint += RefreshView;

			CanvasImage.Invalidate();
		}

		public void ClearContextMenu(ContextMenuStrip menu)
		{
			menu.Items.Clear();
		}

		public void ClearGump()
		{
			BaseElement.ResetID();

			_FileName = String.Empty;

			Text = "Gump Studio (-Unsaved Gump-)";

			_TabPages.TabPages.Clear();
			_TabPages.TabPages.Add(new TabPage("0"));

			ElementStack = new GroupElement(null, null, "Element Stack", true);
			ElementStack.UpdateParent += ChangeActiveElementEventHandler;
			ElementStack.Repaint += RefreshView;

			Stacks.Clear();
			Stacks.Add(ElementStack);

			SetActiveElement(null);

			CanvasImage.Invalidate();

			ChangeActiveStack(0);

			_UndoPoints.Clear();

			CreateUndoPoint("Blank");

			_MenuEditUndo.Enabled = false;
			_MenuEditRedo.Enabled = false;
		}

		public void Copy()
		{
			Clipboard.SetDataObject(ElementStack.GetSelectedElements().Select(e => e.Clone()).ToArray());
		}

		public void CreateElementFromToolbox(object sender, EventArgs e)
		{
			if (sender is Control c && c.Tag is Type t && typeof(BaseElement).IsAssignableFrom(t))
			{
				AddElement((BaseElement)Activator.CreateInstance(t));
			}

			CanvasImage.Invalidate();
			CanvasImage.Focus();
		}

		public void CreateUndoPoint()
		{
			CreateUndoPoint("Unknown Action");
		}

		public void CreateUndoPoint(string action)
		{
			if (SuppressUndoPoints)
			{
				return;
			}

			while (_UndoPoints.Count - 1 > _CurrentUndoPoint)
			{
				_UndoPoints.RemoveAt(_CurrentUndoPoint + 1);
			}

			while (_UndoPoints.Count >= MaxUndoPoints)
			{
				_UndoPoints.RemoveAt(0);
			}

			_CurrentUndoPoint = _UndoPoints.Add(new UndoPoint(this)
			{
				Text = action
			});

			_MenuEditUndo.Enabled = true;
			_MenuEditRedo.Enabled = false;
		}

		public void Cut()
		{
			Clipboard.SetDataObject(ElementStack.GetSelectedElements().ToArray());

			DeleteSelectedElements();
		}

		private void DeleteSelectedElements()
		{
			var flag = false;

			var elements = ElementStack.Elements.ToArray();

			foreach (var element in elements)
			{
				flag = true;

				if (element.Selected)
				{
					ElementStack.RemoveElement(element);
				}
			}

			SetActiveElement(GetLastSelectedControl());

			CanvasImage.Invalidate();

			if (flag)
			{
				CreateUndoPoint("Delete Elements");
			}
		}

		private void DesignerForm_Closed(object sender, EventArgs e)
		{
			_SelectionFG?.Dispose();
			_SelectionBG?.Dispose();

			WritePluginsToLoad();
		}

		private void DesignerForm_Closing(object sender, CancelEventArgs e)
		{
			foreach (var plugin in _AvailablePlugins)
			{
				if (plugin.IsLoaded)
				{
					plugin.Unload();
				}
			}
		}

		private void DesignerForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			XMLSettings.AppSettings.DesignerFormSize = WindowState != FormWindowState.Normal ? RestoreBounds.Size : Size;
			XMLSettings.AppSettings.Save();
		}

		private void DesignerForm_KeyDown(object sender, KeyEventArgs e)
		{
			var hookKeyDown = HookKeyDown;

			hookKeyDown?.Invoke(ActiveControl, ref e);

			if (e.Handled || ActiveControl != CanvasFocus)
			{
				return;
			}

			var flag = false;

			if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back ? 1 : 0) != 0)
			{
				DeleteSelectedElements();

				flag = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Up)
			{
				foreach (var selectedElement in ElementStack.GetSelectedElements())
				{
					var location = selectedElement.Location;

					location.Offset(0, -Convert.ToInt32(ArrowKeyDelta));

					selectedElement.Location = location;
				}

				//ArrowKeyDelta = Decimal.Multiply(ArrowKeyDelta, new decimal(106, 0, 0, false, 2));
				flag = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Down)
			{
				foreach (var selectedElement in ElementStack.GetSelectedElements())
				{
					var location = selectedElement.Location;

					location.Offset(0, Convert.ToInt32(ArrowKeyDelta));

					selectedElement.Location = location;
				}

				//ArrowKeyDelta = Decimal.Multiply(ArrowKeyDelta, new decimal(106, 0, 0, false, 2));
				flag = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Left)
			{
				foreach (var selectedElement in ElementStack.GetSelectedElements())
				{
					var location = selectedElement.Location;

					location.Offset(-Convert.ToInt32(ArrowKeyDelta), 0);

					selectedElement.Location = location;
				}

				//ArrowKeyDelta = Decimal.Multiply(ArrowKeyDelta, new decimal(106, 0, 0, false, 2));
				flag = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Right)
			{
				foreach (var selectedElement in ElementStack.GetSelectedElements())
				{
					var location = selectedElement.Location;

					location.Offset(Convert.ToInt32(ArrowKeyDelta), 0);

					selectedElement.Location = location;
				}

				//ArrowKeyDelta = Decimal.Multiply(ArrowKeyDelta, new decimal(106, 0, 0, false, 2));
				flag = true;
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Next)
			{
				var index = (ActiveElement == null ? ElementStack._Elements.Count - 1 : ActiveElement.Z) - 1;

				if (index < 0)
				{
					index = ElementStack._Elements.Count - 1;
				}

				if (index >= 0 & index <= ElementStack._Elements.Count - 1)
				{
					SetActiveElement((BaseElement)ElementStack._Elements[index], true);
				}

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Prior)
			{
				var index = (ActiveElement == null ? ElementStack._Elements.Count - 1 : ActiveElement.Z) + 1;

				if (index > ElementStack._Elements.Count - 1)
				{
					index = 0;
				}

				SetActiveElement((BaseElement)ElementStack._Elements[index], true);

				e.Handled = true;
			}
			/*
			if (Decimal.Compare(ArrowKeyDelta, new decimal(10)) > 0)
			{
				ArrowKeyDelta = new decimal(10);
			}
			*/
			if (flag)
			{
				CanvasImage.Invalidate();

				_PropertiesGrid.SelectedObjects = _PropertiesGrid.SelectedObjects;
			}
		}

		private void DesignerForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
			{
				CreateUndoPoint("Move element");
				//ArrowKeyDelta = new decimal(1);
			}
		}

		private void DesignerForm_Load(object sender, EventArgs e)
		{
			XMLSettings.AppSettings.Load();

			if (!File.Exists(Path.Combine(XMLSettings.AppSettings.ClientPath, "client.exe")))
			{
				var folderBrowserDialog = new FolderBrowserDialog
				{
					SelectedPath = Environment.SpecialFolder.ProgramFiles.ToString(),
					Description = @"Select the folder that contains a copy of Ultima Online."
				};

				do
				{
					if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
					{
						Close();
						return;
					}
				}
				while (!File.Exists(Path.Combine(folderBrowserDialog.SelectedPath, "client.exe")));

				XMLSettings.AppSettings.ClientPath = folderBrowserDialog.SelectedPath;
				XMLSettings.AppSettings.Save();
			}

			Files.CacheData = false;
			Files.SetMulPath(XMLSettings.AppSettings.ClientPath);

			Size = XMLSettings.AppSettings.DesignerFormSize;
			MaxUndoPoints = XMLSettings.AppSettings.UndoLevels;

			CanvasImage.Width = 1600;
			CanvasImage.Height = 1200;

			CenterToScreen();

			EnumeratePlugins();

			_Canvas = new Bitmap(CanvasImage.Width, CanvasImage.Height, PixelFormat.Format32bppRgb);

			Activate();

			GumpProperties = new GumpProperties();

			ElementStack.UpdateParent += ChangeActiveElementEventHandler;
			ElementStack.Repaint += RefreshView;

			Stacks.Clear();
			Stacks.Add(ElementStack);

			ChangeActiveStack(0);

			_RegisteredTypes.Clear();

			_RegisteredTypes.Add(typeof(LabelElement));
			_RegisteredTypes.Add(typeof(ImageElement));
			_RegisteredTypes.Add(typeof(TiledElement));
			_RegisteredTypes.Add(typeof(BackgroundElement));
			_RegisteredTypes.Add(typeof(AlphaElement));
			_RegisteredTypes.Add(typeof(CheckboxElement));
			_RegisteredTypes.Add(typeof(RadioElement));
			_RegisteredTypes.Add(typeof(ItemElement));
			_RegisteredTypes.Add(typeof(TextEntryElement));
			_RegisteredTypes.Add(typeof(ButtonElement));
			_RegisteredTypes.Add(typeof(HTMLElement));

			BuildToolbox();

			_SelectionFG = new Pen(Color.Blue, 2f);

			_SelectionBG = new LinearGradientBrush(new Rectangle(0, 0, 50, 50), Color.FromArgb(90, Color.Blue), Color.FromArgb(110, Color.Blue), LinearGradientMode.ForwardDiagonal)
			{
				WrapMode = WrapMode.TileFlipXY
			};

			CreateUndoPoint("Blank");

			_MenuEditUndo.Enabled = false;

			SplashBox.DisplaySplash();
		}

		private void ElementProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if (e.ChangedItem.PropertyDescriptor.Name == "Name")
			{
				_PropertiesSelector.Items.Clear();
				_PropertiesSelector.Items.AddRange(ElementStack.Elements.ToArray());
				_PropertiesSelector.SelectedItem = RuntimeHelpers.GetObjectValue(_PropertiesGrid.SelectedObject);
			}

			CanvasImage.Invalidate();

			CreateUndoPoint("Property Changed");
		}

		private void EnumeratePlugins()
		{
			var savedPlugins = GetPluginsToLoad();
			var loadAll = savedPlugins == null;

			PluginsInfo.Clear();

			if (savedPlugins != null)
			{
				PluginsInfo.UnionWith(savedPlugins);
			}

			var appendInfo = new StringBuilder();

			foreach (var file in Directory.GetFiles(Application.StartupPath, "*.dll", SearchOption.AllDirectories))
			{
				try
				{
					if (file.EndsWith("Ultima.dll", StringComparison.OrdinalIgnoreCase) || file.EndsWith("UOFont.dll", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}

					var asm = Assembly.LoadFrom(file);

					foreach (var type in asm.GetTypes())
					{
						if (type.IsAbstract)
						{
							continue;
						}

						if (type.IsSubclassOf(typeof(BasePlugin)))
						{
							try
							{
								var plugin = (BasePlugin)Activator.CreateInstance(type);

								if (plugin != null)
								{
									_AvailablePlugins.Add(plugin);

									appendInfo.AppendLine($"[✓]_ {plugin.Info.Name} {plugin.Info.Version} by {plugin.Info.AuthorName} ({plugin.Info.AuthorContact})");
									appendInfo.AppendLine();
								}
							}
							catch (Exception ex)
							{
								MessageBox.Show($"Error loading '{file}' ({type.FullName}):{Environment.NewLine}{ex.Message}");
							}
						}

						if (type.IsSubclassOf(typeof(BaseElement)))
						{
							_RegisteredTypes.Add(type);
						}
					}
				}
				catch
				{ }
			}

			if (PluginsInfo == null)
			{
				return;
			}

			foreach (var plugin in _AvailablePlugins)
			{
				if (!plugin.IsLoaded && (loadAll || PluginsInfo.Contains(plugin.Info)))
				{
					plugin.Load(this);

					_LoadedPlugins.Add(plugin);

					if (loadAll)
					{
						PluginsInfo.Add(plugin.Info);
					}
				}
			}

			// THIS LINE IS MISSING - add it:
			_AboutElementAppend = appendInfo.ToString();
		}

		private void GetContextMenu(ref BaseElement element, ContextMenuStrip menu)
		{
			var groupMenu = new ToolStripMenuItem("Grouping");
			var positionMenu = new ToolStripMenuItem("Positioning");
			var orderMenu = new ToolStripMenuItem("Order");
			var miscMenu = new ToolStripMenuItem("Misc");
			var editMenu = new ToolStripMenuItem("Edit");

			editMenu.DropDownItems.Add(new ToolStripMenuItem("Cut", null, MenuCut_Click));
			editMenu.DropDownItems.Add(new ToolStripMenuItem("Copy", null, MenuCopy_Click));
			editMenu.DropDownItems.Add(new ToolStripMenuItem("Paste", null, MenuPaste_Click));
			editMenu.DropDownItems.Add(new ToolStripMenuItem("Delete", null, MenuDelete_Click));

			menu.Items.Add(editMenu);
			menu.Items.Add(new ToolStripSeparator());
			menu.Items.Add(groupMenu);
			menu.Items.Add(positionMenu);
			menu.Items.Add(orderMenu);
			menu.Items.Add(new ToolStripSeparator());
			menu.Items.Add(miscMenu);

			if (ElementStack.GetSelectedElements().Count() > 1)
			{
				groupMenu.DropDownItems.Add(new ToolStripMenuItem("Create Group", null, MenuGroupCreate_Click));
			}

			if (element != null)
			{
				element.AddContextMenus(ref groupMenu, ref positionMenu, ref orderMenu, ref miscMenu);

				foreach (var plugin in _LoadedPlugins)
				{
					plugin.AddContextMenus(element, ref groupMenu, ref positionMenu, ref orderMenu, ref miscMenu);
				}
			}

			if (groupMenu.DropDownItems.Count == 0)
			{
				groupMenu.Enabled = false;
			}

			if (positionMenu.DropDownItems.Count == 0)
			{
				positionMenu.Enabled = false;
			}

			if (orderMenu.DropDownItems.Count == 0)
			{
				orderMenu.Enabled = false;
			}

			if (miscMenu.DropDownItems.Count == 0)
			{
				miscMenu.Enabled = false;
			}
		}

		public BaseElement GetLastSelectedControl()
		{
			return ElementStack.Elements.LastOrDefault();
		}

		private PluginInfo[] GetPluginsToLoad()
		{
			var jsonPath = Path.Combine(Application.StartupPath, "LoadInfo.json");

			if (File.Exists(jsonPath))
			{
				try
				{
					var json = File.ReadAllText(jsonPath);
					return System.Text.Json.JsonSerializer.Deserialize<PluginInfo[]>(json) ?? Array.Empty<PluginInfo>();
				}
				catch { }
			}

			// No config file — return empty; all discovered plugins will be loaded below
			return null;
		}

		private Rectangle GetPositiveRect(Rectangle rect)
		{
			if (rect.Height < 0)
			{
				rect.Height = Math.Abs(rect.Height);
				rect.Y -= rect.Height;
			}

			if (rect.Width < 0)
			{
				rect.Width = Math.Abs(rect.Width);
				rect.X -= rect.Width;
			}

			return rect;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_Components?.Dispose();
			}

			base.Dispose(disposing);
		}

		private sealed class DeserializationBinder : SerializationBinder
		{
			public static readonly DeserializationBinder Instance = new DeserializationBinder();

			private DeserializationBinder()
			{ }

			public override Type BindToType(string assemblyName, string typeName)
			{
				var asm = Assembly.GetExecutingAssembly();

				if (assemblyName.Contains("GumpStudioCore"))
				{
					assemblyName = asm.FullName;
				}

				return Type.GetType($"{typeName}, {assemblyName}");
			}
		}

		public void LoadFrom(string Path)
		{
			_StatusLabel.Text = "Loading gump...";

			Stacks.Clear();

			_TabPages.TabPages.Clear();

			try
			{
				using (var fileStream = new FileStream(Path, FileMode.Open))
				{
					var binaryFormatter = new BinaryFormatter { Binder = DeserializationBinder.Instance };

					var list = (ICollection)binaryFormatter.Deserialize(fileStream);

					Stacks.Clear();
					Stacks.AddRange(list.Cast<GroupElement>());

					try
					{
						GumpProperties = (GumpProperties)binaryFormatter.Deserialize(fileStream);
					}
					catch (Exception ex)
					{
						GumpProperties = new GumpProperties();

						MessageBox.Show(ex.InnerException.Message);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			SetActiveElement(null, true);
			RefreshElementList();

			var page = -1;

			foreach (var stack in Stacks)
			{
				_TabPages.TabPages.Add(new TabPage((++page).ToString()));
			}

			NormalizeNames();

			foreach (var o in AllElements)
			{
				o.Selected = false;
			}

			ChangeActiveStack(0);

			ElementStack.UpdateParent += ChangeActiveElementEventHandler;
			ElementStack.Repaint += RefreshView;

			_StatusBar.Text = String.Empty;
		}

		private void MenuMiscLoadGumpling_Click(object sender, EventArgs e)
		{
			_OpenDialog.Filter = "Gumpling (*.gumpling)|*.gumpling|Gump (*.gump)|*.gump";

			if (_OpenDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			try
			{
				using (var fileStream = new FileStream(_OpenDialog.FileName, FileMode.Open))
				{
					var bin = new BinaryFormatter
					{
						Binder = DeserializationBinder.Instance
					};

					var groupElement = (GroupElement)bin.Deserialize(fileStream);

					groupElement._IsBaseWindow = false;
					groupElement.Selected = false;

					groupElement.RecalculateBounds();

					groupElement.Location = Point.Empty;

					AddElement(groupElement);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			NormalizeNames();
		}

		private void MenuAddPage_Click(object sender, EventArgs e)
		{
			AddPage();
			CreateUndoPoint("Add page");
		}

		private void MenuCopy_Click(object sender, EventArgs e)
		{
			Copy();
		}

		private void MenuCut_Click(object sender, EventArgs e)
		{
			Cut();
			CreateUndoPoint();
		}

		private void MenuDataFile_Click(object sender, EventArgs e)
		{
			var folderBrowserDialog = new FolderBrowserDialog
			{
				SelectedPath = Environment.SpecialFolder.ProgramFiles.ToString(),
				Description = @"Select the folder that contains a copy of Ultima Online."
			};

			do
			{
				if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
			}
			while (!File.Exists(Path.Combine(folderBrowserDialog.SelectedPath, "client.exe")));

			XMLSettings.AppSettings.ClientPath = folderBrowserDialog.SelectedPath;
			XMLSettings.AppSettings.Save();

			MessageBox.Show("Changes will be applied after restarting Gump Studio.", "Data Files");
		}

		private void MenuDelete_Click(object sender, EventArgs e)
		{
			DeleteSelectedElements();
		}

		private void MenuEditRedo_Click(object sender, EventArgs e)
		{
			Redo();
		}

		private void MenuEditUndo_Click(object sender, EventArgs e)
		{
			Undo();
		}

		private void MenuFileExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MenuFileNew_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show("Are you sure you want to start a new gump?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

			if (result == DialogResult.OK)
			{
				ClearGump();
			}
		}

		private void MenuFileOpen_Click(object sender, EventArgs e)
		{
			_OpenDialog.CheckFileExists = true;
			_OpenDialog.Filter = @"Gump|*.gump";

			if (_OpenDialog.ShowDialog() == DialogResult.OK)
			{
				LoadFrom(_OpenDialog.FileName);

				_FileName = Path.GetFileName(_OpenDialog.FileName);
				Text = $"Gump Studio ({_FileName})";
			}

			CanvasImage.Invalidate();
		}

		private void MenuFileSave_Click(object sender, EventArgs e)
		{
			_SaveDialog.AddExtension = true;
			_SaveDialog.DefaultExt = "gump";
			_SaveDialog.Filter = "Gump|*.gump";

			if (_SaveDialog.ShowDialog() == DialogResult.OK)
			{
				SaveTo(_SaveDialog.FileName);

				_FileName = Path.GetFileName(_SaveDialog.FileName);
				Text = $"Gump Studio ({_FileName})";
			}
		}

		private void MenuGroupCreate_Click(object sender, EventArgs e)
		{
			var elements = new List<BaseElement>(ElementStack.GetSelectedElements());

			if (elements.Count >= 2)
			{
				var groupElement = new GroupElement(ElementStack, null, "New Group");

				foreach (var element in elements)
				{
					groupElement.AddElement(element);

					ElementStack.RemoveElement(element);
					ElementStack.RemoveEvents(element);
				}

				AddElement(groupElement);

				CanvasImage.Invalidate();
			}

			CreateUndoPoint();
		}

		private void MenuHelpAbout_Click(object sender, EventArgs e)
		{
			var aboutBox = new AboutBox();

			aboutBox.SetText(_AboutElementAppend);
			aboutBox.ShowDialog();
		}

		private void MenuImportGumpling_Click(object sender, EventArgs e)
		{
			_OpenDialog.Filter = @"Gumpling (*.gumpling)|*.gumpling|Gump (*.gump)|*.gump";

			if (_OpenDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			using (var fileStream = new FileStream(_OpenDialog.FileName, FileMode.Open))
			{
				var bin = new BinaryFormatter
				{
					Binder = DeserializationBinder.Instance
				};

				var gumpling = (GroupElement)bin.Deserialize(fileStream);

				gumpling._IsBaseWindow = false;
				gumpling.Selected = false;

				gumpling.RecalculateBounds();

				gumpling.Location = Point.Empty;

				UncategorizedFolder.AddItem(new TreeGumpling(Path.GetFileName(_OpenDialog.FileName), gumpling));
			}

			BuildGumplingTree();
		}

		private void MenuMiscExportGumpling_Click(object sender, EventArgs e)
		{
			var selected = ElementStack.GetSelectedElements().ToList();

			if (selected.Count == 0)
			{
				MessageBox.Show("Please select one or more elements to export as a gumpling.", "Export Gumpling", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			_SaveDialog.Filter = "Gumpling|*.gumpling";
			_SaveDialog.AddExtension = true;
			_SaveDialog.DefaultExt = "gumpling";

			if (_SaveDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			try
			{
				// Wrap selected elements into a GroupElement for export
				var exportGroup = new GroupElement(null, null, Path.GetFileNameWithoutExtension(_SaveDialog.FileName));

				foreach (var element in selected)
				{
					exportGroup.AddElement((BaseElement)element.Clone());
				}

				exportGroup._IsBaseWindow = false;
				exportGroup.RecalculateBounds();
				exportGroup.Location = Point.Empty;

				using (var fileStream = new FileStream(_SaveDialog.FileName, FileMode.Create))
				{
					var bin = new BinaryFormatter
					{
						Binder = DeserializationBinder.Instance
					};

					bin.Serialize(fileStream, exportGroup);
				}

				_StatusLabel.Text = $"Exported gumpling to {Path.GetFileName(_SaveDialog.FileName)}";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Export Gumpling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void MenuPageClear_Click(object sender, EventArgs e)
		{
			ElementStack = new GroupElement(null, null, "Element Stack", true);

			CreateUndoPoint("Clear Page");
		}

		private void MenuPageDelete_Click(object sender, EventArgs e)
		{
			if (_TabPages.SelectedIndex == 0)
			{
				MessageBox.Show("First page cannot be deleted.");
			}
			else
			{
				var selected = _TabPages.SelectedIndex;

				Stacks.RemoveAt(selected);

				_TabPages.TabPages.RemoveAt(selected);

				var index = -1;

				foreach (TabPage page in _TabPages.TabPages)
				{
					page.Text = (++index).ToString();
				}

				ChangeActiveStack(Math.Min(selected, _TabPages.TabCount - 1));

				CreateUndoPoint("Delete Page");
			}
		}

		private void MenuPageInsert_Click(object sender, EventArgs e)
		{
			if (_TabPages.SelectedIndex == 0)
			{
				MessageBox.Show("First page cannot be moved.");
			}
			else
			{
				Stacks.Insert(_TabPages.SelectedIndex, new GroupElement(null, null, "Element Stack", true));

				_TabPages.TabPages.Insert(_TabPages.SelectedIndex, new TabPage());

				var index = -1;

				foreach (TabPage page in _TabPages.TabPages)
				{
					page.Text = (++index).ToString();
				}

				ChangeActiveStack(_TabPages.SelectedIndex);

				CreateUndoPoint("Insert Page");
			}
		}

		private void MenuPaste_Click(object sender, EventArgs e)
		{
			Paste();
			CreateUndoPoint();
		}

		private void MenuPluginManager_Click(object sender, EventArgs e)
		{
			new PluginManager()
			{
				AvailablePlugins = _AvailablePlugins,
				LoadedPlugins = _LoadedPlugins,
				OrderList = PluginsInfo,
				MainForm = this
			}.ShowDialog();
		}

		private void MenuSelectAll_Click(object sender, EventArgs e)
		{
			SelectAll();
		}

		private void MenuShowPage0_Click(object sender, EventArgs e)
		{
			_MenuPageShow0.Checked = _ShowPage0 = !_ShowPage0;

			CanvasImage.Refresh();
		}

		public void Paste()
		{
			try
			{
				var obj = Clipboard.GetDataObject();
				var data = (ICollection)(obj.GetData(typeof(ArrayList)) ?? obj.GetData(typeof(BaseElement[])));

				if (data != null)
				{
					SetActiveElement(null, true);

					foreach (var element in data.OfType<BaseElement>())
					{
						element.Selected = true;

						ElementStack.AddElement(element);
					}

					NormalizeNames();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			CanvasImage.Invalidate();
		}

		private void ImageCanvas_MouseDown(object sender, MouseEventArgs e)
		{
			CanvasFocus.Focus();

			var point = _Anchor = new Point(e.X, e.Y);

			var element = ElementStack.GetElementFromPoint(point);

			if (ActiveElement != null && ActiveElement.HitTest(point) != MoveType.None)
			{
				element = ActiveElement;
			}

			if (element != null)
			{
				_MoveMode = element.HitTest(point);

				if (ActiveElement != null && ActiveElement.HitTest(point) == MoveType.None)
				{
					if (element.Selected)
					{
						if ((ModifierKeys & Keys.Control) > Keys.None)
						{
							element.Selected = false;
						}
						else
						{
							SetActiveElement(element, false);
						}
					}
					else
					{
						SetActiveElement(element, !ModifierKeys.HasFlag(Keys.Control));
					}
				}
				else if (ActiveElement == null)
				{
					SetActiveElement(element, false);
				}
				else if (ActiveElement != null && ModifierKeys.HasFlag(Keys.Control))
				{
					ActiveElement.Selected = false;

					var first = ElementStack.GetSelectedElements().FirstOrDefault();

					if (first != null)
					{
						SetActiveElement(first, false);
					}
					else
					{
						SetActiveElement(null, true);

						_MoveMode = MoveType.None;
					}
				}
			}
			else
			{
				_MoveMode = MoveType.None;

				if (e.Button.HasFlag(MouseButtons.Left))
				{
					SetActiveElement(null, !ModifierKeys.HasFlag(Keys.Control));
				}
			}

			CanvasImage.Invalidate();

			_LastPosition = point;

			if (ActiveElement != null)
			{
				_AnchorOffset.Width = ActiveElement.X - point.X;
				_AnchorOffset.Height = ActiveElement.Y - point.Y;
			}

			_ElementChanged = false;
			_MoveCount = 0;
		}

		private void ImageCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			var point = new Point(e.X, e.Y);

			var element = ElementStack.GetElementFromPoint(point);

			if (ActiveElement != null && ActiveElement.HitTest(point) != MoveType.None)
			{
				element = ActiveElement;
			}

			if (_MoveMode == MoveType.Move)
			{
				point.Offset(_AnchorOffset.Width, _AnchorOffset.Height);
			}

			var args = new MouseMovementEventArgs(element)
			{
				Keys = ModifierKeys,
				Button = e.Button,
				Location = point,
				Mode = _MoveMode
			};

			foreach (var plugin in _LoadedPlugins)
			{
				plugin.MouseMovement(args);
			}

			point = args.Location;

			if (_MoveMode == MoveType.None && Math.Abs(point.X - _LastPosition.X) > 0 && Math.Abs(point.Y - _LastPosition.Y) > 0)
			{
				_MoveMode = MoveType.SelectionBox;
			}

			if (e.Button != MouseButtons.Left)
			{
				if (element != null)
				{
					switch (element.HitTest(point))
					{
						case MoveType.ResizeTopLeft: Cursor = Cursors.SizeNWSE; break;
						case MoveType.ResizeBottomRight: Cursor = Cursors.SizeNWSE; break;
						case MoveType.ResizeTopRight: Cursor = Cursors.SizeNESW; break;
						case MoveType.ResizeBottomLeft: Cursor = Cursors.SizeNESW; break;
						case MoveType.ResizeLeft: Cursor = Cursors.SizeWE; break;
						case MoveType.ResizeRight: Cursor = Cursors.SizeWE; break;
						case MoveType.ResizeTop: Cursor = Cursors.SizeNS; break;
						case MoveType.ResizeBottom: Cursor = Cursors.SizeNS; break;
						case MoveType.Move: Cursor = Cursors.SizeAll; break;
						default: Cursor = Cursors.Default; break;
					}
				}
				else
				{
					Cursor = Cursors.Default;
				}
			}
			else
			{
				++_MoveCount;

				if (_MoveCount > 100)
				{
					_MoveCount = 2;
				}

				var rectangle = new Rectangle(0, 0, CanvasImage.Width, CanvasImage.Height);

				Cursor.Clip = CanvasImage.RectangleToScreen(rectangle);

				if (_MoveMode != MoveType.None)
				{
					switch (_MoveMode)
					{
						case MoveType.ResizeTopLeft: Cursor = Cursors.SizeNWSE; break;
						case MoveType.ResizeBottomRight: Cursor = Cursors.SizeNWSE; break;
						case MoveType.ResizeTopRight: Cursor = Cursors.SizeNESW; break;
						case MoveType.ResizeBottomLeft: Cursor = Cursors.SizeNESW; break;
						case MoveType.ResizeLeft: Cursor = Cursors.SizeWE; break;
						case MoveType.ResizeRight: Cursor = Cursors.SizeWE; break;
						case MoveType.ResizeTop: Cursor = Cursors.SizeNS; break;
						case MoveType.ResizeBottom: Cursor = Cursors.SizeNS; break;
						case MoveType.Move: Cursor = Cursors.SizeAll; break;
						default: Cursor = Cursors.Default; break;
					}

					if (_MoveCount > 1)
					{
						_ElementChanged = true;
					}
				}

				switch (_MoveMode)
				{
					case MoveType.SelectionBox:
						{
							rectangle = new Rectangle(_Anchor, new Size(point.X - _Anchor.X, point.Y - _Anchor.Y));

							_SelectionRect = GetPositiveRect(rectangle);

							_ShowSelectionRect = true;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeTopLeft:
						{
							point.Offset(3, 0);

							var loc = new Point(ActiveElement.X + ActiveElement.Width, ActiveElement.Y + ActiveElement.Height);

							ActiveElement.Location = point;

							var p = ActiveElement.Location;
							var s = ActiveElement.Size;

							s.Width = loc.X - point.X;
							s.Height = loc.Y - point.Y;

							if (s.Width < 1)
							{
								p.X = loc.X - 1;
								s.Width = 1;
							}

							if (s.Height < 1)
							{
								p.Y = loc.Y - 1;
								s.Height = 1;
							}

							ActiveElement.Size = s;
							ActiveElement.Location = p;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeTopRight:
						{
							point.Offset(-3, 0);

							var loc = new Point(ActiveElement.X + ActiveElement.Width, ActiveElement.Y + ActiveElement.Height);

							var p = ActiveElement.Location;

							p.Y = point.Y;

							ActiveElement.Location = p;

							var s = ActiveElement.Size;

							s.Height = loc.Y - point.Y;
							s.Width = Math.Max(1, point.X - ActiveElement.X);

							if (s.Height < 1)
							{
								p.Y = loc.Y - 1;
								s.Height = 1;
							}

							p.X = ActiveElement.Location.X;

							ActiveElement.Size = s;
							ActiveElement.Location = p;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeBottomRight:
						{
							if (ActiveElement == null)
							{
								break;
							}

							point.Offset(-3, -3);

							var s = ActiveElement.Size;

							s.Width = Math.Max(1, point.X - ActiveElement.X);
							s.Height = Math.Max(1, point.Y - ActiveElement.Y);

							ActiveElement.Size = s;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeBottomLeft:
						{
							if (ActiveElement == null)
							{
								break;
							}

							point.Offset(0, -3);

							var loc = new Point(ActiveElement.X + ActiveElement.Width, ActiveElement.Y + ActiveElement.Height);

							var p = ActiveElement.Location;

							p.X = point.X;

							ActiveElement.Location = p;

							var s = ActiveElement.Size;

							s.Width = loc.X - point.X;
							s.Height = Math.Max(1, point.Y - ActiveElement.Y);

							if (s.Width < 1)
							{
								p.X = loc.X - 1;
								s.Width = 1;
							}

							p.Y = ActiveElement.Y;

							ActiveElement.Size = s;
							ActiveElement.Location = p;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.Move:
						{
							if (ActiveElement == null)
							{
								break;
							}

							var p = ActiveElement.Location;

							ActiveElement.Location = point;

							var dx = ActiveElement.X - p.X;
							var dy = ActiveElement.Y - p.Y;

							foreach (var o in ElementStack.GetSelectedElements())
							{
								if (o != ActiveElement)
								{
									var loc = o.Location;

									loc.Offset(dx, dy);

									o.Location = loc;
								}
							}

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeLeft:
						{
							point.Offset(3, 0);

							var loc = new Point(ActiveElement.X + ActiveElement.Width, ActiveElement.Y + ActiveElement.Height);

							var y = ActiveElement.Y;

							ActiveElement.Location = point;

							var p = ActiveElement.Location;
							var s = ActiveElement.Size;

							s.Width = loc.X - point.X;

							if (s.Width < 1)
							{
								p.X = loc.X - 1;
								s.Width = 1;
							}

							p.Y = y;

							ActiveElement.Size = s;
							ActiveElement.Location = p;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeTop:
						{
							point.Offset(0, 3);

							var loc = new Point(ActiveElement.X + ActiveElement.Width, ActiveElement.Y + ActiveElement.Height);

							var x = ActiveElement.X;

							ActiveElement.Location = point;

							var p = ActiveElement.Location;
							var s = ActiveElement.Size;

							s.Height = loc.Y - point.Y;

							if (s.Height < 1)
							{
								p.Y = loc.Y - 1;
								s.Height = 1;
							}

							p.X = x;

							ActiveElement.Size = s;
							ActiveElement.Location = p;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeRight:
						{
							point.Offset(-3, 0);

							var s = ActiveElement.Size;

							s.Width = Math.Max(1, point.X - ActiveElement.X);

							ActiveElement.Size = s;

							CanvasImage.Invalidate();
						}
						break;

					case MoveType.ResizeBottom:
						{
							point.Offset(0, -3);

							var s = ActiveElement.Size;

							s.Height = Math.Max(1, point.Y - ActiveElement.Y);

							ActiveElement.Size = s;

							CanvasImage.Invalidate();
						}
						break;
				}
			}

			_LastPosition = point;
		}

		private void ImageCanvas_MouseUp(object sender, MouseEventArgs e)
		{
			var rectangle = Rectangle.Empty;

			var point = new Point(e.X, e.Y);

			ElementStack.GetElementFromPoint(point);

			_ShowSelectionRect = false;

			Cursor.Clip = rectangle;

			if (_MoveMode == MoveType.SelectionBox)
			{
				BaseElement selected = null;

				foreach (var element in ElementStack.Elements)
				{
					if (element.ContainsTest(_SelectionRect))
					{
						element.Selected = true;
						selected = element;
					}
					else if (!ModifierKeys.HasFlag(Keys.Control))
					{
						element.Selected = false;
					}
				}

				SetActiveElement(selected, false);
			}

			if (_MoveMode != MoveType.None && _MoveMode != MoveType.SelectionBox && _ElementChanged)
			{
				CreateUndoPoint("Element Moved");
				_ElementChanged = false;
			}

			if (e.Button.HasFlag(MouseButtons.Right))
			{
				var menu = _ContextMenu;
				var active = ActiveElement;

				GetContextMenu(ref active, menu);

				ActiveElement = active;

				menu.Show(CanvasImage, point);

				ClearContextMenu(menu);
			}

			SetActiveElement(ActiveElement, false);

			CanvasImage.Invalidate();

			_MoveMode = MoveType.None;
			_AnchorOffset = Size.Empty;
		}

		private void ImageCanvas_Paint(object sender, PaintEventArgs e)
		{
			Render(e.Graphics);
		}

		private void CanvasScroller_MouseLeave(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
		}

		public void RebuildTabPages()
		{
			_TabPages.TabPages.Clear();

			var num = -1;

			foreach (var stack in Stacks)
			{
				_TabPages.TabPages.Add(new TabPage((++num).ToString()));

				if (ElementStack == stack)
				{
					_TabPages.SelectedIndex = num;
				}
			}
		}

		public void Redo()
		{
			if (_CurrentUndoPoint < _UndoPoints.Count)
			{
				RevertToUndoPoint(++_CurrentUndoPoint);
			}

			if (_CurrentUndoPoint == _UndoPoints.Count - 1)
			{
				_MenuEditRedo.Enabled = false;
			}

			_MenuEditUndo.Enabled = true;
		}

		public void RefreshElementList()
		{
			_PropertiesSelector.Items.Clear();
			_PropertiesSelector.Items.AddRange(ElementStack.Elements.ToArray());
		}

		public void RefreshView(object sender)
		{
			RefreshElementList();

			_PropertiesSelector.SelectedItem = ActiveElement;

			var selected = ElementStack.GetSelectedElements().ToArray();

			if (selected.Length > 1)
			{
				_PropertiesGrid.SelectedObjects = selected;
			}
			else
			{
				_PropertiesGrid.SelectedObject = ActiveElement;
			}
		}

		private void Render(Graphics target)
		{
			var g = Graphics.FromImage(_Canvas);

			if (!PluginClearsCanvas)
			{
				g.Clear(Color.Black);
			}

			var hookPreRender = HookPreRender;

			hookPreRender?.Invoke(_Canvas);

			if (Stacks.Count > 0 && _ShowPage0 && Stacks[0] is BaseElement e && e != ElementStack)
			{
				e.Render(g);
			}

			ElementStack.Render(g);

			foreach (var element in ElementStack.Elements)
			{
				if (element.Selected && element != ActiveElement)
				{
					element.DrawBoundingBox(g, false);
				}
			}

			ActiveElement?.DrawBoundingBox(g, true);

			if (_ShowSelectionRect)
			{
				g.FillRectangle(_SelectionBG, _SelectionRect);
				g.DrawRectangle(_SelectionFG, _SelectionRect);
			}

			HookPostRender?.Invoke(_Canvas);

			g.Dispose();

			target.DrawImage(_Canvas, 0, 0);
		}

		public void RevertToUndoPoint(int index)
		{
			var undoPoint = (UndoPoint)_UndoPoints[index];

			GumpProperties = (GumpProperties)undoPoint.GumpProperties.Clone();

			Stacks.Clear();

			foreach (GroupElement group in undoPoint.Stack)
			{
				var clone = (GroupElement)group.Clone();

				Stacks.Add(clone);

				if (undoPoint.ElementStack == group)
				{
					ElementStack = clone;
				}
			}

			RebuildTabPages();

			CanvasImage.Invalidate();

			SetActiveElement(null, true);

			_CurrentUndoPoint = index;
		}

		public void SaveTo(string path)
		{
			_StatusLabel.Text = $@"Saving gump...";

			ElementStack.UpdateParent -= ChangeActiveElementEventHandler;
			ElementStack.Repaint -= RefreshView;

			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				var bin = new BinaryFormatter
				{
					Binder = DeserializationBinder.Instance
				};

				bin.Serialize(fileStream, Stacks);
				bin.Serialize(fileStream, GumpProperties);
			}

			ElementStack.UpdateParent += ChangeActiveElementEventHandler;
			ElementStack.Repaint += RefreshView;

			_StatusBar.Text = String.Empty;
		}

		public void SelectAll()
		{
			foreach (var selectedElement in ElementStack.GetSelectedElements())
			{
				selectedElement.Selected = true;
			}

			CanvasImage.Invalidate();
		}

		public void SetActiveElement(BaseElement e)
		{
			SetActiveElement(e, false);
		}

		public void SetActiveElement(BaseElement element, bool deselectOthers)
		{
			if (deselectOthers)
			{
				foreach (var e in ElementStack.Elements)
				{
					e.Selected = false;
				}
			}

			if (ActiveElement != element)
			{
				RefreshElementList();

				ActiveElement = element;

				_PropertiesSelector.SelectedItem = element;

				if (element != null)
				{
					element.Selected = true;
				}
			}

			var selected = ElementStack.GetSelectedElements().ToArray();

			if (selected.Length > 1)
			{
				_PropertiesGrid.SelectedObjects = selected;
			}
			else if (element != null)
			{
				_PropertiesGrid.SelectedObject = element;
			}
			else
			{
				_PropertiesGrid.SelectedObject = GumpProperties;
			}
		}

		public Point SnapLocToGrid(Point position, Size gridSize)
		{
			var point = position;

			point.X = point.X / gridSize.Width * gridSize.Width;
			point.Y = point.Y / gridSize.Height * gridSize.Height;

			return point;
		}

		private void TabPages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_TabPages.SelectedIndex >= 0)
			{
				ChangeActiveStack(_TabPages.SelectedIndex);
			}

			RefreshElementList();
		}

		private void Gumplings_DoubleClick(object sender, EventArgs e)
		{
			if (_Gumplings.SelectedNode.Tag == null || !(_Gumplings.SelectedNode.Tag is TreeGumpling tg))
			{
				return;
			}

			var group = (GroupElement)tg.Gumpling.Clone();

			group._IsBaseWindow = false;

			group.RecalculateBounds();

			group.Location = Point.Empty;

			AddElement(group);
		}

		private void Gumplings_MouseUp(object sender, MouseEventArgs e)
		{
			_Gumplings.SelectedNode = _Gumplings.GetNodeAt(new Point(e.X, e.Y));
		}

		public void Undo()
		{
			RevertToUndoPoint(--_CurrentUndoPoint);

			if (_CurrentUndoPoint <= 0)
			{
				_MenuEditUndo.Enabled = false;
			}

			_MenuEditRedo.Enabled = true;
		}

		public void WritePluginsToLoad()
		{
			var path = Path.Combine(Application.StartupPath, "LoadInfo.json");

			if (PluginsInfo != null && PluginsInfo.Count > 0)
			{
				var json = System.Text.Json.JsonSerializer.Serialize(PluginsInfo.ToArray(), new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(path, json);
			}
			else if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public delegate void HookKeyDownEventHandler(object sender, ref KeyEventArgs e);
		public delegate void HookPostRenderEventHandler(Bitmap Target);
		public delegate void HookPreRenderEventHandler(Bitmap Target);
	}
}
