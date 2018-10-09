using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ReactiveCalculator
{
    class MainViewModel : BindableObject
    {
        public static readonly BindableProperty NumberProperty = BindableProperty.Create(
            nameof(Number),
            typeof(string),
            typeof(MainViewModel),
            "0",
            BindingMode.TwoWay,null, NumberChanged
            
        );

        private static void NumberChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var main = bindable as MainViewModel;
            if (string.IsNullOrWhiteSpace(main.Number))
                main.Number = "0";
        }


        public string Number
        {
            get => (string)this.GetValue(NumberProperty);
            set => this.SetValue(NumberProperty,value); 
        }
    }
}
