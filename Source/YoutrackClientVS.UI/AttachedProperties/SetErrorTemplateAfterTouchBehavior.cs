using System.Windows;
using System.Windows.Controls;

namespace YouTrackClientVS.UI.AttachedProperties
{
    public class SetErrorTemplateAfterTouchBehavior : AttachableForStyleBehavior<TextBox, SetErrorTemplateAfterTouchBehavior>
    {
        private TextBox _textBox;

        private ControlTemplate _actualTemplate;

        protected override void OnAttached()
        {
            base.OnAttached();
            _textBox = AssociatedObject;
            _textBox.TextChanged += _textBox_TextChanged;
            _textBox.Loaded += _textBox_Loaded;
            _actualTemplate = Validation.GetErrorTemplate(_textBox);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _textBox.TextChanged -= _textBox_TextChanged;
            _textBox.Loaded -= _textBox_Loaded;
        }

        private void _textBox_Loaded(object sender, RoutedEventArgs e)
        {
            _actualTemplate = Validation.GetErrorTemplate(_textBox);
            Validation.SetErrorTemplate(_textBox, new ControlTemplate());
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validation.SetErrorTemplate(_textBox, _actualTemplate);
        }
    }
}
