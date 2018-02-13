using ASMGH_MyAttendance.DataAccess;
using ASMGH_MyAttendance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ASMGH_MyAttendance.ViewModel
{
    class main_ViewModel:ViewModelBase
    {
        private IMainAccess ma = new da_main();

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

        public ICollection<attendace> GetAttendance_AM
        {
            get
            {
             
                return ma.GetAttendance(_emp_num,"Morning", attendance_date.Month, attendance_date.Year).ToList();
            }
        }

        public ICollection<attendace> GetAttendance_PM
        {
            get
            {

                return ma.GetAttendance(_emp_num, "Afternoon", attendance_date.Month, attendance_date.Year).ToList();
            }
        }

        public string employee_name
        {
            get
            {
                return GetEmployeeName(_emp_num);
            }
        }

        



        #region COMMAND

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
