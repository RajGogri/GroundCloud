using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GroundCloud.Contracts;

namespace GroundCloud.Impl
{
    public class CloudObservables : ICloud
    {
        public CloudObservables()
        {

        }

        private CancellationTokenSource TokenSource = null;

        public IObservable<bool> CancelTask()
        {
            if (TokenSource != null)
            {
                TokenSource.Cancel();
            }
            return Observable.Return(TokenSource.IsCancellationRequested);
        }

        public IObservable<Response<ResBody>> Get<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
            {

                try
                {
                    if (string.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    else
                    {
                        var httpClient = new HttpClient();
                        TokenSource = new CancellationTokenSource();

                        if (body != null)
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HelperClass.getSerializationType(bodySerialization)));

                        if (headers != null && headers.Count > 0)
                            headers.ForEach(keyValuePair =>
                            {
                                httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                            });

                        HttpResponseMessage httpResponse = await httpClient.GetAsync(endPoint, TokenSource.Token);
                        if (httpResponse != null)
                        {
                            Response<ResBody> response = new Response<ResBody>();
                            response.StatusCode = httpResponse.StatusCode;
                            string content = await httpResponse.Content?.ReadAsStringAsync();

                            if (!string.IsNullOrEmpty(content))
                            {
                                List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();
                                if (httpResponse.Headers != null)
                                    foreach (var obj in httpResponse.Headers)
                                    {
                                        resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.ToString()));
                                    }
                                response.ResponseHeader = resHeaderList;
                                try
                                {
                                    response.ResponseBody = HelperClass.DeserializeFromJson<ResBody>(content, bodySerialization);
                                }
                                catch (Exception Ex)
                                {
                                    observer.OnError(new Exception(Constants.ERROR_WHILE_SERIALIZATION + "-" + Ex.Message));
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

        public IObservable<Response<ResBody>> GetById<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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
                        TokenSource = new CancellationTokenSource();
                        HttpResponseMessage httpResponse = await httpClient.GetAsync(endPoint, TokenSource.Token);

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = await httpResponse.Content.ReadAsStringAsync();
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

        public IObservable<Response<ResBody>> Post<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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

                        TokenSource = new CancellationTokenSource();

                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = await httpClient.PostAsync(endPoint, httpContent, TokenSource.Token);
                            if (httpResponse != null)
                            {
                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    content = await httpResponse.Content.ReadAsStringAsync();
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

        public IObservable<Response<ResBody>> Put<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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

                        TokenSource = new CancellationTokenSource();

                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = await httpClient.PutAsync(endPoint, httpContent, TokenSource.Token);
                            if (httpResponse != null)
                            {
                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    content = await httpResponse.Content.ReadAsStringAsync();
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

        public IObservable<Response<ResBody>> Delete<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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
                        TokenSource = new CancellationTokenSource();
                        HttpResponseMessage httpResponse = await httpClient.DeleteAsync(endPoint, TokenSource.Token);

                        if (httpResponse != null)
                        {
                            if (httpResponse.IsSuccessStatusCode)
                            {
                                content = await httpResponse.Content.ReadAsStringAsync();
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

        public IObservable<Response<ResBody>> Patch<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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

                        TokenSource = new CancellationTokenSource();
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.REQUEST_BODY_CANNOT_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = await httpClient.PatchAsync(endPoint, httpContent, TokenSource.Token);
                            if (httpResponse != null)
                            {
                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    content = await httpResponse.Content.ReadAsStringAsync();
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

        public IObservable<Response<ResBody>> Head<ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false)
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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
                        TokenSource = new CancellationTokenSource();
                        HttpResponseMessage httpResponse = await httpClient.SendAsync(request, TokenSource.Token);

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = await httpResponse.Content.ReadAsStringAsync();
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

        public IObservable<Response<ResBody>> Options<ReqBody, ResBody>(string endPoint, List<KeyValuePair<string, string>> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT, bool sync = false) where ReqBody : Request
        {
            return Observable.Create<Response<ResBody>>(async (IObserver<Response<ResBody>> observer) =>
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
                        TokenSource = new CancellationTokenSource();
                        HttpResponseMessage httpResponse = await httpClient.SendAsync(request, TokenSource.Token);

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = await httpResponse.Content.ReadAsStringAsync();
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
    }
}
