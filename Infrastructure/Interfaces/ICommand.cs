using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICommand
    {

    }
    public interface ICommand<T>: ICommand
    {
        T Execute();
    }
    public interface ICommand<T,T1>: ICommand
    {
        T1 Execute(T val);
    }
    public interface ICommand<T,T1,T2> : ICommand
    {
        T2 Execute(T val1,T1 val2);
    }

    public interface ICommand<T, T1, T2,T3> : ICommand
    {
        T3 Execute(T val1, T1 val2,T2 val3);
    }
}
