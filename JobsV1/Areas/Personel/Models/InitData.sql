

Insert into HrPersonelStatus([Desc])
values ('ACT'),('INC'),('BAD');

Insert into HrPositions([Desc],[Remarks]) 
values ('Manager','General Manager'),('Staff','Office Staff'),('Driver','Driver/Tour Driver');


Insert into HrSkills([Desc],[Remarks]) 
values	('Leadership','Management'),
		('Marketing','speaking/social media/phone'),
		('Computer Literacy',''),
		('Driving','with license');

Insert into HrProficiencies([Level])
values ('Beginner'),('Intermediate'),('Advanced');

Insert into HrTrainings([Desc],[Remarks])
values ('Management Training','Beginner Course'),
	   ('Marketing Training','Beginner Course'),
	   ('Computer Training','Beginner Course'),
	   ('Driving Training','Beginner Course'),
	   ('Inter Marketing Training','Intermediate Course'),
	   ('Advance Marketing Training','Intermediate Course'),
	   ('Inter Driving Training','Intermediate Course');
	   

Insert into HrTrainingSkills([HrTrainingId],[HrSkillId],[HrProficiencyId])
values	(1,1,1),(2,2,1),(3,3,1),(4,4,1),	--Beginner Trainings
		(5,2,2),(6,2,3),(7,4,2)				--Intermediate Trainings
;


Insert into HrDtrStatus([Desc],[Factor])
values ('Regular',1.0),('Weekend',1.5),('Sp Holiday',1.5),('Holiday',2.0)
;