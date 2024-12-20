using MudBlazor;
using MudBlazor.Utilities;

namespace ShineGuacamole.Components.Layout
{
    /// <summary>
    /// The theme.
    /// </summary>
    public class ShineTheme : MudTheme
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public ShineTheme()
        {
            PaletteLight = new ShinePaletteLight();
            PaletteDark = new ShinePaletteDark();
        }
    }

    /// <summary>
    /// Shine light palette.
    /// </summary>
    public class ShinePaletteLight : PaletteLight
    {
        /// <inheritdoc/>
        public override MudColor AppbarBackground { get; set; } = "#294936";

        /// <inheritdoc/>
        public override MudColor Primary { get; set; } = "#294936";
        
        /// <inheritdoc/>
        public override MudColor Secondary { get; set; } = "#3e6259";
        
        /// <inheritdoc/>
        public override MudColor Tertiary { get; set; } = "#aef6c7";
    }

    /// <summary>
    /// Shine light palette.
    /// </summary>
    public class ShinePaletteDark : PaletteDark
    {
        /// <inheritdoc/>
        public override MudColor AppbarBackground { get; set; } = "#5b8266";

        /// <inheritdoc/>
        public override MudColor AppbarText { get; set; } = "#fff";

        /// <inheritdoc/>
        public override MudColor Primary { get; set; } = "#5b8266";

        /// <inheritdoc/>
        public override MudColor Secondary { get; set; } = "#aef6c7";

        /// <inheritdoc/>
        public override MudColor Tertiary { get; set; } = "#3e6259";
    }
}
