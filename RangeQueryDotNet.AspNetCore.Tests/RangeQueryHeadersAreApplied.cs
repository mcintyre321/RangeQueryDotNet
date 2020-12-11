using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RangeQueryDotNet.AspNetCore.Tests
{
    public class Tests
    {
        [Test]
        public async Task UnfilteredDataIsReturned()
        {
            using var fixture = new Fixture();
            //When the locations endpoint is requested
            var result = await fixture.HttpClient.GetAsync("/locations");
            
            //Then the full content should be returned
            var resultContent = await result.Content.ReadAsStringAsync();
            var expected =
                "{\"locations\":[{\"name\":\"Seattle\",\"state\":\"WA\"},{\"name\":\"New York\",\"state\":\"NY\"},{\"name\":\"Bellevue\",\"state\":\"WA\"},{\"name\":\"Olympia\",\"state\":\"WA\"}]}";
            Assert.AreEqual(expected , resultContent);
        }

        [Test]
        public async Task RangeQueryrHeaderIsAppliedToEndpoint()
        {
            using var fixture = new Fixture();
            //When the locations endpoint is requested with a range header containing a RangeQuery filter
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/locations");
            requestMessage.Headers.TryAddWithoutValidation("range", "jmesrange=locations[?state == 'WA'].name");
            var result = await fixture.HttpClient.SendAsync(requestMessage);
            
            //Then the transformed content should be returned
            var resultContent = await result.Content.ReadAsStringAsync();
            var expected = "[\"Seattle\",\"Bellevue\",\"Olympia\"]";
            Assert.AreEqual(expected, resultContent);
        }


    }
}