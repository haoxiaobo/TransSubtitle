using System.Collections;
using System.Collections.Generic;

public class SubtitleItem
{
    public int Index{get;set;}

    public string TimeFrom{get;set;}

    public string TimeTo{get;set;}

    public List<string> Texts {get;set;} = new List<string>();
}