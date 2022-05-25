using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfLibrary1;

namespace WpfApp1
{


    public partial class MainWindow : Window
    {
        ViewData myviewdata= new ViewData();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = myviewdata;
            timelist.ItemsSource = myviewdata.myben.timecomp;
            acclist.ItemsSource = myviewdata.myben.acccomp;
            vischeck.DataContext = myviewdata.mychan;
        }

        void butnew(object sender, RoutedEventArgs e)
        {
            if (!myviewdata.mychan)
            {
                myviewdata = new ViewData();
            }
            else
            {
                System.Windows.Forms.DialogResult result;
                result = System.Windows.Forms.MessageBox.Show("Unsaved changes.Save before continuing?", "Unsaved changes", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".txt";
                    dlg.Filter = "Text documents (.txt)|*.txt"; 
                    dlg.ShowDialog();
                    myviewdata.Save(dlg.SafeFileName);
                }
                myviewdata = new ViewData();
            }
            
            timelist.ItemsSource = myviewdata.myben.timecomp;
            acclist.ItemsSource = myviewdata.myben.acccomp;
            myviewdata.mychan = false;
            vischeck.DataContext = myviewdata.mychan;
            DataContext = myviewdata;
            funch.Text = "";
            parch.Text = "";
            maxep.DataContext = myviewdata.myben.minEPtoC;
            maxha.DataContext = myviewdata.myben.minHAtoC;
            
        }

        void butopen(object sender, RoutedEventArgs e)
        {
            bool mybl = false;
            if (!myviewdata.mychan)
            {
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                myviewdata = new ViewData();
                myviewdata.myfilename = openFileDialog.FileName;
                myviewdata.Load(myviewdata.myfilename);
            }
            else
            {
                System.Windows.Forms.DialogResult result;
                result = System.Windows.Forms.MessageBox.Show("Unsaved changes.Save before opening?", "Unsaved changes", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".txt";
                    dlg.Filter = "Text documents (.txt)|*.txt";
                    dlg.ShowDialog();
                    myviewdata.Save(dlg.SafeFileName);
                }
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                myviewdata = new ViewData();
                myviewdata.myfilename = openFileDialog.FileName;
                mybl=myviewdata.Load(myviewdata.myfilename);
            }
            if (mybl)
            {
                timelist.ItemsSource = myviewdata.myben.timecomp;
                acclist.ItemsSource = myviewdata.myben.acccomp;
                myviewdata.mychan = false;
                vischeck.DataContext = myviewdata.mychan;
                DataContext = myviewdata;
                funch.Text = "";
                parch.Text = "";
                maxep.DataContext = myviewdata.myben.minEPtoC;
                maxha.DataContext = myviewdata.myben.minHAtoC;
            }

        }

        void butsave(object sender, RoutedEventArgs e)
        {
            bool mybl = false;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            mybl= (bool)dlg.ShowDialog();
            myviewdata.Save(dlg.SafeFileName);
            if (mybl)
            {
                timelist.ItemsSource = myviewdata.myben.timecomp;
                acclist.ItemsSource = myviewdata.myben.acccomp;
                myviewdata.mychan = false;
                vischeck.DataContext = myviewdata.mychan;
                DataContext = myviewdata;
                funch.Text = "";
                parch.Text = "";
            }

        }

        void buttime(object sender, RoutedEventArgs e)
        {
            if (myviewdata.myVMf == -1)
            {
                MessageBox.Show("Choose function!");
            }
            else
            {
                if ((myviewdata.myVMGrid.vecbeg < myviewdata.myVMGrid.vecend) && (myviewdata.myVMGrid.arglen != 0))
                {
                    myviewdata.mychan = true;
                    vischeck.DataContext = myviewdata.mychan;
                    myviewdata.myVMGrid.VMf = myviewdata.myVMf;
                    myviewdata.AddVMTime(myviewdata.myVMGrid);
                    maxep.DataContext = myviewdata.myben.minEPtoC;
                    maxha.DataContext = myviewdata.myben.minHAtoC;
                }
                else
                {
                    MessageBox.Show("Wrong grid");
                }
            }
            timelist.ItemsSource = myviewdata.myben.timecomp;
            acclist.ItemsSource = myviewdata.myben.acccomp;

        }


