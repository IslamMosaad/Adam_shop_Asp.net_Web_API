namespace EcommerceAPI.DTO
{
    public class UserManagerResponseDTO
    {
        public string Message { get; set; }
        public bool? IsAdmin { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
