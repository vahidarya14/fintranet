using congestion.calculator.Domain;
using System;
using System.Collections.Generic;
using System.Text;


public class DatabaseMock : IDatabase
{
    public List<BaseInfoHoliday> Holidaies { get; private set; }
    public List<BaseInfoTollPriceConfig> TollPriceConfigs { get; private set; }


    public DatabaseMock()
    {
        Holidaies = new List<BaseInfoHoliday>
        {
            new BaseInfoHoliday(1,1),
            new BaseInfoHoliday(3,29),
            new BaseInfoHoliday(4,1),
            new BaseInfoHoliday(5,1),
            new BaseInfoHoliday(5,9),
            new BaseInfoHoliday(6,6),
            new BaseInfoHoliday(6,21),
            new BaseInfoHoliday(11,1),
            new BaseInfoHoliday(12,25),
            new BaseInfoHoliday(12,31),
        };


        TollPriceConfigs = new List<BaseInfoTollPriceConfig>
        {
            new BaseInfoTollPriceConfig{Hour=6,FromMinute=0,ToMinute=29,Fee=8},
            new BaseInfoTollPriceConfig{Hour=6,FromMinute=30,ToMinute=59,Fee=13},
            new BaseInfoTollPriceConfig{Hour=7,FromMinute=0,ToMinute=59,Fee=18},
            new BaseInfoTollPriceConfig{Hour=8,FromMinute=0,ToMinute=29,Fee=13},
            new BaseInfoTollPriceConfig{Hour=8,FromMinute=30,ToMinute=59,Fee=8},
            new BaseInfoTollPriceConfig{Hour=15,FromMinute=0,ToMinute=29,Fee=13},
            new BaseInfoTollPriceConfig{Hour=15,FromMinute=30,ToMinute=59,Fee=18},
            new BaseInfoTollPriceConfig{Hour=17,FromMinute=0,ToMinute=59,Fee=13},
            new BaseInfoTollPriceConfig{Hour=18,FromMinute=0,ToMinute=29,Fee=8},
            new BaseInfoTollPriceConfig{Hour=18,FromMinute=30,ToMinute=59,Fee=0},
        };
    }

}

