using SQLite;

namespace bmcdani_c971_task;

public class Term
{
    [PrimaryKey, AutoIncrement]
    public int TermId { get; set; }
    public string TermName { get; set; }
    public DateTime TermStartDate { get; set; }
    public DateTime TermEndDate { get; set; }
}