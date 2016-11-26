using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Janglin.Rest.Sdk;

namespace RestApiSdk.Testing
{
	[TestClass]
	public class ExtensionsUnitTest
	{
		[TestMethod]
		public void AppendTestMethod()
		{
			Assert.AreEqual("http://service.com/testing/", "http://service.com".Append("testing"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append("testing", "testing"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append(new string[] { "testing", "testing" }));

			Assert.AreEqual("http://service.com/testing/", "http://service.com".Append("testing/"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append("testing/", "testing/"));
			Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Append(new string[] { "testing/", "testing//" }));
		}

		[TestMethod]
		public void ParametersTestMethod()
		{
			Assert.AreEqual("http://service.com?param=", "http://service.com".Parameters("param", String.Empty));
			Assert.AreEqual("http://service.com/", "http://service.com".Parameters());
			try { Assert.AreEqual("http://service.com/testing/testing/", "http://service.com".Parameters("testing")); }
			catch (ArgumentException ex)
			{
				Assert.AreEqual("Parameter must be a even numbered collection of strings representing name/value pairs."
					+ Environment.NewLine
					+ "Parameter name: parameterNameValuePairs", ex.Message);
			}
			Assert.AreEqual("http://service.com?parameter=value", "http://service.com".Parameters("parameter", "value"));
			Assert.AreEqual("http://service.com?parameter=value&parameter1=value1", "http://service.com".Parameters("parameter", "value", "parameter1", "value1"));
			var uri = new Uri("http://service.com".Parameters("par ameter", "va=lue", "param-eter1", "va/lue1"));
			Assert.AreEqual("http://service.com?par+ameter=va%3dlue&param-eter1=va%2flue1", "http://service.com".Parameters("par ameter", "va=lue", "param-eter1", "va/lue1"));
		}
	}
}
