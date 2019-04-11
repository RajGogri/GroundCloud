﻿using GroundCloud.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GroundCloud.Impl
{
    public class HttpClientCloud : ICloud
    {
        public HttpClientCloud()
        {

        }

        private CancellationTokenSource Token;
        private CancellationToken CancellationToken;

        public IObservable<Response<ResBody>> Get<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;

                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    else if (body != null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.GET_REQ_BODY_NULL));
                    }
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {

                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HelperClass.getSerializationType(bodySerialization)));
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }

                        HttpResponseMessage httpResponse = httpClient.GetAsync(endPoint).Result;

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = httpResponse.Content.ReadAsStringAsync().Result;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            try
                            {
                                response = new Response<ResBody>();
                                response.StatusCode = httpResponse.StatusCode;

                                List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                foreach (var obj in httpResponse.Headers)
                                {
                                    resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                }
                                response.ResponseHeader = resHeaderList;
                                response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                            }
                            catch (Exception Ex)
                            {
                                observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                            }
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }

                        if (response != null)
                        {
                            observer.OnNext(response);
                            observer.OnCompleted();
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Post<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;


                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    else if (body == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                    }
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        string requestObj = HelperClass.SerializeModelToJson<ReqBody>(body, bodySerialization);
                        var httpContent = new StringContent(requestObj, Encoding.UTF8, HelperClass.getSerializationType(bodySerialization));

                        if (Token == null)
                        {
                            Token = new CancellationTokenSource();
                            //token.Cancel()
                            CancellationToken = Token.Token;
                        }
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = httpClient.PostAsync(endPoint, httpContent, Token.Token).Result;
                            if (httpResponse != null)
                            {
                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    content = httpResponse.Content.ReadAsStringAsync().Result;
                                }
                                else
                                {

                                }
                            }

                            if (!string.IsNullOrEmpty(content))
                            {
                                try
                                {
                                    response = new Response<ResBody>();
                                    response.StatusCode = httpResponse.StatusCode;
                                    List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                    foreach (var obj in httpResponse.Headers)
                                    {
                                        resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                    }
                                    response.ResponseHeader = resHeaderList;
                                    response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                                }
                                catch (Exception Ex)
                                {
                                    observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                                }
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                            }
                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                            }
                        }

                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Put<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;


                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    else if (body == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                    }
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        string requestObj = HelperClass.SerializeModelToJson<ReqBody>(body, bodySerialization);
                        var httpContent = new StringContent(requestObj, Encoding.UTF8, HelperClass.getSerializationType(bodySerialization));
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        if (Token == null)
                        {
                            Token = new CancellationTokenSource();
                            CancellationToken = Token.Token;
                        }
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = httpClient.PutAsync(endPoint, httpContent, Token.Token).Result;
                            if (httpResponse != null)
                            {
                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    content = httpResponse.Content.ReadAsStringAsync().Result;
                                }
                                else
                                {
                                    //TODO
                                }
                            }

                            if (!string.IsNullOrEmpty(content))
                            {
                                try
                                {
                                    response = new Response<ResBody>();
                                    response.StatusCode = httpResponse.StatusCode;
                                    List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                    foreach (var obj in httpResponse.Headers)
                                    {
                                        resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                    }
                                    response.ResponseHeader = resHeaderList;
                                    response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                                }
                                catch (Exception Ex)
                                {
                                    observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                                }
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                            }

                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                            }
                        }

                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Delete<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;

                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    //else if (body != null)
                    //{
                    //    observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.GET_REQ_BODY_NULL));
                    //}
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        HttpResponseMessage httpResponse = httpClient.DeleteAsync(endPoint).Result;

                        if (httpResponse != null)
                        {
                            if (httpResponse.IsSuccessStatusCode)
                            {
                                content = httpResponse.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                //TODO
                            }
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            try
                            {
                                response = new Response<ResBody>();
                                response.StatusCode = httpResponse.StatusCode;
                                List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                foreach (var obj in httpResponse.Headers)
                                {
                                    resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                }
                                response.ResponseHeader = resHeaderList;
                                response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                            }
                            catch (Exception Ex)
                            {
                                observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                            }
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }

                        if (response != null)
                        {
                            observer.OnNext(response);
                            observer.OnCompleted();
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }

                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Patch<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;


                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    else if (body == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                    }
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        string requestObj = HelperClass.SerializeModelToJson<ReqBody>(body, bodySerialization);
                        var httpContent = new StringContent(requestObj, Encoding.UTF8, HelperClass.getSerializationType(bodySerialization));
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        if (Token == null)
                        {
                            Token = new CancellationTokenSource();
                            CancellationToken = Token.Token;
                        }
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = httpClient.PatchAsync(endPoint, httpContent, Token.Token).Result;
                            if (httpResponse != null)
                            {
                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    content = httpResponse.Content.ReadAsStringAsync().Result;
                                }
                                else
                                {
                                    //TODO
                                }
                            }

                            if (!string.IsNullOrEmpty(content))
                            {
                                try
                                {
                                    response = new Response<ResBody>();
                                    response.StatusCode = httpResponse.StatusCode;
                                    List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                    foreach (var obj in httpResponse.Headers)
                                    {
                                        resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                    }
                                    response.ResponseHeader = resHeaderList;
                                    response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                                }
                                catch (Exception Ex)
                                {
                                    observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                                }
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                            }
                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                            }
                        }

                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Head<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;

                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(bodySerialization.ToString()));
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, endPoint);

                        HttpResponseMessage httpResponse = httpClient.SendAsync(request).Result;

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = httpResponse.Content.ReadAsStringAsync().Result;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            try
                            {
                                response = new Response<ResBody>();
                                response.StatusCode = httpResponse.StatusCode;
                                List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                foreach (var obj in httpResponse.Headers)
                                {
                                    resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                }
                                response.ResponseHeader = resHeaderList;
                                response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                            }
                            catch (Exception Ex)
                            {
                                observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                            }
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }
                        if (response != null)
                        {
                            if (response.ResponseBody == null && response.ResponseHeader != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.RESPONSETHEADER_CANNOT_NULL));
                            }
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Options<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;

                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    //else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    //{
                    //    observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    //}
                    else if (headers == null)
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(bodySerialization.ToString()));
                        foreach (KeyValuePair<string, string> keyValuePair in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                        }

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Options, endPoint);

                        HttpResponseMessage httpResponse = httpClient.SendAsync(request).Result;

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = httpResponse.Content.ReadAsStringAsync().Result;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            try
                            {
                                response = new Response<ResBody>();
                                response.StatusCode = httpResponse.StatusCode;
                                List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();

                                foreach (var obj in httpResponse.Headers)
                                {
                                    resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                }
                                response.ResponseHeader = resHeaderList;
                                response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                            }
                            catch (Exception Ex)
                            {
                                observer.OnError(new ArgumentNullException(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
                            }
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }

                        if (response != null)
                        {
                            observer.OnNext(response);
                            observer.OnCompleted();
                        }
                        else
                        {
                            observer.OnError(new ArgumentNullException(Constants.RESPONSE_IS_NULL));
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    observer.OnError(new TaskCanceledException(Constants.TASK_CANCELLED_MSG));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<bool> CancelTask()
        {
            if (Token != null)
            {
                Token.Cancel();
            }
            return Observable.Return(Token.IsCancellationRequested);
        }
    }
}