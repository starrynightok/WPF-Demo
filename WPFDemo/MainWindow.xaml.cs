using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using log4net;
using MessageBox = System.Windows.MessageBox;

namespace WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ILog Logger = LogManager.GetLogger(typeof(MainWindow));
        private BackgroundWorker completeWorker = null;
        private BackgroundWorker incrementWorker = null;
        private ObservableCollection<LogInfo> LogList = new ObservableCollection<LogInfo>();
        private ObservableCollection<UploadFailedInfo> UploadFailedList = new ObservableCollection<UploadFailedInfo>();
        private DateArea dateArea;

        private string incrementUploadNum;
        public string IncrementUploadNum
        {
            get { return incrementUploadNum; }
            set
            {
                incrementUploadNum = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IncrementUploadNum"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public MainWindow()
        {
            InitializeComponent();
            InitUI();
            InitData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
        }

        private void InitUI()
        {
            this.Closing += (sender, args) =>
            {
                if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButton.OKCancel, MessageBoxImage.Question) ==
                    MessageBoxResult.OK)
                {
                    // 关闭所有的线程
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    args.Cancel = true;
                }
            };

            //汉化日期选择器
            var language = System.Windows.Markup.XmlLanguage.GetLanguage("zh-CN");
            StartTimeText.Language = language;
            EndTimeText.Language = language;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dateArea = new DateArea
            {
                StartTime = DateTime.Today,
                EndTime = DateTime.Today
            };

            DateArea.DataContext = dateArea;
            UploadFailedGrid.ItemsSource = UploadFailedList;
            LogGrid.ItemsSource = LogList;
        }

        private void NotifyInfo(object o)
        {
            var res = o as dynamic;
            if (res.Type == 0)
            {
                //日志通知
                var msg = res.Info as string;
                LogList.Add(new LogInfo {Log = msg, Time = DateTime.Now});
            }
            else if (res.Type == 1)
            {
                //上传错误数据
                var failedData = res.Info as UploadFailedInfo;
                if (failedData != null)
                {
                    failedData.No = UploadFailedList.Count + 1;
                    UploadFailedList.Add(failedData);
                }
            }
        }

        private void CompleteUploadWorker_DoWork(object sender,  DoWorkEventArgs e)
        {
            var argument = e.Argument as dynamic;
            var startTime = argument.StartTime;
            var endTime = argument.EndTime;
        }

        private void CompleteUploadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var info = "↑↑↑↑↑↑↑=====结束上传=====↑↑↑↑↑↑↑";
            LogList.Add(new LogInfo { Time = DateTime.Now, Log = info });
        }

        private void CompleteUploadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            NotifyInfo(e.UserState);
        }

        private void IncrementUploadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var argument = e.Argument as dynamic;
            var startTime = argument.StartTime;
            var endTime = argument.EndTime;
        }

        private void IncrementUploadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var info = "↑↑↑↑↑↑↑=====结束上传=====↑↑↑↑↑↑↑";
            LogList.Add(new LogInfo { Time = DateTime.Now, Log = info});
        }


        private void Min_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Max_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NavBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch { }
            }
        }

        private void CompleteUplod_Click(object sender, RoutedEventArgs e)
        {
            var startTime = DateTime.Today;
            var endTime = DateTime.Today;
            try
            {
                startTime = dateArea.StartTime;
                endTime = dateArea.EndTime.AddDays(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("日期格式输入不正确！");
                return;
            }

            if (startTime > endTime)
            {
                MessageBox.Show("开始时间不能晚于结束时间！");
                return;
            }

            var info = "↓↓↓↓↓↓↓=====开始上传=====↓↓↓↓↓↓↓";
            Logger.Info(info);
            LogList.Clear();
            LogList.Add(new LogInfo { Time = DateTime.Now, Log = info });
            UploadFailedList.Clear();

            completeWorker = new BackgroundWorker();
            completeWorker.DoWork += CompleteUploadWorker_DoWork;
            completeWorker.ProgressChanged += CompleteUploadWorker_ProgressChanged;
            completeWorker.RunWorkerCompleted += CompleteUploadWorker_RunWorkerCompleted;
            completeWorker.WorkerReportsProgress = true;
            completeWorker.WorkerSupportsCancellation = true;
            completeWorker.RunWorkerAsync(new {StartTime= startTime, EndTime=endTime});
        }

        private void IncrementUpload_Click(object sender, RoutedEventArgs e)
        {
            var startTime = DateTime.Today;
            var endTime = DateTime.Today;

            try
            {
                startTime = dateArea.StartTime;
                endTime = dateArea.EndTime.AddDays(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("日期格式输入不正确！");
                return;
            }

            if (startTime > endTime)
            {
                MessageBox.Show("开始时间不能晚于结束时间！");
                return;
            }

            var info = "↓↓↓↓↓↓↓=====开始上传=====↓↓↓↓↓↓↓";
            Logger.Info(info);
            LogList.Clear();
            LogList.Add(new LogInfo { Time = DateTime.Now, Log = info });
            UploadFailedList.Clear();

            incrementWorker = new BackgroundWorker();
            incrementWorker.DoWork += IncrementUploadWorker_DoWork;
            incrementWorker.RunWorkerCompleted += IncrementUploadWorker_RunWorkerCompleted;
            incrementWorker.WorkerReportsProgress = true;
            incrementWorker.WorkerSupportsCancellation = true;
            incrementWorker.RunWorkerAsync(new { StartTime = startTime, EndTime = endTime });

        }

        /// <summary>
        /// 停止上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopUpload_Click(object sender, RoutedEventArgs e)
        {
            if (completeWorker != null && completeWorker.IsBusy)
            {
                completeWorker.CancelAsync();
            }
            if (incrementWorker != null && incrementWorker.IsBusy)
            {
                incrementWorker.CancelAsync();
            }
        }

    }
}
