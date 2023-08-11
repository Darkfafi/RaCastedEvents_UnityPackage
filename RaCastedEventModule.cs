using System;

namespace RaCastedEvents.Core
{
	public class RaCastedEventModule<T> : IRaCastedEventModule, IRaCastedEventCoreSource<T>
	{
		public event Action<T> InvokedMethod;

		public bool Invoke(object data)
		{
			if(data is T castedData && InvokedMethod != null)
			{
				InvokedMethod.Invoke(castedData);
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			InvokedMethod = null;
		}
	}

	public interface IRaCastedEventModule : IDisposable
	{
		bool Invoke(object data);
	}

	public interface IRaCastedEventCoreSource<T>
	{
		event Action<T> InvokedMethod;
	}
}
