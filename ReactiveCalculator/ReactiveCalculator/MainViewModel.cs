using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReactiveUI;

namespace ReactiveCalculator
{
    class MainViewModel : ReactiveObject
    {
        
        public ICommand Append { get; private set; }
        public ICommand Clear { get; private set; }
        public ICommand Backspace { get; private set; }
        public ICommand Negate { get; private set; }

        public MainViewModel()
        {
            Append = ReactiveCommand.Create<string>( (x) => { this.Number = Number += x; });
            Clear = ReactiveCommand.Create( () => this.Number = "0");
            Backspace = ReactiveCommand.Create(() => this.Number = this.Number.Substring(0, this.Number.Length - 1));

            Negate = ReactiveCommand.Create(() =>
                {

                    if (this.Number == "0")
                    {
                        //Do nothing, no such thing as negative zero
                    }
                    else if (this.Number.StartsWith("-"))
                    {
                        this.Number = this.Number.Substring(1, this.Number.Length - 1);
                    }
                    else
                    {
                        this.Number = "-" + this.Number;
                    }
                }
            );

            this.WhenAnyValue(x => x.Number).Subscribe(_ => ValidateNumber());
        }
        

        private void ValidateNumber()
        {
            if (string.IsNullOrWhiteSpace(this.Number))
            {
                this.Number = "0";
            }
            else if (this.Number != "0")
            {
                this.Number = this.Number.Trim().TrimStart(new char[] { '0' });
            }
        }

        private string number;
        
        public string Number
        {
            get => number;
            set { this.RaiseAndSetIfChanged(ref number, value); }
        }
    }
}
