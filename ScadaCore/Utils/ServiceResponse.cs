using System.Net;

namespace ScadaCore.Utils;

public class ServiceResponse<T> where T : class {
    public T? Body { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorMessage { get; set; }

    public ServiceResponse(T body, HttpStatusCode statusCode) {
        Body = body;
        StatusCode = statusCode;
        ErrorMessage = null;
    }
    
    public ServiceResponse(HttpStatusCode statusCode, string errorMessage) {
        Body = null;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}