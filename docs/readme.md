Validation
===

Validation classes
---

- Validation // Abstract
```
{
	Type,
	Config // Nullable
}
```
- SimpleValidation : Validation
```
{
	Type: MinValidation,
	Config: { // Nullable
		MinValue: 1
	}
}
```
- CollectionValidation : Validation // Low prio
```
{
	Type: Each,
	Config: null, // Nullable
	Validations: [ // type: Validation[] - (may be coll of collections or complex objects)
		{
			Type: MinValidation,
			Config: {
				MinValue: 0
			}
		},
		{
			Type: NotNull
		}
	]
}
```
- DictionaryValidation : Validation // Or EnumerableOfKeyValuePairsValidation; Lowest prio
```
{
	Type: Each,
	Config: null, // Nullable
	Validations: [ // type: Validation[] - (may be dict of collections or complex objects)
		{
			Type: MinValidation,
			Config: {
				MinValue: 0
			}
		},
		{
			Type: NotNull
		}
	]
}
```
- ComplexValidation : Validation
```
{
	Type: ItemValidation, // Is needed for ComplexValidation?
	Config: null, // Nullable
	FieldValidations: { // type: string->Validation[] dict
		Number: [ // type: Validation[]
			{
				Type: MinValidation,
				Config: {
					MinValue: 1
				}
			}
		]
	},
	OverallValidations: [ // type: Validation[]
		{
			Type: EmailPlusNameMaxLength,
			Config: {
				MaxLength: 300
			}
		}
	]
}
```

TODO: AggregateValidations

- All, Any, StopOnFirstFailure etc.



Validations
---

SimpleValidations

- NotNull
- Min
- Email
- EmailStartsWithNumber
- EmailPlusNameMaxLength

CollectionValidations // Low prio

- Each
- Any

DictionaryValidations // Lowest prio

- Each
- Any

TODO: AggregateValidations

- All, Any, StopOnFirstFailure etc.



Validation configs
---

```
MinValidationConfig
{
	MinValue: 1
}
```

```
EmailPlusNameMaxLenghtConfig
{
	MaxLength: 300
}
```

- should be POCOs without Action / Func properties, but this may be too limited
  - consider some conversion of Config to "Descriptor" (which could be serialized to json)



Validation error
---

TODO: Is SimpleValidationError needed?

- Maybe ComplexValidationError.FieldErrors can just be string->Validation?
- On the other hand Validators may want to put some custom data in errors

- ValidationError // Abstract
```
{
	Validation
}
```
- SimpleValidationError : ValidationError
```
{
	Validation: {
		Type: MinValidation,
		Config: {
			MinValue: 1
		}
	}
}
```
- CollectionValidationError : ValidationError
```
{
	// TODO
}
```
- DictionaryValidationError : ValidationError
```
{
	// TODO
}
```
- ComplexValidationError : ValidationError
```
{
	FieldErrors: { // type: string->ValidationError[] dict
		Number: [
			{
				Validation: {
					Type: MinValidation,
					Config: {
						MinValue: 1
					}
				}
			}
		]
	},
	OverallErrors: [ // type: ValidationError[]
		{
			Validation: {
				Type: EmailPlusNameMaxLength,
				Config: {
					MaxLength: 300
				}
			}
		}
	]
}
```



TODO
---

- passing custom context to Validators
  - object or strongly typed (TContext)?
- custom data in validation errors?
- validation strategy configuration
  - "and / or" validation groups
  - stop on first error / stop on nth error / gather all errors
  - conditional execution of validation rules
- async validation
- check if it will be convenient to create shared validation rules (e.g. for creation / update of same type - update would have couple additional rules)
  - should be addressed in Validation builders design



Example
---

```
Item
{
	Number: 1,
	Email: "1@a.com",
	Name: "Item1",
	Collection: [ 1, 2, 3 ]
}
```

```
ComplexValidation<Item>
{
	FieldValidations: {
		Number: [
			{
				Type: MinValidation,
				Config: {
					MinValue: 1
				}
			}
		],
		Email: [
			{
				Type: EmailValidation
			},
			{
				Type: EmailStartsWithNumber
			}
		],
		Collection: [
			{
				Type: NotNull
			},
			{
				Type: Each,
				Validations: [
					{
						Type: MinValidation,
						Config: MinValue: 0
					},
					{
						Type: NotNull
					}
				]
			}
		]
	},
	OverallValidations: [
		{
			Type: EmailPlusNameMaxLength,
			Config: {
				MaxLength: 300
			}
		}
	]
}
```
