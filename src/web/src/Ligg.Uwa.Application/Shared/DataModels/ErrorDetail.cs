namespace Ligg.Uwa.Application.Shared
{
    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Object { get; set; }
        public string Message { get; set; }
        public string AdditionalMessage { get; set; }
        public ErrorDetail(string code, string obj, string msg, string addlMsg)
        {

            Code = code;
            Object = obj;
            Message = msg;
            AdditionalMessage = addlMsg;
        }

    }



}
