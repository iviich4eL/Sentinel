using System;

namespace Sentinel.Shared.Dto
{
    public class ErrorDto
    {
        public DateTimeOffset TimeStamp { get; set; }

        public string From { get; set; }

        public string Error { get; set; }
    }
}
