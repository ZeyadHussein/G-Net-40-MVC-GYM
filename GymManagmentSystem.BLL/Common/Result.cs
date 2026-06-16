using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.BLL.Common
{
    public sealed record Result(bool Success, string?Error=null, ResultKind kind = ResultKind.OK)
    {
        public static Result OK() => new(true);
        public static Result Fail(string error, ResultKind Kind = ResultKind.Conflict) => new(false, error, Kind);
        public static Result NotFound(string error="Not Found")=> new(false, error,ResultKind.NotFound);
        public static Result Validation(string error)=>new(false,error,ResultKind.ValidationFailed);
    }
    public sealed record Result<T>(bool Success,T?Value,string?Error=null,ResultKind kind=ResultKind.OK)
    {
        public static Result<T> OK(T Value) => new(true, Value);
        public static Result<T> Fail(string Error, ResultKind Kind = ResultKind.Conflict) => new(false, default, Error, Kind);
        public static Result<T> NotFound(string error = "NotFound") => new(false, default, error, ResultKind.NotFound);


    }
    public enum ResultKind
    {
        OK,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }
}
