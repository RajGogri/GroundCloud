using System;
namespace GroundCloud.Contracts
{
    public static class Constants
    {
        public static string TEST_API = "https://test.com/testapi";

        public static string INVALID_ENDPOINT_URL = "http://test.com";

        public static string TEST_REQUEST_HEADER1 = "test header";

        public static string TEST_REQUEST_HEADER2 = "test header";

        public static string TEST_REQUEST_BODY = "Test request body";

        public static string TEST_RESPONSE_BODY = "Test response body";

        public static string GET_REQ_BODY_NULL = "Get request body should be null";

        public static string BAD_REQUEST_MSG = "Bad Request";

        public static string REQUEST_BODY_CANNOT_NULL = "Request body can not be null";

        public static string ENDPOINT_CANNOT_NULL = "Endpoint url can not be null";

        public static string ENDPOINT_INVALID = "Endpoint url is not valid";

        public static string REQUESTHEADER_CANNOT_NULL = "Request header can not be null";

        public static string RESPONSETHEADER_CANNOT_NULL = "Response header is null";

        public static string RESPONSE_IS_NULL = "Response is null";

        public static string ERROR_WHILE_SERIALIZATION = "Some error occured while Serialization/DeSerialization";

        public static int REQUEST_HEADER_LIMIT = 5;

        public static string STARTS_WITHTEXT = "https://";

        public static string ENDPOINT_SHOULD_START_WITH = "Endpoint should start with https";

        public static string PARAMETERNAME_TEXT = "\nParameter name: ";

        public static string PARAM_ENDPOINT = "endpoint";

        public static string PARAM_REQBODY = "body";

        public static string PARAM_REQHEADER = "headers";

        public static string PARAM_ID = "id";

        public static string PARAM_CANNOT_BE_NULL = "Argument cannot be null";

        public static string TASK_CANCELLED_MSG = "Task has been cancelled";

        public static string PARAM_ENTITY = "entity";

        public static string INSERT_FAILED = "Insertion operation failed";

        public static string NOT_ABLE_GET_INSERTED_ENTITY = "Not able to get inserted entity";

        public static string UPDATE_FAILED = "Update operation failed since entity not found";

        public static string UPSERT_FAILED = "Upsert operation failed";

        public static string DELETE_FAILED = "Delete operation failed";

        public static string DELETE_ENTITY_NOTFOUND = "Not found entity to delete";

        public static string NO_ENTITIES_FOUND = "No entity collection found";

        public static string NO_ENTITY_FOUND = "No entity found";

    }
}
