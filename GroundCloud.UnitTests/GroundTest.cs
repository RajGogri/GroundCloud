using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using GroundCloud.Contracts;
using Moq;
using Xunit;

namespace GroundCloud.UnitTests
{
    public class GroundTest
    {
        Mock<IGround> mockGround = new Mock<IGround>();
        public GroundTest()
        {
        }


        #region TestCases for Insert API
        /// <summary>
        /// Should throw exception if insert argument is null.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfInsertArgumentIsNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;
            string param = null;

            //Act
            mockGround.Setup(x => x.Insert<string>(param)).Returns(Observable.Create<string>((IObserver<string> observer) =>
            {
                observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.PARAM_CANNOT_BE_NULL));
                return Disposable.Empty;
            }));
            IObservable<string> observable = mockGroundObject.Insert(param);
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.PARAM_CANNOT_BE_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, ErrMsg);
        }

        /// <summary>
        /// Should validate if insert argument is not null.
        /// </summary>
        [Fact]
        public void ShouldValidateIfInsertArgumentIsNotNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;

            //Act
            mockGround.Setup(x => x.Insert<string>(Constants.TEST_PARAM)).Returns(Observable.Return<string>("Test"));
            IObservable<string> ResponseObject = mockGroundObject.Insert<string>(Constants.TEST_PARAM);

            //Assert
            Assert.NotNull(ResponseObject);
        }

        #endregion

        #region TestCases for Update API
        /// <summary>
        /// Should throw exception if Update argument is null.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfUpdateArgumentIsNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;
            string param = null;

            //Act
            mockGround.Setup(x => x.Update<string>(param)).Returns(Observable.Create<string>((IObserver<string> observer) =>
            {
                observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.PARAM_CANNOT_BE_NULL));
                return Disposable.Empty;
            }));
            IObservable<string> observable = mockGroundObject.Update(param);
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.PARAM_CANNOT_BE_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, ErrMsg);
        }

        #endregion

        #region TestCases for Delete API
        /// <summary>
        /// Should throw exception if Delete argument is null.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfDeleteArgumentIsNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;
            string param = null;

            //Act
            mockGround.Setup(x => x.Delete<string>(param)).Returns(Observable.Create<string>((IObserver<string> observer) =>
            {
                observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.PARAM_CANNOT_BE_NULL));
                return Disposable.Empty;
            }));
            IObservable<string> observable = mockGroundObject.Delete(param);
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.PARAM_CANNOT_BE_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, ErrMsg);
        }

        #endregion

        #region TestCases for FetchById API
        /// <summary>
        /// Should throw exception if Update argument is null.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfFetchByIdArgumentIsNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;
            string param = null;

            //Act
            mockGround.Setup(x => x.FetchById<string>(param)).Returns(Observable.Create<string>((IObserver<string> observer) =>
            {
                observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.PARAM_CANNOT_BE_NULL));
                return Disposable.Empty;
            }));
            IObservable<string> observable = mockGroundObject.FetchById<string>(param);
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.PARAM_CANNOT_BE_NULL + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, ErrMsg);
        }

        /// <summary>
        /// Should throw exception if fetch by identifier generates bad request.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfFetchByIdGeneratesBadRequest()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;
            int param = 0;

            //Act
            mockGround.Setup(x => x.FetchById<string>(param.ToString())).Returns(Observable.Create<string>((IObserver<string> observer) =>
            {
                observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.BAD_REQUEST_MSG));
                return Disposable.Empty;
            }));
            IObservable<string> observable = mockGroundObject.FetchById<string>(param.ToString());
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.BAD_REQUEST_MSG + Constants.PARAMETERNAME_TEXT + Constants.PARAM_ENDPOINT, ErrMsg);
        }

        /// <summary>
        /// Should validate if fetch by id response is not null.
        /// </summary>
        [Fact]
        public void ShouldValidateIfFetchByIdResponseIsNotNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;

            //Act
            mockGround.Setup(x => x.FetchById<string>(Constants.TEST_PARAM)).Returns(Observable.Return<string>("Test"));
            IObservable<string> ResponseObject = mockGroundObject.FetchById<string>(Constants.TEST_PARAM);

            //Assert
            Assert.NotNull(ResponseObject);
        }


        //TODO:
        public void ShouldValidateIfFetchByIdResponseIsNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;

            //Act
            mockGround.Setup(x => x.FetchById<string>(Constants.TEST_PARAM)).Returns(Observable.Empty<string>());
            IObservable<string> ResponseObject = mockGroundObject.FetchById<string>(Constants.TEST_PARAM);

            //Assert
            Assert.Null(ResponseObject);
        }

        /// <summary>
        /// Should validate the correct type generated by FetchById API
        /// </summary>
        [Fact]
        public void ShouldValidateIfFetchByIdResponseIsCorrectType()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;

            //Act
            mockGround.Setup(x => x.FetchById<string>(Constants.TEST_PARAM)).Returns(Observable.Return<string>("Test"));
            IObservable<string> ResponseObject = mockGroundObject.FetchById<string>(Constants.TEST_PARAM);

            //Assert
            Assert.IsType<string>(ResponseObject.FirstOrDefault());
        }

        /// <summary>
        /// Should throw exception if response is not found.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfResponseisNotFound()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;
            string param = null;

            //Act
            mockGround.Setup(x => x.FetchById<string>(param)).Returns(Observable.Create<string>((IObserver<string> observer) =>
            {
                observer.OnError(new Exception(HttpStatusCode.NotFound.ToString()));
                return Disposable.Empty;
            }));
            IObservable<string> observable = mockGroundObject.FetchById<string>(param);
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.RESPONSE_NOT_FOUND, ErrMsg);
        }


        #endregion

        #region TestCases for FetchALL

        /// <summary>
        /// Should throw exception if fetch all results is null.
        /// </summary>
        [Fact]
        public void ShouldThrowExceptionIfFetchAllIsNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;

            //Act
            mockGround.Setup(x => x.FetchAll<string>()).Returns(Observable.Create((IObserver<IEnumerable<string>> observer) =>
            {
                observer.OnError(new NullReferenceException());
                return Disposable.Empty;
            }));
            IObservable<IEnumerable<string>> observable = mockGroundObject.FetchAll<string>();
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.RESPONSE_NULL, ErrMsg);
        }

        [Fact]
        public void ShouldThrowExceptionIfFetchAllGeneratesBadRequest()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            string ErrMsg = null;

            //Act
            mockGround.Setup(x => x.FetchAll<string>()).Returns(Observable.Create((IObserver<IEnumerable<string>> observer) =>
            {
                observer.OnError(new NullReferenceException(Constants.BAD_REQUEST_MSG));
                return Disposable.Empty;
            }));
            IObservable<IEnumerable<string>> observable = mockGroundObject.FetchAll<string>();
            observable.Subscribe((response) => { }, (err) => { ErrMsg = err.Message; });

            //Assert
            Assert.Equal(Constants.BAD_REQUEST_MSG, ErrMsg);
        }

        /// <summary>
        /// Should validate if fetch all response is not null.
        /// </summary>
        [Fact]
        public void ShouldValidateIfFetchAllResponseIsNotNull()
        {
            //Arrange
            IGround mockGroundObject = mockGround.Object;
            IEnumerable<string> DummyList = Enumerable.Empty<string>();

            //Act
            mockGround.Setup(x => x.FetchAll<string>()).Returns(Observable.Return(DummyList));
            IObservable<IEnumerable<string>> ResponseObject = mockGroundObject.FetchAll<string>();

            //Assert
            Assert.NotNull(ResponseObject);
        }

        #endregion
    }
}
