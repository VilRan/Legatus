using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Legatus.UI
{
    public interface IUIBase
    {
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
    }
}
