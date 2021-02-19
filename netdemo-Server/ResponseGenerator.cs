using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLib;

namespace Server {
	class ResponseGenerator {

		private readonly RegistryOperations registryOperations;

		public ResponseGenerator(RegistryOperations regops = null) {
			if (null == regops) {
				registryOperations = new SharedLib.RegistryOperations();
			} else {
				registryOperations = regops;
			}
		}

		public string GetResponseString() {
			string response = @"Server says: {{message}}";
			response = response.Replace("{{message}}", registryOperations.GetMessage());
			return response;
		}
	}
}
