using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides the base interface for all provider implementations.
	/// </summary>
	/// <remarks>
	/// The following values will be passed in when a constructor is found that accepts them:
	///		InjectionSession
	///		ProviderOptions
	/// </remarks>
	public interface IInjectionProvider {
		/// <summary>
		/// Gets the name of this provider.
		/// This result must be immutable, unique (case insensitive), and require no initialization logic.
		/// It is possible for this method to be called on an instance that has not had it's constructor called.
		/// </summary>
		string Name { get; }
	}
}
