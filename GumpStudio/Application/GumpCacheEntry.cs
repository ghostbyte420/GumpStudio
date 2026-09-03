// Decompiled with JetBrains decompiler
// Type: GumpStudio.GumpCacheEntry
// Assembly: GumpStudioCore, Version=1.8.3024.24259, Culture=neutral, PublicKeyToken=null
// MVID: A77D32E5-7519-4865-AA26-DCCB34429732
// Assembly location: C:\GumpStudio_1_8_R3_quinted-02\GumpStudioCore.dll

using System;
using System.Drawing;
using System.Text.Json.Serialization;

namespace GumpStudio
{
	[Serializable]
	public class GumpCacheEntry
	{
		public int ID { get; set; }
		[NonSerialized]
		[JsonIgnore]
		public Image ImageCache;
		public string Name { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		[JsonIgnore]
		public Size Size
		{
			get => new Size(Width, Height);
			set { Width = value.Width; Height = value.Height; }
		}

		public override string ToString()
		{
			return ID.ToString();
		}
	}
}
