using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace iInject {
	
	/// <summary>
	/// Represents a single form returned in an html page, with options to set control values.
	/// </summary>
	public class WebForm {

		/// <summary>
		/// Gets the name of this form. This is never null, but may be auto-generated.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the Uri to send the response to.
		/// </summary>
		public Uri Target { get; private set; }
		/// <summary>
		/// Gets the method to use when sending the response.
		/// </summary>
		public HttpMethod Method { get; private set; }
		/// <summary>
		/// Gets a list of controls contained by the form.
		/// </summary>
		public List<WebFormControl> Controls { get; private set; }

		public WebForm(string Name, Uri Target, HttpMethod Method, IEnumerable<WebFormControl> Controls) {
			this.Name = Name;
			this.Target = Target;
			this.Method = Method;
			this.Controls = Controls.ToList();
		}
		
	}
}
