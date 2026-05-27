using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proge.PublicAPI
{
    public class Result<T> : Result
    {
        public T Value { get; private set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        public static new Result<T> Failure(string error) => new Result<T> { IsSuccess = false, ErrorMessage = error };
    }
}
