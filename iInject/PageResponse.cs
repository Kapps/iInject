using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides information about the results of an HTTP request.
	/// </summary>
	public class PageResponse {

		/// <summary>
		/// Gets the status code that was returned.
		/// </summary>
		public HttpStatusCode StatusCode { get; private set; }

		/// <summary>
		/// Gets the forms contained within this page.
		/// </summary>
		public IEnumerable<WebForm> Forms { get; private set; }

		/// <summary>
		/// Gets the absolute path for this page.
		/// </summary>
		public Uri Uri { get; private set; }

		public PageResponse(HttpStatusCode StatusCode, IEnumerable<WebForm> Forms, Uri Uri) {
			this.StatusCode = StatusCode;
			this.Forms = Forms;
			this.Uri = Uri;
		}

		public override string ToString() {
			string Result = "Status Code: " + this.StatusCode + " - URI: " + this.Uri.LocalPath + " - Forms:\r\n";
			foreach(var Form in this.Forms)
				Result += "\t" + Form.ToString().Replace("\t", "\t\t") + "\r\n";
			return Result.Trim();
		}
	}
}