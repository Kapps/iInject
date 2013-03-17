using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Indicates a single control inside a web form.
	/// </summary>
	public class WebFormControl {
		
		/// <summary>
		/// Gets the name of this control.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets or sets the value for this control.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Gets the type of this control, such as 'text' or 'password'.
		/// If no input type is specified, this defaults to text.
		/// </summary>
		public string Type { get; private set; }

		public WebFormControl(string Name, string Type, string Value) {
			this.Name = Name;
			this.Type = Type;
			this.Value = Value;
		}
	}
}
