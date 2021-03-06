*__Caution:__ please note that YouShallNotPass is in alpha stage. The concept proved promising though, and you are encouraged to play around with it. (Please give feedback if you do so! Write an email - you'll find the address [here](https://github.com/manisero) - or create an [issue](https://github.com/manisero/YouShallNotPass/issues).) But please be aware that API, features, and built-in validations are subject to change.*

# YouShallNotPass

_**What is the hardest part of user input validation? It's not checking if the input is valid - it's telling the user what exactly is wrong!**_

YouShallNotPass is a .NET data validation tool designed with this assumption in mind. It's structured. The validation rules are (mostly) POCO objects:

```C#
// Rule checking if value is greater than or equal to 1:
var rule = new MinValidationRule<int> { MinValue: 1 };
```

, the error messages are POCO objects:

```
// Error returned if value violates the rule:
{
  "code": "Min",
  "minValue": 1
}
```

You can compare this to structured (semantic) logging libraries (like [Serilog](https://serilog.net/)) vs. unstructered ones.

So what does structured validation give you?

- Rules (data) are separated from validators (behavior).
  - This enables proper implementation of attributes-based validation (similar to [DataAnnotations](https://msdn.microsoft.com/en-us/library/ee256141(v=vs.98).aspx)) - rules would be attributes, validators would be "normal" classes which could accept dependencies in constructors. (*This is just an idea, attributes-based validation is not a built-in YSNP feature.*)
- Composing validation rules is straightforward (samples below).
- Analyzing validation rules and errors is easy.
- When adding a new validation rule, you don't have to come up with a user-friendly, grammatically-correct error message. You just think of what data may be useful to form such message and put this data in validation error object.
- Validation error payloads are usually smaller than natural-language messages.
- You don't have to implement validation errors localization on the back-end. You pass the error objects to the front-end, and client-specific formatting and localization happens there.
- In a web application, you can serialize your rules and pass them to the client app - this way the server app will become the source of truth about validation rules, avoiding duplicating validation logic on both sides (*this feature is not fully supported yet*).
- You can track validation errors (by their codes) to see which rules fail most frequently. You can then improve user experience of your application (e.g. provide better description to the problematic form field) to help users provide valid values.

## How to use

1. Install [Manisero.YouShallNotPass](https://www.nuget.org/packages/Manisero.YouShallNotPass/) NuGet package.

2. Find data type you want to validate:

```C#
public class CreateUserCommand
{
    public string Name { get; set; }
    public int Age { get; set; } 
}
```

3. Create validation rule (*you'll find ValidationRuleBuilder syntax in docs*):

```C#
var rule = new ValidationRuleBuilder<CreateUserCommand>()
    .All( // Command fulfills all of the following:
        b => b.Member(x => x.Name, b1 => b1.NotNullNorWhiteSpace()), // Name is not null, nor white space
        b => b.Member(x => x.Age, b1 => b1.Min(0))); // Age is greater than or equal to 0
```

4. Create ValidationEngine (*it's here where you register your rules and custom validators - more in docs*):

```C#
var validationEngine = new ValidationEngineBuilder();
    .RegisterValidationRule(typeof(CreateUserCommand), rule);
    .Build();
```

5. Use ValidationEngine to validate the data:

```C#
public void Handle(CreateUserCommand command)
{
    var validationResult = _validationEngine.Validate(command);

    if (validationResult.HasError())
    {
        throw ... // Handle validation error
    }

    ... // Handle valid command
}
```

You'll find more samples in the docs.


## Features

[TODO]

- easy rules composition
  - easy to either create large complex rules or compose rules of other rules
  - does not get unmanagable in case of large families of complex objects
- flexible error formatting mechanism
- extensibility
  - you can create custom rules and validators exactly the same way the built-in ones are implemented
- powerful validators
  - e.g. core validators registry mechanism is simple, handling only validator instances - validator factories are implemented via validator wrappers (link to code?)
- strongly typed
- performance



## More

- Docs: [TODO].
- Samples: as the docs are not yet ready, you can check [samples written in form of unit tests](https://github.com/manisero/YouShallNotPass/tree/master/src/Manisero.YouShallNotPass.Samples) - feel free to execute them!
- Sample web application demonstrating usage of YouShallNotPass: [YouShallNotPass.SampleApp](https://github.com/manisero/YouShallNotPass.SampleApp).
