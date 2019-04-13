using System;

using IndexNotification.Context;

namespace IndexNotification.Controller
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
    }
}
