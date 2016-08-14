CREATE TABLE TestTable  
(  
    [Key] varchar(50) NOT NULL,
    Title varchar(50) NOT NULL,  
    PubDate datetime NULL,  
    Author varchar(50) NULL,
	PRIMARY KEY ([Key])
);
GO

INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('1', 'Title 1', SYSDATETIME(), 'Author 1');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('2', 'Title 2', SYSDATETIME(), 'Author 1');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('3', 'Title 3', SYSDATETIME(), 'Author 2');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('4', 'Title 4', SYSDATETIME(), 'Author 1');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('5', 'Title 5', SYSDATETIME(), 'Author 1');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('6', 'Title 6', SYSDATETIME(), 'Author 2');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('7', 'Title 7', SYSDATETIME(), 'Author 2');
INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES ('8', 'Title 8', SYSDATETIME(), 'Author 2');
GO




CREATE PROCEDURE dbo.CreateProcedure
	@Key nvarchar(50) = '000',   
    @Title nvarchar(50) = ' Default Title',  
    @PubDate datetime = SYSDATETIME,  
    @Author varchar(50) = 'Default Author'
AS
	INSERT INTO TestTable ([Key], Title, PubDate, Author) VALUES(@Key, @Title, @PubDate, @Author)

	SELECT *
	FROM TestTable
	WHERE [Key] = @Key
GO

CREATE PROCEDURE dbo.UpdateProcedure
	@Key nvarchar(50) = '000',   
    @Title nvarchar(50) = ' Default Title',  
    @PubDate datetime = SYSDATETIME,  
    @Author varchar(50) = 'Default Author'
AS
	UPDATE TestTable 
	SET Title = @Title, PubDate = @PubDate, Author = @Author 
	WHERE [Key] = @Key;

	SELECT *
	FROM TestTable
	WHERE [Key] = @Key
GO


CREATE PROCEDURE dbo.GetAllProcedure
AS
	SELECT * FROM TestTable
GO

CREATE PROCEDURE GetProcedure
	@key varchar(50)
AS
	SELECT * FROM TestTable WHERE [Key] = @key
GO

select* from TestTable;

drop table dbo.TestTable;
drop procedure dbo.CreateProcedure;
drop procedure dbo.CreateProcedure2;
drop procedure dbo.UpdateProcedure;
drop procedure dbo.GetAllProcedure;
drop procedure dbo.GetProcedure;
go
