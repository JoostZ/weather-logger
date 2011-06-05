using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

using WeatherLoggerLib;
using UsbLibrary;

namespace WeatherLogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WS4000HidPort port;
        DbAccess _dbAccess;
        List<WeatherSnapshot> _weatherSnapshots;

        private TimeOffsets _timeOffsets;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // do this to get the HWND and set up the window hook  
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource.FromHwnd(helper.Handle).AddHook(HwndSourceHookHandler);
            port = new WS4000HidPort();
            port.RegisterHandle(helper.Handle);

            _dbAccess = new DbAccess();
            _timeOffsets = new TimeOffsets();
            _weatherSnapshots = _dbAccess.Load(_dbAccess.FirstDate, _dbAccess.LastDate);


            stackPanel1.DataContext = _dbAccess;
            //cmbToInterval.DataContext = _timeOffsets;

            cmbToInterval.Items.Clear();
            foreach (TimeOffset offset in _timeOffsets.list)
            {
                cmbToInterval.Items.Add(offset);
                cmbFromInterval.Items.Add(offset);
            }
            cmbToInterval.SelectedIndex = 0;
            cmbFromInterval.SelectedIndex = 0;

            // Pre-load the two DatePickers
            dateFrom.SelectedDate = _dbAccess.FirstDate;
            dateTo.SelectedDate = _dbAccess.LastDate;


            System.Windows.Data.CollectionViewSource dbAccessViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("dbAccessViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // dbAccessViewSource.Source = [generic data source]

            System.Windows.Data.CollectionViewSource weatherSnapshotViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("weatherSnapshotViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            weatherSnapshotViewSource.Source = _weatherSnapshots;
        }

        private IntPtr HwndSourceHookHandler(IntPtr hwnd, int msg, IntPtr wParam,
            IntPtr lParam, ref bool handled)
        {
            Message m = new Message();
            m.HWnd = hwnd;
            m.Msg = msg;
            m.WParam = wParam;
            m.LParam = lParam;

            port.ParseMessages(ref m);

            return IntPtr.Zero;  
        }

        private void btnToMax_Click(object sender, RoutedEventArgs e)
        {
            dateTo.SelectedDate = _dbAccess.LastDate;
        }

        private void btnChangeTo_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan change = ((TimeOffset)(cmbToInterval.SelectedItem)).Interval;
            dateTo.SelectedDate = dateFrom.SelectedDate + change;
            if (dateTo.SelectedDate > _dbAccess.LastDate)
            {
                dateTo.SelectedDate = _dbAccess.LastDate;
            }

        }

        private void btnChangeFrom_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan change = ((TimeOffset)(cmbFromInterval.SelectedItem)).Interval;
            dateFrom.SelectedDate = dateTo.SelectedDate - change;
            if (dateFrom.SelectedDate < _dbAccess.FirstDate)
            {
                dateFrom.SelectedDate = _dbAccess.FirstDate;
            }
        }
    }
}
