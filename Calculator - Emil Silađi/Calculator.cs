using System;
using System.Globalization;
using System.Windows.Forms;

namespace Calculator___Emil_Silađi
{
    public partial class Calculator : Form
    {
        // Starting initialization of main variables

        // Represents a string where the input is saved
        string _inputText = string.Empty;
        // Operands represent numbers where the intermediate and final reslt is saved
        double operand1, operand2, operandWait, operand2Last = 0;
        // Operations are strings with descriptive meanings of mathematical operations to be performed 
        // eg. "addition"
        string operation, operationWait, operationLast = null;
        // CultureInfo serves to display the result in number format with a decimal dot instead of decimal comma
        // eg. 12.34 instead of 12,34 as it is saved in the system
        CultureInfo EnglishCulture = new CultureInfo("en-EN");

        public Calculator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Accessor declaration that serves to return and properly set the _inputText varible that is used as 
        /// a primary identifier for user input.
        /// </summary>
        public string InputText
        {
            // Get accessor invocation
            get { return _inputText; }

            // Set accessor invocation
            set
            {
                // If the value set is not an error NaN 
                if (value != "NaN")
                    // Set the _inputText to be the value
                    // If additional nubers were added by the user, parse the input so it doesn't contain "NaN" string
                    _inputText = value.Replace("NaN", "");
                else
                    // If the value set is an error NaN, set the _inputText to be "NaN"
                    _inputText = value;

                // Display _inputText in the main Text box
                txtBoxDisplay.Text = _inputText;
            }
        }

        /// <summary>
        /// btnClear Click event handler.
        /// 
        /// It's purpose is to clear all the varibles and return the Calculator to it's starting state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            _inputText = string.Empty;
            txtBoxDisplay.Text = "0";
            operand1 = 0;
            operand2 = 0;
            operandWait = 0;
            operand2Last = 0;
            operation = null;
            operationWait = null;
            operationLast = null;
        }

        /// <summary>
        /// btnEquals Click event handler.
        /// 
        /// By clicking the Equals button a series of operations is performed that result in the calculations 
        /// and display of the final result.
        /// 
        /// If the user keeps clicking the button, the last operation with the last operand is performed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEquals_Click(object sender, EventArgs e)
        {
            // If the operatio was not set to null, meaning this is the first time the button was clicked
            if (operation != null)
            {
                // Calculate the result based on operand1 and operand2
                PerformCalculation(true);

                // Set temporaray variables to survive the clear operation
                // This is required in order to keep the input if the user performs multiple clicks
                var tempOp1 = operand1;
                var tempOp2 = operand2;
                var tempOper = operation;

                // Clear
                btnClear.PerformClick();

                // Display the latest result
                InputText = tempOp1.ToString(EnglishCulture);
                // Set the variables to latest values 
                operand2Last = tempOp2;
                operationLast = tempOper;
            }

            // If this is not the first time the equals button was clicked and there was an operation
            // previosly performed
            else if (operation == null && operationLast != null)
            {
                // Current input is operand1, do not set an operation (null)
                SetOperandAndOperation(null);
                // Calculate the result based on the last operation and operand
                operand1 = Calculate(operand1, operand2Last, operationLast);
                // Set the input to be the operand1
                InputText = operand1.ToString(EnglishCulture);
            }
        }

        /// <summary>
        /// btnDivide Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDivide_Click(object sender, EventArgs e)
        {
            // Decide a course of action based on the operation
            switch (operation)
            {
                // If there was no previos operation
                case null:
                    // Set the input to be the operand1 and set division as the operation
                    SetOperandAndOperation("division");
                    break;

                // If the button divide was clicked and the previos operation was multiplication or division
                case "multiplication":
                case "division":
                    // Do not perform a calculation if the button was clicked more than once
                    // (The input text is set to be empty after each operation click)
                    if (InputText != string.Empty)
                        // Perform the previos operation
                        PerformCalculation();

                    // Set the next operation to division
                    operation = "division";
                    break;

                // If the button divide was clicked and the previos operation was addition or subtraction
                case "addition":
                case "subtraction":
                    // Do not perform if the button was clicked more than once
                    // (The input text is set to be empty after each operation click)
                    if (InputText != string.Empty)
                    {
                        // Remeber the operand and operation because the division has precedence over 
                        // addition or subtraction
                        operandWait = operand1;
                        operationWait = operation;

                        // Current input is operand1, current operation is division
                        SetOperandAndOperation("division");
                    }
                    // If the button was clicked more than once, just set the next operation to division
                    else
                        operation = "division";
                    break;
            }
        }

