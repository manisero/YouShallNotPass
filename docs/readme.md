YouShallNotPass
===

TODO
---

- write more tests testing engine
- write more tests testing individual rules
- while caching results in classes, care for thread safety
  - and benchmark to check if caches really help
- passing custom context to Validators
  - object or strongly typed (TContext)?
  - rather Context type with dynamic or Dictionary<string, object> field inside
- validation strategy configuration
  - "and / or" validation groups
  - stop on first error / stop on nth error / gather all errors
  - conditional execution of validation rules
  - at least N of rules pass
- check if it will be convenient to create shared validation rules (e.g. for creation / update of same type - update would have couple additional rules)
  - should be addressed in Rule builder's design
- make internal classes internal
- validation rules (and errors) should be POCOs without Action / Func properties
  - but this may be too limiting
  - consider some conversion of rule (or error) to "Descriptor" (which could be serialized to json)
- make ValidationResult generic (<TRule, TError>) and Engine.Validate method generic
- move ValidationResult to root folder
