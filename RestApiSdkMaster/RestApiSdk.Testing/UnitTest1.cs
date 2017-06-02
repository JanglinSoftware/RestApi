using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Janglin.Rest.Sdk;

namespace RestApiSdk.Testing
{
	[TestClass]
	public class ExtensionsUnitTest
	{
		/// <summary>
		/// Test Append extension method.
		/// </summary>
		/// <remarks>
		/// Append method must:
		/// Ignore null, empty collection, or items in collection that are null, empty or whitespace.
		/// Handle route members entered individually or as a collection.
		/// Return result with single trailing slash.
		/// Ignore leading or traling slashes on parameters.
		/// baseUri parameter cannot be empty or whitespace.
		/// Appended route parts must be URI formatted.
		/// </remarks>
		[TestMethod]
		public void AppendTestMethod()
		{
			Assert.AreEqual("http://service.com/", "http://service.com".Append(null));
			Assert.AreEqual("http://service.com/", "http://service.com".Append());
			Assert.AreEqual("http://service.com/", "http://service.com".Append(String.Empty));
			Assert.AreEqual("http://service.com/", "http://service.com".Append(String.Empty, String.Empty));
			Assert.AreEqual("http://service.com/", "http://service.com".Append(null, String.Empty));
			Assert.AreEqual("http://service.com/", "http://service.com".Append(null, String.Empty, "    "));

			Assert.AreEqual("http://service.com/testing/", "http://service.com".Append("testing"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append("testing", "testing"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append(new string[] { "testing", "testing" }));

			Assert.AreEqual("http://service.com/testing/", "http://service.com".Append("testing"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append("testing", "testing"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append(new string[] { "testing", "testing" }));

			Assert.AreEqual("http://service.com/testing/", "http://service.com".Append("testing/"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append("testing/", "testing/"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append(new string[] { "testing/", "testing//" }));

			Assert.AreEqual("http://service.com/testing/", "http://service.com/".Append("testing/"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com/".Append("testing/", "testing/"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com/".Append(new string[] { "//testing/", "testing//" }));

			Assert.AreEqual("http://service.com/testing%40/tes%20ting/testing%3F/", "http://service.com".Append("testing@", "tes ting", "testing?"));

			try { String.Empty.Append("testing/", "testing/"); }
			catch (ArgumentException ex) { Assert.IsTrue(ex.Message.StartsWith("Parameter cannot be empty or whitespace.")); }

			try { "       ".Append("testing/", "testing/"); }
			catch (ArgumentException ex) { Assert.IsTrue(ex.Message.StartsWith("Parameter cannot be empty or whitespace.")); }
		}

		/// <summary>
		/// Test Parameters extention method.
		/// </summary>
		/// <remarks>
		/// Parameters method must:
		/// Treat null, empty or whitespace values as and empty string.
		/// Ignore empty collection in key/value pair parameter.
		/// Key/value parameter collection much contain an even number or items.
		/// Keys in Key/value parameter collection cannot be null, empty or whitespace.
		/// All strings in key/value parameters must be URI formatted.
		/// </remarks>
		[TestMethod]
		public void ParametersTestMethod()
		{
			Assert.AreEqual("http://service.com?param=", "http://service.com".Parameters("param", String.Empty));
			Assert.AreEqual("http://service.com?param=", "http://service.com".Parameters("param", null));
			Assert.AreEqual("http://service.com?param=", "http://service.com".Parameters("param", "              "));

			Assert.AreEqual("http://service.com/", "http://service.com".Parameters());

			try { "http://service.com".Parameters("testing"); }
			catch (ArgumentException ex)
			{
				Assert.AreEqual("Parameter must be a even numbered collection of strings representing name/value pairs."
					+ Environment.NewLine
					+ "Parameter name: parameterNameValuePairs", ex.Message);
			}

			try { "http://service.com".Parameters(null, "value");}
			catch (ArgumentException ex) { Assert.IsTrue(ex.Message.StartsWith("Key values in name/value list cannot be null, empty or whitespace.")); }

			Assert.AreEqual("http://service.com?parameter=value", "http://service.com".Parameters("parameter", "value"));
			Assert.AreEqual("http://service.com?parameter=value&parameter1=value1", "http://service.com".Parameters("parameter", "value", "parameter1", "value1"));

			var uri = new Uri("http://service.com".Parameters("par ameter", "va=lue", "param-eter1", "va/lue1"));
			Assert.AreEqual(new Uri("http://service.com?par%20ameter=va%3Dlue&param-eter1=va%2Flue1"), uri.ToString());
			Assert.AreEqual("http://service.com?par%20ameter=va%3Dlue&param-eter1=va%2Flue1", "http://service.com".Parameters("par ameter", "va=lue", "param-eter1", "va/lue1"));
		}
	}
}