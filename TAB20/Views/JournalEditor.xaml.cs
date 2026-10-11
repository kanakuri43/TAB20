using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TAB20.ViewModels;

namespace TAB20.Views
{
    /// <summary>
    /// Interaction logic for JournalEditor
    /// </summary>
    public partial class JournalEditor : UserControl
    {
        public JournalEditor()
        {
            InitializeComponent();
        }

        // Enter moves focus to the next field; buttons keep their normal Enter behavior.
        private void EditGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var focused = Keyboard.FocusedElement as FrameworkElement;
            if (focused == null || focused is Button || focused is ComboBoxItem) return;

            e.Handled = true;

            if (focused == IdTextBox)
            {
                // blank ID = new slip
                if (IdTextBox.Text != "")
                    (DataContext as JournalEditorViewModel)?.JournalSearchCommand.Execute(IdTextBox);
                SlipDatePicker.Focus();
                return;
            }

            if (SlipDatePicker.IsKeyboardFocusWithin)
            {
                if (focused is TextBox tb && DateTime.TryParseExact(tb.Text, "yyyyMMdd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                    SlipDatePicker.SelectedDate = d;
                DescriptionTextBox.Focus();
                return;
            }

            focused.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private static bool IsDigits(string s) => s.All(c => c >= '0' && c <= '9');

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
            => e.Handled = !IsDigits(e.Text);

        private void Price_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!(e.DataObject.GetData(DataFormats.UnicodeText) is string s && IsDigits(s)))
                e.CancelCommand();
        }

        private void Price_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
            => ((TextBox)sender).SelectAll();

        // a mouse click would otherwise drop the selection; focus first, then the click is swallowed
        private void Price_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.IsKeyboardFocusWithin) return;
            tb.Focus();
            e.Handled = true;
        }

        private void Combo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            // skip when focus returns from one of its own items (after picking)
            if (e.OriginalSource == cb && ItemsControl.ItemsControlFromItemContainer(e.OldFocus as DependencyObject) != cb)
                cb.IsDropDownOpen = true;
        }
    }
}
