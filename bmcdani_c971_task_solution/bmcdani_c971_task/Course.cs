using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace bmcdani_c971_task;

public class Course
{
    [PrimaryKey, AutoIncrement]
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public DateTime CourseStartDate { get; set; }
    public DateTime CourseEndDate { get; set; }
    public string CourseStatus { get; set; }
    public string InstructorName { get; set; }
    public string InstructorPhone { get; set; }
    public string InstructorEmail { get; set; }
    public bool CourseNotifyOnStartDate { get; set; }
    public bool CourseNotifyOnEndDate {  get; set; }

    [ForeignKey("Term")]
    public int TermId { get; set; }
}