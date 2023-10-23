using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmcdani_c971_task
{
    public static class DataServices
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null)
                return;
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            //await db.DropTableAsync<Term>();
            //await db.DropTableAsync<Course>();

            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            await db.CreateTableAsync<NotesList>();
            await db.CreateTableAsync<Assessment>();
        }

        // Term Methods

        public static async Task AddTerm(string termName, DateTime termStartDate, DateTime termEndDate)
        {
            await Init();
            var term = new Term
            {
                TermName = termName,
                TermStartDate = termStartDate,
                TermEndDate = termEndDate
            };

            await db.InsertAsync(term);
        }

        public static async Task RemoveTerm(int termId)
        {
            await Init();

            await db.DeleteAsync<Term>(termId);
        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var term = await db.Table<Term>().ToListAsync();
            return term;
        }

        public static async Task<Term> GetTermById(int termId)
        {
            await Init();

            var term = await db.Table<Term>().Where(t => t.TermId == termId).FirstOrDefaultAsync();
            return term;
        }

        public static async Task UpdateTerm(int termId, string termName, DateTime termStartDate, DateTime termEndDate)
        {
            await Init();

            var termToUpdate = await db.Table<Term>().Where(t => t.TermId == termId).FirstOrDefaultAsync();
            if (termToUpdate != null)
            {
                termToUpdate.TermName = termName;
                termToUpdate.TermStartDate = termStartDate;
                termToUpdate.TermEndDate = termEndDate;

                await db.UpdateAsync(termToUpdate);
            }
        }

        // Course Methods

        public static async Task AddCourse(string courseName,
                                           DateTime courseStartDate,
                                           DateTime courseEndDate,
                                           string courseStatus,
                                           string instructorName,
                                           string instructorPhone,
                                           string instructorEmail,
                                           bool courseNotifyOnStartDate,
                                           bool courseNotifyOnEndDate,
                                           int termId)
        {
            await Init();
            var course = new Course
            {
                CourseName = courseName,
                CourseStartDate = courseStartDate,
                CourseEndDate = courseEndDate,
                CourseStatus = courseStatus,
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,
                CourseNotifyOnStartDate = courseNotifyOnStartDate,
                CourseNotifyOnEndDate = courseNotifyOnEndDate,
                TermId = termId
            };

            await db.InsertAsync(course);
        }

        public static async Task RemoveCourse(int courseId)
        {
            await Init();

            await db.DeleteAsync<Course>(courseId);
        }

        public static async Task<IEnumerable<Course>> GetCourses(int? TermId = null)
        {
            await Init();

            var courseQuery = db.Table<Course>();
            if (TermId.HasValue)
            {
                courseQuery = courseQuery.Where(c => c.TermId == TermId.Value);
            }

            var courses = await courseQuery.ToListAsync();
            return courses;
        }

        public static async Task<Course> GetCourseById(int courseId)
        {
            await Init();

            var course = await db.Table<Course>().Where(c => c.CourseId == courseId).FirstOrDefaultAsync();
            return course;
        }

        public static async Task UpdateCourse(int courseId,
                                              string courseName,
                                              DateTime courseStartDate,
                                              DateTime courseEndDate,
                                              string courseStatus,
                                              string instructorName,
                                              string instructorPhone,
                                              string instructorEmail,
                                              bool courseNotifyOnStartDate,
                                              bool courseNotifyOnEndDate,
                                              int termId)
        {
            await Init();

            var courseToUpdate = await db.Table<Course>().Where(c => c.CourseId == courseId).FirstOrDefaultAsync();
            if (courseToUpdate != null)
            {
                courseToUpdate.CourseName = courseName;
                courseToUpdate.CourseStartDate = courseStartDate;
                courseToUpdate.CourseEndDate = courseEndDate;
                courseToUpdate.CourseStatus = courseStatus;
                courseToUpdate.InstructorName = instructorName;
                courseToUpdate.InstructorPhone = instructorPhone;
                courseToUpdate.InstructorEmail = instructorEmail;
                courseToUpdate.CourseNotifyOnStartDate = courseNotifyOnStartDate;
                courseToUpdate.CourseNotifyOnEndDate = courseNotifyOnEndDate;
                courseToUpdate.TermId = termId;

                await db.UpdateAsync(courseToUpdate);
            }
        }

        // Assessment methods

        public static async Task<IEnumerable<Assessment>> GetAssessments(int CourseId)
        {
            await Init();

            var assessment = await db.Table<Assessment>().Where(a => a.CourseId == CourseId).ToListAsync();
            return assessment;
        }

        public static async Task RemoveAssessment(int courseId)
        {
            await Init();

            await db.DeleteAsync<Assessment>(courseId);
        }
    }
}
