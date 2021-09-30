﻿using Isu.Entities;
using IsuExtra.Entities;
using IsuExtra.Services;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class IsuExtraTests
    {
        private UniversityManager _universityManager;
        
        [SetUp]
        public void Setup()
        {
            _universityManager = new UniversityManager();
        }

        [Test]
        public void GiveNullParameters_ThrowsException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                var faculty = new Faculty(null);
            });
            
            Assert.Catch<IsuExtraException>(() =>
            {
                _universityManager.RegisterFaculty(null);
            });
            
            Assert.Catch<IsuExtraException>(() =>
            {
                var faculty = new Faculty("123");
                var GSA = new GsaCourse(faculty, null);
            });
            
            Assert.Catch<IsuExtraException>(() =>
            {
                var GSA = new GsaCourse(null, "123");
            });
            
            Assert.Catch<IsuExtraException>(() =>
            {
                var lesson = new Lesson(8, 10, 30);
            });
            
            Assert.Catch<IsuExtraException>(() =>
            {
                var lesson = new Lesson(5, 60, 30);
            });
            
            Assert.Catch<IsuExtraException>(() =>
            {
                var lesson = new Lesson(5, 8, 90);
            });
        }

        [Test]
        public void TryToAddStudentSecondTime_ThrowsException()
        {
            var faculty = new Faculty("testName");
            _universityManager.RegisterFaculty(faculty);
            Assert.Catch<IsuExtraException>(() =>
            {
                _universityManager.RegisterFaculty(faculty);
            });
        }

        [Test]
        public void AddSeveralLessonsAtTheSameTime_ThrowsException()
        {
            var lesson = new Lesson(1, 8, 20);
            var schedule = new Schedule();
            schedule.AddLesson(lesson);
            Assert.Catch<IsuExtraException>(() =>
            {
                schedule.AddLesson(lesson);
            });
        }

        [Test]
        public void CreatingSchedule_FirstSecondStackSecondThirdNot()
        {
            var lesson1 = new Lesson(1, 8, 20);
            var lesson2 = new Lesson(1, 10, 20);
            var lesson3 = new Lesson(1, 9, 0);
            var lesson4 = new Lesson(2, 8, 20);
            
            var schedule1 = new Schedule();
            var schedule2 = new Schedule();
            var schedule3 = new Schedule();
            
            schedule1.AddLesson(lesson1);
            schedule1.AddLesson(lesson2);
            schedule2.AddLesson(lesson3);
            schedule3.AddLesson(lesson4);
            
            Assert.True(schedule1.Stacked(schedule2));
            Assert.True(schedule2.Stacked(schedule1));
            Assert.False(schedule1.Stacked(schedule3));
        }

        [Test]
        public void TryingAddStudentToHisFacultyGSA_ThrowsException()
        {
            var faculty = new Faculty("Dungeon");
            var gsaSchedule = new Schedule();
            var gsa = new GsaCourse(faculty, "UltimateGSA");
            
            var schedule = new Schedule();

            _universityManager.RegisterFaculty(faculty);
            Group @group = _universityManager.CreateGroup(faculty, schedule, "M3100");
            Student student = _universityManager.CreateStudent(group, "Ilusha");
            
            Assert.Catch<IsuExtraException>(() =>
            {
                _universityManager.RegisterStudentOnGsa(gsa.CreateNewFlow(gsaSchedule), student);
            });
        }
        
        [Test]
        public void TryingAddStudentToGSAWithIntersectSchedules_ThrowsException()
        {
            var faculty1 = new Faculty("Dungeon");
            var faculty2 = new Faculty("Master");
            
            var gsaSchedule = new Schedule();
            var lesson = new Lesson(1, 8, 0);
            gsaSchedule.AddLesson(lesson);
            
            var gsa = new GsaCourse(faculty2, "UltimateGSA");
            
            var schedule = new Schedule();
            lesson = new Lesson(1, 9, 0);
            schedule.AddLesson(lesson);

            _universityManager.RegisterFaculty(faculty1);
            _universityManager.RegisterFaculty(faculty2);

            Group @group = _universityManager.CreateGroup(faculty1, schedule, "M3100");
            
            Student student = _universityManager.CreateStudent(group, "Ilusha");
            
            Assert.Catch<IsuExtraException>(() =>
            {
                _universityManager.RegisterStudentOnGsa(gsa.CreateNewFlow(gsaSchedule), student);
            });
        }

        [Test]
        public void GetListOfStudentsWithoutGsa_ObtainedListContainsStudents()
        {
            var faculty = new Faculty("Dungeon");
            
            var gsa = new GsaCourse(faculty, "UltimateGSA");
            
            var schedule = new Schedule();
            var lesson = new Lesson(1, 8, 0);
            schedule.AddLesson(lesson);

            _universityManager.RegisterFaculty(faculty);

            Group @group = _universityManager.CreateGroup(faculty, schedule, "M3100");
            
            Student student1 = _universityManager.CreateStudent(group, "Ilusha");
            Student student2 = _universityManager.CreateStudent(group, "Ilusha1");
            Student student3 = _universityManager.CreateStudent(group, "Ilusha2");

            Assert.AreEqual(_universityManager.GetListOfStudentsWithoutGsa(group).Count, 3);
        }

        [Test]
        public void AddsStudentOnGsa_GsaContainsStudent()
        {
            var faculty = new Faculty("Dungeon");
            _universityManager.RegisterFaculty(faculty);
            var schedule = new Schedule();
            Group @group = _universityManager.CreateGroup(faculty, schedule, "M3100");
            Student student = _universityManager.CreateStudent(group, "Ilusha");

            var gsaFaculty = new Faculty("Other");
            var gsa = new GsaCourse(gsaFaculty, "UltimateGSA");
            _universityManager.RegisterFaculty(gsaFaculty);
            
            _universityManager.RegisterStudentOnGsa(gsa.CreateNewFlow(new Schedule()), student);
            _universityManager.CancelRegistrationOnGsa(gsa, student);
            Assert.Catch<IsuExtraException>(() =>
            {
                _universityManager.CancelRegistrationOnGsa(gsa, student);
            });
        }
    }
}