using System;
using System.Collections.Generic;
using System.Text;

namespace TestTaskManager.Exceptions
{
    public class NoFileException: Exception
    {
        public string Message { get; }

        public NoFileException(string message)
        {
            Message = message;
        }

    }
}
