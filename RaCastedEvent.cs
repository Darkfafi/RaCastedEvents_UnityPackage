using RaCastedEvents.Core;
using System;
using System.Collections.Generic;

namespace RaCastedEvents
{
	public class RaCastedEvent : IRaCastedEvent
	{
		private readonly Dictionary<Type, IRaCastedEventModule> _typeToModulesMap = new Dictionary<Type, IRaCastedEventModule>();
		private readonly List<IRaCastedEventModule> _modules = new List<IRaCastedEventModule>();

		public bool Invoke(object data)
		{
			bool wasInvoked = false;
			IRaCastedEventModule[] modules = _modules.ToArray();
			for(int i = 0, c = modules.Length; i < c; i++)
			{
				wasInvoked |= modules[i].Invoke(data);
			}
			return wasInvoked;
		}

		public void RegisterMethod<T>(Action<T> method)
		{
			GetModule<T>().InvokedMethod += method;
		}

		public void UnregisterMethod<T>(Action<T> method)
		{
			GetModule<T>().InvokedMethod -= method;
		}

		public void Dispose()
		{
			for(int i = _modules.Count - 1; i >= 0; i--)
			{
				_modules[i].Dispose();
			}

			_modules.Clear();
			_typeToModulesMap.Clear();
		}

		private RaCastedEventModule<T> GetModule<T>()
		{
			Type key = typeof(T);
			RaCastedEventModule<T> module;

			if(_typeToModulesMap.TryGetValue(key, out IRaCastedEventModule rawModule))
			{
				module = (RaCastedEventModule<T>)rawModule;
			}
			else
			{
				_typeToModulesMap[key] = rawModule = module = new RaCastedEventModule<T>();
				_modules.Add(rawModule);
			}

			return module;
		}
	}

	public interface IRaCastedEvent : IRaCastedEventInvoker, IRaCastedEventSource, IDisposable
	{

	}

	public interface IRaCastedEventInvoker 
	{
		bool Invoke(object data);
	}

	public interface IRaCastedEventSource
	{
		void RegisterMethod<T>(Action<T> method);
		void UnregisterMethod<T>(Action<T> method);
	}
}
