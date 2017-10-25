﻿using Manisero.YouShallNotPass.Validations;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class SampleComplexType
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class SampleComplexValidation
    {
        public static ComplexValidation Sample = new ComplexValidation
        {
            MemberValidations = new Dictionary<string, IValidation>
            {
                [nameof(SampleComplexType.Id)] = new MinValidation<int>
                {
                    Config = new MinValidationConfig<int>
                    {
                        MinValue = 1
                    }
                },
                [nameof(SampleComplexType.Email)] = new EmailValidation()
            }
        };
    }
}
