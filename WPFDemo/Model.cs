using System;
using System.ComponentModel;

namespace WPFDemo
{
    /// <summary>
    /// 上传日志记录
    /// </summary>
    public class LogInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(string x)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(x));
            }
        }

        private string _log;

        public string Log
        {
            get { return _log; }
            set { _log = value; Notify("Log"); }
        }

        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; Notify("Time");}
        }
    }

    public class UploadFailedInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(string x)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(x));
            }
        }

        private int _no;

        public int No
        {
            get { return _no; }
            set { _no = value; Notify("No");}
        }

        private string _hisApplyNo;

        public string HisApplyNo
        {
            get { return _hisApplyNo; }
            set { _hisApplyNo = value; Notify("HisApplyNo");}
        }

        private string _studyInstanceUID;
        private string _patientName;
        private string _sex;
        private string _age;
        //private string _ageUnit;
        private string _failedUploadRemark;

        public string StudyInstanceUid
        {
            get { return _studyInstanceUID; }
            set { _studyInstanceUID = value; Notify("StudyInstanceUid");}
        }

        public string PatientName
        {
            get { return _patientName; }
            set { _patientName = value; Notify("PatientName");}
        }

        public string Sex
        {
            get
            {
                var sexCN = _sex;
                switch (_sex)
                {
                    case "F":
                        sexCN = "男";
                        break;
                    case "M":
                        sexCN = "女";
                        break;
                    case "U":
                        sexCN = "未知";
                        break;
                }
                return sexCN;
            }
            set
            {
                switch (value)
                {
                    case "男":
                        _sex = "F";
                        break;
                    case "女":
                        _sex = "M";
                        break;
                    case "未知":
                        _sex = "U";
                        break;
                    default:
                        _sex = value;
                        break;
                }
                Notify("Sex");
            }
        }

        public string Age
        {
            get
            {
                return _age;
            }
            set { _age = value; Notify("Age");}
        }

        public string FailedUploadRemark
        {
            get { return _failedUploadRemark; }
            set { _failedUploadRemark = value; Notify("FailedUploadRemark");}
        }
    }

    public class DateArea
    {
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnNotify(nameof(StartTime));
            }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnNotify(nameof(EndTime));
            }
        }

        private void OnNotify(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
