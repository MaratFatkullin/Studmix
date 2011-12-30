namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Requests
{
    public class GetUserListRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public GetUserListRequest(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}