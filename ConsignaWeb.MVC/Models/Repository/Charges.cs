using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsignaWeb.MVC.Models.ChargesExtensions;

namespace ConsignaWeb.MVC.Models.Repository
{
    [ActiveRecord(Schema = "yuffiedb")]
    public class Charges : ActiveRecordLinqBase<Charges>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id { get; set; }

        [BelongsTo]
        public Users Client { get; set; }

        [BelongsTo]
        public Users Vendor { get; set; }

        [BelongsTo]
        public Products Products { get; set; }

        [Property(Column = "Type")]
        public ChargesTypes Type { get; set; }


        [Property(Column = "Data")]
        public DateTime Data { get; set; }

        [Property(Column = "Amount")]
        public int Amount { get; set; }

        [Property(Column = "Value")]
        public double Value { get; set; }

    }
}