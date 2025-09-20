using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "書名")]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "作者")]
        public string Author { get; set; } = string.Empty;
        
        [Display(Name = "ISBN")]
        public string? ISBN { get; set; }
        
        [Display(Name = "出版社")]
        public string? Publisher { get; set; }
        
        [Display(Name = "出版年份")]
        public int? PublicationYear { get; set; }
        
        [Display(Name = "分類")]
        public string Category { get; set; } = string.Empty;
        
        [Display(Name = "館藏位置")]
        public string Location { get; set; } = string.Empty;
        
        [Display(Name = "庫存數量")]
        public int TotalCopies { get; set; }
        
        [Display(Name = "可借數量")]
        public int AvailableCopies { get; set; }
        
        [Display(Name = "書籍描述")]
        public string? Description { get; set; }
        
        [Display(Name = "封面圖片")]
        public string? CoverImageUrl { get; set; }
        
        [Display(Name = "是否為新書")]
        public bool IsNewArrival { get; set; }
        
        [Display(Name = "是否為熱門書籍")]
        public bool IsPopular { get; set; }
        
        [Display(Name = "是否為精選書籍")]
        public bool IsFeatured { get; set; }
        
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    public class LibraryHours
    {
        public string Day { get; set; } = string.Empty;
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsOpen { get; set; }
        public string? SpecialNote { get; set; }
    }

    public class BorrowingStats
    {
        [Display(Name = "今日開館時間")]
        public string TodayHours { get; set; } = string.Empty;
        
        [Display(Name = "17樓期刊室開放時間")]
        public string JournalRoomHours { get; set; } = string.Empty;
        
        [Display(Name = "17樓特殊收藏")]
        public int SpecialCollections { get; set; }
        
        [Display(Name = "3樓閱覽")]
        public int ThirdFloorReading { get; set; }
        
        [Display(Name = "今日借閱統計")]
        public string TodayBorrowingNote { get; set; } = string.Empty;
    }

    public class LibraryNews
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "標題")]
        public string Title { get; set; } = string.Empty;
        
        [Display(Name = "內容")]
        public string? Content { get; set; }
        
        [Display(Name = "發布日期")]
        public DateTime PublishDate { get; set; }
        
        [Display(Name = "是否置頂")]
        public bool IsPinned { get; set; }
        
        [Display(Name = "新聞連結")]
        public string? Link { get; set; }
    }

    public class LibraryEvent
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "活動名稱")]
        public string Title { get; set; } = string.Empty;
        
        [Display(Name = "活動描述")]
        public string? Description { get; set; }
        
        [Display(Name = "活動圖片")]
        public string? ImageUrl { get; set; }
        
        [Display(Name = "開始時間")]
        public DateTime StartDate { get; set; }
        
        [Display(Name = "結束時間")]
        public DateTime EndDate { get; set; }
        
        [Display(Name = "活動地點")]
        public string? Location { get; set; }
        
        [Display(Name = "是否進行中")]
        public bool IsActive { get; set; }
        
        [Display(Name = "活動連結")]
        public string? Link { get; set; }
    }

    public class UserAccount
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
        public List<ReservedBook> ReservedBooks { get; set; } = new List<ReservedBook>();
        public decimal Fines { get; set; }
    }

    public class BorrowedBook
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsOverdue => DateTime.Now > DueDate;
    }

    public class ReservedBook
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public int QueuePosition { get; set; }
    }

    public class LibraryServiceInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }

    public class DigitalResource
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool RequiresAuthentication { get; set; }
    }

    public class LibraryViewModel
    {
        public BorrowingStats BorrowingStats { get; set; } = new BorrowingStats();
        public List<LibraryNews> LatestNews { get; set; } = new List<LibraryNews>();
        public List<Book> NewArrivals { get; set; } = new List<Book>();
        public List<Book> FeaturedBooks { get; set; } = new List<Book>();
        public List<Book> PopularBooks { get; set; } = new List<Book>();
        public LibraryEvent? CurrentEvent { get; set; }
        public LibraryHours LibraryHours { get; set; } = new LibraryHours();
    }
}