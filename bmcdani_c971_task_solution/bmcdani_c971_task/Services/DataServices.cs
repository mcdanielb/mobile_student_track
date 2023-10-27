using SQLite;
using System;
using System.IO;
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
            //await db.DropTableAsync<NotesList>();
            //await db.DropTableAsync<Assessment>();

            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            await db.CreateTableAsync<NotesList>();
            await db.CreateTableAsync<Assessment>();
        }

        // Prepopulate Data (Called in MainPage.xaml.cs)

        public static async Task PrepopulateData()
        {
            await Init();

            DateTime specificDate = new DateTime(2023, 10, 1);

            var terms = await GetTerms();
            int termId;
            if (!terms.Any())
            {
                await AddTerm("Sample Term", specificDate, DateTime.Now.AddMonths(1));
                var sampleAddedTerm = await GetTerms();
                termId = sampleAddedTerm.First().TermId;
            }
            else
            {
                termId = terms.First().TermId;
            }

            var courses = await GetCourses(termId);
            int courseId;
            if (!courses.Any())
            {
                await AddCourse("Sample Course", specificDate, DateTime.Now.AddDays(10), "In Progress", "Anika Patel", "555-123-4567", "anika.patel@strimeuniversity.edu", true, true, termId);
                var sampleAddedCourse = await GetCourses(termId);
                courseId = sampleAddedCourse.First().CourseId;
            }
            else
            {
                courseId = courses.First().CourseId;
            }

            var assessments = await GetAssessments(courseId);
            if (!assessments.Any())
            {
                await AddAssessment("Final Exam", specificDate, DateTime.Now.AddDays(10), "Performance Assessment", true, true, courseId);
                await AddAssessment("Final Project", specificDate, DateTime.Now.AddDays(10), "Objective Assessment", true, true, courseId);
            }
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

        public static async Task AddCourse(string courseName, DateTime courseStartDate, DateTime courseEndDate, string courseStatus, string instructorName, string instructorPhone, string instructorEmail, bool courseNotifyOnStartDate, bool courseNotifyOnEndDate, int termId)
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

        public static async Task UpdateCourse(int courseId, string courseName, DateTime courseStartDate, DateTime courseEndDate, string courseStatus, string instructorName, string instructorPhone, string instructorEmail, bool courseNotifyOnStartDate, bool courseNotifyOnEndDate, int termId)
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

        public static async Task<List<NotesList>> GetNotesByCourseId(int courseId)
        {
            await Init();

            var notes = await db.Table<NotesList>().Where(n => n.CourseId == courseId).ToListAsync();

            return notes;
        }

        public static async Task AddNote(int courseId, string noteContent)
        {
            await Init();
            var note = new NotesList
            {
                NoteContent = noteContent,
                CourseId = courseId
            };

            await db.InsertAsync(note);
        }

        public static async Task DeleteNote(int noteId)
        {
            await Init();

            var note = await db.FindAsync<NotesList>(noteId);
            if (note != null)
            {
                await db.DeleteAsync(note);
            }
        }

        public static async Task<int> GetCourseCountByTermId(int termId)
        {
            await Init();

            return await db.Table<Course>().Where(c => c.TermId == termId).CountAsync();
        }

        // Assessment methods

        public static async Task AddAssessment(string assessmentName, DateTime assessmentStartDate, DateTime assessmentEndDate, string assessmentType, bool assessmentNotifyStartDate, bool assessmentNotifyEndDate, int courseId)
        {
            await Init();
            var assessment = new Assessment
            {
                AssessmentName = assessmentName,
                AssessmentStartDate = assessmentStartDate,
                AssessmentEndDate = assessmentEndDate,
                AssessmentType = assessmentType,
                AssessmentNotifyStartDate = assessmentNotifyStartDate,
                AssessmentNotifyEndDate = assessmentNotifyEndDate,
                CourseId = courseId
            };

            await db.InsertAsync(assessment);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments(int? CourseId = null)
        {
            await Init();

            var assessmentQuery = db.Table<Assessment>();
            if (CourseId.HasValue)
            { 
                assessmentQuery = assessmentQuery.Where(a => a.CourseId == CourseId.Value);
            }

            var assessments = await assessmentQuery.ToListAsync();
            return assessments;
        }

        public static async Task RemoveAssessment(int assessmentId)
        {
            await Init();

            await db.DeleteAsync<Assessment>(assessmentId);
        }

        public static async Task<Assessment> GetAssessmentById(int assessmentId)
        {
            await Init();

            var assessment = await db.Table<Assessment>().Where(a => a.AssessmentId == assessmentId).FirstOrDefaultAsync();
            return assessment;
        }

        public static async Task UpdateAssessment(int assessmentId, string assessmentName, DateTime assessmentStartDate, DateTime assessmentEndDate, string assessmentType, bool assessmentNotifyStartDate, bool assessmentNotifyEndDate, int courseId)
        {
            await Init();

            var assessmentToUpdate = await db.Table<Assessment>().Where(a => a.AssessmentId == assessmentId).FirstOrDefaultAsync();
            if (assessmentToUpdate != null)
            {
                assessmentToUpdate.AssessmentName = assessmentName;
                assessmentToUpdate.AssessmentStartDate = assessmentStartDate;
                assessmentToUpdate.AssessmentEndDate = assessmentEndDate;
                assessmentToUpdate.AssessmentType = assessmentType;
                assessmentToUpdate.AssessmentNotifyStartDate = assessmentNotifyStartDate;
                assessmentToUpdate.AssessmentNotifyEndDate = assessmentNotifyEndDate;
                assessmentToUpdate.CourseId = courseId;

                await db.UpdateAsync(assessmentToUpdate);
            }
        }

        public static async Task<int> GetAssessmentCountByType(string type, int courseId)
        {
            await Init();

            return await db.Table<Assessment>().Where(a => a.CourseId == courseId && a.AssessmentType == type).CountAsync();
        }
    }
}
