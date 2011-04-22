using System.Diagnostics.Contracts;

namespace System.IO
{
	public sealed class FileSystemNotification
	{
		#region Public Properties
		public WatcherChangeTypes Change
		{
			get
			{
				Contract.Ensures(Enum.IsDefined(typeof(WatcherChangeTypes), Contract.Result<WatcherChangeTypes>()));

				return change;
			}
		}

		public string Name
		{
			get
			{
				Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

				return name;
			}
		}

		public string FullPath
		{
			get
			{
				Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

				return fullPath;
			}
		}

		public string OldName
		{
			get
			{
				Contract.Ensures(Change != WatcherChangeTypes.Renamed || !string.IsNullOrEmpty(Contract.Result<string>()));

				return oldName;
			}
		}

		public string OldFullPath
		{
			get
			{
				Contract.Ensures(Change != WatcherChangeTypes.Renamed || !string.IsNullOrEmpty(Contract.Result<string>()));

				return oldFullPath;
			}
		}
		#endregion

		#region Private / Protected
		[ContractPublicPropertyName("Change")]
		private readonly WatcherChangeTypes change;
		private readonly string name, fullPath, oldName, oldFullPath;
		#endregion

		#region Constructors
		public FileSystemNotification(WatcherChangeTypes change, string name, string fullPath)
		{
			Contract.Requires(change != WatcherChangeTypes.Renamed);
			Contract.Requires(Enum.IsDefined(typeof(WatcherChangeTypes), change));
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Requires(!string.IsNullOrEmpty(fullPath));

			this.change = change;
			this.name = name;
			this.fullPath = fullPath;
		}

		public FileSystemNotification(string oldName, string oldFullPath, string name, string fullPath)
		{
			Contract.Requires(!string.IsNullOrEmpty(oldName));
			Contract.Requires(!string.IsNullOrEmpty(oldFullPath));
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Requires(!string.IsNullOrEmpty(fullPath));
			Contract.Ensures(change == WatcherChangeTypes.Renamed);

			this.change = WatcherChangeTypes.Renamed;
			this.name = name;
			this.fullPath = fullPath;
			this.oldName = oldName;
			this.oldFullPath = oldFullPath;
		}
		#endregion

		#region Methods
		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(WatcherChangeTypes), change));
			Contract.Invariant(!string.IsNullOrEmpty(name));
			Contract.Invariant(!string.IsNullOrEmpty(fullPath));
			Contract.Invariant(change != WatcherChangeTypes.Renamed || !string.IsNullOrEmpty(oldName));
			Contract.Invariant(change != WatcherChangeTypes.Renamed || !string.IsNullOrEmpty(oldFullPath));
		}

		public override string ToString()
		{
			if (change == WatcherChangeTypes.Renamed)
			{
				return "Renamed \"" + oldName + "\" to \"" + name + "\"";
			}
			else
			{
				return string.Concat(change, " ", name);
			}
		}
		#endregion
	}
}
