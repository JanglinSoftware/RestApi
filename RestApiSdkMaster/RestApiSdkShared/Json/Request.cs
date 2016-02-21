using System;

namespace Janglin.RestApiSdk.Json
{
	public class Request : Janglin.RestApiSdk.Request
	{
		protected internal override byte[] ByteArrayBuffer
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
