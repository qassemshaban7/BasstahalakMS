namespace BasstahalakMS.Models
{
    public class EditBFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BookName { get; set; }
        public string BranchName { get; set; }  
        public int UnitsCount { get; set; }
        public int LessonsCount { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
