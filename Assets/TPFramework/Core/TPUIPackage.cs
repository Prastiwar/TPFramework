using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPFramework.Core
{
    public interface ITPUI
    {
        bool IsInitialized { get; }
        bool IsActive();
        void Initialize();
        void SetActive(bool enable);
    }
}
