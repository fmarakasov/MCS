using WindowsTaskDialog;

namespace AppExceptions
{
    public struct ExceptionDescription
    {
        public string Content { get; set; } 
        public TaskDialogIcon Icon { get; set; }
        public string Instruction { get; set; }
        public string Title { get; set; }
        public bool ShowFooter { get; set; }
           
    }
}