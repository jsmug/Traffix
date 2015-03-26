using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Traffix.Behaviors
{
    
    public static class NumericTextBoxBehavior
    {

        private static readonly IDictionary<TextBox, TextBox> _textBoxes = new Dictionary<TextBox, TextBox>();
        private static readonly Key[] _validKeys = new Key[] { Key.Back, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9 };


        public static readonly DependencyProperty NumericTextBoxProperty = DependencyProperty.RegisterAttached("NumericTextBox", typeof(bool), typeof(NumericTextBoxBehavior), new PropertyMetadata(false, OnNumericTextBoxChanged));


        #region public

        public static bool GetNumericTextBox(TextBox target)
        {

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return (bool)target.GetValue(NumericTextBoxProperty);

        }

        public static void SetNumericTextBox(TextBox target, bool value)
        {

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            target.SetValue(NumericTextBoxProperty, value);

        }

        #endregion

        #region private

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {

            if (!_validKeys.Contains(e.Key))
            {
                e.Handled = true;
            }

        }

        private static void OnNumericTextBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            TextBox tb = d as TextBox;

            if (tb != null)
            {

                if ((bool)e.NewValue)
                {

                    if (!_textBoxes.ContainsKey(tb))
                    {

                        tb.InputScope = new InputScope(); 
                        tb.InputScope.Names.Add(new InputScopeName() { NameValue = InputScopeNameValue.TelephoneLocalNumber });

                        tb.KeyDown += HandleKeyDown;
                        _textBoxes.Add(tb, tb);

                    }

                }
                else
                {

                    if (_textBoxes.ContainsKey(tb))
                    {

                        _textBoxes[tb].KeyDown -= HandleKeyDown;
                        _textBoxes.Remove(tb);

                    }

                }


            }

        }

        #endregion

    }

}
