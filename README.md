# Product Name
> Book Project



## Endpoints Examples

POST endpoint to create a new Book:

![](README/CreateBook.png)

GET endpoint to retrieve a Book:

![](README/GetBook.png)

PUT endpoint to amend an already existing Book (single field):

![](README/AmendSingleField.png)

PUT endpoint to amend an already existing Book (multiple fields):

![](README/AmendMultipleFields.png)

GET endpoint to retrieve the latest version of a Book after applying the changes:

![](README/BookAfterAmendments.png)

GET endpoint to retrieve a book's history:

![](README/BookHistory.png)

GET endpoint to retrieve a book's history with ordering:

![](README/BookHistoryDesc.png)

GET endpoint to retrieve a book's history with filtering:

![](README/BookHistoryFiltering.png)

## Endpoints Validation

The Book is not present in the in-memory DB:

![](README/FailedBookRetreival.png)

Trying to amend a Book without adding any new fields in the JSON body:

![](README/BookAmendmentNoFields.png)

Trying to amend a Book's field with a value that is the same as the latest value that the Book has:

![](README/AmendAlreadyExistingField.png)

Trying to amend a Book's field (Authors) with an already present Author in the list (check being done on Author's id)

![](README/BookAmendmentSameField.png)

Trying to amend a Book's field (Authors) with an empty list of Authors (theoretically a Book should have at least one author)

![](README/BookCannotHaveEmptyAuthors.png)