        /// <summary>
        /// btnMultiply Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMultiply_Click(object sender, EventArgs e)
        {
            // Decide a course of action based on the operation
            switch (operation)
            {
                // If there was no previos operation
                case null:
                    // Set the input to be the operand1 and set multiplication as the operation
                    SetOperandAndOperation("multiplication");
                    break;

                // If the button multiply was clicked and the previos operation was multiplication or division
                case "multiplication":
                case "division":
                    // Do not perform a calculation if the button was clicked more than once
                    // (The input text is set to be empty after each operation click)
                    if (InputText != string.Empty)
                        // Perform the previos operation
                        PerformCalculation();

                    // Set the next operation to multiplication
                    operation = "multiplication";
                    break;

                // If the button multiply was clicked and the previos operation was addition or subtraction
                case "addition":
                case "subtraction":
                    // Do not perform if the button was clicked more than once
                    // (The input text is set to be empty after each operation click)
                    if (InputText != string.Empty)
                    {
                        // Remeber the operand and operation because the multiplication has precedence over 
                        // addition or subtraction
                        operandWait = operand1;
                        operationWait = operation;

                        // Current input is operand1, current operation is multiplication
                        SetOperandAndOperation("multiplication");
                    }
                    // If the button was clicked more than once, just set the next operation to multiplication
                    else
                        operation = "multiplication";
                    break;
            }
        }

        /// <summary>
        /// btnMinus Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinus_Click(object sender, EventArgs e)
        {
            // Decide a course of action based on the operation
            switch (operation)
            {
                // If there was no previos operation
                case null:
                    // Set the input to be the operand1 and set subtraction as the operation
                    SetOperandAndOperation("subtraction");
                    break;

                // If the minus button was clicked and the previos operation was any other operation
                default:
                    // Do not perform a calculation if the button was clicked more than once
                    // (The input text is set to be empty after each operation click)
                    if (InputText != string.Empty)
                        // Perform the previos operation and any operations in waiting
                        PerformCalculation(true);

                    // Set the next operation to subtraction
                    operation = "subtraction";
                    break;
            }
        }

        /// <summary>
        /// btnPlus Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlus_Click(object sender, EventArgs e)
        {
            // Decide a course of action based on the operation
            switch (operation)
            {
                // If there was no previos operation
                case null:
                    // Set the input to be the operand1 and set addition as the operation
                    SetOperandAndOperation("addition");
                    break;

                // If the plus button was clicked and the previos operation was any other operation
                default:
                    // Do not perform a calculation if the button was clicked more than once
                    // (The input text is set to be empty after each operation click)
                    if (InputText != string.Empty)
                        // Perform the previos operation and any operations in waiting
                        PerformCalculation(true);

                    // Set the next operation to addition
                    operation = "addition";
                    break;
            }
        }

        /// <summary>
        /// This operation is called when we wish to begin the calculations and it's job is
        /// to set the first operand and the operation
        /// </summary>
        /// <param name="op"> Set the next operation </param>
        private void SetOperandAndOperation(string op)
        {
            // Set the next operation to be the input parameter op
            operation = op;

            // Retrieve operand1 from the input screen
            double.TryParse(InputText, NumberStyles.Any, EnglishCulture, out operand1);
            // If operand1 is NaN
            if (double.IsNaN(operand1))
                // Set the operand1 to be 0 instead, this is used to reset the input if one of the
                // operations was clicked
                operand1 = 0; 

            // Reset the input text 
            InputText = string.Empty;
            // display current operand as text
            txtBoxDisplay.Text = operand1.ToString(EnglishCulture);
        }

