using DataAccessLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeclutterMeRazorUI.Models;

public class ProductUpsertModel
{
    public Product Product { get; set; }
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
