# YouShallNotPass

_**What is the hardest part of user input validation? It's not checking if the input is valid - it's telling the user what exactly is wrong!**_

YouShallNotPass is a .NET data validation tool designed with this assumption in mind. It's structured. The validation rules are (mostly) POCO objects:

```
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

- Composing validation rules is straightforward (samples below).
- Analyzing validation rules and errors is easy.
- When adding a new validation rule, you don't have to come up with a user-friendly, grammatically-correct error message. You just think of what data may be useful to form such message and put this data in validation error object.
- Validation error payloads are usually smaller than natural-language messages.
- You don't have to implement validation errors localization on the back-end. You pass the error objects to the front-end, and client-specific formatting and localization happens there.
- In a web application, you can serialize your rules and pass them to the client app - this way the server app will become the source of truth about validation rules, avoiding duplicating validation logic on both sides (*this feature is not fully supported yet*).
- You can track validation errors (by their codes) to see which rules fail most frequently. You can then improve user experience of your application (e.g. provide better description to the problematic form field) to help users provide valid values.

## How to use it

[TODO: get from nuget, then samples]

## Features

[TODO: extensibility, performance, powerful validators]

## More

[TODO: Docs etc]
