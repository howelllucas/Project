using System;

public class BussinessException : ApplicationException
{
    public int m_ErrorCode;

    public BussinessException(int errorCode)
    {
        m_ErrorCode = errorCode;
    }
}