        void butacc(object sender, RoutedEventArgs e)
        {
            if (myviewdata.myVMf == -1)
            {
                MessageBox.Show("Choose function!");
            }
            else
            {
                if ((myviewdata.myVMGrid.vecbeg < myviewdata.myVMGrid.vecend) && (myviewdata.myVMGrid.arglen != 0))
                {
                    myviewdata.mychan = true;
                    vischeck.DataContext = myviewdata.mychan;
                    myviewdata.myVMGrid.VMf = myviewdata.myVMf;
                    myviewdata.AddVMAccuracy(myviewdata.myVMGrid);
                }
                else
                {
                    MessageBox.Show("Wrong grid");
                }
            }
            timelist.ItemsSource = myviewdata.myben.timecomp;
            acclist.ItemsSource = myviewdata.myben.acccomp;

        }


    void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (myviewdata.mychan)
            {
                System.Windows.Forms.DialogResult result;
                result = System.Windows.Forms.MessageBox.Show("Unsaved changes.Save before exiting?", "Unsaved changes", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".txt"; 
                    dlg.Filter = "Text documents (.txt)|*.txt"; 
                    dlg.ShowDialog();
                    myviewdata.Save(dlg.SafeFileName);
                }
            }
        }


    }
    class ViewData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _selectedtime = 0;
        public int selectedtime
        {
            get { return _selectedtime; }
            set
            {
                if ((value < myben.timecomp.Count) && (value >= 0))
                { svmt = myben.timecomp[value]; }
                _selectedtime = value;
            }
        }
        private VMTime _svmt;
        public VMTime svmt
        {
            get { return _svmt; }
            set
            {
                _svmt = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new
                    PropertyChangedEventArgs("svmt"));

            }
        }
        private int _selectedacc = 0;
        public int selectedacc
        {
            get { return _selectedacc; }
            set
            {
                if ((value < myben.acccomp.Count) && (value >= 0))
                { svma = myben.acccomp[value]; }
                _selectedacc = value;
            }
        }
        private VMAccuracy _svma;
        public VMAccuracy svma
        {
            get { return _svma; }
            set
            {
                _svma = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new
                    PropertyChangedEventArgs("svma"));

            }
        }
        private static bool mychan1=false;
        public bool mychan { get { return mychan1; } set{ mychan1 = value; } }
        public string myfilename { get; set; } = "";
        public VMBenchmark myben = new VMBenchmark();
        static private int _myVMf;
        public int myVMf
        {
            get { return _myVMf; }
            set
            {
                    _myVMf = value;
            }
        }

        static private VMGrid _myVMGrid { get; set;}
        public VMGrid myVMGrid
        {
            get { return _myVMGrid; }
            set
            {
                _myVMGrid = value;
            }
        }


        public void AddVMTime(VMGrid mygrid)
        {
            myben.AddVMTime(mygrid);
        }
        public void AddVMAccuracy(VMGrid mygrid)
        {
            myben.AddVMAccuracy(mygrid);
        }
        public bool Save(string filename)
        {
            bool mybl = false;
            try
            {
                StreamWriter writer = new StreamWriter(filename, false);
                writer.WriteLine(myben.ToString());
                writer.Flush();
                mybl = true;

            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to save file.\n" + e.Message);
            }
            return mybl;
        }
        public bool Load(string filename)
        {
            bool mybl = false;
            try
            {
                StreamReader reader = new StreamReader(filename);
                string mystring = "";
                mystring = reader.ReadLine();
                int n = Convert.ToInt32(mystring);
                VMBenchmark myben1 = new VMBenchmark();
                for (int i = 0; i < n; i++)
                {
                    VMGrid mygrid1 = new VMGrid();
                    mystring = reader.ReadLine();
                    mygrid1.VMf = Convert.ToInt32(mystring);
                    mystring = reader.ReadLine();
                    mygrid1.arglen = Convert.ToInt32(mystring);
                    mystring = reader.ReadLine();
                    mygrid1.vecbeg = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    mygrid1.vecend = Convert.ToDouble(mystring);
                    VMTime myVMTime1 = new VMTime();
                    myVMTime1.grid = mygrid1;
                    mystring = reader.ReadLine();
                    myVMTime1.timeEP = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMTime1.timeHa = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMTime1.timeC = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMTime1.timeEPtoC = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMTime1.timeHAtoC = Convert.ToDouble(mystring);
                    myben1.AddVMTime(myVMTime1);
                }
                mystring = reader.ReadLine();
                n = Convert.ToInt32(mystring);
                for (int i = 0; i < n; i++)
                {
                    VMGrid mygrid1 = new VMGrid();
                    mystring = reader.ReadLine();
                    mygrid1.VMf = Convert.ToInt32(mystring);
                    mystring = reader.ReadLine();
                    mygrid1.arglen = Convert.ToInt32(mystring);
                    mystring = reader.ReadLine();
                    mygrid1.vecbeg = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    mygrid1.vecend = Convert.ToDouble(mystring);
                    VMAccuracy myVMAccuracy1 = new VMAccuracy();
                    myVMAccuracy1.grid = mygrid1;
                    mystring = reader.ReadLine();
                    myVMAccuracy1.maxdif = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMAccuracy1.maxdifarg = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMAccuracy1.maxdifHa = Convert.ToDouble(mystring);
                    mystring = reader.ReadLine();
                    myVMAccuracy1.maxdifEP = Convert.ToDouble(mystring);
                    myben1.AddVMAccuracy(myVMAccuracy1);
                }
                mystring = reader.ReadLine();
                myben1.minHAtoC = Convert.ToDouble(mystring);
                mystring = reader.ReadLine();
                myben1.minEPtoC = Convert.ToDouble(mystring);
                myben = myben1;

                mybl = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to load file.\n" + e.Message);
            }
            return mybl;
        }

    }

    public class myconv : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VMGrid b = new VMGrid();
            try
            {
                string a = (string)value;
                if ((a != null) && (a!=""))
                {
                    double[] doubles = Array.ConvertAll(a.Split(' '), Double.Parse);
                    b.arglen = (int)doubles[0];
                    b.vecbeg = doubles[1];
                    b.vecend = doubles[2];
                    return b;
                }
                return b;
            }
            catch (Exception e)
            {
                MessageBox.Show("Wrong format of grid\n" + e.Message);
                return b;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "1 1 1";
        }
    }

    public class myconv1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Controls.TextBlock a = (System.Windows.Controls.TextBlock)value;
            if (a != null)
            {

                if (a.Text == "vmdTan")
                {
                    return 1;
                }
                if (a.Text == "vmdErfInv")
                {
                    return 3;
                }
            }
            return -1;
        }
    }


    public class myconv2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VMGrid a = (VMGrid)value;
            string b = "";
            if (a.VMf == 3)
            {
                b += "vmdErfInv ";
            }
            if (a.VMf == 1)
            {
                b += "vmdTan ";
            }
            b += a.arglen;
            b += " ";
            b += a.vecbeg;
            b += " ";
            b += a.vecend;
            return b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }

    public class myconv3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VMTime a = new VMTime();
            if (value.GetType() == a.GetType())
            {
                
                a = (VMTime)value;
                string b = "";
                b += "Time EP = " + a.timeEP.ToString() + "\n";
                b += "Time HA = " + a.timeHa.ToString() + "\n";
                b += "Time C = " + a.timeC.ToString() + "\n";
                b += "Time EP to C = " + a.timeEPtoC.ToString() + "\n";
                b += "Time HA to C = " + a.timeHAtoC.ToString() + "\n";
                return b;
            }
            return " ";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }

    public class myconv4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VMAccuracy a = new VMAccuracy();
            if (value.GetType() == a.GetType())
            {
                a = (VMAccuracy)value;
                string b = "";
                b += "Max difference = " + a.maxdif.ToString() + "\n";
                b += "Max difference id = " + a.maxdifarg.ToString() + "\n";
                b += "Max difference EP value = " + a.maxdifEP.ToString() + "\n";
                b += "Max difference HA value = " + a.maxdifHa.ToString() + "\n";
                return b;
            }
            return " ";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }


    public class myconv5 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            bool flag = new bool();
            if (value.GetType() == flag.GetType())
            {
                flag = (bool)value;
                return (flag ? Visibility.Visible : Visibility.Hidden);
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }

    public class myconv6 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double flag = new double();
            if (value.GetType() == flag.GetType())
            {
                flag = (double)value;
                string rez = flag.ToString(("0." + new string('#', 10)));
                return rez;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }

}