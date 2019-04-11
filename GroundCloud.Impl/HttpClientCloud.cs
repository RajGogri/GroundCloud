using GroundCloud.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace GroundCloud.Impl
{
    public class HttpClientCloud : ICloud
    {
        private CancellationTokenSource TokenSource = null;

        private IObservable<HttpResponseMessage> SendRequest(HttpRequestMessage httpRequest)
        {
            HttpClient httpClient = new HttpClient();
            TokenSource = new CancellationTokenSource();
            return Observable.FromAsync<HttpResponseMessage>(() => httpClient.SendAsync(httpRequest, TokenSource.Token));
        }

        private IObservable<Response<ResBody>> ParseResponse<ResBody>(
            HttpResponseMessage httpResponse,
            BodySerialization bodySerialization)
        {
            return Observable.Create<Response<ResBody>>(async obsr =>
            {
                if (httpResponse != null)
                {
                    //set http response code
                    Response<ResBody> response = new Response<ResBody>
                    {
                        StatusCode = httpResponse.StatusCode
                    };

                    //set http headers
                    if (httpResponse.Headers != null)
                    {
                        List<KeyValuePair<string, string>> resHeaderList = new List<KeyValuePair<string, string>>();
                        foreach (KeyValuePair<string, IEnumerable<string>> obj in httpResponse.Headers)
                        {
                            resHeaderList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value?.ToString()));
                        }
                        response.ResponseHeader = resHeaderList;
                    }

                    //set http response body
                    string content = await httpResponse.Content?.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content) && !string.IsNullOrWhiteSpace(content))
                    {
                        try
                        {
                            response.ResponseBody = await CloudHelper.DeserializeAsync<ResBody>(content, bodySerialization);
                        }
                        catch (Exception ex)
                        {
                            obsr.OnError(ex);
                        }
                    }

                    obsr.OnNext(response);
                    obsr.OnCompleted();
                }
                else
                {
                    obsr.OnError(new Exception(Constants.RESPONSE_IS_NULL));
                }
                return Disposable.Empty;
            });
        }

        private IObservable<HttpRequestMessage> PrepareRequest<ReqBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization,
            HttpMethod httpMethod) where ReqBody : Request
        {
            //throw error when uri endpoint is null
            if (string.IsNullOrEmpty(endPoint))
                return Observable.Throw<HttpRequestMessage>(new ArgumentNullException(Constants.PARAM_ENDPOINT, Constants.ENDPOINT_CANNOT_NULL));

            return Observable.Create<HttpRequestMessage>(async (obsr) =>
            {
                HttpRequestMessage reqMessage = new HttpRequestMessage(httpMethod, endPoint);
                //add content
                if (body != null)
                {
                    reqMessage.Content = new StringContent(await CloudHelper.SerializeAsync(body, bodySerialization), Encoding.UTF8, CloudHelper.GetSerializationType(bodySerialization));
                }
                //add headers
                if (headers != null && headers.Count > 0)
                {
                    headers.ForEach(keyValuePair =>
                    {
                        reqMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    });
                }
                obsr.OnNext(reqMessage);
                obsr.OnCompleted();
                return Disposable.Empty;
            });
        }
        public IObservable<bool> CancelTask()
        {
            if (TokenSource != null)
            {
                TokenSource.Cancel();
                return Observable.Return(TokenSource.IsCancellationRequested);
            }
            else
                return Observable.Return(false);
        }
        public IObservable<Response<ResBody>> Get<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, body, bodySerialization, HttpMethod.Get)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));

        public IObservable<Response<ResBody>> GetById<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, body, bodySerialization, HttpMethod.Get)
                .SelectMany((httpReq) => SendRequest(httpReq)).SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));


        public IObservable<Response<ResBody>> Post<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, body, bodySerialization, HttpMethod.Post)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));


        public IObservable<Response<ResBody>> Put<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, body, bodySerialization, HttpMethod.Put)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));


        public IObservable<Response<ResBody>> Delete<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, body, bodySerialization, HttpMethod.Delete)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));


        public IObservable<Response<ResBody>> Patch<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, body, bodySerialization, HttpMethod.Patch)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));


        public IObservable<Response<ResBody>> Head<ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) =>
            PrepareRequest(endPoint, headers, default(Request), bodySerialization, HttpMethod.Head)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));


        public IObservable<Response<ResBody>> Options<ReqBody, ResBody>(
            string endPoint,
            List<KeyValuePair<string, string>> headers,
            ReqBody body,
            BodySerialization bodySerialization = BodySerialization.DEFAULT,
            bool sync = false) where ReqBody : Request =>
            PrepareRequest(endPoint, headers, default(Request), bodySerialization, HttpMethod.Options)
            .SelectMany((httpReq) => SendRequest(httpReq))
            .SelectMany(httpRes => ParseResponse<ResBody>(httpRes, bodySerialization));

    }
}