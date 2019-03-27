using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace GroundCloud.Contracts
{
    public class CloudObservables:ICloud
    {
        public CloudObservables()
        {
        }
        public static CancellationTokenSource token = null;
        public static CancellationToken CancellationToken;

        public IObservable<Response<ResBody>> Get<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) => {

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
                        else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                        {
                            observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                        }
                        else if (string.IsNullOrEmpty(headers.Key))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                        }
                        else
                        {
                            //TODO-need to change parameter
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(bodySerialization.ToString()));

                            HttpResponseMessage httpResponse = httpClient.GetAsync(endPoint).Result;

                            if (httpResponse!=null && httpResponse.IsSuccessStatusCode)
                            {
                                content = httpResponse.Content.ReadAsStringAsync().Result;
                            }

                            if (!string.IsNullOrEmpty(content))
                            {
                                response = HelperClass.DeserializeFromJson<Response<ResBody>>(content);
                            }
                            else
                            {
                                response = new Response<ResBody>();
                            }

                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                         }
                    }
                    catch(Exception ex){
                        observer.OnError(new Exception(ex.Message));
                    }
                     
                    return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Post<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) => {

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
                    else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    {
                        observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    }
                    else if (string.IsNullOrEmpty(headers.Key))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        string requestObj = HelperClass.SerializeModelToJson(body);
                        var httpContent = new StringContent(requestObj, Encoding.UTF8, bodySerialization.ToString());

                        if (token == null)
                        {
                            token = new CancellationTokenSource();
                            CancellationToken = token.Token;
                        }
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.GET_REQ_BODY_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = httpClient.PostAsync(endPoint, httpContent, token.Token).Result;
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
                                response = HelperClass.DeserializeFromJson<Response<ResBody>>(content);
                            }
                            else
                            {
                                response = new Response<ResBody>();
                            }

                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(new Exception(ex.Message));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Put<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) => {

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
                    else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    {
                        observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    }
                    else if (string.IsNullOrEmpty(headers.Key))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        string requestObj = HelperClass.SerializeModelToJson(body);
                        var httpContent = new StringContent(requestObj, Encoding.UTF8, bodySerialization.ToString());

                        if (token == null)
                        {
                            token = new CancellationTokenSource();
                            CancellationToken = token.Token;
                        }
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.GET_REQ_BODY_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = httpClient.PutAsync(endPoint, httpContent, token.Token).Result;
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
                                response = HelperClass.DeserializeFromJson<Response<ResBody>>(content);
                            }
                            else
                            {
                                response = new Response<ResBody>();
                            }

                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(new Exception(ex.Message));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Delete<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) => {

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
                    else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    {
                        observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    }
                    else if (string.IsNullOrEmpty(headers.Key))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        HttpResponseMessage httpResponse = httpClient.DeleteAsync(endPoint).Result;

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
                            response = HelperClass.DeserializeFromJson<Response<ResBody>>(content);
                        }
                        else
                        {
                            response = new Response<ResBody>();
                        }
                        if (response != null)
                        {
                            observer.OnNext(response);
                            observer.OnCompleted();
                        }

                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(new Exception(ex.Message));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Patch<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) => {

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
                    else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    {
                        observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    }
                    else if (string.IsNullOrEmpty(headers.Key))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        string requestObj = HelperClass.SerializeModelToJson(body);
                        var httpContent = new StringContent(requestObj, Encoding.UTF8, bodySerialization.ToString());

                        if (token == null)
                        {
                            token = new CancellationTokenSource();
                            CancellationToken = token.Token;
                        }
                        if (string.IsNullOrEmpty(requestObj))
                        {
                            observer.OnError(new ArgumentNullException(Constants.PARAM_REQBODY, Constants.GET_REQ_BODY_NULL));
                        }
                        else
                        {
                            HttpResponseMessage httpResponse = httpClient.PatchAsync(endPoint, httpContent, token.Token).Result;
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
                                response = HelperClass.DeserializeFromJson<Response<ResBody>>(content);
                            }
                            else
                            {
                                response = new Response<ResBody>();
                            }

                            if (response != null)
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(new Exception(ex.Message));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Head<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            return Observable.Create<Response<ResBody>>((IObserver<Response<ResBody>> observer) => {

                try
                {
                    string content = string.Empty;
                    var httpClient = new HttpClient();
                    Response<ResBody> response = null;

                    if (String.IsNullOrEmpty(endPoint))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));
                    }
                    else if (!endPoint.StartsWith(Constants.STARTS_WITHTEXT))
                    {
                        observer.OnError(new Exception(Constants.ENDPOINT_SHOULD_START_WITH));
                    }
                    else if (string.IsNullOrEmpty(headers.Key))
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_REQHEADER, Constants.REQUESTHEADER_CANNOT_NULL));
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(bodySerialization.ToString()));

                        HttpResponseMessage httpResponse = httpClient.GetAsync(endPoint).Result;

                        if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                        {
                            content = httpResponse.Content.ReadAsStringAsync().Result;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            response = HelperClass.DeserializeFromJson<Response<ResBody>>(content);
                        }
                        else
                        {
                            response = new Response<ResBody>();
                        }

                        if (response != null)
                        {
                            if(response.ResponseBody == null && !string.IsNullOrEmpty(response.ResponseHeader.Key))
                            {
                                observer.OnNext(response);
                                observer.OnCompleted();
                            }
                            else
                            {
                                //TODO
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(new Exception(ex.Message));
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Response<ResBody>> Options<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization = BodySerialization.DEFAULT)
        {
            throw new NotImplementedException();
        }




    }
}
