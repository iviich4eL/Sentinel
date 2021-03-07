using System;

namespace Sentinel.Shared.Dto
{
    public class ResponseDto
    {
        public DateTimeOffset TimeStamp { get; set; }

        public string From { get; set; }

        public string Message { get; set; }
    }
}
