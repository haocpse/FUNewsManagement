using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Entity
{
    public class Category
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey(nameof(ParentCategoryId))]
        public Category? ParentCategory { get; set; }

        public ICollection<Category>? SubCategories { get; set; }

    }
}
