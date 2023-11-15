using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace bmcdani_c971_task;

public class Assessment
{
    [PrimaryKey, AutoIncrement]
    public int AssessmentId { get; set; }
    public string AssessmentName { get; set; }
    public DateTime AssessmentStartDate { get; set; }
    public DateTime AssessmentEndDate { get; set; }
    public string AssessmentType { get; set; }
    public bool AssessmentNotifyStartDate { get; set; }
    public bool AssessmentNotifyEndDate { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }
}