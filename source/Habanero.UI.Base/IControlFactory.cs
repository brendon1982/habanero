using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public interface IControlFactory
    {
        IFilterControl CreateFilterControl();
        ITextBox CreateTextBox();
    }
}