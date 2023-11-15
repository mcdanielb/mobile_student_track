using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace bmcdani_academic_tracking;

public class NotesList
{
    [PrimaryKey, AutoIncrement]
    public int NoteId { get; set; }
    public string NoteContent { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }
}