using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a list of provider-specific options to use for a provider object.
	/// </summary>
	public class ProviderOptions {

		/// <summary>
		/// Creates a new ProviderOptions instance with no values set.
		/// </summary>
		public ProviderOptions() {

		}

		/// <summary>
		/// Gets or sets the option with the given name, case insensitive.
		/// </summary>
		public dynamic this[string Name] {
			get { return _Options[Name]; }
			set { _Options[Name] = value; }
		}

		private Dictionary<string, dynamic> _Options = new Dictionary<string, dynamic>(StringComparer.InvariantCultureIgnoreCase);
	}
}
