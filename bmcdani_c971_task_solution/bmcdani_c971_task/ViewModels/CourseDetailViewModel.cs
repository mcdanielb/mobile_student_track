using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace bmcdani_c971_task.ViewModels;

public class CourseDetailViewModel : INotifyPropertyChanged
{
	public ObservableCollection<NotesList> NotesList { get; set; } = new ObservableCollection<NotesList>();
	public ICommand AddNoteCommand { get; set; }
	public ICommand DeleteNoteCommand { get; set; }
	public ICommand ShareAllNotesCommand { get; set; }


	private int _courseId;

	public CourseDetailViewModel(int courseId)
	{
		_courseId = courseId;
		
		AddNoteCommand = new Command<string>(AddNote);
		DeleteNoteCommand = new Command<NotesList>(async (note) => await DeleteNote(note));
		ShareAllNotesCommand = new Command(async () => await ShareNotes());

		LoadNotes();
	}

	public async Task LoadNotes()
	{
		var notes = await DataServices.GetNotesByCourseId(_courseId);
		NotesList.Clear();
		foreach (var note in notes)
		{
			NotesList.Add(note);
		}
	}

	private async void AddNote(string noteContent)
	{
		if (string.IsNullOrWhiteSpace(noteContent))
			return;

		NotesList note = new NotesList
		{
			NoteContent = noteContent,
			CourseId = _courseId
		};

		await DataServices.AddNote(_courseId, noteContent);

		NotesList.Add(note);
	}

	private async Task DeleteNote(NotesList note)
	{
		if (note == null) return;

		await DataServices.DeleteNote(note.NoteId);

		NotesList.Remove(note);
	}

    public async Task ShareNotes()
    {
        var notesForCourse = await DataServices.GetNotesByCourseId(_courseId);

        if (notesForCourse?.Any() == true) 
		{
            StringBuilder notesTextBuilder = new StringBuilder();
            foreach (var note in NotesList)
            {
                notesTextBuilder.AppendLine(note.NoteContent);
                notesTextBuilder.AppendLine("------------");
            }

            string notesText = notesTextBuilder.ToString();

            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = notesText,
                Title = "Share Notes"
            });
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}