using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Janglin.RestApiSdk
{
	public static class Extentions
	{
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

		/// <summary>Generate an XML namespace qualified tag name.</summary>
		/// <param name="value">Tag name.</param>
		/// <param name="nameSpace">Optional namespace.</param>
		/// <returns>The fully qualified tag name as an XName type.</returns>
		static public XName Xmlns(this string value, string nameSpace = null)		{			return XName.Get(value, nameSpace);		}

		/// <summary>Append any number of subroutes to a base URL.</summary>
		/// <param name="value">The base URL.</param>
		/// <param name="subRoutes">An array of strings representing subroutes.</param>
		/// <returns>The base url with the subroutes appended and delimited by a forward slash (/).</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="subRoutes"/> parameter cannot be null.</exception>
		static public string SubRoutes(this string value, params string[] subRoutes)
		{
			if (subRoutes == null)
				throw new ArgumentNullException("subRoutes", "Parameter value cannot be null.");
			else if (subRoutes.Length < 1)
				return value;
			else
				return String.Concat(value, "/", String.Join("/", subRoutes));
		}


		/// <summary>Creates a URL parameter string.</summary>
		/// <param name="urlbase">URL base string on which to append the resulting parameter string.</param>
		/// <param name="nameValuePairs">A list of Name/Value pairs representing the parameter names and values which to append to the target URL base string.</param>
		/// <remarks>
		/// <para>If the name parameter of a name/value pair is null, empty or whitespace, that name and its corresponding value will be left out of the result.</para>
		/// <para>If the value parameter of a name/value pair is null, that name and its corresponding value will be left out of the result.</para>
		/// <para>If the value parameter of a name/value pair is empty or whitespace, then its corresponding parameter name will be included but set equal to an empty string.</para>
		/// </remarks>
		/// <example>
		/// <c>http://base.url?name1=value1&name2=&name3=value3</c> The name2 parameter equals nothing.		
		/// </example>		
		/// <exception cref="ArgumentNullException">The <paramref name="nameValuePairs"/> parameter cannot be null.</exception>
		/// <exception cref="ArgumentException">There must be an even number of <paramref name="nameValuePairs"/> parameters.</exception>
		/// <returns>The complete URL string with the base URL string appended with the parameters.</returns>
		static public string Parameters(this string urlbase, params string[] nameValuePairs)
		{
			if (nameValuePairs == null) throw new ArgumentNullException("nameValuePairs", "Parameter is null");
			if (nameValuePairs.Length % 2 != 0) throw new ArgumentException("There must be an even number of Name/Value Pair parameters.", "nameValuePairs");

			if (nameValuePairs != null && nameValuePairs.Length > 0)
			{
				var output = new StringBuilder(urlbase);

				var list = new List<string>();

				for (var i = 0; i < nameValuePairs.Length - 1; i += 2)
				{
					var name = nameValuePairs[i];
					var value = nameValuePairs[i + 1];

					if (!String.IsNullOrWhiteSpace(name) && value != null)
						list.Add(String.Format("{0}={1}", name.Trim(), value.Trim()));
				}

				if (list.Count > 0)
				{
					output.Append('?');
					output.Append(String.Join("&", list));
				}

				return output.ToString();
			}
			else
				return urlbase;
		}

		static internal string ToLowerString(this bool value) { return value.ToString().ToLowerInvariant(); }

		static internal string GetElementString(this XElement element, string tagName, string nameSpace = null)
		{
			return (string)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal decimal GetElementDecimal(this XElement element, string tagName, string nameSpace = null)
		{
			return (decimal)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal short GetElementShort(this XElement element, string tagName, string nameSpace = null)
		{
			return (short)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal short? GetElementShortN(this XElement element, string tagName, string nameSpace = null)
		{
			return (short?)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal bool GetElementBool(this XElement element, string tagName, string nameSpace = null)
		{
			return (bool)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal bool? GetElementBoolN(this XElement element, string tagName, string nameSpace = null)
		{
			return (bool?)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal DateTime GetElementDateTime(this XElement element, string tagName, string nameSpace = null)
		{
			return (DateTime)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal DateTime? GetElementDateTimeNullable(this XElement element, string tagName, string nameSpace = null)
		{
			return (DateTime?)element.Element(tagName.Xmlns(nameSpace));
		}
		static internal int GetElementInt(this XElement element, string tagName, string nameSpace = null)
		{
			return (int)element.Element(tagName.Xmlns(nameSpace));
		}
	}
}