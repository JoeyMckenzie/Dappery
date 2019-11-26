namespace Dappery.Core.Exceptions
{
    public class DapperyApiError
    {
        public DapperyApiError(string errorMessage, string propertyName)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }

        public string ErrorMessage { get; }

        public string PropertyName { get; }
    }
}