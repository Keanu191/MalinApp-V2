/*
 * 30074191 / Keanu Farro
 * 8/10/2024
 * Astronomical processing application
 * MalinAT2
 */
using MalinAstroAPP.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MalinAstroAPP
{
    /*
     * USEFUL LINKS:
     * https://stackoverflow.com/questions/76192571/how-switch-between-form-languages-in-c
     * https://stackoverflow.com/questions/16181980/how-to-add-image-to-toolstripmenuitem
     * 14/10/2024 do the second link for images
     * Student Activity 13 on blackboard
     * 
     */

    /*
     * TO DO FOR 20/10/2024:
     * Get the DataGrid working and be able to display information from the client/server, just start of with attempting to do the temp conversion from
     * inserting a temp in a textbox in the form and try to get the servers result in the datagrid and move on from there
     */
    public partial class Form1 : Form
    {

        private DataGridView FromWCFDataGridView = new DataGridView();

        // create an instance of resource manager to display the status strip string in different languages when french or german is selected by the user
        ResourceManager rm = new ResourceManager("MalinAstroApp.Form1", typeof(Form1).Assembly);

        // load the server

        // set to static string to fix compiler error CS0236
        static string address = "net.pipe://localhost/AstroServer";
        // set to static NetNamedPipeBinding to fix compiler error CS0236
        static NetNamedPipeBinding binding =
        new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
        // set to static EndpointAddress to fix compiler error CS0236
        static EndpointAddress ep = new EndpointAddress(address);
        readonly IAstroContract channel =
        ChannelFactory<IAstroContract>.CreateChannel(binding, ep);

        // to do: create an instance of the server
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Languages

        // custom method to change the language and form background
        // create changeLanguage method with the parameter set as a string
        private void ChangeLanguage(string language)
        {
            // make a switch case statement for all 3 languages
            switch (language)
            {
                case "English":
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); // change language to english
                    DefaultUI(); // make sure darkMode isn't left on when the user switch languages or there will be an ugly visual glitch
                    break;
                case "French":
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR"); // change language to french
                    DefaultUI();
                    break;
                case "German":
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de"); // change language to german
                    DefaultUI();
                    break;
            }
            Controls.Clear(); // clear controls
            InitializeComponent();
        }

        // Buttons for language selection
        private void btnEnglish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("English"); // change language to english when the button is clicked
        }

        private void btnFrench_Click(object sender, EventArgs e)
        {
            ChangeLanguage("French"); // change language to french when the button is clicked
        }

        private void btnGerman_Click(object sender, EventArgs e)
        {
            ChangeLanguage("German"); // change language to german when the button is clicked
        }

        // Menu Strip Items for language selection
        private void menuItemEnglish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("English"); // change language to english when menustripitem is clicked
        }

        private void menuItemFrench_Click(object sender, EventArgs e)
        {
            ChangeLanguage("French"); // change language to french when menustripitem is clicked
        }

        private void menuItemGerman_Click(object sender, EventArgs e)
        {
            ChangeLanguage("German"); // change language to german when menustripitem is clicked
        }
        #endregion


        private void btnSendData_Click(object sender, EventArgs e)
        {


        }

        #region Connection Testing
        // method to test if the ping in IAstroContract in the forms solution actually is connecting with the WCF server
        private bool IsConnected()
        {
            try
            {
                channel.tempConversion(1);
                toolStripStatusLabel1.Text = rm.GetString("ConnectionSuccess"); // message will be outputed in the status strip with the users selected language
                return true;
            }
            catch
            {
                toolStripStatusLabel1.Text = rm.GetString("ErrorCatch"); // message will be outputed in the status strip with the users selected language
                return false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IsConnected();
        }
        #endregion

        #region Calculations

        // method to calculate the temperature in kelvin when the calculate button is clicked
        private void btnTemperatureInKelvin_Click(object sender, EventArgs e)
        {
            // Check if the form is connected to the server before attempting to calculate the temp in kelvin
            if (IsConnected() == true)
            {
                double celsiusInput; // this variable is whatever the user writes in the textbox set as a double due to the parameter in IAstroContract
                try
                {
                    // Try to parse the values entered in the input textboxes as doubles to calculate the selected equation, if unsuccessful return error message
                    if (double.TryParse(inputTempInKelvin.Text, out celsiusInput))
                    {
                        // Call the server to convert the users inputted temperature in degrees celsius to kelvin
                        double kelvinToDegrees = channel.tempConversion(celsiusInput);

                        // Add the Kelvin Result to the DataGrid
                        dataGridView1.Rows.Add(kelvinToDegrees);
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = rm.GetString("ErrorCatch" + ex); // message will be outputed in the status strip with the users selected language
                }
            }
            else
            {
                toolStripStatusLabel1.Text = rm.GetString("ErrorConnection"); // message will be outputed in the status strip with the users selected language
            }
        }

        // method to calculate the star distance when the calculate button is clicked
        private void btnCalculateStarDistance_Click(object sender, EventArgs e)
        {
            // Check if the form is connected to the server before attempting to calculate the star distance
            if (IsConnected() == true)
            {
                double arcSecondsInput; // this variable is whatever the user writes in the textbox set as a double due to the parameter in IAstroContract
                try
                {
                    // Try to parse the values entered in the input textboxes as doubles to calculate the selected equation, if unsuccessful return error message
                    if (double.TryParse(inputTempInKelvin.Text, out arcSecondsInput))
                    {
                        // Call the server to calculate the event horizon from the double entered in the textbox from the user
                        double arcSecondsCalculation = channel.calculateStarDistance(arcSecondsInput);

                        // Add the Star Distance Calculation to the DataGrid
                        dataGridView1.Rows.Add(arcSecondsCalculation);
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = rm.GetString("ErrorCatch" + ex); // message will be outputed in the status strip with the users selected language
                }
            }
            else
            {
                toolStripStatusLabel1.Text = rm.GetString("ErrorConnection"); // message will be outputed in the status strip with the users selected language
            }
        }

        // method to calculate the star Velocity when the calculate button is clicked
        private void btnCalculateStarVelocity_Click(object sender, EventArgs e)
        {
            // Check if the form is connected to the server before attempting to calculate the star velocity
            if (IsConnected() == true)
            {
                // variables set as double in place for observed wavelength and rest wavelength for the two textboxes on the form
                double arcObservedWaveInput;
                double restWaveInput;
                try
                {
                    // Try to parse the values entered in the input textboxes as doubles to calculate the selected equation, if unsuccessful return error message
                    if (double.TryParse(txtBoxObserved.Text, out arcObservedWaveInput) & double.TryParse(txtBoxRest.Text, out restWaveInput))
                    {
                        // Call the server to calculate the event horizon from the double entered in the textbox from the user
                        double ObservedRestCalculation = channel.measureVolicity(arcObservedWaveInput, restWaveInput); // make sure both the observed and rest wavelength are set as parameters in the channel

                        // Add the Star Velocity Calculation to the DataGrid
                        dataGridView1.Rows.Add(ObservedRestCalculation);
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = rm.GetString("ErrorCatch" + ex); // message will be outputed in the status strip with the users selected language
                }
            }
            else
            {
                toolStripStatusLabel1.Text = rm.GetString("ErrorConnection"); // message will be outputed in the status strip with the users selected language
            }
        }

        // method to calculate the event horizon when the calculate button is clicked
        private void btnCalculateEventHorizon_Click(object sender, EventArgs e)
        {
            // Check if the form is connected to the server before attempting to calculate the event horizon
            if (IsConnected() == true)
            {
                // variables set as double in place for the blackhole mass for the textbox on the form
                double blackHoleMassInput;
                try
                {
                    // Try to parse the values entered in the input textboxes as doubles to calculate the selected equation, if unsuccessful return error message
                    if (double.TryParse(txtBoxEventHorizon.Text, out blackHoleMassInput))
                    {
                        // Call the server to calculate the event horizon from the double entered in the textbox from the user
                        double ObservedRestCalculation = channel.calculateEventHorizon(blackHoleMassInput);

                        // Add the event horizon Calculation to the DataGrid
                        dataGridView1.Rows.Add(ObservedRestCalculation);
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = rm.GetString("ErrorCatch" + ex); // message will be outputed in the status strip with the users selected language
                }
            }
            else
            {
                toolStripStatusLabel1.Text = rm.GetString("ErrorConnection"); // message will be outputed in the status strip with the users selected language
            }
        }
        #endregion

        #region Colour's and Visual Appearance
        // UI METHODS
        private void DarkMode()
        {
            // set the colour as black for the controls/background of the form
            this.BackColor = Color.Black;

            foreach (Control control in this.Controls)
            {
                control.BackColor = Color.Black;
                control.ForeColor = Color.White;
            }
        }

        // method for returning the form back to its defaultish look
        private void DefaultUI()
        {
            this.BackColor = Color.White; // set the background colour in the form as white

            foreach (Control control in this.Controls)
            {
                control.BackColor = Color.White;
                control.ForeColor = Color.Black; // make sure the fore color is set to black so the labels and the text on the buttons don't go invisible
            }
        }

        // BUTTONS

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            DarkMode(); // Call DarkMode method
        }

        private void btnDefaultMode_Click(object sender, EventArgs e)
        {
            DefaultUI(); // call default UI method
        }

        // MENU STRIP BUTTONS

        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DarkMode(); // call dark mode method
        }


        private void defaultUIStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultUI(); // call default ui method
        }

        // DialogBox

        // method for allowing you to select any colour of the background of the forms
        private void btnDialog_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog(); // create an instance of colorDialog

            if (color.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = color.Color; // set color as whatever has been chosen

                // set control color as whatever the user has chosen
                foreach (Control control in this.Controls)
                {
                    control.BackColor = color.Color;
                    control.ForeColor = Color.Black; // make sure the control color is set as black so the user can see the labels/texts for whatever color they choose
                }
            }
        }

        // menu button color dialogbox option
        private void chooseColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnDialog_Click(sender, e); // hahaha im very lazy so ill just call the code to click the button in the form
        }

        // Font Dialogbox

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog(); // create an instance of fontDialog

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                // set labels to the users selected font
                this.label_StarVelocity.Font = fontDialog.Font;
                this.label_StarDistance.Font = fontDialog.Font;
                this.label_TempInKelvin.Font = fontDialog.Font;
                this.label_EventHorizon.Font = fontDialog.Font;

                // set buttons to the users selected font
                this.btnCalculateStarVelocity.Font = fontDialog.Font;
                this.btnCalculateStarDistance.Font = fontDialog.Font;
                this.btnTemperatureInKelvin.Font = fontDialog.Font;
                this.btnCalculateEventHorizon.Font = fontDialog.Font;
                this.btnConnect.Font = fontDialog.Font;
                this.btnEnglish.Font = fontDialog.Font;
                this.btnFrench.Font = fontDialog.Font;
                this.btnGerman.Font = fontDialog.Font;
                this.btnDefaultMode.Font = fontDialog.Font;
                this.btnDarkMode.Font = fontDialog.Font;
                this.btnDialog.Font = fontDialog.Font;

                // set menustrip and toolstrip to the users selected font
                this.menuStrip1.Font = fontDialog.Font;
                this.toolStripStatusLabel1.Font = fontDialog.Font;
            }
        }

        // menu button font dialogbox option
        private void chooseFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFont_Click(sender, e); // hahaha im very lazy so ill just call the code to click the button in the form
        }
        #endregion

    }
}
