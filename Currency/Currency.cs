using System;
using System.Collections.Generic;
using System.Text;

namespace Currency
{
    //чтобы при парсинге XML и последующей рефлексии не было проблем с типом полей класса сделал все строковыми ( но так лучше не делать!:)  )
    public class Currency
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string Nominal { get; set; } //int
        public string ParentCode { get; set; }
        public string ISO_Num_Code { get; set; } //int
        public string ISO_Char_Code { get; set; }
    }
}
