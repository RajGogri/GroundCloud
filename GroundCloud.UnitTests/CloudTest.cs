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

        #region Get Method Testcases

        [Fact]
        public void Get_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Get<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Get_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Get<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //         null, BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //     null, BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Get_IsRequestNotNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, 
                new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, Constants.BAD_REQUEST_MSG));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        [Fact]
        public void Get_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null,null),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null,null),
             null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //[Fact]
        //public void Get_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //        null, BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1,Constants.TEST_REQUEST_HEADER2),
        //    null, BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Get_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Get<string, string>(It.Is<string>(a =>a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                null, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                null, BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Get_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, "Test Response Body"));

            var resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }
        #endregion

        #region Post method testcases
        [Fact]
        public void Post_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Post<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Post_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Post<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //         Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //        Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Post_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, Constants.BAD_REQUEST_MSG));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        [Fact]
        public void Post_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null,null),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //[Fact]
        //public void Post_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //        It.IsAny<string>(), BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //    Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Post_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Post<string, string>(It.Is<string>(a => a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1,Constants.TEST_REQUEST_HEADER2),
                 It.IsAny<string>(), BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Post_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Post_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2), 
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Put method testcases
        [Fact]
        public void Put_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Put<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Put_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Put<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //         Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //        Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Put_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, Constants.BAD_REQUEST_MSG));

            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        [Fact]
        public void Put_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //[Fact]
        //public void Put_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //        It.IsAny<string>(), BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //    Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Put_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Put<string, string>(It.Is<string>(a => a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 It.IsAny<string>(), BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void Put_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Put_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Delete method testcases
        [Fact]
        public void Delete_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Delete<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Delete_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Delete<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //         Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //        Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Delete_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, Constants.BAD_REQUEST_MSG));

            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        [Fact]
        public void Delete_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //[Fact]
        //public void Delete_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //        It.IsAny<string>(), BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //    Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Delete_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Delete<string, string>(It.Is<string>(a => a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 It.IsAny<string>(), BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void Delete_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Delete_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert.IsType<IObservable<Response<string>>>(resObj);
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Patch method testcases
        [Fact]
        public void Patch_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Patch<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Patch_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Patch<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //         Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //        Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Patch_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, Constants.BAD_REQUEST_MSG));

            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        [Fact]
        public void Patch_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //[Fact]
        //public void Patch_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //        It.IsAny<string>(), BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //    Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Patch_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Patch<string, string>(It.Is<string>(a => a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 It.IsAny<string>(), BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void Patch_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Patch_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Head method testcases
        [Fact]
        public void Head_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Head<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2)
                , BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2)
                , BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Head_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Head<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //          BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //        BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Head_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                 BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null,null),
                 BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
        //[Fact]
        //public void Head_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //         BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //    BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Head_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Head<string, string>(It.Is<string>(a => a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                  BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Head_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Head_IsResponseCorrectType()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT);

            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }
        [Fact]
        public void Head_IsResponseHeaderShouldNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT);

            Assert.NotNull(resObj.FirstOrDefault().ResponseHeader);
        }

        [Fact]
        public void Head_IsResponseBodyShouldNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, null));

            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                BodySerialization.DEFAULT);

            Assert.Null(resObj.FirstOrDefault().ResponseBody);
        }

        #endregion

        #region Options method testcases
        [Fact]
        public void Options_IsEndPointNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Options<string, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.NotFound, Constants.ENDPOINT_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(null, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        //[Fact]
        //public void Options_IsEndPointInvalid_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Options<string, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //         Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
        //       .Returns(ReturnResponseObject(HttpStatusCode.InternalServerError, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.INVALID_ENDPOINT_URL, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //        Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.InternalServerError, statusCode);
        //}

        [Fact]
        public void Options_IsRequestBodyNull_ReturnErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.BadRequest, Constants.BAD_REQUEST_MSG));

            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 null, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);

        }

        [Fact]
        public void Options_IsRequestHeaderNull_ReturnsErrResponse()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.Unauthorized, Constants.REQUESTHEADER_CANNOT_NULL));

            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(null, null),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        //[Fact]
        //public void Options_CheckRequestHeaderLimit_ReturnsErrResponse()
        //{
        //    ICloud mockCloudObject = mockCloud.Object;
        //    HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

        //    mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT), It.Is<string>(a => a.Length >= Constants.REQUEST_HEADER_LIMIT)),
        //        It.IsAny<string>(), BodySerialization.DEFAULT))
        //      .Returns(ReturnResponseObject(HttpStatusCode.RequestHeaderFieldsTooLarge, Constants.ENDPOINT_INVALID));

        //    IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
        //    Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);


        //    resObj.Subscribe(async =>
        //    {
        //        statusCode = resObj.FirstOrDefault().StatusCode;
        //    });

        //    Assert.Equal(HttpStatusCode.RequestHeaderFieldsTooLarge, statusCode);
        //}

        [Fact]
        public void Options_IsEndpointStartWithHttps_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;
            HttpStatusCode statusCode = HttpStatusCode.NotImplemented;

            mockCloud.Setup(x => x.Options<string, string>(It.Is<string>(a => a.StartsWith("https://")), new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                 It.IsAny<string>(), BodySerialization.DEFAULT))
               .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            resObj.Subscribe(async =>
            {
                statusCode = resObj.FirstOrDefault().StatusCode;
            });

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Options_IsResponseNotNull_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.NotNull(resObj);
        }

        [Fact]
        public void Options_IsResponseCorrectType_Verify()
        {
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new KeyValuePair<string, string>(Constants.TEST_REQUEST_HEADER1, Constants.TEST_REQUEST_HEADER2),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion
    }
}
