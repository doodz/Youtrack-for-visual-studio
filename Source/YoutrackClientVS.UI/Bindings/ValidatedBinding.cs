using System.Windows.Data;

namespace YouTrackClientVS.UI.Bindings
{
    public class ValidatedBinding : Binding
    {
        public ValidatedBinding(string path) : base(path)
        {
            ValidatesOnDataErrors = true;
            ValidatesOnExceptions = true;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Mode = BindingMode.TwoWay;
        }
    }
}