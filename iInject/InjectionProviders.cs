using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Globally stores information about available injection providers and allows them to be created as desired.
	/// </summary>
	public static class InjectionProviders {

		/// <summary>
		/// Registers a new provider of the given type.
		/// If a provider with the same Name is already registered, it is overwritten.
		/// </summary>
		/// <param name="Type"></param>
		public static void RegisterProvider(Type Type) {
			Type ProviderBase = typeof(IInjectionProvider);
			if(!Type.GetInterfaces().Contains(ProviderBase))
				throw new ArgumentException("The type '" + Type.Name + "' did not implement the IInjectionProvider interface.");
			// This is a very sketchy workaround to get the name of a provider without having to create one.
			var UninitializedInstance = (IInjectionProvider)FormatterServices.GetUninitializedObject(Type);
			var Name = UninitializedInstance.Name;
			RegisteredProviders[Name] = Type;
		}

		/// <summary>
		/// Creates a new InjectionProvider for the provider registered with the given name.
		/// If the provider accepts an InjectionSession or ProviderOptions, they will be passed in.
		/// </summary>
		public static IInjectionProvider CreateProvider(string Name, InjectionSession Session, ProviderOptions Options) {
			var Type = RegisteredProviders[Name];
			var Constructor = GetBestConstructor(Type);
			var Arguments = MapConstructorArguments(Constructor, Session, Options);
			var Instance = (IInjectionProvider)Activator.CreateInstance(Type, Arguments);
			return Instance;
		}

		private static ConstructorInfo GetBestConstructor(Type Type) {
			ConstructorInfo Result = null;
			foreach(var Constructor in Type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
				if(Result != null && Result.GetParameters().Length > Constructor.GetParameters().Length)
					continue;
				var Arguments = MapConstructorArguments(Constructor, null, null);
				if(Arguments == null)
					continue;
				Result = Constructor;
			}
			if(Result == null)
				throw new MissingMethodException("Unable to get a constructor for '" + Type.Name + "'. See the remarks in IInjectionProvider for allowed parameters.");
			return Result;
		}

		private static object[] MapConstructorArguments(ConstructorInfo Constructor, InjectionSession Session, ProviderOptions Options) {
			List<object> Result = new List<object>();
			foreach(var Param in Constructor.GetParameters()) {
				if(Param.ParameterType == typeof(InjectionSession))
					Result.Add(Session);
				else if(Param.ParameterType == typeof(ProviderOptions))
					Result.Add(Options);
				else
					return null;
			}
			return Result.ToArray();
		}

		private static Dictionary<string, Type> RegisteredProviders = new Dictionary<string, Type>();
	}
}
