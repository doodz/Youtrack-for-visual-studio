using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using YouTrackClientVS.Contracts.Interfaces.AutoCompleteTextBox;
using YouTrackClientVS.Infrastructure.AutoCompleteTextBox;

namespace YouTrackClientVS.UI.Controls.AutoCompleteTextBox
{
    [TemplatePart(Name = PartTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartListBox, Type = typeof(ListBox))]
    public class AutoCompleteTextBox : Control
    {
        public const string PartTextBox = "PART_TextBox";
        public const string PartPopup = "PART_Popup";
        public const string PartListBox = "PART_ListBox";

        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

        private IObservable<string> _textChangedObservable;

        private bool _isTemplateApplied;
        private ListBox _partListBox;
        private Popup _partPopup;
        private TextBox _partTextBox;

        private IConnectableObservable<IEnumerable<object>> _publishedResultsObservable;
        private IDisposable _publishedResultsObservableConnection;

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox),
                new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        #region Dependency Properties

        #region PopupHeight

        /// <summary>
        /// The <see cref="PopupHeight" /> dependency property's name.
        /// </summary>
        public const string PopupHeightPropertyName = nameof(PopupHeight);

        /// <summary>
        /// Gets or sets the value of the <see cref="PopupHeight" />
        /// property. This is a dependency property.
        /// </summary>
        public double PopupHeight
        {
            get => (double)GetValue(PopupHeightProperty);
            set => SetValue(PopupHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PopupHeight" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupHeightProperty = DependencyProperty.Register(
            PopupHeightPropertyName,
            typeof(double),
            typeof(AutoCompleteTextBox),
            new UIPropertyMetadata(250.0));


        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(AutoCompleteTextBox), new UIPropertyMetadata(string.Empty, OnTextChanged));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (AutoCompleteTextBox)d;
            if (self._partTextBox != null)
            self._partTextBox.Text = (string)e.NewValue;

        }

        #endregion

        #region AutoCompleteQueryResultProvider

        /// <summary>
        /// The <see cref="AutoCompleteQueryResultProvider" /> dependency property's name.
        /// </summary>
        public const string AutoCompleteQueryResultProviderName = nameof(AutoCompleteQueryResultProvider);

        /// <summary>
        /// Gets or sets the value of the <see cref="AutoCompleteQueryResultProvider" />
        /// property. This is a dependency property.
        /// </summary>
        public AutoCompleteQueryResultProvider AutoCompleteQueryResultProvider
        {
            get => (AutoCompleteQueryResultProvider)GetValue(QueryResultFunctionProperty);
            set => SetValue(QueryResultFunctionProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="AutoCompleteQueryResultProvider" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty QueryResultFunctionProperty = DependencyProperty.Register(
            AutoCompleteQueryResultProviderName,
            typeof(AutoCompleteQueryResultProvider),
            typeof(AutoCompleteTextBox),
            new UIPropertyMetadata(AutoCompleteQueryResultProvider.Empty, OnProviderFunctionChanged));

        private static void OnProviderFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (AutoCompleteTextBox)d;
            self.OnConfigurationChanged();
        }

        #endregion

        #region IsBusy (readonly)

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsBusy),
            typeof(bool),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            protected set => SetValue(IsBusyPropertyKey, value);
        }

        #endregion

        #region Results (readonly)

        private static readonly DependencyPropertyKey ResultsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Results),
            typeof(ObservableCollection<object>),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(new ObservableCollection<object>()));

        public static readonly DependencyProperty ResultsProperty = ResultsPropertyKey.DependencyProperty;

        public ObservableCollection<object> Results
        {
            get => (ObservableCollection<object>)GetValue(ResultsProperty);
            protected set => SetValue(ResultsPropertyKey, value);
        }

        #endregion

        #region Delay

        /// <summary>
        /// The <see cref="Delay" /> dependency property's name.
        /// </summary>
        public const string DelayPropertyName = nameof(Delay);

        /// <summary>
        /// Gets or sets the value of the <see cref="Delay" />
        /// property. This is a dependency property.
        /// </summary>
        public int Delay
        {
            get => (int)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Delay" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
            DelayPropertyName,
            typeof(int),
            typeof(AutoCompleteTextBox), new UIPropertyMetadata(200, OnDelayChanged, OnDelayCoerceValue));

        private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (AutoCompleteTextBox)d;
            self.OnConfigurationChanged();
        }

        private static object OnDelayCoerceValue(DependencyObject d, object baseValue)
        {
            var value = (int)baseValue;
            if (value <= 0) return 0;

            return value;
        }

        #endregion

        #region MinimumCharacters

        /// <summary>
        /// The <see cref="MinimumCharacters" /> dependency property's name.
        /// </summary>
        public const string MinimumCharactersPropertyName = nameof(MinimumCharacters);

        /// <summary>
        /// Gets or sets the value of the <see cref="MinimumCharacters" />
        /// property. This is a dependency property.
        /// </summary>
        public int MinimumCharacters
        {
            get => (int)GetValue(MinimumCharactersProperty);
            set => SetValue(MinimumCharactersProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MinimumCharacters" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumCharactersProperty = DependencyProperty.Register(
            MinimumCharactersPropertyName,
            typeof(int),
            typeof(AutoCompleteTextBox),
            new UIPropertyMetadata(2, OnMinimumCharactersChanged, OnMinimumCharactersCoerceValue));

        private static void OnMinimumCharactersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (AutoCompleteTextBox)d;
            self.OnConfigurationChanged();
        }

        private static object OnMinimumCharactersCoerceValue(DependencyObject d, object baseValue)
        {
            var value = (int)baseValue;
            if (value <= 0) return 1;

            return value;
        }

        #endregion

        #region ItemTemplate

        /// <summary>
        /// The <see cref="ItemTemplate" /> dependency property's name.
        /// </summary>
        public const string ItemTemplatePropertyName = nameof(ItemTemplate);

        /// <summary>
        /// Gets or sets the value of the <see cref="ItemTemplate" />
        /// property. This is a dependency property.
        /// </summary>
        [Bindable(true)]
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemTemplate" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            ItemTemplatePropertyName,
            typeof(DataTemplate),
            typeof(AutoCompleteTextBox),
            new UIPropertyMetadata(null));

        #endregion

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            _partTextBox = GetTemplateChild(PartTextBox) as TextBox;
            if (_partTextBox == null)
                throw new InvalidOperationException(
                    "Associated ControlTemplate has a bad part configuration. Expected a TextBox part.");

            _partPopup = GetTemplateChild(PartPopup) as Popup;
            if (_partPopup == null)
                throw new InvalidOperationException(
                    "Associated ControlTemplate has a bad part configuration. Expected a Popup part.");

            _partListBox = GetTemplateChild(PartListBox) as ListBox;
            if (_partListBox == null)
                throw new InvalidOperationException(
                    "Associated ControlTemplate has a bad part configuration. Expected a ListBox part.");

            _isTemplateApplied = true;
            //Text = _partTextBox.Text;

            _textChangedObservable = Observable.FromEventPattern<TextChangedEventArgs>(_partTextBox, "TextChanged")
                .Select(evt => ((TextBox)evt.Sender).Text);

            CaptureWindowMovementForPopupPlacement();
            RegisterKeyboardAndMouseEventHandlers();
            OnConfigurationChanged();
        }

        private void OnConfigurationChanged()
        {
            if (_isTemplateApplied)
            {
                _partTextBox.Text = string.Empty;
                Results.Clear();

                ClearSubscriptions();

                TextChangesToPopupVisibility();

                SubscribeToQueryEvents();
            }
        }

        private void SubscribeToQueryEvents()
        {

            _textChangedObservable.Subscribe(_=> Text = _partTextBox.Text);
            var textChanged = _textChangedObservable
                .Where(text => text != null && text.Length >= MinimumCharacters)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromMilliseconds(Delay));

            // Capture the function to avoid access to the dependency property on the subscriber thread.
            var getResultsFunc = AutoCompleteQueryResultProvider.GetResults;
            var resultsObservable = from searchTerm in textChanged
                                    from suggestions in
                                        getResultsFunc(new AutoCompleteQuery(searchTerm)).TakeUntil(textChanged)
                                    select suggestions;

            // Feed results into result list
            _publishedResultsObservable = resultsObservable.Publish();
            _publishedResultsObservableConnection = _publishedResultsObservable.Connect();

            var publishedResultsObservableSubscription =
                _publishedResultsObservable.Retry().ObserveOn(this).Subscribe(
                    results =>
                    {
                        IsBusy = false;
                        Results.Clear();

                        foreach (var result in results) Results.Add(result);

                        if (_partListBox.SelectedItem == null) _partListBox.SelectedItem = Results.FirstOrDefault();

                        if (_partListBox.SelectedItem != null) _partListBox.ScrollIntoView(_partListBox.SelectedItem);
                    });

            _subscriptions.Add(publishedResultsObservableSubscription);
        }

        private void TextChangesToPopupVisibility()
        {
            var textChangedBelowQueryMinimum =
                _textChangedObservable
                    .Where(text => text != null && text.Length < MinimumCharacters);

            var textChangedAboveQueryMinimum =
                _textChangedObservable
                    .Where(text => text != null && text.Length >= MinimumCharacters);

            var textBelowMinimumSubscription =
                textChangedBelowQueryMinimum.SubscribeOn(this).Subscribe(ctx => { _partPopup.IsOpen = false; });

            _subscriptions.Add(textBelowMinimumSubscription);

            var textAboveMinimumSubscription =
                textChangedAboveQueryMinimum.SubscribeOn(this).Subscribe(ctx =>
                {
                    IsBusy = true;
                    _partPopup.IsOpen = true;
                });

            _subscriptions.Add(textAboveMinimumSubscription);
        }

        private void SetResultText(IAutoCompleteQueryResult autoCompleteQueryResult)
        {
            _publishedResultsObservableConnection.Dispose();

            var savedText = _partTextBox.Text;
            if (_partTextBox.Text.Length > autoCompleteQueryResult.Cursor)
                savedText = savedText.Remove((int)autoCompleteQueryResult.Cursor);
            savedText += autoCompleteQueryResult.Title;

            _partTextBox.Text = savedText;
            _partPopup.IsOpen = false;
            _publishedResultsObservableConnection = _publishedResultsObservable.Connect();
            _partTextBox.CaretIndex = _partTextBox.Text.Length;
            _partTextBox.Focus();
            Text = _partTextBox.Text;
        }

        private void ClearSubscriptions()
        {
            foreach (var subscription in _subscriptions) subscription.Dispose();

            _subscriptions.Clear();
        }

        private void CaptureWindowMovementForPopupPlacement()
        {
            var window = Window.GetWindow(_partTextBox);
            if (window != null)
                WeakEventManager<Window, EventArgs>.AddHandler(
                    window,
                    "LocationChanged",
                    (sender, args) =>
                    {
                        if (!_partPopup.IsOpen) return;

                        var offset = _partPopup.HorizontalOffset;
                        _partPopup.HorizontalOffset = offset + 1;
                        _partPopup.HorizontalOffset = offset;
                    });
        }

        /// <summary>
        /// Registers the keyboard and mouse event handlers.
        /// These event handlers take care of 
        /// </summary>
        private void RegisterKeyboardAndMouseEventHandlers()
        {
            _partTextBox.PreviewKeyDown += (sender, args) =>
            {
                if (args.Key == Key.Up && _partListBox.SelectedIndex > 0)
                {
                    _partListBox.SelectedIndex--;
                }
                else if (args.Key == Key.Down && _partListBox.SelectedIndex < _partListBox.Items.Count - 1)
                {
                    _partListBox.SelectedIndex++;
                }
                else if ((args.Key == Key.Return || args.Key == Key.Enter) && _partListBox.SelectedIndex != -1)
                {
                    SetResultText((IAutoCompleteQueryResult)_partListBox.SelectedItem);
                    args.Handled = true;
                }
                else if (args.Key == Key.Escape)
                {
                    _partPopup.IsOpen = false;
                    args.Handled = true;
                }

                _partListBox.ScrollIntoView(_partListBox.SelectedItem);
            };

            _partListBox.PreviewTextInput += (sender, args) => { _partTextBox.Text += args.Text; };

            _partListBox.PreviewKeyDown += (sender, args) =>
            {
                if ((args.Key == Key.Return || args.Key == Key.Enter) && _partListBox.SelectedIndex != -1)
                {
                    SetResultText((IAutoCompleteQueryResult)_partListBox.SelectedItem);
                    args.Handled = true;
                }
                else if (args.Key == Key.Escape)
                {
                    _partPopup.IsOpen = false;
                    args.Handled = true;
                    _partTextBox.CaretIndex = _partTextBox.Text.Length;
                    _partTextBox.Focus();
                }
            };

            _partListBox.PreviewMouseDown += (sender, args) =>
            {
                var listboxItem = TreeUtils.FindParent<ListBoxItem>((DependencyObject)args.OriginalSource);
                if (listboxItem != null)
                {
                    SetResultText((IAutoCompleteQueryResult)listboxItem.DataContext);
                    args.Handled = true;
                }
            };
        }
    }
}