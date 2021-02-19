using System;
using System.Net;
using System.Threading;

namespace Server {
	class Program {

		private static HttpListener listener;

		static void Main(string[] args) {
			bool done = false;
			string port = "8080",
				machine = "localhost";
			if (args.Length == 2) {
				port = args[1];
				machine = args[0];
			}
			try {
				listener = new HttpListener();
				listener.Prefixes.Add("http://"+machine+":"+port+"/");
				Console.WriteLine("Using machine: " + machine);
				Console.WriteLine("Starting server on port: " + port);
				listener.Start();
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				done = true;
			}

			try {
				while (!done) {
					ThreadPool.QueueUserWorkItem(HandleRequest, listener.GetContext());
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
			Console.WriteLine("Exiting....");
		}

		private static void HandleRequest(object state) {
			HttpListenerContext context = state as HttpListenerContext;
			if (listener != null) {
				ResponseGenerator rg = new ResponseGenerator();
				HttpListenerResponse response = context.Response;
				response.StatusCode = 200;
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes(rg.GetResponseString());
				response.ContentLength64 = buffer.Length;
				System.IO.Stream output = response.OutputStream;
				output.Write(buffer, 0, buffer.Length);
				output.Close();
			}
		}
	}
}
