use test2_db;
/* Получить имена проектов, обеспечиваемых поставщиком под номером 1.*/
select Jname
  from
(
select Jno
from SPJ
where Sno = 1) AS M
inner join J
    on M.Jno = J.Jno;

/* Получить цвета деталей, поставляемых поставщиком под номером 1*/
select DISTINCT Color
  from
(
select Pno
from SPJ
where Sno = 1) AS M
INNER JOIN  P
    on P.Pno = M.Pno;

/*Получить номера деталей, поставляемых для какого-либо проекта в Смоленске */
select DISTINCT Pno, SPJ.Jno
from SPJ
inner join J
  on Spj.Jno = j.Jno
where City = 'Smolensk';

/*Получить номера поставщиков со статусом, меньшим, чем у поставщика под номером 1.*/
select Sno
from S
where Status = (
select Status
from S
where S.Sno = 1);

/*Получить номера проектов, для которых не поставляются красные детали поставщиками из
Смоленска. */
select *
from SPJ
INNER JOIN S
  on S.Sno = SPJ.Sno
INNER JOIN P
  on P.Pno = SPJ.Pno
where S.City <> 'Smolensk' AND P.Color <> 'Red';

/*Получить номера проектов, полностью обеспечиваемых поставщиком под номером 1.*/

select *
  from
(
select Jno
from SPJ
where Sno = 1) AS A
inner join SPJ
    on SPJ.Jno = A.Jno;


