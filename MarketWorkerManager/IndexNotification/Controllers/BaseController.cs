using System;

using IndexNotification.Context;

namespace IndexNotification.Controllers
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
