using System;

namespace Ligg.Infrastructure.DataModels
{
    public class TResult
    {
        public TResult(int flag = 0, string message = "", string description = "")
        {
            Flag = flag;
            //Code = flag == 0 ? 1 : 0;
            Message = message;
            Description = description;
        }

        private int _flag;
        public int Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                Code = _flag == 0 ? 1 : 0;
            }
        }
        public int Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

    }

    public class TResult<T> : TResult
    {
        public TResult()
        {
        }
        public TResult(int flag, T data, string message = "", string description = "", int total = 0)
        {
            Data = data;
            Flag = flag;
            Message = message;
            Description = description;
            Total = total;
        }
        public int Total { get; set; }
        public T Data { get; set; }



    }

}
