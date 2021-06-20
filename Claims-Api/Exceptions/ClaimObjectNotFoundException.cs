using System;

namespace Claims_Api.Exceptions
{
    public class ClaimObjectNotFoundException : Exception    
    {
        public ClaimObjectNotFoundException(Guid id): base($"Claim Not Found for Id {id}") {
            
        }
    }
}