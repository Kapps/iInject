using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iInject {
	/// <summary>
	/// Provides a crawler that will go through a list of pages from a PageQueue and retrieve them,
	/// passing the results on to a IPageHandler or IPageParser.
	/// </summary>
	public class PageCrawler {

		/// <summary>
		/// Gets the queue to use for crawling pages.
		/// </summary>
		public PageQueue Queue { get; private set; }

		/// <summary>
		/// Gets the parser being used for handling crawled pages.
		/// </summary>
		public PageParser Parser { get; private set; }

		/// <summary>
		/// Creates a new PageCrawler with an empty PageQueue.
		/// </summary>
		public PageCrawler(InjectionSession Session) {
			this.Parser = new PageParser();
			this.Queue = new PageQueue(Session);
		}

		/// <summary>
		/// Begins crawling all pages currently in the queue asynchronously.
		/// All pages that are loaded are passed in to the ResponseHandler for processing.
		/// </summary>
		public async Task CrawlPagesAsync(Action<PageResponse> ResponseHandler) {
			foreach(var PageUri in Queue.GetPages()) {
				HttpClient Client = new HttpClient();
				var Response = await Client.GetAsync(PageUri);
				var StatusCode = Response.StatusCode;
				var Contents = await Response.Content.ReadAsStringAsync();
				var ResponseData = Parser.GetResponse(PageUri, StatusCode, Contents);
				ResponseHandler(ResponseData);
			}
		}
	}
}
