using System;
using System.Collections.Generic;
using CommonBase.Exceptions;
using MContracts.ViewModel;
using CommonBase;

namespace MContracts
{
    public class SaveWorkspaceExceptionDescription : IExceptionDescriptionResolver
    {
        public bool GetDescription(Exception exception, ref ExceptionDescription description)
        {
            if (!(exception is SaveWorkspaceException)) return false;
            description.Icon = IconDescription.Warning;
            description.Content = exception.InnerException.Return(x => x.AggregateMessages(), "Нет описания ошибки");
            description.Instruction = exception.Message;
            description.ShowFooter = true;
            return true;
        }
    }
}
