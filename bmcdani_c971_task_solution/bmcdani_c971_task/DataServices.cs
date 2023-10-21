using SQLite;
using System;
using System.Collections.Generic;
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

            await db.DropTableAsync<Term>();
            await db.DropTableAsync<Course>();

            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            // await db.CreateTableAsync<Assessments>();
        }

        // Term methods

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

        // Course methods

        public static async Task AddCourse(string courseName, DateTime courseStartDate, DateTime courseEndDate, string status, int termId)
        {
            await Init();
            var course = new Course
            {
                CourseName = courseName,
                CourseStartDate = courseStartDate,
                CourseEndDate = courseEndDate,
                Status = status,
                TermId = termId
            };

            await db.InsertAsync(course);
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await db.DeleteAllAsync<Course>();
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
    }
}
