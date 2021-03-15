using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF
{
    /*
         *General rules and standard practices fro metadata
         * 1) All metadata can exist in a single file for all classes associated to Entity Framework.
         * 2) Or, it can be split between files for each class associated to Entity Framework.
         * We will be doing (1) in our examples.
         * 3) Metadata classes must be in the same namespace as the original EF class.
         * 4) Short guide for connecting each pair of metadata classes and EF classes.
         * 
         *  a) ensure that the namespaces of the files match (match the .tt namespace). Add the using for System.ComponentModel.DataAnnotations
         *  
         *  b) Create the metadata class (empty)
         *      ex- public class MyTableMetadata{}
         *      
         *  c) Apply the metadata type attribute to the meatadata partial class
         *      ex- [MetadataType(typeof(MyTableMetadata))]
         *      
         *  d) Create the metadata partial class with the same exact name as the EF class
         *      ex- public partial class MyTable{}
         *      
         *  e) Use the EF class and copy the properties to our metadata class
         *      ex- public class MyTableMetadata
         *      {
         *          public int MyTableID { get; set; }
         *          public string MyField { get; set; }
         *       }
         *     
         *  f) Apply all necessary metadata attributes
         *      ex- public class MyTableMetadata
         *      {
         *          [Required(ErrorMessage = "*")]
         *          public int MyTableID { get; set; }
         *          [Display(Name ="My Field")]
         *          public string MyField { get; set; }
         *       }
         *       
         *  5) Use the diagram from SSMS to ensure that all validation and metadata is correct.     
         */


    /*
     *Notes for MetaData Attributes
     * 1) If a DB column name changes in the DB then the associated Metadata names need to change too.
     * 2) Primary keys dont actually need annotations
     * 3) We can change how the DB field names are displayed in the UI using:[Display(Name = "First Name")]
     * 4) We want to use the DB to know the field length and if Null's are allowed.*/

    #region
    public class BrandMetadata
    {
        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Brand Name is required")]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
    }//end class

    [MetadataType(typeof(BrandMetadata))]
    public partial class Brand { }

    #endregion

    //**********************************************************************************//

    #region Departments
    public class DepartmentsMetadata
    {
        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Department Name is required")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }//end class

    [MetadataType(typeof(DepartmentsMetadata))]
    public partial class Departments { }

    #endregion

    //**********************************************************************************//


    #region Employees

    public class EmployeesMetadata
    {
        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "*Department ID is required")]
        public int DepartmentID { get; set; }
    }//end class

    [MetadataType(typeof(EmployeesMetadata))]
    public partial class Employees { }


    #endregion

    //**********************************************************************************//

    #region Model Categories

    public class ModelCategoriesMetadata
    {
        
        [Required(ErrorMessage = "*Brand Name is required")]
        [Display(Name = "Brand Name")]
        public int BrandID { get; set; }

        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Model Name is required")]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

    }//end class

    [MetadataType(typeof(ModelCategoriesMetadata))]
    public partial class ModelCategories { }

    #endregion

    //**********************************************************************************//

    #region Production Status 

    public class ProductStatusMetadata
    {
        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Production Status is required")]
        [Display(Name = "Production Status")]
        public string ProductStatusName { get; set; }
    }//end class

    [MetadataType(typeof(ProductStatusMetadata))]
    public partial class ProductStatus { }

    #endregion


    //**********************************************************************************//

    #region Product


    public class ProductMetadata
    {
        [Required(ErrorMessage = "*Model ID is required")]
        [Display(Name = "Model ID")]
        public int ModelID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "*Value must be a valid number, 0 or larger")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Display(Name = "Units Sold")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Range(0, int.MaxValue, ErrorMessage = "*Value must be a valid number, 0 or larger")]
        public Nullable<int> UnitsSold { get; set; }

        [Required(ErrorMessage = "*Production Status ID is required")]
        [Display(Name = "Production Status ID")]
        public int ProductStatusID { get; set; }

        [StringLength(100, ErrorMessage = "*The value must be 100 characters or less.")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "*The value must be 100 characters or less.")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string ImageUrl { get; set; }


    }//end class

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product { }

    #endregion

}

