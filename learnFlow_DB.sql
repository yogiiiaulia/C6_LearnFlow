-- ============================================================
--   learnFlow Desktop System - SQL Server (LocalDB) Script
--   Mata Kuliah: Pengembangan Aplikasi Basis Data
--   [VERSI FINAL - Sesuai Class Diagram & Koreksi]
-- ============================================================

USE master;
GO

-- Buat database baru
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'DBlearnFlow')
    DROP DATABASE DBlearnFlow;
GO

CREATE DATABASE DBlearnFlow;
GO

USE DBlearnFlow;
GO

-- ============================================================
-- TABEL: Users (Base entity)
-- ============================================================
CREATE TABLE Users (
    idUser      INT IDENTITY(1,1) PRIMARY KEY,
    username    VARCHAR(50)  NOT NULL UNIQUE,
    password    VARCHAR(255) NOT NULL,
    fullName    VARCHAR(100) NOT NULL,
    role        VARCHAR(20)  NOT NULL
        CHECK (role IN ('Instructor', 'Student', 'Assistant'))
);
GO

-- ============================================================
-- TABEL: Instructors
-- ============================================================
CREATE TABLE Instructors (
    idUser      INT PRIMARY KEY,
    NIP         VARCHAR(30)  NOT NULL UNIQUE,
    expertise   VARCHAR(100),
    CONSTRAINT FK_Instructor_User 
        FOREIGN KEY (idUser) REFERENCES Users(idUser) ON DELETE CASCADE
);
GO

-- ============================================================
-- TABEL: Students
-- ============================================================
CREATE TABLE Students (
    idUser      INT PRIMARY KEY,
    nim         VARCHAR(20)  NOT NULL UNIQUE,
    prodi       VARCHAR(100),
    angkatan    INT,
    CONSTRAINT FK_Student_User 
        FOREIGN KEY (idUser) REFERENCES Users(idUser) ON DELETE CASCADE
);
GO

-- ============================================================
-- TABEL: Assistants
-- ============================================================
CREATE TABLE Assistants (
    idUser          INT PRIMARY KEY,
    assistantCode   VARCHAR(20) NOT NULL UNIQUE,
    expertise       VARCHAR(100),
    CONSTRAINT FK_Assistant_User 
        FOREIGN KEY (idUser) REFERENCES Users(idUser) ON DELETE CASCADE
);
GO

-- ============================================================
-- TABEL: Courses
-- [KOREKSI] FK instructor_id → Instructors(idUser)
--           FK assistant_id  → Assistants(idUser)
-- ============================================================
CREATE TABLE Courses (
    idCourse        INT IDENTITY(1,1) PRIMARY KEY,
    title           VARCHAR(150) NOT NULL,
    description     VARCHAR(MAX),
    quota           INT          NOT NULL DEFAULT 30,
    instructor_id   INT          NOT NULL,
    assistant_id    INT          NULL,
    isActive        BIT          NOT NULL DEFAULT 1,
    createdAt       DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Course_Instructor 
        FOREIGN KEY (instructor_id) REFERENCES Instructors(idUser),
    CONSTRAINT FK_Course_Assistant  
        FOREIGN KEY (assistant_id)  REFERENCES Assistants(idUser)
);
GO

-- ============================================================
-- TABEL: Materials
-- ============================================================
CREATE TABLE Materials (
    idMaterial      INT IDENTITY(1,1) PRIMARY KEY,
    idCourse        INT          NOT NULL,
    title           VARCHAR(150) NOT NULL,
    filePath        VARCHAR(500) NOT NULL,
    uploadedBy      INT          NOT NULL,
    uploadedAt      DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Material_Course 
        FOREIGN KEY (idCourse)    REFERENCES Courses(idCourse) ON DELETE CASCADE,
    CONSTRAINT FK_Material_User   
        FOREIGN KEY (uploadedBy)  REFERENCES Users(idUser)
);
GO

