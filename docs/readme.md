YouShallNotPass
===

TODO
---

- write more tests testing engine
- write more tests testing individual rules
- while caching results in classes, care for thread safety
  - and benchmark to check if caches really help
- validation strategy configuration
  - stop on first error / stop on nth error / gather all errors
  - conditional execution of validation rules
- check if it will be convenient to create shared validation rules (e.g. for creation / update of same type - update would have couple additional rules)
  - probably should be addressed in Rule builder's design
  - or can be achieved by AllValidator
- make internal classes internal
- use ETW to report failed validations?
- validation rules (and errors) should be POCOs without Action / Func properties
  - but this may be too limiting
  - consider some conversion of rule (or error) to "Descriptor" (which could be serialized to json)
- registering rules
  - so that while passing value, user does not have to pass rule
- rule builder
  - fleunt api for building rules
- approach to displaying validation errors in real scenario (e.g. web app)
- consider allowing validators to return custom result, e.g. AnyValidator could return violated rules (although at least one rule passed, so there is no error)
- consider possibility to register Func instead of IValidator / IValidationErrorFormatter
  - but keep the interfaces, they are good for generic validators / formatters
- use MethodInfoExtensions in Core.Engine
- use RuleKeyedOperationResolver in Core.Engine
- consider implementing validator / formatter factories as feature outside engine
  - factory would be wrapped by sigle-instance wrapper
    - wrapper would accept factory in constructor and ivoke it in Validate / Format method



Desired API
---

Validators registration:

```
var builder = new ValidationEngineBuilder()
```

- non-generic rule without parameters

```
builder.RegisterValidator<Email>(x => x.IsEmail)
```

- generic rule without parameters

```
builder.RegisterValidator<NotNull<T>>(x => x != null)
```

- non-generic rule with parameters

```
builder.RegisterValidator<MinLength>((x, rule) => x.Length >= rule.MinLength)
```

- generic rule with parameters

```
builder.RegisterValidator<Min<T>>((x, rule) => x >= rule.MinValue)
```

- context-dependent rule

```
builder.RegisterValidator<Complex<T>>((x, rule, context) => {
  ...
})
```

Rules registration:

```
var cucRule = new ComplexRule<CreateUserCommand> = {
  Members: {
    Id: Min(1)
    Email: [ NotNull, Email ],
    Name: [ NotNull, MinLength(1) ]
  }
}

builder.RegisterRule<CreateUserCommand>(cucRule)
```

Validation:

```
var engine = builder.Build()
```

- with registered rule:

```
var cuc = new CreateUserCommand()

var result = engine.Validate(cuc)
```

- with specified rule:

```
var result = engine.Validate("a", new Email())
```

Error formatters registration:

```
var builder = new ValidationErrorFormattingEngineBuilder()
```

- just error; non-generic

```
builder.Register<Email>(e => "Value should be email.")
```

- just error; non-generic; result of generic validation

```
builder.Register<NotNull>(e => "Value is required.")
```

- just error; generic

```
TODO
```

- error and rule; non-generic

```
builder.Register<MinLength>((e, rule) => $"Value should have at least {rule.MinLength} characters.")
```

- error and rule; generic

```
builder.Register<Min<T>>((e, rule) => $"Value should be at least {rule.Min}.")
```

- error, rule and value; non-generic

```
builder.Register<MinLength>((e, rule, value) => $"Value has {value.Length} characters, while should have at least {rule.MinLength}.")
```

- error, rule and value; generic

```
builder.Register<Min<T>>((e, rule, value) => $"Value was {value}, while should be at least {rule.Min}.")
```

- context-dependent

```
builder.Register<Complex<T>>((e, rule, value, context) => {
  ...
})
```

Error formatting:

```
var engine = builder.Build()

var formatted = engine.Format(validationResult)
```
