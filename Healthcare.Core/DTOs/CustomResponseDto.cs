using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public List<string> Message { get; set; }

        public static CustomResponseDto<T> Success(int statusCode, T data, List<string> mesajList = null)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Data = data, Message = mesajList };
        }
        public static CustomResponseDto<T> Success(int statusCode, List<string> mesajList)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Message = mesajList };
        }
        public static CustomResponseDto<T> Fail(int statusCode, List<string> mesajList)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Message = mesajList };
        }
    }
}