-- ============================================================
-- TABEL: Enrollments
-- [KOREKSI] FK idUser → Students(idUser), bukan ke Users langsung
-- ============================================================
CREATE TABLE Enrollments (
    idEnrollment    INT IDENTITY(1,1) PRIMARY KEY,
    idUser          INT          NOT NULL,
    idCourse        INT          NOT NULL,
    enrollDate      DATETIME     NOT NULL DEFAULT GETDATE(),
    grade           FLOAT        NULL,
    CONSTRAINT FK_Enrollment_User   
        FOREIGN KEY (idUser)    REFERENCES Students(idUser),
    CONSTRAINT FK_Enrollment_Course 
        FOREIGN KEY (idCourse)  REFERENCES Courses(idCourse),
    CONSTRAINT UQ_Enrollment 
        UNIQUE (idUser, idCourse)
);
GO

-- ============================================================
-- SEED DATA
-- Urutan insert harus diperhatikan karena ada FK dependency:
-- Users → Instructors/Students/Assistants → Courses → Enrollments
-- ============================================================

-- Step 1: Insert Users
INSERT INTO Users (username, password, fullName, role) VALUES
('instructor1', 'password123', 'Dr. Budi Santoso',  'Instructor'),
('instructor2', 'password123', 'Prof. Sari Dewi',   'Instructor'),
('student1',    'password123', 'Andi Pratama',      'Student'),
('student2',    'password123', 'Bela Safitri',      'Student'),
('student3',    'password123', 'Candra Wijaya',     'Student'),
('assistant1',  'password123', 'Dina Rahayu',       'Assistant'),
('assistant2',  'password123', 'Eko Prasetyo',      'Assistant');
GO

-- Step 2: Pastikan constraint CHECK tidak konflik
ALTER TABLE Users DROP CONSTRAINT IF EXISTS CK_Users_Role;
GO
ALTER TABLE Users
ADD CONSTRAINT CK_Users_Role
CHECK (role IN ('Instructor', 'Student', 'Assistant'));
GO

-- Step 3: Insert Instructors (idUser 1 & 2)
INSERT INTO Instructors (idUser, NIP, expertise) VALUES
(1, 'NIP-001-2024', 'Pemrograman Web & Mobile'),
(2, 'NIP-002-2024', 'Basis Data & Data Science');
GO

-- Step 4: Insert Students (idUser 3, 4, 5)
INSERT INTO Students (idUser, nim, prodi, angkatan) VALUES
(3, '22TI001', 'Teknik Informatika', 2022),
(4, '22TI002', 'Teknik Informatika', 2022),
(5, '23TI001', 'Teknik Informatika', 2023);
GO

-- Step 5: Insert Assistants (idUser 6 & 7)
INSERT INTO Assistants (idUser, assistantCode, expertise) VALUES
(6, 'ASST-001', 'Pemrograman Web'),
(7, 'ASST-002', 'Basis Data');
GO

-- Step 6: Insert Courses (bergantung Instructors & Assistants)
INSERT INTO Courses (title, description, quota, instructor_id, assistant_id) VALUES
('Pemrograman Web Lanjut', 'Kursus membahas React, Node.js, dan REST API',          30, 1, 6),
('Basis Data Terapan',     'Kursus SQL Server, normalisasi, dan stored procedure',   25, 2, 7),
('Mobile Programming',     'Kursus pengembangan aplikasi Android dengan Kotlin',     20, 1, NULL);
GO

-- Step 7: Insert Enrollments (bergantung Students & Courses)
INSERT INTO Enrollments (idUser, idCourse) VALUES
(3, 1),
(3, 2),
(4, 1),
(5, 2);
GO

-- ============================================================
-- STORED PROCEDURES
-- ============================================================

-- SP: Login
CREATE OR ALTER PROCEDURE sp_Login
    @username VARCHAR(50)
AS
BEGIN
    SELECT idUser, username, password, fullName, role
    FROM Users
    WHERE username = @username;
END;
GO

-- SP: Get semua kursus aktif
CREATE OR ALTER PROCEDURE sp_GetActiveCourses
AS
BEGIN
    SELECT c.idCourse, c.title, c.description, c.quota,
           u.fullName AS instructorName,
           (SELECT COUNT(*) FROM Enrollments e WHERE e.idCourse = c.idCourse) AS enrolled
    FROM Courses c
    INNER JOIN Users u ON c.instructor_id = u.idUser
    WHERE c.isActive = 1;
END;
GO

-- SP: Enroll student ke kursus
CREATE OR ALTER PROCEDURE sp_EnrollStudent
    @idUser   INT,
    @idCourse INT
