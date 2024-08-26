namespace api.main.tecnicah.Models.PagedList
{
    public class PagedListDto<T>
    {
        public int TotalRows { get; set; }
        public IList<T> Paged { get; set; }

        public PagedListDto()
        {
            TotalRows = 0;
            Paged = new List<T>();
        }

        public PagedListDto(int totalRows, IList<T> result)
        {
            TotalRows = totalRows;
            Paged = result;
        }
    }

    public class PagedListUserDto<UserDto>
    {
        public int TotalRows { get; set; }
        public IList<UserDto> Paged { get; set; }

        public PagedListUserDto()
        {
            TotalRows = 0;
            Paged = new List<UserDto>();
        }

        public PagedListUserDto(int totalRows, IList<UserDto> result)
        {
            TotalRows = totalRows;
            Paged = result;
        }
    }
}
