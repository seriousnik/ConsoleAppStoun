CREATE TABLE currency (
	Item_ID VARCHAR (50) PRIMARY KEY,
	ISO_Num_Code INT NOT NULL,
    Name VARCHAR (50) NOT NULL,
    EngName VARCHAR (50) NOT NULL,
	ParentCode VARCHAR(50) NOT NULL,
    ISO_Char_Code VARCHAR(50),
	Nominal INT NOT NULL   
);

CREATE TABLE currencyByDate (
	Item_ID VARCHAR (50) FOREIGN KEY REFERENCES currency(Item_ID),
	Value FLOAT NOT NULL,
	Date DATE NOT NULL
);

USE [STOUN]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  ALTER FUNCTION [dbo].[GET_Exchange_Rate_By_Date](
    @currency varchar(50),
    @date date
)
RETURNS FLOAT
AS 
BEGIN
    RETURN (SELECT top 1 a.VALUE/b.Nominal from dbo.currencyByDate a 
			left join dbo.currency b 
			ON a.item_id = b.item_id
			where a.date = @date and a.item_id = @currency);
END;