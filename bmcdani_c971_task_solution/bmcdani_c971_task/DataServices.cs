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

            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            // await db.CreateTableAsync<Assessments>();
        }

        // Term methods

        public static async Task AddTerm(string name)
        {
            await Init();
            var term = new Term
            {
                Name = name
            };

            await db.InsertAsync(term);
        }

        public static async Task RemoveTerm(int id)
        {
            await Init();

            await db.DeleteAsync<Term>(id);
        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var term = await db.Table<Term>().ToListAsync();
            return term;
        }

        // Course methods

        public static async Task AddCourse(string name, DateTime startdate, DateTime enddate, string status, string notes, int termid)
        {
            await Init();
            var course = new Course
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Status = status,
                Notes = notes,
                TermId = termid
            };

            await db.InsertAsync(course);
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await db.DeleteAsync<Course>(id);
        }

        public static async Task<IEnumerable<Course>> GetCourses(int? termId = null)
        {
            await Init();

            var courseQuery = db.Table<Course>();
            if (termId.HasValue)
            {
                courseQuery = courseQuery.Where(c => c.TermId == termId.Value);
            }

            var courses = await courseQuery.ToListAsync();
            return courses;
        }
    }
}
