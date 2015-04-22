CREATE TABLE Person
(
	Id		INT IDENTITY(1,1),
	LastName	VARCHAR(100),
	FirstName	VARCHAR(100),
	Born		Date,
	Died		Date,
	CONSTRAINT pk_Person_Id PRIMARY KEY (Id)
);
Go

CREATE TABLE Quote
(
	Id			INT IDENTITY(1,1),
	Text		VARCHAR(6000),
	Year		VARCHAR(4),
	Context		VARCHAR(6000),
	PersonId	Int,
	CONSTRAINT pk_Quote_Id PRIMARY KEY (Id),
	CONSTRAINT fk_Quote_Person FOREIGN KEY (PersonId) REFERENCES Person
);

GO
