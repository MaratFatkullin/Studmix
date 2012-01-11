using System;

namespace AI_.Studmix.ApplicationServices
{
    public interface ILogger
    {
        void Write(string message, string category, int priority);
        void Error(Exception exception);
    }
}