namespace BasstahalakMS.Models
{
    public class ReviewPermission
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public int IsAdmin { get; set; } = 0;

        public ICollection<UserReviewPermission> UserReviewPermissions { get; set; }

    }
}
