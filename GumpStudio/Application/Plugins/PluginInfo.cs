using System;
using System.Collections.Generic;

namespace GumpStudio.Plugins
{
	[Serializable]
	public sealed class PluginInfo : IEquatable<PluginInfo>
	{
		private int _Hash;

		public string Name { get; set; }
		public string Version { get; set; }
		public string AuthorName { get; set; }
		public string AuthorContact { get; set; }
		public string Description { get; set; }

		public PluginInfo() { }

		public PluginInfo(string name, string version, string authorName, string authorContact, string description)
		{
			Name = name;
			Version = version;
			AuthorName = authorName;
			AuthorContact = authorContact;
			Description = description;

			ComputeHash();
		}

		private void ComputeHash()
		{
			unchecked
			{
				var hash = 1;

				var comparer = EqualityComparer<string>.Default;

				hash = (hash * 397) ^ comparer.GetHashCode(Name ?? "");
				hash = (hash * 397) ^ comparer.GetHashCode(Version ?? "");
				hash = (hash * 397) ^ comparer.GetHashCode(AuthorName ?? "");
				hash = (hash * 397) ^ comparer.GetHashCode(AuthorContact ?? "");
				hash = (hash * 397) ^ comparer.GetHashCode(Description ?? "");

				_Hash = hash;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is PluginInfo info && Equals(info);
		}

		public bool Equals(PluginInfo info)
		{
			return GetHashCode() == info?.GetHashCode();
		}

		public override int GetHashCode()
		{
			if (_Hash == 0) ComputeHash();
			return _Hash;
		}

		public override string ToString()
		{
			return Name;
		}

		public static bool operator ==(PluginInfo left, PluginInfo right)
		{
			return left?._Hash == right?._Hash;
		}

		public static bool operator !=(PluginInfo left, PluginInfo right)
		{
			return left?._Hash != right?._Hash;
		}
	}
}