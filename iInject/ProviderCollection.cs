using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace iInject {
	/// <summary>
	/// Represents a collection of injection providers.
	/// </summary>
	public class ProviderCollection : KeyedCollection<string, IInjectionProvider> {

		// TODO: Just serialize the type instead, don't even bother with ProviderCollection.

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

		/*/// <summary>
		/// Parses a ProviderCollection from the given JSON input.
		/// Each provider has a 'Name' and 'Options' properties.
		/// </summary>
		public static ProviderCollection Parse(string Json) {
			ProviderCollection Result = new ProviderCollection();
			JObject Input = JObject.Parse(Json);
			foreach(var ProviderObj in Input) {
				
				foreach(var Child in ProviderObj["options"].Children()) {
					
				}
				
			}
		}*/
	}
}