AS
BEGIN
    -- Cek duplikasi enrollment
    IF EXISTS (SELECT 1 FROM Enrollments WHERE idUser = @idUser AND idCourse = @idCourse)
    BEGIN
        SELECT -1 AS result, 'Anda sudah terdaftar di kursus ini.' AS message;
        RETURN;
    END

    -- Cek ketersediaan kuota
    DECLARE @quota INT, @enrolled INT;
    SELECT @quota    = quota FROM Courses WHERE idCourse = @idCourse;
    SELECT @enrolled = COUNT(*) FROM Enrollments WHERE idCourse = @idCourse;

    IF @enrolled >= @quota
    BEGIN
        SELECT -2 AS result, 'Kuota kursus sudah penuh.' AS message;
        RETURN;
    END

    -- Proses enroll
    INSERT INTO Enrollments (idUser, idCourse) VALUES (@idUser, @idCourse);
    SELECT 1 AS result, 'Pendaftaran berhasil!' AS message;
END;
GO

-- SP: Input nilai
CREATE OR ALTER PROCEDURE sp_InputGrade
    @idEnrollment INT,
    @grade        FLOAT
AS
BEGIN
    UPDATE Enrollments SET grade = @grade WHERE idEnrollment = @idEnrollment;
    SELECT @@ROWCOUNT AS affected;
END;
GO

-- SP: Get peserta kursus
CREATE OR ALTER PROCEDURE sp_GetCourseParticipants
    @idCourse INT
AS
BEGIN
    SELECT e.idEnrollment, u.fullName, s.nim, e.enrollDate, e.grade
    FROM Enrollments e
    INNER JOIN Users    u ON e.idUser = u.idUser
    INNER JOIN Students s ON s.idUser = u.idUser
    WHERE e.idCourse = @idCourse
    ORDER BY u.fullName;
END;
GO

-- SP: Upload / Update material
CREATE OR ALTER PROCEDURE sp_UpsertMaterial
    @idMaterial INT = NULL,
    @idCourse   INT,
    @title      VARCHAR(150),
    @filePath   VARCHAR(500),
    @uploadedBy INT
AS
BEGIN
    IF @idMaterial IS NULL
        INSERT INTO Materials (idCourse, title, filePath, uploadedBy)
        VALUES (@idCourse, @title, @filePath, @uploadedBy);
    ELSE
        UPDATE Materials
        SET title    = @title,
            filePath = @filePath
        WHERE idMaterial = @idMaterial;
END;
GO

-- SP: Lihat Nilai (Student)
-- Sesuai Use Case "Lihat Nilai" & method viewGrades() di Class Diagram
CREATE OR ALTER PROCEDURE sp_ViewGrades
    @idUser INT
AS
BEGIN
    SELECT c.title AS CourseName, e.enrollDate, e.grade
    FROM Enrollments e
    INNER JOIN Courses c ON e.idCourse = c.idCourse
    WHERE e.idUser = @idUser;
END;
GO

-- SP: Edit Profil
-- Sesuai Use Case "Edit Profil" & method updateProfile() di Class Diagram
CREATE OR ALTER PROCEDURE sp_UpdateProfile
    @idUser   INT,
    @fullName VARCHAR(100),
    @password VARCHAR(255)
AS
BEGIN
    UPDATE Users
    SET fullName = @fullName,
        password = @password
    WHERE idUser = @idUser;

    SELECT 1 AS result, 'Profil berhasil diupdate!' AS message;
END;
GO

-- ============================================================
-- VERIFIKASI AKHIR
-- ============================================================
PRINT 'Database DBlearnFlow berhasil dibuat dan disesuaikan!';
GO

-- Cek hasil
SELECT * FROM Users;
SELECT * FROM Instructors;
SELECT * FROM Students;
SELECT * FROM Assistants;
SELECT * FROM Courses;
SELECT * FROM Enrollments;
GO

SELECT *
INTO Courses_Backup
FROM Courses

UPDATE Courses
SET title = 'HACKED'

UPDATE Courses
SET title = 'Pemrograman Web Lanjut'
WHERE idCourse = 1;

UPDATE Courses
SET title = 'Mobile Programming'
WHERE idCourse = 3;

UPDATE Courses
SET title = 'Web Developer'
WHERE idCourse = 4;

UPDATE Courses
SET title = 'wooooo'
WHERE idCourse = 6;

UPDATE Courses
SET title = 'PABD'
WHERE idCourse = 7;