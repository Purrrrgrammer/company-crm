namespace CompanyCRM.Common
{
    public sealed class Result
    {
        public bool IsSuccessed { get; private set; }  
        public string Message { get; private set; }

        private Result() {}
        
        public static Result CreateSuccessed() =>
            new Result { IsSuccessed = true, Message = null };
        
        public static Result CreateFailed(string message) =>
            new Result { IsSuccessed = false, Message = message };
    }
}