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

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="ex">Web exception which to wrap in this RestException including its inner exception.</param>
		public RestException(WebException ex)
			: base("The call to the RESTful API was unsuccessful. Please see items in the Data property collection in this exception for more information.", ex)
		{
			if (ex == null)
				throw new ArgumentNullException("ex");
			else
				HandleWebException(ex);
		}

		/// <summary>
		/// Parse the input web exception into the base exception.
		/// </summary>
		/// <param name="ex">Web Exception to parse.</param>
		/// <remarks></remarks>
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

		/// <summary>
		/// Error details from response text of web exception.
		/// </summary>
		public string ErrorDetails
		{
			get
			{
				return Data.Contains(ErrorDetailsKey)
					? (string)Data[ErrorDetailsKey]
					: String.Empty;
			}
		}

		/// <summary>
		/// Message from web exception.
		/// </summary>
		public string WebExceptionMessage
		{
			get
			{
				return Data.Contains(WebExceptionMessageKey)
					? (string)Data[ErrorDetailsKey]
					: String.Empty;
			}
		}

		/// <summary>
		/// HTTP status code.
		/// </summary>
		public HttpStatusCode? StatusCode
		{
			get
			{
				return Data.Contains(StatusCodeKey)
					? (HttpStatusCode?)Data[StatusCodeKey]
					: null;
			}
		}

		/// <summary>
		/// HTTP status description.
		/// </summary>
		public string StatusDescription
		{
			get
			{
				return Data.Contains(StatusDescriptionKey)
					? (string)Data[StatusDescriptionKey]
					: String.Empty;
			}
		}

		/// <summary>
		/// HTTP web response.
		/// </summary>
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