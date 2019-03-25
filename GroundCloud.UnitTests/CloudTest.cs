using System;
using Moq;
using GroundCloud.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Xunit;
using System.Reactive;
using System.Reactive.Linq;


namespace GroundCloud.UnitTests
{
    public class CloudTest
    {
        Mock<ICloud> mockCloud = new Mock<ICloud>();
        Response<string> response;

        public CloudTest()
        {
            response =new Response<string>();
        }

        public IObservable<Response<string>> ReturnResponseObject(HttpStatusCode httpStatusCode,string resBody)
        {
            response.ResponseBody = resBody;
            response.StatusCode = httpStatusCode;
            response.ResponseHeader = new KeyValuePair<string, string>("", "");
            return Observable.Return<Response<string>>(response);
        }

        //Get Method

        [Fact]
        public void Get_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), "Test Request Body", BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Get_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                "Test Request Body", BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, "Test Response Body"));

            var resObj = mockCloudObject.Get<String, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), "Test Request Body", BodySerialization.DEFAULT);

            Assert.IsType<IObservable<Response<string>>>(resObj);

        }
        [Fact]

        public void Get_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Get<string, string>(null, new KeyValuePair<string, string>("abc", "xyz"),
                 "request body", BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, "Endpoint url can not be null"));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), "request body", BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void Get_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Get<string, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, "Bad Request"));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        //Post Method

        [Fact]
        public void Post_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Post<string, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                "Test Request Body", BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, "Test Response Body"));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), "Test Request Body", BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Post_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Post<string, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               "Test Request Body", BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, "Test Response Body"));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), "request body", BodySerialization.DEFAULT);

            Assert.IsType<IObservable<Response<string>>>(resObj);

        }
        [Fact]
        public void Post_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Post<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 "request body", BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, "Endpoint url can not be null"));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), "request body", BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void Post_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Post<string, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, "Bad Request"));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>("http://test.com/testapi", new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }
    }
}
