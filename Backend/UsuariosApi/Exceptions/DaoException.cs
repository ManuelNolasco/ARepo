using System;

public class DaoException : Exception
{
    public DaoException(string message, Exception inner = null)
        : base(message, inner) { }

}
