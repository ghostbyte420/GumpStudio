using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace GumpStudio
{
	public enum ConfigFormat
	{ 
		Xml,
		Bin
	}

	public abstract class BaseConfig
	{
		[NonSerialized, XmlIgnore]
			private PropertyEditor _Editor;

			private PropertyEditor Editor
				{
					get
					{
						if (_Editor == null || _Editor.IsDisposed)
						{
							_Editor = new PropertyEditor();
							_Editor.SourceObject = this;
							_Editor.Text = Name;
							_Editor.PropertyValueChanged += OnValueChanged;
							_Editor.FormClosing += OnEditorClosing;
						}
						return _Editor;
					}
				}

				[NonSerialized, XmlIgnore]
				private readonly Type _Type;

				[NonSerialized, XmlIgnore]
				private readonly PropertyInfo[] _Props;

				[NonSerialized, XmlIgnore]
				private XmlSerializer _XmlSerializer;

				[NonSerialized, XmlIgnore]
				private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

				[XmlIgnore, Browsable(false)]
				public virtual ConfigFormat Format => ConfigFormat.Xml;

				[XmlIgnore, Browsable(false)]
				public virtual string Name => _Type.Name;

				[XmlIgnore, Browsable(false)]
				public virtual string FileName => $"{_Type.DeclaringType?.Name ?? _Type.Namespace}.{_Type.Name}.{Format.ToString().ToLower()}";

				[XmlIgnore, Browsable(false)]
				public bool ChangesPending => Editor.ChangesPending;

				[Browsable(false)]
				public event PropertyValueChangedEventHandler ValueChanged
				{
					add => Editor.PropertyValueChanged += value;
					remove => Editor.PropertyValueChanged -= value;
				}

				public BaseConfig()
				{
					_Type = GetType();
					_Props = _Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
				}

		protected virtual void OnValueChanged(object s, PropertyValueChangedEventArgs e)
		{
		}

		protected virtual void OnEditorClosing(object sender, FormClosingEventArgs e)
		{
			if (ChangesPending)
			{
				Save();
			}
		}

		public override string ToString()
		{
			return Name ?? _Type.Name;
		}

		public void Close()
		{
			Close(true);
		}

		public void Close(bool save)
		{
			if (_Editor == null || _Editor.IsDisposed) return;

			if (!save)
			{
				_Editor.ChangesPending = false;
			}
			else if (ChangesPending)
			{
				Save();
			}

			_Editor.Close();
		}

		public void Edit()
		{
			Editor.Show(Form.ActiveForm);
		}

		public void Save()
		{
			switch (Format)
			{
				case ConfigFormat.Xml:
				{
					if (_XmlSerializer == null)
					{
						_XmlSerializer = new XmlSerializer(_Type);
					}

					using (var xml = new XmlTextWriter(FileName, Encoding.UTF8) { Formatting = Formatting.Indented })
					{
						_XmlSerializer.Serialize(xml, this);
					}
				}
				break;

				case ConfigFormat.Bin:
				{
					var json = JsonSerializer.Serialize(this, _Type, _jsonOptions);
					File.WriteAllText(FileName, json);
				}
				break;
			}
		}

		public void Load()
		{
			if (!File.Exists(FileName))
			{
				return;
			}

			object loaded = null;

			switch (Format)
			{
				case ConfigFormat.Xml:
				{
					if (_XmlSerializer == null)
					{
						_XmlSerializer = new XmlSerializer(_Type);
					}

					using (var xml = new XmlTextReader(FileName))
					{
						loaded = _XmlSerializer.Deserialize(xml);
					}
				}
				break;

				case ConfigFormat.Bin:
				{
					var json = File.ReadAllText(FileName);
					loaded = JsonSerializer.Deserialize(json, _Type, _jsonOptions);
				}
				break;
			}

			if (loaded == null)
			{
				return;
			}

			foreach (var p in _Props.Where(p => p.CanRead && p.CanWrite))
			{
				p.SetValue(this, p.GetValue(loaded));
			}
		}
	}
}
