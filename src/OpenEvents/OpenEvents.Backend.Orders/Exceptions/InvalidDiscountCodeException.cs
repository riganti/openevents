using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Orders.Exceptions
{
    public class InvalidDiscountCodeException : ConflictException
    {
        public InvalidDiscountCodeException() : base("The discount code cannot be applied to this order!")
        {
        }

    }
}