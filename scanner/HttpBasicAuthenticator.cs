﻿using System;
using System.Linq;
using System.Text;

namespace RestSharp.Authenticators
{
	public class HttpBasicAuthenticator : IAuthenticator
	{
		private readonly string authHeader;

		public HttpBasicAuthenticator(string username, string password)
		{
			string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));

			this.authHeader = string.Format("Basic {0}", token);
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			// NetworkCredentials always makes two trips, even if with PreAuthenticate,
			// it is also unsafe for many partial trust scenarios
			// request.Credentials = Credentials;
			// thanks TweetSharp!

			// request.Credentials = new NetworkCredential(_username, _password);

			// only add the Authorization parameter if it hasn't been added by a previous Execute
			if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
			{
				request.AddParameter("Authorization", this.authHeader, ParameterType.HttpHeader);
			}
		}
	}
}