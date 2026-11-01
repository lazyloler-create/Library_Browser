using System;

namespace Library.Models;

public class BookClass
{
    public string Title {get; set;} = null!;
    public string DatePublished {get; set;} = null!;
    public List<string> Author {get; set;} = null!;
    public List<string> Categories {get; set;} = null!;
    public long ISBN {get; set;} = 0;
    public float Rating {get; set;} = 0.0f;
    public float PriceUSD {get; set;} = 0.0f;
    public int Pages {get; set;} = 0;
}