        /// <summary>
        /// This operation is called when there is operand1 and operation in order to perform 
        /// the final calculation
        /// </summary>
        /// <param name="final"> Set to true if all operations need to be performed, 
        /// including the operations that are waiting becasue others have precedence
        /// </param>
        private void PerformCalculation(bool final = false)
        {
            // Retrieve operand2 from the input screen
            double.TryParse(InputText, NumberStyles.Any, EnglishCulture, out operand2);

            // Calculate the result
            operand1 = Calculate(operand1, operand2, operation);
            // Reset the input text 
            InputText = string.Empty;
            // display current operand as text
            txtBoxDisplay.Text = operand1.ToString(EnglishCulture);
            
            // If it is required to perform all calculations, including the ones waiting
            // because of precedence 
            if (final && operationWait != null)
            {
                // Use the previosly calculated operand and the operand that is waiting
                // to calculate the final result 
                operand1 = Calculate(operandWait, operand1, operationWait);
                // Reset the input text 
                InputText = string.Empty;
                // display current operand as text
                txtBoxDisplay.Text = operand1.ToString(EnglishCulture);

                // Reset the waiting operand and operation
                operandWait = 0;
                operationWait = null;
            }
        }

        /// <summary>
        /// Function that offers basic calculations based on the input operation
        /// </summary>
        /// <param name="operand1"></param>
        /// <param name="operand2"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private double Calculate(double operand1, double operand2, string operation)
        {
            // Perform basic calculation based on the operation
            switch (operation)
            {
                case "addition":
                    return operand1 + operand2;
                case "subtraction":
                    return operand1 - operand2;
                case "multiplication":
                    return operand1 * operand2;
                case "division":
                    if (operand2 == 0)
                    {
                        // If the operand2 is zero, clear the calculator and return NaN
                        btnClear.PerformClick();
                        return double.NaN;
                    }
                    else
                        return operand1 / operand2;
            }
            return double.NaN;
        }

        /// <summary>
        /// btnDot Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDot_Click(object sender, EventArgs e)
        {
            // If there is no input on screen or there is an error, set the input decimal dot
            // to have a preceding 0
            if (InputText == string.Empty || InputText == "NaN")
                InputText = "0.";
            // If there is input on screen, add a decimal dot to input
            else
                // If there already is a decimal dot, do not add one to the input text
                if (!InputText.Contains("."))
                    InputText += ".";
        }

        /// <summary>
        /// btn0 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn0_Click(object sender, EventArgs e)
        {
            // If there is already some input text or operand1 is not 0, add 0 to input text
            // This prevents leading zeroes and allows adding zeroes on other numbers
            if (InputText != "" || operand1 != 0)
                InputText += "0";
        }

        /// <summary>
        /// btn1 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1_Click(object sender, EventArgs e)
        {
            InputText += "1";
        }

        /// <summary>
        /// btn2 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn2_Click(object sender, EventArgs e)
        {
            InputText += "2";
        }

        /// <summary>
        /// btn3 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3_Click(object sender, EventArgs e)
        {
            InputText += "3";
        }

        /// <summary>
        /// btn4 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn4_Click(object sender, EventArgs e)
        {
            InputText += "4";
        }

        /// <summary>
        /// btn5 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn5_Click(object sender, EventArgs e)
        {
            InputText += "5";
        }

        /// <summary>
        /// btn6 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn6_Click(object sender, EventArgs e)
        {
            InputText += "6";
        }

        /// <summary>
        /// btn7 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn7_Click(object sender, EventArgs e)
        {
            InputText += "7";
        }

        /// <summary>
        /// btn8 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn8_Click(object sender, EventArgs e)
        {
            InputText += "8";
        }

        /// <summary>
        /// btn9 Click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn9_Click(object sender, EventArgs e)
        {
            InputText += "9";
        }
    }
}
