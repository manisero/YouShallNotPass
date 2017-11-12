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
