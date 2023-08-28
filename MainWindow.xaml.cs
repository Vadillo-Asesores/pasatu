using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pasatu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly SolidColorBrush NokPanelBackground = new SolidColorBrush(Color.FromArgb(255, 255, 200, 200));
        private readonly SolidColorBrush NokBorderColor = new SolidColorBrush(Color.FromArgb(255, 111, 1, 1));
        private readonly SolidColorBrush NokTextColor = new SolidColorBrush(Color.FromArgb(255, 193, 13, 13));

        private readonly SolidColorBrush OkPanelBackground = new SolidColorBrush(Color.FromArgb(255, 234, 255, 198));
        private readonly SolidColorBrush OkBorderColor = new SolidColorBrush(Color.FromArgb(255, 62, 90, 16));
        private readonly SolidColorBrush OkTextColor = new SolidColorBrush(Color.FromArgb(255, 43, 74, 6));

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the domain name. If empty, app is disabled
            // Submit relies on this being made. If changed, check submit function
            String? domainName = ADUtils.GetDomainName();
            if (domainName == null || domainName.Length == 0)
            {
                domainErrorText.Visibility = Visibility.Visible;
                submitButton.IsEnabled = false;
                this.setErrorPanel("La aplicación sólo puede usarse en una red de dominio");
            }
            else
            {
                domainText.Text = domainName;
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable submit button
            submitButton.IsEnabled = false;

            // First clear all error labels
            this.ClearAllErrors();

            bool errors = false;

            // Check if username is set
            if (usernameText.Text.Length == 0)
            {
                errors = true;
                usernameErrorText.Visibility = Visibility.Visible;
            }

            // Check if password is set
            if (passwordText.Password.Length == 0)
            {
                errors = true;
                passwordErrorText.Visibility = Visibility.Visible;
            }

            // Check if new password is set
            // TODO check if equals to old password
            if (newPasswordText.Password.Length == 0)
            {
                errors = true;
                newPasswordErrorText.Visibility = Visibility.Visible;
            }

            // Check if passwords are equals
            if (!newPasswordText.Password.Equals(repeatText.Password))
            {
                errors = true;
                repeatErrorText.Visibility = Visibility.Visible;
            }

            if (!errors)
            {
                // Retrieve all DC controllers
                List<IPAddress> dcs = ADUtils.GetDCServersIP();

                foreach (IPAddress dcsip in dcs)
                {
                    PasswordChangeResult result = ADUtils.ChangeUserADPassword(dcsip.ToString(), domainText.Text, usernameText.Text, passwordText.Password, repeatText.Password);
                    if (result.IsOk())
                    {
                        setOkPanel("Contraseña modificada correctamente.");
                        break;
                    }

                    if (result.IsNok() && !result.ShouldRetry)
                    {
                        setErrorPanel(result.Message == null ? "Error desconocido" : result.Message);
                        break;
                    }
                }
            }

            // Need a finally?
            submitButton.IsEnabled = true;
        }

        /// <summary>
        /// Hides all error labels
        /// </summary>
        private void ClearAllErrors()
        {
            domainErrorText.Visibility = Visibility.Hidden;
            usernameErrorText.Visibility = Visibility.Hidden;
            passwordErrorText.Visibility = Visibility.Hidden;
            newPasswordErrorText.Visibility = Visibility.Hidden;
            repeatErrorText.Visibility = Visibility.Hidden;
            messagePanel.Visibility = Visibility.Hidden;
            this.HideMessagePanel();
        }

        private void setOkPanel(string message)
        {
            messagePanel.Background = OkPanelBackground;
            messageBorder.BorderBrush = OkBorderColor;
            messageText.Foreground = OkTextColor;
            messageText.Text = message;
            this.ShowMessagePanel();
        }

        private void setErrorPanel(string message)
        {
            messagePanel.Background = NokPanelBackground;
            messageBorder.BorderBrush = NokBorderColor;
            messageText.Foreground = NokTextColor;
            messageText.Text = message;
            this.ShowMessagePanel();
        }

        private void ShowMessagePanel()
        {
            messagePanel.Visibility = Visibility.Visible;
        }

        private void HideMessagePanel()
        {
            messagePanel.Visibility = Visibility.Hidden;
        }
    }
 }
