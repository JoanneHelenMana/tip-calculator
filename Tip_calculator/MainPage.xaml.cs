namespace Tip_calculator;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
    }

    public double billAmount = 0;
    public double tipAmount = 0;
    public double totalAmount = 0;
    public double costPerDiner = 0;

    public string currentEntry = "";
    public bool hasStarted = false;
    public bool hasTypedDecimal = false;
    public int numberOfDecimalDigits = -1;  // -1: no decimal added || 0: decimal added || 1 and 2: amount of digits added 


    /// <summary>
    /// This is triggered when the buttons on the keypad are clicked. The user uses the keypad to enter the bill amount.
    /// The program retrieves each value and updates the screen as the buttons are clicked.
    /// Rules:
    /// 
    /// - 'C' clears all values. 
    /// - A maximum of 2 digits is allowed after the decimal point. 
    /// - Numbers starting with '0' are not allowed.
    /// - If the first button the user clicks is '.'(decimal point), it is understood the user meant a fraction of a number, e.g., 0,50.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnButtonPressed(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string pressed = button.Text;

        if (pressed == "C") // resets amounts and variables to default
        {
            currentEntry = "0";
            hasStarted = false;
            hasTypedDecimal = false;
            numberOfDecimalDigits = -1;
            billAmount = 0;
        }

        else if (pressed == "0")
        {
            if (hasStarted == false)    // 0 in inital position
            {
                return;
            }

            else if (hasStarted == true)    // 0 not in initial position
            {
                if (hasTypedDecimal == true && numberOfDecimalDigits > 2)   // prevents adding more than two decimal digits
                {
                    return;
                }

                else if (hasTypedDecimal == true && numberOfDecimalDigits < 2)  // adds 0 as a decimal digit
                {
                    currentEntry += pressed;
                    numberOfDecimalDigits++;
                }

                else
                {
                    currentEntry += pressed;
                }
            }
        }

        else if (pressed == ".")
        {
            if (hasTypedDecimal == false)
            {
                hasTypedDecimal = true; // first decimal place
                numberOfDecimalDigits++;

                if (hasStarted == true) // decimal not in initial position
                {                       
                    currentEntry += pressed;
                }

                else if (hasStarted == false)   // decimal in initial position, 0 is added to the left
                {
                    currentEntry = "0" + pressed;                        
                }
            }

            else
            {
                return; // prevents decimal place being added more than once
            }
        }

        else
        {
            if (hasTypedDecimal == true && numberOfDecimalDigits < 2)   // prevents adding more than 2 decimal digits
            {
                currentEntry += pressed;
                numberOfDecimalDigits++;
            }

            else if (hasTypedDecimal == false)
            {
                currentEntry += pressed;
                hasStarted = true;
            }
        }
        
        billAmountLabel.Text = $"${currentEntry}";
        UpdateUI();
    }


    /// <summary>
    /// Retrieves the tip percentage selected by the user and updates the screen accordingly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void percentageSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double percentage = percentageSlider.Value;
        percentageLabel.Text = Convert.ToInt32(percentage) + "%";
        UpdateUI();
    }


    /// <summary>
    /// Retrieves the number of diners entered by the user and updates the screen accordingly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dinersStepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int diners = Convert.ToInt32(dinersStepper.Value);
        dinersNumberLabel.Text = diners.ToString();
        UpdateUI();
    }


    /// <summary>
    /// Updates the bill, tip, total, and cost per diner amounts on the screen. 
    /// </summary>
    private void UpdateUI()
    {
        int inputTipPercentage = Convert.ToInt32(percentageSlider.Value);
        string dollarSignRemoved = billAmountLabel.Text.TrimStart('$');
        billAmount = double.Parse(dollarSignRemoved);

        tipAmount = Convert.ToDouble(billAmount * inputTipPercentage / 100);
        totalAmount = billAmount + tipAmount;
        costPerDiner = totalAmount / dinersStepper.Value;

        billAmountLabel.Text = "$" + billAmount.ToString("F2");
        tipAmountLabel.Text = "$" + tipAmount.ToString("F2");
        totalAmountLabel.Text = "$" + totalAmount.ToString("F2");
        costPerDinerLabel.Text = "$" + costPerDiner.ToString("F2");
    }
}
