using SQLite;

namespace bmcdani_academic_tracking;

public class Term
{
    [PrimaryKey, AutoIncrement]
    public int TermId { get; set; }
    public string TermName { get; set; }
    public DateTime TermStartDate { get; set; }
    public DateTime TermEndDate { get; set; }
}