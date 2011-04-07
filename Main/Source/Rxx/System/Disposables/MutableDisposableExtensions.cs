using System.Diagnostics.Contracts;

namespace System.Disposables
{
	public static class MutableDisposableExtensions
	{
		/// <seealso href="http://social.msdn.microsoft.com/Forums/en-IE/rx/thread/4e15feae-9c4c-4962-af32-95dde1420dda#4d5fe8c8-e5e8-4ee7-93ca-b48b6a56b8af">
		/// Double indirection pattern example in Rx
		/// </seealso>
		public static void SetDisposableIndirectly(this MutableDisposable disposable, Func<IDisposable> factory)
		{
			Contract.Requires(disposable != null);
			Contract.Requires(factory != null);

			var indirection = new MutableDisposable();

			disposable.Disposable = indirection;

			indirection.Disposable = factory();
		}
	}
}
