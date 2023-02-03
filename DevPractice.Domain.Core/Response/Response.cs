namespace DevPractice.Domain.Core.Response
{
    public class Response<T>
    {
        public Response()
        { }

        public Response(T res)
        {
            Result= res;
        }

        public Response(string error)
        {
            Error= error;
        }

        public T Result { get; set; }
        public string Error { get; set; }
    }
}
