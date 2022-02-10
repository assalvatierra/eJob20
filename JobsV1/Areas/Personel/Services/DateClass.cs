using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Services
{
    public class DateClass
    {
        public DateTime GetCurrentDate()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }

        public DateTime GetCurrentDateTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            return _localTime;
        }
    }
}