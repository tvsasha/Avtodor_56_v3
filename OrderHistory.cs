//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Avtodor_56_v3
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderHistory
    {
        public int HistoryID { get; set; }
        public Nullable<int> MaterialID { get; set; }
        public decimal StockRemaining { get; set; }
        public System.DateTime LastOperationDate { get; set; }
    
        public virtual Materials Materials { get; set; }
    }
}
