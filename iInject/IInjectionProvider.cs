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
	/// When the plugin handler creates a class that implements this interface,
	/// it will first scan for a constructor that takes in a ProviderOptions instance.
	/// If that exists, it will use that. Otherwise, it will use an empty constructor.
	/// </remarks>
	public interface IInjectionProvider {
		/// <summary>
		/// Gets the name of this provider.
		/// This result must be immutable and unique (case insensitive).
		/// </summary>
		string Name { get; }
	}
}
