using ASMGH_MyAttendance.DataAccess;
using ASMGH_MyAttendance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ASMGH_MyAttendance.ViewModel
{
    class main_ViewModel:ViewModelBase
    {
        private IMainAccess ma = new da_main();



        public string SaturdaySchedule
        {
            get
            {
                return _saturdaySchedule;
            }
            set
            {
                SetProperty(ref _saturdaySchedule, value, "SaturdaySchedule");
            }
        }
        
        private string _saturdaySchedule = "AM: 8:00 - 12:00|PM: 1:00 - 5:00";




        public DateTime attendance_date
        {
            get
            {
                return _attendance_date;
            }
            set
            {
                SetProperty(ref _attendance_date, value, "attendance_date");
            }
        }


      

        private DateTime _attendance_date = DateTime.Now;

        public DateTime selected_attendance_date
        {
            get
            {
                _selected_attendance_date = attendance_date;
                return _selected_attendance_date;
            }
            set
            {
                SetProperty(ref _selected_attendance_date, value, "selected_attendance_date");
            }
        }

        private DateTime _selected_attendance_date;

        public int emp_num
        {
            get
            {
                return _emp_num;
            }
            set
            {
                SetProperty(ref _emp_num, value, "emp_num");
            }
        }
        private int _emp_num = 0;      
        public string timeTable
        {
            get
            {
                return _timeTable;
            }
            set
            {
                SetProperty(ref _timeTable, value, "timeTable");
            }
        }
        private string _timeTable;

        public string employee_name
        {
            get
            {
                return GetEmployeeName(_emp_num);
            }
        }

        public ICollection<EmpTimeRecord> GetEmployeeTimeRecord
        {
            get
            {
                _employeeTimeRecord =  ma.GetEmpTimeRecord(_emp_num, attendance_date.Month, attendance_date.Year).ToList();
                return _employeeTimeRecord;
            }
        }

        public static ICollection<EmpTimeRecord> _employeeTimeRecord;

        public int UndertimeHour
        {
            get
            {

                return (int)_employeeTimeRecord.Sum(i => i.UHour) + (CalculateUndertime() / 60);

                //(int)_employeeTimeRecord.Sum(i => i.UHour);
                //CalculateUndertime() / 60;
            }
        } 

        public int UndertimeMin
        {
            get
            {
                return CalculateUndertime() % 60;
            }
        }


        private int CalculateUndertime()
        {
            int totalMin;

            if (_employeeTimeRecord != null)
            {
                totalMin = (int)_employeeTimeRecord.Sum(i => i.UMin);
            }

            else
            {
                totalMin = 0;
            }

            return totalMin;
        }


        #region COMMAND


        public ICommand RestoreDefaultDate_cmd
        {
            get
            {
                return new ActionCommand(p => RestoreDefaultDate());
            }
        }

        public void RestoreDefaultDate()
        {
            SaturdaySchedule = "AM: 8:00 - 12:00|PM: 1:00 - 5:00";
        }

        public ICommand PrintDTR_cmd
        {
            get
            {

         
                return new ActionCommand(p => PrintDTR((FlowDocument)p));
            }
        }


        public void PrintDTR(FlowDocument v)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintDocument(((IDocumentPaginatorSource)v).DocumentPaginator, "DTR-" + employee_name);


        }

        public ICommand cmd_ScrollMonth
        {
            get
            {
                return new ActionCommand(p => ScrollMonth((string)p));
            }
        }
  

        public void ScrollMonth(string _mode)
        {
            if (_mode.Equals("A"))
            {
                attendance_date = attendance_date.AddMonths(1);
                NotifyPropertyChanged("attendance_date");
            }
            else
            {
                attendance_date = attendance_date.AddMonths(-1);
                NotifyPropertyChanged("attendance_date");
            }
        }


        public string GetEmployeeName(int emp_num)
        {
            try
            {
                string _name = ma.GetEmployeeName(emp_num).ToString();
                return _name;
            }
            catch
            {
                return null;
            }
        }


        public ICommand cmd_updateAtt
        {
            get
            {
                return new ActionCommand(p => updateAttendance());
            }
        }

        public void updateAttendance()
        {
            NotifyPropertyChanged("GetAttendance_AM");
            NotifyPropertyChanged("GetAttendance_PM");
            NotifyPropertyChanged("employee_name");

            NotifyPropertyChanged("attendance_date");

            NotifyPropertyChanged("selected_attendance_date");
            NotifyPropertyChanged("GetEmployeeTimeRecord");

            NotifyPropertyChanged("UndertimeHour");
            NotifyPropertyChanged("UndertimeMin");

        }

        public ICommand CloseWindow_cmd
        {
            get
            {
                return new ActionCommand(p => CloseWindow((Window)p, 0));
            }
        }

        private async void CloseWindow(Window win, int mil_delay)
        {
            if(win!=null)
            {
                await Task.Delay(mil_delay);
                win.Close();
            }
        }


     
#endregion

    }
}
