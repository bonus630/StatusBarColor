using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StatusBarColor
{
    /// <summary>
    /// Interaction logic for NumericTextBoxMS.xaml
    /// </summary>

    public partial class NumericTextBoxMS : TextBox
    {
        char regionSymbol;
        char toReplaceSymbol;
        Regex rg;
        public NumericTextBoxMS()
        {
            InitializeComponent();
            regionSymbol = (1.1).ToString()[1];
            if (regionSymbol == ',')
                toReplaceSymbol = '.';
            else
                toReplaceSymbol = ',';

            rg = NumericTextBoxMS.InstantiateRegex(this.DecimalPlaces, this.NumericType);
            CommandBinding commandBinding = new CommandBinding(RepeatDown, ValueDown);
            CommandBindings.Add(commandBinding);
            CommandBinding commandBinding2 = new CommandBinding(RepeatUp, ValueUp);
            CommandBindings.Add(commandBinding2);


        }
        public event Action<double> ValueChangedEvent;

        #region DependencyProperties

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit",
            typeof(string), typeof(NumericTextBoxMS), new PropertyMetadata(null));
        [Browsable(true)]
        [Category("Values")]
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        public static readonly DependencyProperty NumericTypeProperty = DependencyProperty.Register("NumericType",
            typeof(ControlDownNumericTypeMS), typeof(NumericTextBoxMS), new PropertyMetadata(ControlDownNumericTypeMS._Double));
        [Browsable(true)]
        [Category("Values")]
        public ControlDownNumericTypeMS NumericType
        {
            get { return (ControlDownNumericTypeMS)GetValue(NumericTypeProperty); }
            set { SetValue(NumericTypeProperty, value); }
        }

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue",
            typeof(double), typeof(NumericTextBoxMS), new PropertyMetadata(0d, new PropertyChangedCallback(OnDefaultValuePropertyChangedCallback)));
        [Browsable(true)]
        [Category("Values")]
        public double DefaultValue
        {
            get { return (double)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces",
            typeof(int), typeof(NumericTextBoxMS), new PropertyMetadata(2, new PropertyChangedCallback(OnChangedDecimalPlacesCallback)));
        [Browsable(true)]
        [Category("Values")]
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.Register("MinimumValue",
       typeof(double), typeof(NumericTextBoxMS), new PropertyMetadata(null));
        [Browsable(true)]
        [Category("Values")]
        public double MinimumValue
        {
            get { return (double)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue",
       typeof(double), typeof(NumericTextBoxMS), new PropertyMetadata(null));
        [Browsable(true)]
        [Category("Values")]
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly RoutedCommand RepeatDown = new RoutedCommand();
        public static readonly RoutedCommand RepeatUp = new RoutedCommand();

        #endregion
        public string UserInputText
        {
            get
            {
                return this.Text.ToString(CultureInfo.InvariantCulture);
            }
            set
            {
                this.Text = value;
            }
        }
        public Visibility SetVisible { set { this.Visibility = value; } }


        //public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", 
        //    typeof(double), typeof(NumericTextBox), new PropertyMetadata(0,OnValuePropertyChangedCallback));

        private double prevValue = 0;
        private double _value = 0;
        [Browsable(false)]
        public double Value
        {
            get
            {

                double.TryParse(Text.Replace(toReplaceSymbol, regionSymbol), out _value);
                return _value;
            }
            set
            {
                _value = value;
                Text = _value.ToString();
            }
        }

        private void restoreDefault()
        {
            this.Text = DefaultValue.ToString();
        }
        private void checkTextFormat(object sender, TextCompositionEventArgs e)
        {
            //e.Handled = true; não continua a adição de texto
            //return;

            string text = (e.Source as TextBox).Text;
            string prevText = e.Text;
            bool stop = false;
            stop = !(rg.IsMatch(text) & rg.IsMatch(prevText));
            if (prevText == regionSymbol.ToString() || prevText == toReplaceSymbol.ToString())
            {

                stop = (text.Contains(regionSymbol) || text.Contains(toReplaceSymbol));
                if (this.NumericType == ControlDownNumericTypeMS._Int)
                    stop = true;
                if (prevText == toReplaceSymbol.ToString() && !stop)
                {
                    TextBox tb = sender as TextBox;
                    if (tb.Text.Length == 0)
                        tb.AppendText(regionSymbol.ToString());
                    int caretIndex = tb.CaretIndex;
                    tb.Text = tb.Text.Insert(caretIndex, "0");

                    tb.CaretIndex = caretIndex + 1;
                    e.Handled = true;
                    return;
                }
                if (prevText == regionSymbol.ToString() && !stop)
                {
                    TextBox tb = sender as TextBox;
                    if (tb.Text.Length == 0)
                        tb.AppendText("0");
                    int caretIndex = tb.CaretIndex;
                    tb.Text = tb.Text.Insert(caretIndex, regionSymbol.ToString());

                    tb.CaretIndex = caretIndex + 1;
                    e.Handled = true;
                    return;
                }
            }
            e.Handled = stop;
            Debug.WriteLine("TXT:" + (sender as TextBox).Text);
        }
        protected void ValueDown(object sender, ExecutedRoutedEventArgs e)
        {
            
            if (Value > MinimumValue)
                 Value -= 1;

        }
        protected void ValueUp(object sender, ExecutedRoutedEventArgs e)
        {
           
            if (Value < MaxValue)
                 Value += 1; 

        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {

            int d = e.Delta / 30; Console.WriteLine(d);
            double tempValue = Value;
            tempValue += d;
            if (tempValue < MinimumValue)
                tempValue = MinimumValue;
            if (tempValue > MaxValue)
                tempValue = MaxValue;
            Value = tempValue;
            base.OnMouseWheel(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
        private static Regex InstantiateRegex(int decimalPlaces, ControlDownNumericTypeMS numericType)
        {

            string pattern = "";
            int d = 1;
            if (decimalPlaces > d)
                d = decimalPlaces - d;
            if (numericType == ControlDownNumericTypeMS._Double)
                pattern = @"^(-)?(\d+)?([\.|,]{1})?([\d]{0," + d + "})?$";
            if (numericType == ControlDownNumericTypeMS._Int)
                pattern = @"^(-)?(\d+)?$";
            Regex rg = new Regex(pattern);
            return rg;
        }

        #region overrides

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
        private void OnValueChanged()
        {
            if (prevValue == Value)
                return;
            else
                prevValue = Value;
            if (ValueChangedEvent != null)
                ValueChangedEvent(Value);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (this.Text == "0")
                this.Text = "";
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (this.Text == "")
                this.Text = "0";

        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            checkTextFormat(this, e);
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            OnValueChanged();
        }
        #endregion

        #region Callbacks
        private static void OnChangedDecimalPlacesCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBoxMS ntx = d as NumericTextBoxMS;
            ntx.rg = InstantiateRegex((int)e.NewValue, ntx.NumericType);
        }
        private static void OnDefaultValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBoxMS ntx = d as NumericTextBoxMS;
            ntx.Text = e.NewValue.ToString();
        }
        #endregion

    }
    [Flags]
    public enum ControlDownNumericTypeMS
    {
        _Double,
        _Int
    }
}

