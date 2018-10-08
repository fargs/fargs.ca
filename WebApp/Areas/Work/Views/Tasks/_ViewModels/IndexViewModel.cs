namespace WebApp.Areas.Work.Views.Tasks
{
    public class IndexViewModel
    {
        public IndexViewModel(TasksViewModel tasks)
        {
            Tasks = tasks;
        }
        public TasksViewModel Tasks { get; private set; }
    }
}