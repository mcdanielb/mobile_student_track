using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace bmcdani_c971_task;

public class Assessments
{
    [PrimaryKey, AutoIncrement]
    public int AssessmentId { get; set; }
    public string AssessmentName { get; set; }
    public DateTime AssessmentStartDate { get; set; }
    public DateTime AssessmentEndDate { get; set; }
    public string AssessmentType { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }
}