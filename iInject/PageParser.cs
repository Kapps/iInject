using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a parser that can be used to parse forms out of a response.
	/// </summary>
	public class PageParser {
		/// <summary>
		/// Parses the forms present in the given HTML response data.
		/// </summary>
		public PageResponse GetResponse(Uri Uri, HttpStatusCode Code, string ResponseData) {
			HtmlNode.ElementsFlags.Remove("form");
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(ResponseData);
			List<WebForm> Forms = new List<WebForm>();
			var FormNodes = doc.DocumentNode.SelectNodes("//form");
			if(FormNodes != null) {
				foreach(HtmlNode FormNode in FormNodes) {
					string FormName = FormNode.GetAttributeValue("name", GetDefaultFormName());
					HttpMethod Method = new HttpMethod(FormNode.GetAttributeValue("method", "GET").ToUpper());
					string TargetPath = FormNode.GetAttributeValue("action", Uri.LocalPath);
					Uri Target = new Uri(Uri, TargetPath);
					List<WebFormControl> Controls = new List<WebFormControl>();
					foreach(HtmlNode InputNode in FormNode.SelectNodes(".//input")) {
						string InputType = InputNode.GetAttributeValue("type", "text").ToLower();
						if(InputType == "submit" || InputType == "button")
							continue; // We don't want to include submit buttons.
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
					Forms.Add(Form);
				}
			}
			return new PageResponse(ResponseData, Code, Forms, Uri);
		}

		private string GetDefaultFormName() {
			int CurrID = Interlocked.Increment(ref DefaultNamesCreated);
			return "Form" + CurrID;
		}

		static int DefaultNamesCreated = 0;
	}
}
