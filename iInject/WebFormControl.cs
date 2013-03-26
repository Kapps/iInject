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

		private static readonly HashSet<string> SpecialNames = new HashSet<string>() { "__EVENTVALIDATION", "__VIEWSTATE" };
		
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

		public override string ToString() {
			return "Control: " + this.Name + " = " + this.Value;
		}

		/// <summary>
		/// Indicates if this control is a special control that usually should not attempt to be exploited.
		/// An example is the viewstate control for ASP.NET applications.
		/// </summary>
		public bool IsSpecialControl() {
			return SpecialNames.Contains(this.Name);
		}

		/// <summary>
		/// Returns a default value that may be valid for this control.
		/// </summary>
		/// <param name="UseExisting">If true, Value is returned if it is not null or empty.</param>
		public string GenerateDefaultValue(bool UseExisting) {
			if(UseExisting && !String.IsNullOrWhiteSpace(Value))
				return Value;
			
			switch(Type.ToLower()) {
				case "text":
				default:
					string Result = "";
					int NumChars = rnd.Next(3, 9);
					for(int i = 0; i < NumChars; i++)
						Result += (char)rnd.Next((int)'a', (int)'z' + 1);
					return Result;
			}
		}

		private Random rnd = new Random();
	}
}
