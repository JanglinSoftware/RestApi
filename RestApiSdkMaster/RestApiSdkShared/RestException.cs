using System;
using System.IO;
using System.Net;

namespace Janglin.Rest.Sdk
{
	public class RestException : Exception
	{
		const string ErrorDetailsKey = "ErrorDetails";
		const string StatusCodeKey = "StatusCode";
		const string StatusDescriptionKey = "StatusDescription";
		const string WebExceptionMessageKey = "WebExceptionMessage";
		const string ResponseKey = "Response";

		public RestException(WebException ex)
			: base("The call to the RESTful API was unsuccessful. Please see items in the Data property collection in this exception for more information.", ex)
		{
			if (ex == null)
				throw new ArgumentNullException("ex");
			else
			{
				HandleWebException(ex);
			}
		}

		private void HandleWebException(WebException ex)
		{
			var httpresponse = (HttpWebResponse)ex.Response;

			if (httpresponse != null)
			{
				Data.Add(StatusCodeKey, httpresponse.StatusCode);
				Data.Add(StatusDescriptionKey, httpresponse.StatusDescription);
				Data.Add(WebExceptionMessageKey, ex.Message);
				Data.Add(ResponseKey, ex.Response);

				var data = httpresponse.GetResponseStream();

				if (data != null && data.CanRead)
				{
					var reader = new StreamReader(data);

					try
					{
						var responsetext = reader.ReadToEnd();

						if (!String.IsNullOrWhiteSpace(responsetext))
							Data.Add(ErrorDetailsKey, responsetext);
					}
					finally
					{
						reader.DiscardBufferedData();
						reader.Dispose();
					}
				}
			}
		}

		public string ErrorDetails
		{
			get
			{
				return Data.Contains(ErrorDetailsKey)
					? (string)Data[ErrorDetailsKey]
					: String.Empty;
			}
		}

		public string WebExceptionMessage
		{
			get
			{
				return Data.Contains(WebExceptionMessageKey)
				? (string)Data[ErrorDetailsKey]
				: String.Empty;
			}
		}

		public HttpStatusCode? StatusCode
		{
			get
			{
				return Data.Contains(StatusCodeKey)
				? (HttpStatusCode?)Data[StatusCodeKey]
				: null;
			}
		}

		public string StatusDescription
		{
			get
			{
				return Data.Contains(StatusDescriptionKey)
				? (string)Data[StatusDescriptionKey]
				: String.Empty;
			}
		}

		public WebResponse Response
		{
			get
			{
				return Data.Contains(ResponseKey)
				? (WebResponse)Data[ResponseKey]
				: null;
			}
		}
	}
}