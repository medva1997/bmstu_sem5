CREATE DATABASE School;
GO
USE School;
GO

CREATE TABLE Students
(
  id         INT NOT NULL PRIMARY KEY,
  first_name NVARCHAR(50)  NOT NULL,
  last_name  NVARCHAR(50)  NOT NULL,
)

CREATE TABLE Lessons
(
  id           INT NOT NULL PRIMARY KEY,
  teacher_name NVARCHAR(50)  NOT NULL,
  lesson_date  DATETIME,
  lesson_name NVARCHAR(50)  NOT NULL,
  room         INT,
);

 CREATE TABLE Marks (
   mark_id    INT NOT NULL PRIMARY KEY,
   student_id INT NOT NULL,
   lesson_id  INT NOT NULL,
   mark INT,
  CONSTRAINT FK_StudentID FOREIGN KEY (student_id)
  REFERENCES Students(id),
  CONSTRAINT FK_LessonID FOREIGN KEY (lesson_id)
  REFERENCES Lessons(id)
 );
GO

INSERT Students (id, first_name, last_name) VALUES (1,N'Водопьянова',	N'Дарья')
INSERT Students (id, first_name, last_name) VALUES (2,N'Арутюнова',	N'Евгения')
INSERT Students (id, first_name, last_name) VALUES (3,N'Юлдашев',	N'Никанор')
INSERT Students (id, first_name, last_name) VALUES (4,N'Кривоусов',	N'Виталий')
INSERT Students (id, first_name, last_name) VALUES (5,N'Канчеева',	N'Валентина')
INSERT Students (id, first_name, last_name) VALUES (6,N'Степашкина',	N'Алиса')
INSERT Students (id, first_name, last_name) VALUES (7,N'Светланова',	N'Оксана')
INSERT Students (id, first_name, last_name) VALUES (8,N'Варшавщик',	N'Василий')
INSERT Students (id, first_name, last_name) VALUES (9,N'Корнилов',	N'Григорий')
INSERT Students (id, first_name, last_name) VALUES (10,N'Краснорепов',	N'Леонид')
INSERT Students (id, first_name, last_name) VALUES (11,N'Писемский',	N'Константин')
GO

INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (1,N'Мелехин', '2017-11-02 08:30:00.000',101,N'Математика')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (2,N'Жданкина','2017-11-05 10:30:00.000',102,N'Математика')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (3,N'Елецких','2017-11-01 11:30:00.000',103,N'Математика')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (4,N'Воронихин','2017-11-03 08:30:00.000',104,N'Английский')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (5,N'Демидовский','2017-11-01 08:30:00.000',105,N'Английский')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (6,N'Фионова','2017-11-01 12:30:00.000',101,N'Математика')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (7,N'Орлов','2017-11-02 08:30:00.000',102,N'Английский')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (8,N'Павина','2017-11-03 08:30:00.000',103,N'Математика')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (9,N'Коломенская','2017-11-01 08:30:00.000',102,N'Английский')
INSERT Lessons(id,teacher_name,lesson_date,room,lesson_name) VALUES (10,N'Полудомников','2017-11-04 08:30:00.000',102,N'Английский')
GO

INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (1,1,10,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (2,6,9,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (3,3,4,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (4,9,9,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (5,9,10,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (6,2,9,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (7,8,6,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (8,9,2,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (9,7,6,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (10,8,10,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (11,8,5,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (12,3,2,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (13,5,7,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (14,7,1,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (15,1,6,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (16,8,2,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (17,9,3,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (18,6,5,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (19,4,10,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (20,1,10,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (21,1,2,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (22,4,10,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (23,3,7,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (24,4,2,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (25,7,5,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (26,7,1,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (27,1,2,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (28,5,3,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (29,5,7,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (30,6,8,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (31,1,4,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (32,1,4,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (33,9,2,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (34,2,7,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (35,6,5,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (36,1,7,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (37,7,4,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (38,5,8,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (39,4,7,4)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (40,9,7,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (41,5,1,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (42,9,1,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (43,6,8,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (44,1,1,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (45,9,9,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (46,5,2,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (47,4,1,3)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (48,5,9,5)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (49,7,6,2)
INSERT Marks(mark_id, student_id, lesson_id, mark) VALUES (50,8,3,3)
GO




--join который выводит все оценки учеников с их фамилиями сортируя по индексу ученика
SELECT first_name, last_name, mark
FROM Marks
LEFT JOIN Students ON Marks.student_id = Students.id
ORDER BY student_id;


-- Средняя оценка каждого из учеников при помощи group by
SELECT student_id,first_name, avg(mark) as 'Средняя оценка'
FROM Students
RIGHT JOIN Marks ON Marks.student_id = Students.id
GROUP BY student_id,first_name;



--Оценки полученные студентом номером 9  в указаном ниже интервале
SELECT student_id, mark
FROM
  (
    SELECT * FROM Lessons
    RIGHT JOIN Marks on Lessons.id = Marks.lesson_id
    WHERE lesson_date BETWEEN '2017-11-01 8:30:00.000' and '2017-11-01 13:30:00.000'
  ) as T
WHERE student_id=9;

--Соединяем все на свете и выводим кто на каком предмете что получил
SELECT last_name,first_name,lesson_name,mark
FROM
(
    SELECT *
    FROM Marks
    LEFT JOIN Students ON Marks.student_id = Students.id
) as T
  LEFT JOIN Lessons on T.lesson_id=Lessons.id
