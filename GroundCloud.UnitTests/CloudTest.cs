using System;
using Moq;
using GroundCloud.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Xunit;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace GroundCloud.UnitTests
{
    public class MyDisposable : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("");
            //throw new NotImplementedException();
        }
    }
    public class CloudTest
    {
        Mock<ICloud> mockCloud = new Mock<ICloud>();
        Response<string> response;

        public CloudTest()
        {
            response =new Response<string>();
        }
        /// <summary>
        /// Returns the response object.
        /// </summary>
        /// <returns>The response object.</returns>
        /// <param name="httpStatusCode">Http status code.</param>
        /// <param name="resBody">Res body.</param>
        public IObservable<Response<string>> ReturnResponseObject(HttpStatusCode httpStatusCode,string resBody)
        {
            response.ResponseBody = resBody;
            response.StatusCode = httpStatusCode;
            response.ResponseHeader = new List<KeyValuePair<string, string>>();
            return Observable.Return<Response<string>>(response);
            //return Observable.Create((IObserver<Response<string>> arg) => arg.OnError();)
        }

       
        public string ReturnErrResponse()
        {
            return "";
        }

        #region Get Method Testcases

        /// <summary>
        /// If the end point null throws error response.
        /// </summary>
        [Fact]
        public void Get_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;


            mockCloud.Setup(x => x.Get<string, string>(null, new List<KeyValuePair<string, string>>(),
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
                 }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Get<String, string>(null, new List<KeyValuePair<string, string>>(),
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response)=> { },(err) =>{
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

        /// <summary>
        /// Gets the is request not null throws argument null exception.
        /// </summary>
        [Fact]
        public void Get_IsRequestNotNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.GET_REQ_BODY_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Get<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.GET_REQ_BODY_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQBODY, errMsg);

        }

        /// <summary>
        /// Gets the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Get_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, null,
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Get<String, string>(Constants.TEST_API, null,
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

      
        /// <summary>
        /// Gets the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Get_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Get<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Get<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Gets the is response not null verify.
        /// </summary>
        [Fact]
        public void Get_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                null, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                null, BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Gets the is response correct type verify.
        /// </summary>
        [Fact]
        public void Get_IsResponseCorrectType_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Get<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, "Test Response Body"));

            //Act
            var resObj = mockCloudObject.Get<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }
        #endregion

        #region Post method testcases

        /// <summary>
        /// Posts the is end point null throws argument null exception.
        /// </summary>
        [Fact]
        public void Post_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Post<string, string>(null, new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Post<String, string>(null, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

        /// <summary>
        /// Posts the is request body null throws argument null exception.
        /// </summary>
        [Fact]
        public void Post_IsRequestBodyNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Post<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUEST_BODY_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQBODY, errMsg);

        }

        /// <summary>
        /// Posts the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Post_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, null,
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Post<String, string>(Constants.TEST_API, null,
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

       
        /// <summary>
        /// Posts the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Post_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Post<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Post<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Posts the is response not null verify.
        /// </summary>
        [Fact]
        public void Post_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Posts the is response correct type verify.
        /// </summary>
        [Fact]
        public void Post_IsResponseCorrectType_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Post<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Post<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(), 
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Put method testcases
        /// <summary>
        /// Puts the is end point null throws argument null exception.
        /// </summary>
        [Fact]
        public void Put_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Put<string, string>(null, new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Put<String, string>(null, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

        /// <summary>
        /// Puts the is request body null throws argument null exception.
        /// </summary>
        [Fact]
        public void Put_IsRequestBodyNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Put<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUEST_BODY_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQBODY, errMsg);

        }

        /// <summary>
        /// Puts the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Put_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API,null,
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Put<String, string>(Constants.TEST_API, null,
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

      
        /// <summary>
        /// Puts the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Put_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Put<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Put<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Puts the is response not null verify.
        /// </summary>
        [Fact]
        public void Put_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Puts the is response correct type verify.
        /// </summary>
        [Fact]
        public void Put_IsResponseCorrectType_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Put<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Put<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Delete method testcases
        /// <summary>
        /// Deletes the is end point null throws argument null exception.
        /// </summary>
        [Fact]
        public void Delete_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Delete<string, string>(null, new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Delete<String, string>(null, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

      
        ///// <summary>
        ///// Deletes the is request body null throws argument null exception.
        ///// </summary>
        //[Fact]
        //public void Delete_IsRequestBodyNull_ThrowsArgumentNullException()
        //{
        //    //Arrange
        //    ICloud mockCloudObject = mockCloud.Object;
        //    string errMsg = null;

        //    mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
        //         null, BodySerialization.DEFAULT))
        //       .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
        //           observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
        //           return Disposable.Empty;
        //       }));

        //    //Act
        //    IObservable<Response<string>> observable = mockCloudObject.Delete<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
        //     null, BodySerialization.DEFAULT);

        //    observable.Subscribe((response) => { }, (err) => {
        //        errMsg = err.Message;
        //    });

        //    //Assert
        //    Assert.Equal(Constants.REQUEST_BODY_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQBODY, errMsg);

        //}

        /// <summary>
        /// Deletes the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Delete_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, null,
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) =>
               {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Delete<String, string>(Constants.TEST_API,null,
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) =>
            {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

        /// <summary>
        /// Deletes the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Delete_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Delete<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Delete<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Deletes the is response not null verify.
        /// </summary>
        [Fact]
        public void Delete_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Deletes the is response correct type verify.
        /// </summary>
        [Fact]
        public void Delete_IsResponseCorrectType_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Delete<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Delete<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Patch method testcases
        /// <summary>
        /// Patchs the is end point null throws argument null exception.
        /// </summary>
        [Fact]
        public void Patch_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Patch<string, string>(null, new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Patch<String, string>(null, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

        /// <summary>
        /// Patchs the is request body null throws argument null exception.
        /// </summary>
        [Fact]
        public void Patch_IsRequestBodyNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Patch<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUEST_BODY_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQBODY, errMsg);
        }

        /// <summary>
        /// Patchs the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Patch_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, null,
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Patch<String, string>(Constants.TEST_API, null,
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

        /// <summary>
        /// Patchs the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Patch_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Patch<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Patch<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Patchs the is response not null verify.
        /// </summary>
        [Fact]
        public void Patch_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Patchs the is response correct type verify.
        /// </summary>
        [Fact]
        public void Patch_IsResponseCorrectType_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Patch<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Patch<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion

        #region Head method testcases
        /// <summary>
        /// Heads the is end point null throws argument null exception.
        /// </summary>
        [Fact]
        public void Head_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Head<string, string>(null, new List<KeyValuePair<string, string>>(),
                 BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Head<String, string>(null, new List<KeyValuePair<string, string>>(),
              BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

        /// <summary>
        /// Heads the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Head_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, null,
                  BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Head<String, string>(Constants.TEST_API, null,
             BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

        /// <summary>
        /// Heads the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Head_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Head<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Head<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
              BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Heads the is response not null verify.
        /// </summary>
        [Fact]
        public void Head_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Heads the type of the is response correct.
        /// </summary>
        [Fact]
        public void Head_IsResponseCorrectType()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
               BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        /// <summary>
        /// Heads the is response header should not null verify.
        /// </summary>
        [Fact]
        public void Head_IsResponseHeaderShouldNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj.FirstOrDefault().ResponseHeader);
        }

        /// <summary>
        /// Heads the is response body should null verify.
        /// </summary>
        [Fact]
        public void Head_IsResponseBodyShouldNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Head<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, null));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Head<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                BodySerialization.DEFAULT);

            //Assert
            Assert.Null(resObj.FirstOrDefault().ResponseBody);
        }

        #endregion

        #region Options method testcases

        /// <summary>
        /// Optionses the is end point null throws argument null exception.
        /// </summary>
        [Fact]
        public void Options_IsEndPointNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Options<string, string>(null, new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Options<String, string>(null, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, errMsg);
        }

        /// <summary>
        /// Optionses the is request body null throws argument null exception.
        /// </summary>
        [Fact]
        public void Options_IsRequestBodyNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                 null, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Options<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
             null, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUEST_BODY_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQBODY, errMsg);

        }

        /// <summary>
        /// Optionses the is request header null throws argument null exception.
        /// </summary>
        [Fact]
        public void Options_IsRequestHeaderNull_ThrowsArgumentNullException()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, null,
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Options<String, string>(Constants.TEST_API, null,
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.REQUESTHEADER_CANNOT_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_REQHEADER, errMsg);
        }

        /// <summary>
        /// Optionses the is endpoint start with https verify.
        /// </summary>
        [Fact]
        public void Options_IsEndpointStartWithHttps_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;
            string errMsg = null;

            mockCloud.Setup(x => x.Options<string, string>(It.Is<string>(a => !a.StartsWith(Constants.STARTS_WITHTEXT)), new List<KeyValuePair<string, string>>(),
                 Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
               .Returns(Observable.Create<Response<string>>((IObserver<Response<string>> observer) => {
                   observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                   return Disposable.Empty;
               }));

            //Act
            IObservable<Response<string>> observable = mockCloudObject.Options<String, string>(Constants.INVALID_ENDPOINT_URL, new List<KeyValuePair<string, string>>(),
             Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            observable.Subscribe((response) => { }, (err) => {
                errMsg = err.Message;
            });

            //Assert
            Assert.Equal(Constants.ENDPOINT_SHOULD_START_WITH, errMsg);
        }

        /// <summary>
        /// Optionses the is response not null verify.
        /// </summary>
        [Fact]
        public void Options_IsResponseNotNull_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
              .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.NotNull(resObj);
        }

        /// <summary>
        /// Optionses the is response correct type verify.
        /// </summary>
        [Fact]
        public void Options_IsResponseCorrectType_Verify()
        {
            //Arrange
            ICloud mockCloudObject = mockCloud.Object;

            mockCloud.Setup(x => x.Options<string, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
               Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT))
             .Returns(ReturnResponseObject(HttpStatusCode.OK, Constants.TEST_RESPONSE_BODY));

            //Act
            IObservable<Response<string>> resObj = mockCloudObject.Options<String, string>(Constants.TEST_API, new List<KeyValuePair<string, string>>(),
                Constants.TEST_REQUEST_BODY, BodySerialization.DEFAULT);

            //Assert
            Assert.IsType<Response<string>>(resObj.FirstOrDefault());

        }

        #endregion
    }
}
