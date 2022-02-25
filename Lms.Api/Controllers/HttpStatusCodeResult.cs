namespace Lms.Api.Controllers
{
    internal class HttpStatusCodeResult
    {
        private int internalServerError;

        public HttpStatusCodeResult(int internalServerError)
        {
            this.internalServerError = internalServerError;
        }
    }
}