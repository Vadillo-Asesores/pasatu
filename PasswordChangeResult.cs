using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasatu
{
    class PasswordChangeResult
    {
        public Boolean Result { get; set; }
        public Boolean ShouldRetry { get; set; }
        public string? Message { get; set; }

        public static PasswordChangeResult OK = new PasswordChangeResult(true);

        public PasswordChangeResult(Boolean Result)
        {
            this.Result = Result;
        }

        public PasswordChangeResult(Boolean Result, string? Message) : this (Result)
        {
            this.Message = Message;
        }

        public PasswordChangeResult(Boolean Result, string? Message, bool Retry) : this(Result, Message)
        {
            ShouldRetry = Retry;
        }

        public Boolean IsOk() => Result;

        public Boolean IsNok() => !Result;

    }
}
