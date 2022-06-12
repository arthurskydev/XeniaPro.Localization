using System;

namespace XeniaPro.Localization.Core.Exceptions;

public class InvalidStringInterpolationException : Exception
{
    public InvalidStringInterpolationException(string key) : base($"String {key} is malformed. Please update your locale file.")
    {
        
    }
}