using System;
using System.Collections.Generic;
using System.Net;

namespace GroundCloud.Contracts
{
    /// <summary>
    /// Cloud Interface Declaring Http Request Methods
    /// </summary>
    public interface ICloud
    {
        /// <summary>
        /// HTTP Get Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Get<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
        
        /// <summary>
        /// HTTP Post Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Post<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
    
        /// <summary>
        /// HTTP Put Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Put<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
       
        /// <summary>
        /// HTTP Delete Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Delete<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
        
        /// <summary>
        /// HTTP Patch Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Patch<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
        
        /// <summary>
        /// HTTP Head Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Head<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
       
        /// <summary>
        /// HTTP Options Request
        /// </summary>
        /// <typeparam name="ReqBody">Type Of HTTP Request Body</typeparam>
        /// <typeparam name="ResBody">Type Of HTTP Response Body</typeparam>
        /// <param name="body">HTTP Request Body</param>
        /// <param name="bodySerialization">HTTP Request Body Serialization</param>
        /// <param name="endPoint">HTTP Request Endpoint</param>
        /// <param name="headers">HTTP Request Headers</param>
        /// <returns>IObservable Emitting HTTP Response</returns>
        IObservable<Response<ResBody>> Options<ReqBody, ResBody>(string endPoint, KeyValuePair<string, string> headers, ReqBody body, BodySerialization bodySerialization);
    }

    /// <summary>
    /// Class For HTTP Response
    /// </summary>
    public class Response<ResBody>
    {

        /// <summary>
        /// HTTP Response Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// HTTP Response Body
        /// </summary>
        public ResBody ResponseBody { get; set; }
        /// <summary>
        /// HTTP Response Header
        /// </summary>
        public KeyValuePair<string, string> ResponseHeader { get; set; }
    }

    /// <summary>
    /// Enum For HTTP Request/HTTP Response Serialization Format
    /// </summary>
    public enum BodySerialization
    {
        /// <summary>
        /// Default Serialization - JSON
        /// </summary>
        DEFAULT, /// <summary>
                 /// JSON Serialization Format
                 /// </summary>
        JSON, /// <summary>
              /// XML Serialization Format
              /// </summary>
        XML, /// <summary>
/// Form Url Encoded Serialization
/// </summary>
        URL_FORM_ENCODED
    }
}