using company.G03.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL
{
    public  interface IUnitOfWork:IDisposable
    {
        IDeptRepository DeptRepository { get; }
        IEmpRepository EmpRepository { get; }
        int Complete();
    }
}
