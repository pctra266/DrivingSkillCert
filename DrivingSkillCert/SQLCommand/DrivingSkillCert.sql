USE master
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'DrivingSkillCert')
BEGIN
    ALTER DATABASE DrivingSkillCert SET OFFLINE WITH ROLLBACK IMMEDIATE;
    ALTER DATABASE DrivingSkillCert SET ONLINE;
    DROP DATABASE DrivingSkillCert;
END

GO
CREATE DATABASE DrivingSkillCert
GO
USE DrivingSkillCert
GO
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Student', 'Teacher', 'TrafficPolice')) NOT NULL,
    Class NVARCHAR(50) NULL,
    School NVARCHAR(100) NULL,
    Phone NVARCHAR(15) NULL
);

CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL,
    TeacherID INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    FOREIGN KEY (TeacherID) REFERENCES Users(UserID)
);

CREATE TABLE Registrations (
    RegistrationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    CourseID INT NOT NULL,
    Status NVARCHAR(10) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    Comments NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

CREATE TABLE Exams (
    ExamID INT PRIMARY KEY IDENTITY(1,1),
    CourseID INT NOT NULL,
    Date DATE NOT NULL,
    Room NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

CREATE TABLE Results (
    ResultID INT PRIMARY KEY IDENTITY(1,1),
    ExamID INT NOT NULL,
    UserID INT NOT NULL,
    Score DECIMAL(5, 2) NOT NULL,
    PassStatus BIT NOT NULL,
    FOREIGN KEY (ExamID) REFERENCES Exams(ExamID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Certificates (
    CertificateID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    IssuedDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    CertificateCode NVARCHAR(50) UNIQUE NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    SentDate DATETIME DEFAULT GETDATE(),
    IsRead BIT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
