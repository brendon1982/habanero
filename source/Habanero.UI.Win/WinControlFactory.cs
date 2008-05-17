using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.LayoutManagers;

namespace Habanero.UI.Win
{
    public class WinControlFactory : IControlFactory
    {
        public IFilterControl CreateFilterControl()
        {
            return new FilterControlWin(this);
        }

        public ITextBox CreateTextBox()
        {
            return new TextBoxWin();
        }

        public IComboBox CreateComboBox()
        {
            return new ComboBoxWin();
        }

        public IListBox CreateListBox()
        {
            return new ListBoxWin();
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorWin<T>();
        }

        public IButton CreateButton()
        {
            return new ButtonWin();
        }

        public ICheckBox CreateCheckBox()
        {
            return new CheckBoxWin();
        }

        public ILabel CreateLabel()
        {
            return new LabelWin();
        }

        public ILabel CreateLabel(string labelText)
        {
            ILabel label = CreateLabel();
            label.Text = labelText;
            return label;
        }

        public IDateTimePicker CreateDateTimePicker()
        {
            return new  DateTimePickerWin();
        }

        public BorderLayoutManager CreateBorderLayoutManager(IChilliControl control)
        {
            return new BorderLayoutManagerWin(control);
        }

        public IPanel CreatePanel()
        {
            return new PanelWin();
        }
        public IReadOnlyGrid CreateReadOnlyGrid()
        {
            return new ReadOnlyGridWin();
        }

        public IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons()
        {
            return  new ReadOnlyGridWithButtonsWin();
        }


        public IButtonGroupControl CreateButtonGroupControl()
        {
            return new ButtonGroupControlWin(this);
        }

        public IReadOnlyGridButtonsControl CreateReadOnlyGridButtonsControl()
        {
            throw new System.NotImplementedException();
        }

        public IChilliControl CreateControl()
        {
            return new ControlWin();
        }
    }
}