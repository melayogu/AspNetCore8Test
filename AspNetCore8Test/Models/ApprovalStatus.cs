namespace AspNetCore8Test.Models
{
    /// <summary>
    /// 簽核狀態
    /// </summary>
    public enum ApprovalStatus
    {
        /// <summary>
        /// 待簽核
        /// </summary>
        Pending = 0,
        
        /// <summary>
        /// 進行中
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// 已同意
        /// </summary>
        Approved = 2,
        
        /// <summary>
        /// 已拒絕
        /// </summary>
        Rejected = 3,
        
        /// <summary>
        /// 已撤回
        /// </summary>
        Withdrawn = 4,
        
        /// <summary>
        /// 已過期
        /// </summary>
        Expired = 5
    }
}