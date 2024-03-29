﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// CreateAcccountPage.xaml etkileşim mantığı
    /// </summary>
    public partial class CreateAcccountPage : Page
    {
        public CreateAcccountPage()
        {
            InitializeComponent();
            DataContext = new ComboBoxViewModel();

        }

        static bool ValidateMail(string mail)
        {
            var isMail = new Regex(@"^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@"
                                  + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$");
            return isMail.IsMatch(mail);
        }

        static bool ValidateTC(string tcno)
        {
            if (tcno == null) return false;

            if (tcno.Length != 11) return false;

            char[] chars = tcno.ToCharArray();
            int[] a = new int[11];

            for (int i = 0; i < 11; i++)
            {
                a[i] = chars[i] - '0';
            }

            if (a[0] == 0) return false;
            if (a[10] % 2 == 1) return false;

            if (((a[0] + a[2] + a[4] + a[6] + a[8]) * 7 - (a[1] + a[3] + a[5] + a[7])) % 10 != a[9]) return false;

            if ((a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9]) % 10 != a[10]) return false;

            return true;
        }

        static bool PasswordValidation(String pass)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");


            return hasNumber.IsMatch(pass) && hasUpperChar.IsMatch(pass) && hasLowerChar.IsMatch(pass) && hasMinimum8Chars.IsMatch(pass);
        }
        private void CreateAccountBtn(object sender, RoutedEventArgs e)
        {
            
            int index = monthCmb.SelectedIndex + 1;
            string indexStr;
            if (index < 10)
            {
                indexStr = "0" + index.ToString();
            } else
            {
                indexStr = index.ToString();
            }

            string birthday = dayCmb.Text + "-" + indexStr + "-" + yearCmb.Text;
            

            UTIL.ServicesPortTypeClient client = new UTIL.ServicesPortTypeClient();

            if (client.createAccount(FName.Text, LName.Text, PhoneNum.Text, Mail.Text, password.Password, birthday, TCNo.Text) == 1)
            {
                MainPage wd = new MainPage();
                //MainWindow wd = new MainWindow();
                //Application.Current.Windows[0].Close();
                wd.AccCreated.Content = "Hesabınız Oluşturuldu! Giriş Yapabilirsiniz";
                //wd.ShowDialog();
                Application.Current.MainWindow.Content = wd;
            }
            else
            {
                ErrorMsg.Content = "Geçersiz Veya Önceden Girilmiş Bilgi Girdiniz";
                
                if(String.IsNullOrEmpty(FName.Text))
                {
                    ErrorMsgFName.Content = "*Lütfen Doldurunuz";
                } else
                {
                    ErrorMsgFName.Content = "";
                }

                if (String.IsNullOrEmpty(LName.Text))
                {
                    ErrorMsgLName.Content = "*Lütfen Doldurunuz";
                }
                else
                {
                    ErrorMsgLName.Content = "";
                }

                if (String.IsNullOrEmpty(Mail.Text) || !ValidateMail(Mail.Text))
                {
                    ErrorMsgMail.Content = "*Lütfen geçerli ve kullanılmamış bir mail adresi giriniz";
                }
                else
                {
                    ErrorMsgMail.Content = "";
                }

                if (String.IsNullOrEmpty(PhoneNum.Text) || PhoneNum.Text.IndexOf('5') != 0)
                {
                    ErrorMsgPhoneNum.Content = "*Lütfen geçerli bir telefon numarası giriniz";
                }
                else
                {
                    ErrorMsgPhoneNum.Content = "";
                }

                if (String.IsNullOrEmpty(password.Password) || !PasswordValidation(password.Password))
                {
                    ErrorMsgPassword.Foreground = new SolidColorBrush(Colors.Red);
                    ErrorMsgPassword.Content = "*Geçersiz şifre. Şifreniz en az bir büyük harf, bir küçük harf ve bir rakam içermelidir";
                }
                else
                {
                    ErrorMsgPassword.Foreground = new SolidColorBrush(Colors.Gray);
                    ErrorMsgPassword.Content = "*Şifreniz en az bir büyük harf, bir küçük harf ve bir rakam içermelidir";
                }


                if (String.IsNullOrEmpty(TCNo.Text) || !ValidateTC(TCNo.Text) )
                {
                    ErrorMsgTC.Content = "*Lütfen geçerli bir telefon numarası giriniz";
                }
                else
                {
                    ErrorMsgTC.Content = "";
                }
                
                if (String.IsNullOrEmpty(dayCmb.Text) || String.IsNullOrEmpty(monthCmb.Text) || String.IsNullOrEmpty(yearCmb.Text))
                {
                    ErrorMsgBirthday.Content = "*Lütfen doğum tarihinizi giriniz";
                }
                else
                {
                    ErrorMsgBirthday.Content = "";
                }
                
            }


        }

        private void BackBtn(object sender, RoutedEventArgs e)
        {

            MainPage pg = new MainPage();
            Application.Current.MainWindow.Content = pg;
        }
    }
}
