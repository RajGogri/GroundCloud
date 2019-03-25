using System;
using Moq;
using GroundCloud.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Xunit;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;

namespace GroundCloud.UnitTests
{
    public class CloudReactiveTest:ReactiveTest
    {
        Mock<ICloud> mockCloud = new Mock<ICloud>();
        Response<string> response;

        public CloudReactiveTest()
        {
            response =new Response<string>();

           
            //mockCloud.Setup(x => x.Get<string, string>("http://abc.com/testmethod", new KeyValuePair<string, string>("abc", "xyz"), "request body", BodySerialization.DEFAULT))
               //.Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, "Endpoint url is not valid"));
        }

        public IObservable<Response<string>> ReturnResponseObject(HttpStatusCode httpStatusCode,string resBody)
        {
            response.ResponseBody = resBody;
            response.StatusCode = httpStatusCode;
            response.ResponseHeader = new KeyValuePair<string, string>("", "");
            return Observable.Return<Response<string>>(response);
            //return response;


        }

        [Fact]
        public void Get_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>(null, new KeyValuePair<string, string>("abc", "xyz"), "request body", BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, "Endpoint url can not be null"));


            IObservable<Response<string>> resObj =mockCloudObject.Get<String, string>(null, new KeyValuePair<string, string>("abc", "xyz"), "request body", BodySerialization.DEFAULT);

            //resObj.Subscribe
            Assert.Equal<HttpStatusCode>(HttpStatusCode.NotFound,resObj.FirstOrDefault().StatusCode);
        }

        [Fact]
        public void Get_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(null, new KeyValuePair<string, string>("abc", "xyz"), "request body", BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Get_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(null, new KeyValuePair<string, string>("abc", "xyz"), "request body", BodySerialization.DEFAULT);

            Assert.IsType<IObservable<Response<string>>>(resObj);

        }
        [Fact]
        public void Get_IsEndPointNotValid_ReturnsErrResponse()
        {
            //var scheduler = new TestScheduler();
            //ICloud mockCloudObject = mockCloud.Object;

            //var actual = scheduler.Start(
            //       () => mockCloudObject.Get<string,string>(null,new KeyValuePair<string, string>("",""),"test request",BodySerialization.DEFAULT)
            //     );

            //var expected= ;

            //ReactiveAssert.AreElementsEqual(expected, actual.Messages);

        }

    }
}
