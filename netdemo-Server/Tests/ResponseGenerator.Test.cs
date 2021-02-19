using System;
using Xunit;

namespace Server.Tests {
	public class ResponseGeneratorTest {

		[Fact]
		void ResponseGeneratorNotNull() {
			ResponseGenerator rg = new ResponseGenerator();
			Assert.NotNull(rg);
		}

		[Fact]
		void delme() {
			Assert.True(true);
		}
	}
}
