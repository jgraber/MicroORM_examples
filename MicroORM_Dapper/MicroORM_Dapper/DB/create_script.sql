CREATE TABLE Book
(
	Id		INT IDENTITY(1,1),
	Title	VARCHAR(255),
	ISBN	VARCHAR(13),
	Pages	SMALLINT,
	Summary VARCHAR(MAX),
	Rating	NUMERIC(2,1),
	CONSTRAINT pk_Book_Id PRIMARY KEY (Id),
);

INSERT INTO Book (Title, ISBN, Pages, Summary, Rating) 
VALUES ('Rails 4 Test Prescriptions: Build a Healthy Codebase',
'9781941222195', 350, 
'Your Ruby on Rails application is sick. Deadlines are looming, but every 
time you make the slightest change to the code, something else breaks. Nobody 
remembers what that tricky piece of code was supposed to do, and nobody can 
tell what it actually does. Plus, it has bugs. You need test-driven development, 
a process for improving the design, maintainability, and long-term viability of 
software.', 4);

INSERT INTO Book (Title, ISBN, Pages, Summary, Rating) 
VALUES ('The Nature of Software Development: Keep It Simple, Make It Valuable, Build It Piece by Piece',
'9781941222379', 178, 
'The book describes software development, starting from our natural desire to 
get something of value. Each topic is described with a picture and a few paragraphs. 
You’re invited to think about each topic; to take it in. You’ll think about how 
each step into the process leads to the next. You’ll begin to see why Agile methods 
ask for what they do, and you’ll learn why a shallow implementation of Agile can 
lead to only limited improvement.', 5);

INSERT INTO Book (Title, ISBN, Pages, Summary, Rating) 
VALUES ('Crafting Rails 4 Applications: Expert Practices for Everyday Rails Development', '9781937785550', 208,
'Rails is one of the most extensible frameworks out there. This pioneering book 
deep-dives into the Rails plugin APIs and shows you, the intermediate Rails 
developer, how to use them to write better web applications and make your day-to-day 
work with Rails more productive.', 4.5);

INSERT INTO Book (Title, ISBN, Pages, Summary, Rating) 
VALUES ('Good Math: A Geek''s Guide to the Beauty of Numbers, Logic, and Computation', '9781937785338', 282,
'Why do Roman numerals persist? How do we know that some infinities are larger 
than others? And how can we know for certain a program will ever finish? In this 
fast-paced tour of modern and not-so-modern math, computer scientist Mark 
Chu-Carroll explores some of the greatest breakthroughs and disappointments of 
more than two thousand years of mathematical thought. There is joy and beauty in 
mathematics, and in more than two dozen essays drawn from his popular “Good Math” 
blog, you’ll find concepts, proofs, and examples that are often surprising, 
counterintuitive, or just plain weird.', 3);

Go

CREATE VIEW BookStats AS
SELECT	count(*) as 'BookCount', 
		sum(Pages) as 'TotalPages', 
		avg(Rating) as 'AverageRating' 
FROM Book;

Go

CREATE TABLE Publisher
(
	Id		INT IDENTITY(1,1),
	Name	VARCHAR(255),
	Url		VARCHAR(255),
	EMail	VARCHAR(255),
	CONSTRAINT pk_Publisher_Id PRIMARY KEY (Id),
);

Go

ALTER TABLE Book
ADD PublisherId INT NULL,
CONSTRAINT fk_Book_Publisher FOREIGN KEY (PublisherId) REFERENCES Publisher;

Go

CREATE TABLE Cover
(
	BookId	INT,
	Cover	VARBINARY(MAX),
	CONSTRAINT pk_Cover_BookId PRIMARY KEY (BookId),
	CONSTRAINT fk_Cover_Book FOREIGN KEY (BookId) REFERENCES Book
);

Go

CREATE TABLE Author
(
	Id			INT IDENTITY(1,1),
	FirstName	VARCHAR(255),
	LastName	VARCHAR(255),
	EMail		VARCHAR(255),
	Web 		VARCHAR(255),
	Twitter		VARCHAR(50),
	CONSTRAINT pk_Author_Id PRIMARY KEY (Id),
);

Go

CREATE TABLE BookAuthor
(
	BookId 		INT,
	AuthorId 	INT,
	CONSTRAINT pk_BookAuthor PRIMARY KEY (BookId, AuthorId),
	CONSTRAINT fk_BookAuthor_Book FOREIGN KEY (BookId) REFERENCES Book,
	CONSTRAINT fk_BookAuthor_Author FOREIGN KEY (AuthorId) REFERENCES Author
);

Go


// Full Text
CREATE FULLTEXT CATALOG [Fulltext_Book]WITH ACCENT_SENSITIVITY = OFF

GO
