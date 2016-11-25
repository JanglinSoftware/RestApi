using System;
using System.Text;

namespace Janglin.Rest.Sdk
{
	public static class Extensions
	{
		public static string Append(this string baseUri, params string[] routes)
		{
			var output = new StringBuilder(baseUri.Trim().Trim('/'));

			if (routes != null)
			{
				foreach (var route in routes)
				{
					if (!String.IsNullOrWhiteSpace(route))
						output.AppendFormat("/{0}", Uri.EscapeDataString(route.Trim().Trim('/')));
				}
			}

			return output.Append('/').ToString();
		}

		public static string Parameters(this string url, params string[] parameterNameValuePairs)
		{
			if (parameterNameValuePairs == null) return String.Concat(url.Trim(), '/');
			if (parameterNameValuePairs.Length < 1) return String.Concat(url.Trim(), '/');

			if (parameterNameValuePairs.Length % 2 != 0)
				throw new ArgumentException("Parameter must be a even numbered collection of strings representing name/value pairs.", "parameterNameValuePairs");

			var output = new StringBuilder(url.Trim().Trim('/'));

			output.Append('?');

			for (var index = 0; index < parameterNameValuePairs.Length - 1; index += 2)
			{
				if (!String.IsNullOrWhiteSpace(parameterNameValuePairs[index]) && parameterNameValuePairs[index + 1] != null)
					output.AppendFormat("{0}={1}&",
						Uri.EscapeDataString(parameterNameValuePairs[index].Trim()),
						Uri.EscapeDataString(parameterNameValuePairs[index + 1].Trim()));
			}

			return output.ToString().Trim(new char[] { '&', '?' }).Trim();
		}

		/// <summary>Convenient method to convert Nullable boolean values into an string appropriate for a URL.</summary>
		/// <param name="value">Nullable Boolean value.</param>
		/// <returns>Return an all lower case string version of the Boolean input value if it's not null. If it is null, then return null.</returns>
		public static string ToStringIfNotNull(this bool? value)
		{
			if (value.HasValue)
				return value.ToString().ToLowerInvariant();
			else
				return null;
		}

		/// <summary>Convenient method to convert Nullable byte values into an string appropriate for a URL.</summary>
		/// <param name="value">Nullable Byte value.</param>
		/// <returns>Return an all lower case string version of the Boolean input value if it's not null. If it is null, then return null.</returns>
		public static string ToStringIfNotNull(this byte? value)
		{
			if (value.HasValue)
				return value.ToString().ToLowerInvariant();
			else
				return null;
		}

		/// <summary>Convenient method to convert class object into an string appropriate for a URL.</summary>
		/// <param name="value">Class instantiation.</param>
		/// <returns>Return an all lower case string version of the objects ToString() method's return value if it's not null. If it is null, then return null.</returns>
		public static string ToStringIfNotNull(this object value)
		{
			if (value == null)
				return null;
			else
				return value.ToString();
		}

		/// <summary>Convenient method to convert string into an string appropriate for a URL.</summary>
		/// <param name="value">String.</param>
		/// <returns>Return an all lower case string version of the objects ToString() method's return value if it's not null. If it is null, then return null.</returns>
		public static string EmptyIfNull(this string value) { return value == null ? String.Empty : value; }
	}
}