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
    Role NVARCHAR(20) CHECK (Role IN ('Student', 'Teacher', 'TrafficPolice', 'Admin')) NOT NULL,
    Class NVARCHAR(50) NULL,
    School NVARCHAR(100) NULL,
    Phone NVARCHAR(15) NULL,
	IsDelete bit default 0
);

CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL,
    TeacherID INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    FOREIGN KEY (TeacherID) REFERENCES Users(UserID),
	IsDelete bit default 0

);

CREATE TABLE BankQuestion (
    BankID INT PRIMARY KEY IDENTITY(1,1),
	CourseID INT REFERENCES Courses(CourseID),
	BankName NVARCHAR(100),
);

CREATE TABLE Questions (
    QuestionID INT PRIMARY KEY IDENTITY(1,1),
	BankID INT REFERENCES BankQuestion(BankID),
	Question NVARCHAR(MAX)
)

CREATE TABLE Answers (
    AnswerID INT PRIMARY KEY IDENTITY(1,1),
	QuestionID INT REFERENCES Questions(QuestionID),
	Answer NVARCHAR(MAX),
	IsTrue BIT
)

CREATE TABLE Registrations (
    RegistrationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    CourseID INT NOT NULL,
    Status NVARCHAR(10) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    Comments NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
	IsDelete bit default 0
);

CREATE TABLE Exams (
    ExamID INT PRIMARY KEY IDENTITY(1,1),
    CourseID INT NOT NULL,
    Date DATETIME NOT NULL,
    Room NVARCHAR(50) NOT NULL,
	Type NVARCHAR(20) check (Type in ('Theory','Practice')),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
	IsDelete bit default 0
);

CREATE TABLE Results (
    ResultID INT PRIMARY KEY IDENTITY(1,1),
    ExamID INT NOT NULL,
    UserID INT NOT NULL,
    Score DECIMAL(5, 2) NOT NULL,
    PassStatus BIT NOT NULL,
    FOREIGN KEY (ExamID) REFERENCES Exams(ExamID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
	IsDelete bit default 0
);

CREATE TABLE Certificates (
    CertificateID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    IssuedDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    CertificateCode NVARCHAR(50) UNIQUE NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
	IsDelete bit default 0
);

CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    SentDate DATETIME DEFAULT GETDATE(),
    IsRead BIT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
	IsDelete bit default 0
);

USE DrivingSkillCert
GO

-- Insert Users
INSERT INTO Users (FullName, Email, Password, Role, Class, School, Phone) VALUES
(N'Nguyễn Văn A', 'a@student.com', 'hashedpassword1', N'Student', '10A1', N'Trường THPT A', '0987654321'),
(N'Trần Thị B', 'b@student.com', 'hashedpassword2', N'Student', '10A2', N'Trường THPT A', '0987654322'),
(N'Phạm Văn C', 'c@teacher.com', 'hashedpassword3', N'Teacher', NULL, N'Trường THPT A', '0987654323'),
(N'Lê Thị D', 'd@teacher.com', 'hashedpassword4', N'Teacher', NULL, N'Trường THPT B', '0987654324'),
(N'Admin', 'admin', 'admin', N'Admin', NULL, NULL , NULL),
(N'Hoàng Văn E', 'e@police.com', 'hashedpassword5', N'TrafficPolice', NULL, NULL, '0987654325');

-- Insert Courses
INSERT INTO Courses (CourseName, TeacherID, StartDate, EndDate) VALUES
(N'Lái xe cơ bản', 3, '2025-03-01', '2025-06-01'),
(N'Lái xe nâng cao', 4, '2025-04-01', '2025-07-01');

-- Insert Registrations
INSERT INTO Registrations (UserID, CourseID, Status, Comments) VALUES
(1, 1, N'Approved', N'Học viên đạt yêu cầu'),
(2, 2, N'Pending', NULL);

-- Insert Exams
INSERT INTO Exams (CourseID, Date, Room, Type) VALUES
(1, '2025-06-15', N'Phòng 101','Theory'),
(2, '2025-07-15', N'Phòng 102','Theory');

-- Insert Results
INSERT INTO Results (ExamID, UserID, Score, PassStatus) VALUES
(1, 1, 85.5, 1),
(2, 2, 60.0, 0);

-- Insert Certificates
INSERT INTO Certificates (UserID, IssuedDate, ExpirationDate, CertificateCode) VALUES
(1, '2025-06-20', '2030-06-20', N'CERT123456');

-- Insert Notifications
INSERT INTO Notifications (UserID, Message) VALUES
(1, N'Bạn đã đậu kỳ thi và nhận chứng chỉ.'),
(2, N'Kết quả thi đã có, vui lòng kiểm tra.');

-- Insert more Users (Students, Teachers, TrafficPolice)
INSERT INTO Users (FullName, Email, Password, Role, Class, School, Phone) VALUES
(N'Nguyễn Thị F', 'f@student.com', 'hashedpassword6', 'Student', '11A1', N'Trường THPT C', '0987654326'),
(N'Lê Văn G', 'g@student.com', 'hashedpassword7', 'Student', '12A1', N'Trường THPT D', '0987654327'),
(N'Phan Thị H', 'h@teacher.com', 'hashedpassword8', 'Teacher', NULL, N'Trường THPT C', '0987654328'),
(N'Vũ Văn I', 'i@teacher.com', 'hashedpassword9', 'Teacher', NULL, N'Trường THPT D', '0987654329'),
(N'Trịnh Hoàng J', 'j@police.com', 'hashedpassword10', 'TrafficPolice', NULL, NULL, '0987654330');

