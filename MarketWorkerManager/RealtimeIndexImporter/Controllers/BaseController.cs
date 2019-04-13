using System;
using RealtimeIndexImporter.Context;

namespace RealtimeIndexImporter.Controller
{
    class BaseController : IDisposable
    {
        public LogicContext Logic { get; private set; }

        public BaseController()
        {
            Logic = new LogicContext();
        }

        public void Dispose()
        {
            Logic.Dispose();
        }

        /// <summary>
        /// ローカルマシンのタイムゾーンを取得
        /// </summary>
        /// <returns></returns>
        internal TimeSpan GetMyTimeZone()
        {
            return TimeZoneInfo.Local.BaseUtcOffset;
        }
    }
}
