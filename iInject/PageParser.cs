using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iInject {
	public class PageParser {

		/// <summary>
		/// Parses the forms present in the given HTML response data.
		/// </summary>
		public IEnumerable<WebForm> ParseForms(Uri Uri, string ResponseData) {
			HtmlNode.ElementsFlags.Remove("form");
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(ResponseData);
			foreach(HtmlNode FormNode in doc.CreateNavigator().Select("//form")) {
				string FormName = FormNode.GetAttributeValue("name", GetDefaultFormName());
				HttpMethod Method = new HttpMethod(FormNode.GetAttributeValue("method", "GET").ToUpper());
				string TargetPath = FormNode.GetAttributeValue("target", Uri.LocalPath);
				Uri Target = new Uri(Uri, TargetPath);
				List<WebFormControl> Controls = new List<WebFormControl>();
				foreach(HtmlNode InputNode in FormNode.SelectNodes(".//input")) {
					string InputType = InputNode.GetAttributeValue("type", "text").ToLower();
					string InputName = InputNode.GetAttributeValue("name", null);
					// If it's null, skip this element as we can't do much with it.
					if(String.IsNullOrWhiteSpace(InputName))
						continue;
					// TODO: Allow something like <input type="text">Blah</input>.
					string InputValue = InputNode.GetAttributeValue("value", null);
					WebFormControl Control = new WebFormControl(InputName, InputType, InputValue);
					Controls.Add(Control);
				}
				WebForm Form = new WebForm(FormName, Target, Method, Controls);
				yield return Form;
			}
		}

		private string GetDefaultFormName() {
			int CurrID = Interlocked.Increment(ref DefaultNamesCreated);
			return "Form" + CurrID;
		}

		static int DefaultNamesCreated = 0;
	}
}
