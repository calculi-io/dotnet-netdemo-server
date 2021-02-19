using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;
using System.Net.Sockets;

namespace Server.Integration.Tests {

	public class ResponseGeneratorIntegrationTest {

		readonly string url;

		public ResponseGeneratorIntegrationTest() {
			url = GetTestURL();
		}

		private string GetTestURL() {
			string ipAddress = "localhost",
				port;

			if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0) {
				ipAddress = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => !System.Net.IPAddress.IsLoopback(ip) && ip.AddressFamily.Equals(AddressFamily.InterNetwork)).ToArray().GetValue(0).ToString();
			}

			port = System.Environment.GetEnvironmentVariable("DEMOSERVER_PORT");
			if (port.Length == 0) {
				port = "8080";
			}

			return "http://" + ipAddress + ":" + port + "/";
		}

		[Fact]
		[Trait("Category", "Integration")]
		void VerifyServerResponse() {
			HttpClientHandler handler = new HttpClientHandler();
			HttpClient httpClient = new HttpClient(handler);
			HttpResponseMessage response;
			response = httpClient.GetAsync(url).Result;
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		[Trait("Category", "Integration")]
		void SimpleAssertAsTrue() {
			Assert.True(true);
		}

		[Fact]
		[Trait("Category", "Integration")]
		void TestTrhee() {
			Assert.True(true);
		}

		[Fact]
		[Trait("Category", "Integration")]
		void MakeTestFail() {
			Assert.True(false);
		}

	}
}
