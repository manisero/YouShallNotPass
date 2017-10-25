﻿namespace Manisero.YouShallNotPass
{
    public interface IValidation
    {
        int Type { get; }
    }

    public interface IValidation<TConfig> : IValidation
    {
        TConfig Config { get; }
    }
}
