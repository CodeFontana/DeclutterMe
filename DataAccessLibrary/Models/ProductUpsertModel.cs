﻿namespace DataAccessLibrary.Models;

public class ProductUpsertModel
{
    public Product Product { get; set; }
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
