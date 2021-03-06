﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

		/// <summary>
		/// Creates a new WebForm with the given data.
		/// </summary>
		public WebForm(string Name, Uri Target, HttpMethod Method, IEnumerable<WebFormControl> Controls) {
			this.Name = Name;
			this.Target = Target;
			this.Method = Method;
			this.Controls = Controls.ToList();
		}
		
		/// <summary>
		/// Asynchronously submits this form with the current values it holds, returning the new page.
		/// </summary>
		/// <param name="Parser">The parser used to parse the response into a PageResponse.</param>
		/// <param name="Timeout">The amount of time that must pass before the request times out.</param>
		public async Task<PageResponse> SubmitAsync(PageParser Parser, TimeSpan Timeout) {
			HttpClient Client = new HttpClient();
			Client.Timeout = Timeout;
			var Content = new FormUrlEncodedContent(this.Controls.Select(c => new KeyValuePair<string, string>(c.Name, c.Value)));
			var Message = new HttpRequestMessage(this.Method, this.Target);
			Message.Content = Content;
			var Response = await Client.SendAsync(Message);
			var StatusCode = Response.StatusCode;
			var Contents = await Response.Content.ReadAsStringAsync();
			var ResponseData = Parser.GetResponse(Target, StatusCode, Contents);
			return ResponseData;
		}

		/// <summary>
		/// Returns a string representation of this form that includes all controls.
		/// </summary>
		public override string ToString() {
			string Result = this.Name;
			foreach(var Control in this.Controls)
				Result += "\r\n\t" + Control.Name + " = " + Control.Value;
			return Result;
		}
	}
}