-- Insert more Courses
INSERT INTO Courses (CourseName, TeacherID, StartDate, EndDate) VALUES
(N'Lái xe ô tô B2', 3, '2025-05-01', '2025-09-01'),
(N'Lái xe tải hạng C', 4, '2025-06-01', '2025-10-01'),
(N'Lái xe mô tô A1', 7, '2025-07-01', '2025-11-01'),
(N'Lái xe mô tô A2', 8, '2025-08-01', '2025-12-01');

-- Insert more Registrations
INSERT INTO Registrations (UserID, CourseID, Status, Comments) VALUES
(1, 3, 'Approved', N'Đã được xét duyệt.'),
(2, 4, 'Approved', N'Học viên đủ điều kiện.'),
(6, 1, 'Pending', N'Chờ xét duyệt.'),
(7, 2, 'Rejected', N'Không đủ điều kiện đăng ký.'),
(6, 5, 'Approved', N'Học viên đạt yêu cầu.');

-- Insert more Exams
INSERT INTO Exams (CourseID, Date, Room, Type) VALUES
(1, '2025-09-15', N'Phòng 201','Theory'),
(2, '2025-10-20', N'Phòng 202','Theory'),
(3, '2025-11-25', N'Phòng 203','Theory'),
(4, '2025-12-05', N'Phòng 204','Theory'),
(1, '2025-06-20', N'Phòng 101', 'Practice'),
(2, '2025-07-20', N'Phòng 102', 'Practice'),
(3, '2025-09-20', N'Phòng 201', 'Practice'),
(4, '2025-10-25', N'Phòng 202', 'Practice'),
(5, '2025-11-30', N'Phòng 203', 'Practice');

-- Insert more Results
INSERT INTO Results (ExamID, UserID, Score, PassStatus) VALUES
(1, 1, 88.0, 1),
(2, 2, 72.5, 1),
(3, 6, 45.0, 0),
(4, 7, 91.0, 1),
(3, 6, 80.0, 1),
(5, 1, 85.0, 1),
(6, 2, 78.5, 1),
(7, 6, 60.0, 0),
(8, 7, 90.0, 1),
(9, 1, 88.0, 1);
-- Insert more Certificates
INSERT INTO Certificates (UserID, IssuedDate, ExpirationDate, CertificateCode) VALUES
(1, '2025-09-20', '2030-09-20', 'CERT987654'),
(2, '2025-10-25', '2030-10-25', 'CERT654321'),
(7, '2025-11-30', '2030-11-30', 'CERT123987');

-- Insert more Notifications
INSERT INTO Notifications (UserID, Message) VALUES
(1, N'Bạn đã đậu kỳ thi và nhận chứng chỉ. Chúc mừng!'),
(2, N'Bạn đã hoàn thành khóa học, kiểm tra kết quả thi.'),
(6, N'Bạn đã không đạt trong kỳ thi, vui lòng đăng ký thi lại.'),
(7, N'Xin chúc mừng! Bạn đã vượt qua kỳ thi sát hạch.');




-- Thêm ngân hàng câu hỏi cho Course ID 1
INSERT INTO BankQuestion (CourseID, BankName) VALUES (1, N'Ngân hàng câu hỏi khóa học 1');
INSERT INTO BankQuestion (CourseID, BankName) VALUES (2, N'Ngân hàng câu hỏi khóa học 2');
INSERT INTO BankQuestion (CourseID, BankName) VALUES (3, N'Ngân hàng câu hỏi khóa học 3');
INSERT INTO BankQuestion (CourseID, BankName) VALUES (4, N'Ngân hàng câu hỏi khóa học 4');
INSERT INTO BankQuestion (CourseID, BankName) VALUES (5, N'Ngân hàng câu hỏi khóa học 5');
INSERT INTO BankQuestion (CourseID, BankName) VALUES (6, N'Ngân hàng câu hỏi khóa học 6');


-- Thêm 25 câu hỏi vào bảng Questions
DECLARE @BankID INT = 1;
DECLARE @QuestionID INT;
DECLARE @i INT = 1;
WHILE @BankID<=6
BEGIN
WHILE @i <= 25
BEGIN
    INSERT INTO Questions (BankID, Question) 
    VALUES (@BankID, CONCAT(N'Câu hỏi số ', @i, N' cho khóa học ', @BankID,'?'));
    
    SET @QuestionID = SCOPE_IDENTITY();

    -- Thêm 2 đáp án cho mỗi câu hỏi
    INSERT INTO Answers (QuestionID, Answer, IsTrue) 
    VALUES (@QuestionID, CONCAT(N'Đáp án 1 cho câu ', @i), 0);
    
    INSERT INTO Answers (QuestionID, Answer, IsTrue) 
    VALUES (@QuestionID, CONCAT(N'Đáp án 2 cho câu ', @i), 1);

    SET @i = @i + 1;
END
SET @BankID=@BankID+1;
END
