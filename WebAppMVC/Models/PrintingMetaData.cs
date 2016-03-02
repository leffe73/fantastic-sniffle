using System.ComponentModel.DataAnnotations;
using WebAppMVC;

namespace WebAppMVC.Models
{
    [MetadataType(typeof(PrintingMetaData))]
    public partial class Printing
    {
    }

    public class PrintingMetaData
    {
        [Required]
        public object NoOfCopies { get; set; }
    }
}