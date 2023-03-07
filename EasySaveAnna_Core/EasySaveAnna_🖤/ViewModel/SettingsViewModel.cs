using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EasySave_2._0.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        public SettingsViewModel()
        {
            FormatOptions = new List<string> { "JSON", "XML" };
            FileTypes = new List<string>();
        }

        public List<string> FormatOptions { get; set; }

        private static string _logFormat = "JSON"; 

        public static string logFormat
        {
            get
            {
                return _logFormat;
            }
            set
            {
                _logFormat = value;
                //OnPropertyChanged(_logFormat);

            }
        }

        public static List<string> FileTypes { get; set; }
        public static string _fileType = "";
        public static string fileType
        {
            get
            {
                return _fileType;
            }
            set
            {
                _fileType = value;
            }
        }

        //private void Feature_CheckedChanged(object sender, RoutedEventArgs e)
        //{
        //    _fileType = ".txt";
        //    MessageBox.Show(_fileType);
        //}

        //private void Featuretxt_Click(object sender, RoutedEventArgs e)
        //{
        //    //CheckBox cb = sender as CheckBox;
        //    _fileType = ".txt";
        //    MessageBox.Show(_fileType);
        //    //if(cb.Name == "Featuretxt") _fileType = ".txt";
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
