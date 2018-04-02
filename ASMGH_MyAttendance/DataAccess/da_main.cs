using ASMGH_MyAttendance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMGH_MyAttendance.DataAccess
{

    public interface IMainAccess
    {
        ICollection<EmpTimeRecord> GetEmpTimeRecord(int emp_num, int _month, int _year);

        string GetEmployeeName(int emp_num);
    }


    class da_main: IMainAccess
    {
        private readonly AttendanceEntities context;

        public da_main()
        {
            context = new AttendanceEntities();
        }

        public string GetEmployeeName(int emp_num)
        {
            try
            {
                return context.usp_getName(emp_num).FirstOrDefault().ToString();
            }
            catch
            {
                return null;
            }
        }

  

        public ICollection<EmpTimeRecord> GetEmpTimeRecord(int emp_num, int _month, int _year)
        {
            var result = context.usp_GetTimeRecord(emp_num, _month, _year);
            return result.ToList();
        }
    }
}
