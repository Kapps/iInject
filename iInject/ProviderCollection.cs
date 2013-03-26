using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Represents a collection of injection providers.
	/// </summary>
	public class ProviderCollection : KeyedCollection<string, IInjectionProvider> {

		/// <summary>
		/// Creates an empty ProviderCollection.
		/// </summary>
		public ProviderCollection() : base(StringComparer.InvariantCultureIgnoreCase) { }

		/// <summary>
		/// Returns the name of the provider as a key for this item.
		/// </summary>
		protected override string GetKeyForItem(IInjectionProvider item) {
			return item.Name;
		}
	}
}

