using Microsoft.AspNetCore.Mvc;

namespace Claims_Api.Models
{
    public class ErrorResponse :ActionResult
    {
        public int? StatusCode { get; set; }
        public string Message { get; set; }
    }
}
