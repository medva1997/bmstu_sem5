CREATE DATABASE MYWATCH2;
GO
USE MYWATCH2;
GO

CREATE TABLE Visitor
(
  VID INT NOT NULL PRIMARY KEY, -- id посетителя
  LastName    NVARCHAR(60)  NOT NULL, -- Фамилия
  FirstName   NVARCHAR(60)  NOT NULL, -- Имя
  SecondName  NVARCHAR(60)  NOT NULL, -- Отчество
  Email       NVARCHAR(60)  NOT NULL, -- Почта
  Password    NVARCHAR(100) NOT NULL, -- Пароль для входа
  PhoneNumber NVARCHAR(11)  NOT NULL  -- Номер телефона
)

CREATE TABLE  Company(
  CompID INT NOT NULL PRIMARY KEY,    --Номер компании
  CompAdminMID INT NOT NULL,       --Главный менеджер
  CompName NVARCHAR(100) NOT NULL ,   --Название компании
  CompCity NVARCHAR(60) NOT NULL,     --Города

)

CREATE TABLE Manager
(
  MID    INT NOT NULL PRIMARY KEY, --ID менеджера
  CompID INT NOT NULL, --ID компании в которой он работает
  LastName    NVARCHAR(60)  NOT NULL, -- Фамилия
  FirstName   NVARCHAR(60)  NOT NULL, -- Имя
  SecondName  NVARCHAR(60)  NOT NULL, -- Отчество
  Email       NVARCHAR(60)  NOT NULL, -- Почта
  Password    NVARCHAR(100) NOT NULL, -- Пароль для входа
  PhoneNumber NVARCHAR(11)  NOT NULL  -- Номер телефона
  CONSTRAINT FK_CompID FOREIGN KEY (CompID)
  REFERENCES Company(CompID)
)

ALTER TABLE Company ADD
CONSTRAINT FK_CompAdminMID FOREIGN KEY (CompAdminMID)
    REFERENCES Manager(MID)


CREATE TABLE Meeting
(
  Meet_ID         INT      NOT NULL,   -- ИД встречи
  MID            INT      NOT NULL,   -- Номер менеджера
  VID            INT      NOT NULL,   -- Номер посетителя
  TimeStart      DATETIME NOT NULL,   -- Время начала
  TimeEnd        DATETIME NOT NULL,   -- Время конца
  M_Confirmation BIT DEFAULT NULL,    -- Подтверждение от менеджера
  V_confirmation BIT DEFAULT NULL,    -- Подтверждение от пользователя
  Location           NVARCHAR(200),       -- Место встречи
  CONSTRAINT FK_MID FOREIGN KEY (MID)
  REFERENCES Manager (MID),
  CONSTRAINT FK_VID FOREIGN KEY (VID)
  REFERENCES Visitor (VID)

)

CREATE TABLE M_BUSY
(
  MID    INT  NOT NULL, --ИД менеджера
  Time   DATETIME NOT NULL,   --Время начала слота в 15 мнит
  Status BIT DEFAULT 0,   --0 Не занято, 1-занято
  CONSTRAINT FK_MID_Busy FOREIGN KEY (MID)
  REFERENCES Manager (MID)
)

CREATE TABLE V_BUSY
(
  VID      INT  NOT NULL,     --ИД менеджера
  Time     DATETIME NOT NULL, --Время начала слота в 15 мнит
  Status   BIT DEFAULT 0,     --0 Не занято, 1-занято
  CONSTRAINT FK_VID_Busy FOREIGN KEY (VID)
  REFERENCES Visitor (VID)
)
 GO

