using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    class PanelGiz:Panel, IPanel
    {
        IList IChilliControl.Controls
        {
            get { return this.Controls; }
        }
    }
}
