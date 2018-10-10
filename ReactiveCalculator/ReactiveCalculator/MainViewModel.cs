using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ReactiveUI;
using DynamicData;

namespace ReactiveCalculator
{
    public class MainViewModel : ReactiveObject
    {

        public ICommand Append { get; private set; }
        public ICommand Clear { get; private set; }
        public ICommand ClearEverything { get; set; }
        public ICommand Backspace { get; private set; }


        public ICommand Negate { get; private set; }
        public ICommand Add { get; private set; }
        public ICommand Calculate { get; private set; }

        

        public MainViewModel()
        {
            Append = ReactiveCommand.Create<string>((x) =>
            {
                if (this.setNewNumber)
                {
                    this.Number = x;
                    this.setNewNumber = false;
                }
                else
                {
                    this.Number = Number += x;
                }

            });
            Clear = ReactiveCommand.Create( () => this.Number = "0");
            ClearEverything = ReactiveCommand.Create(() =>
            {
                this.Number = "0";
                savedNumbers.Clear();
                savedOperators.Clear();
            });

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

            Add = ReactiveCommand.Create(() =>
            {
                SaveNumber();
                savedOperators.Enqueue(Operators.Addition);
            });

            Calculate = ReactiveCommand.Create(() =>
            {
                if (!setNewNumber)
                    SaveNumber();
                decimal total = savedNumbers.Dequeue();
                while (savedOperators.Count > 0 && savedNumbers.Count > 0)
                {
                    switch (savedOperators.Dequeue())
                    {
                        case Operators.Addition:
                            total += savedNumbers.Dequeue();
                            break;
                        default:
                            break;
                    }
                }

                Number = total.ToString();
                Expressions.Add(new Expression {Result = total});
                    //this.RaisePropertyChanged(nameof(Expressions));
            
            });

            this.WhenAnyValue(x => x.Number).Subscribe(_ => ValidateNumber());
        }

        //Adds the existing number to the operations queue and sets the new number flag
        private void SaveNumber()
        {
            if (Decimal.TryParse(number, out decimal parsedNumber))
            {
                savedNumbers.Enqueue(parsedNumber);
                setNewNumber = true;
            }
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

        private Queue<decimal> savedNumbers = new Queue<decimal>();

        private Queue<Operators> savedOperators = new Queue<Operators>();
        
        private bool setNewNumber;

        private string number;

        
        public string Number
        {
            get => number;
            set { this.RaiseAndSetIfChanged(ref number, value); }
        }
        
        public ObservableCollection<Expression> Expressions { get; } = new ObservableCollection<Expression>();
    
    

    }
}
