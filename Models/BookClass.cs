using System;

namespace Library.Models;

public class BookClass
{
    public string Title {get; set;} = null!;
    public string Author {get; set;} = null!;
    public string DatePublished {get; set;} = null!;
    public long ISBN {get; set;} = 0;
    public float Rating {get; set;} = 0.0;
    public float PriceUSD {get; set;} = 0.0;
    public int Pages {get; set;} = 0;
}