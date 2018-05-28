USE BookCatalog;
GO
CREATE TABLE Books
(
Id INT IDENTITY(1,1), 
Title NVARCHAR(100) NOT NULL, 
PublishingYear INT NOT NULL, 
PagesAmount INT NOT NULL,
Rate INT NOT NULL,
CONSTRAINT Book_Id_Pk PRIMARY KEY (id)
);

CREATE TABLE Authors
(
Id INT IDENTITY(1,1),
FirstName nvarchar(100) NOT NULL,
LastName nvarchar(100) NOT NULL,
CONSTRAINT Author_Id_Pk PRIMARY KEY (id)
);

CREATE TABLE BooksAuthors
(
Id INT IDENTITY(1,1),
AuthorId INT NOT NULL,
BookId INT NOT NULL,
CONSTRAINT Item_Id_pk PRIMARY KEY (Id),
CONSTRAINT Author_Id_Fk FOREIGN KEY (AuthorId)
	REFERENCES Authors (Id) ON UPDATE CASCADE ON DELETE CASCADE,
CONSTRAINT Book_Id_Fk FOREIGN KEY (BookId)
	REFERENCES Books (Id) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

INSERT 
INTO Authors(FirstName, LastName)
VALUES 
('Stephen', 'Cleary'),
('Sayed', 'Y. Hashimi'),
('Sayed', 'Ibrahim Hashimi'),
('Mark', 'Seeman'),
('Alan', 'Mark Berg'),
('Matthew', 'MacDonald')
;

INSERT 
INTO Books(Title, PublishingYear, PagesAmount, Rate)
VALUES 
('Concurrency in C#. Cookbook', 2014, 205, 5),
('Deploying .NET Applications: Learning MSBuild and ClickOnce', 2006, 267, 5),
('Dependency Injection in .Net', 2014, 464, 5),
('Jenkins Continuous Integration Cookbook', 2015, 408, 4),
('Pro WPF 4.5 in C#', 2012, 1095, 4)
;

GO