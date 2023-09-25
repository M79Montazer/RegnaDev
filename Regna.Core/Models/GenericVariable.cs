namespace Regna.Core.Models
{
    public class GenericVariable : BaseEntity
    {
        public long OVariableId { get; set; }
        public string OVariableName { get; set; }
        public VariableType VariableType { get; set; }
        public string DefaultValue { get; set; }
    }
}
